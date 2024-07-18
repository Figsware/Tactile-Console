using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Tactile.Console.Commands;
using Tactile.Console.Printing;
using UnityEditor;
using UnityEngine;

namespace Tactile.Console.Components
{
    public class NetworkedConsole : MonoBehaviour
    {
        [SerializeField] private string bindAddress = "0.0.0.0";
        [SerializeField] private int port = 8888;
        [SerializeField] private string prefix = "$ ";
        [SerializeField] private bool runInEditor = true;
        [SerializeField] private bool runInReleaseBuilds = false;
        private Socket _socket;
        private ConcurrentQueue<(Console console, string command)> _queue;
        private ConcurrentBag<Socket> _clients;
        
        #region Unity Events
        private void OnEnable()
        {
            RunSocket();
        }

        private void OnDisable()
        {
            // Disconnect clients.
            if (_clients != null)
            {
                foreach (var client in _clients)
                {
                    client.Close();
                }
                _clients.Clear();
            }
            
            _socket?.Close();
        }

        private void Update()
        {
            if (_queue == null) return;
            while (_queue.TryDequeue(out var pending))
            {
                var (console, command) = pending;
                console.Execute(command);
            }
        }
        
        #endregion

        private async void RunSocket()
        {
            // If we don't allow running in editor, stop now.
            if (Application.isEditor && !runInEditor)
                return;

            // If we're a release build but don't allow running in release builds, stop now.
            if (!Application.isEditor && !Debug.isDebugBuild && !runInReleaseBuilds)
                return;
            
            _queue = new ConcurrentQueue<(Console console, string command)>();
            _clients = new ConcurrentBag<Socket>();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (!IPAddress.TryParse(bindAddress, out var addr))
            {
                Debug.LogError("Cannot parse IP address!");
                return;
            }
            
            var endpoint = new IPEndPoint(addr, port);
            _socket.Bind(endpoint);
            _socket.Listen(100);
            
            Debug.Log($"Listening for console clients on {bindAddress}:{port}");
            while (true)
            {
                Socket client;
                try
                {
                    client = await _socket.AcceptAsync();
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                
                _clients.Add(client);
                HandleClient(client);
            }
        }

        private async void HandleClient(Socket client)
        {
            var endpoint = client.RemoteEndPoint;
            Debug.Log($"New client connected: {endpoint}");

            var console = PrepareConsoleForClient(client);
            
            console.Print($"{Application.productName} {Application.version} at your service.");
            
            var buffer = new byte[65536];
            while (client.Connected)
            {
                int received;
                try
                {
                    received = await client.ReceiveAsync(buffer, SocketFlags.None);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                
                var cmd = Encoding.UTF8.GetString(buffer, 0, received);
                if (!cmd.EndsWith("\n")) continue;
                
                cmd = cmd[..^1];
                _queue.Enqueue((console, cmd));
                
#if UNITY_EDITOR
                if (!Application.isPlaying && runInEditor)
                {
                    EditorApplication.QueuePlayerLoopUpdate();
                }                
#endif
            }
            
            Debug.Log($"Client {endpoint} disconnected");
        }

        private Console PrepareConsoleForClient(Socket client)
        {
            var console = new Console(new AnsiPrintBuilder());
            console.OnPrintLine += line => SendStringToClient(client, AnsiPrintBuilder.PrintLineAndRestoreCursor(line, prefix));
            
            console.AddConsoleCommand(new Command("disconnect", "Closes your connection with the server.", (_, _) =>
            {
                client.Close();
            }));
            
            console.AddConsoleCommand(new Command("clear", "Clears the screen", (_, _) =>
            {
                SendStringToClient(client, AnsiPrintBuilder.ClearSequence(prefix));
            }));

            return console;
        }

        private static void SendStringToClient(Socket client, string message)
        {
            if (!client.Connected) return;
            var bytes = Encoding.UTF8.GetBytes(message);
            client.SendAsync(bytes, SocketFlags.None);
        }
    }
}