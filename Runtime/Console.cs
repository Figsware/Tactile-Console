﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tactile.Console.Commands;
using UnityEngine;

namespace Tactile.Console
{
    public class Console
    {
        public bool EmitUnityLogMessages = true;
        public bool UseGlobalCommands = true;
        
        public PrintBuilder.PrintFormat Format;
        public event Action<string> OnPrintLine;
        
        private readonly Dictionary<string, BaseCommand> _consoleCommands = new();
        private static readonly Dictionary<string, BaseCommand> GlobalConsoleCommands = new();
        private bool _emitUnityLogMessages;
        private static bool _didLoadGlobalCommands = false;
        private readonly PrintBuilder _printBuilder;
     
        public Console()
        {
            Format = new PrintBuilder.PrintFormat();
            _printBuilder = new PrintBuilder(Format);
            Application.logMessageReceived += OnUnityLogMessage;
            LoadGlobalCommands();
        }
        
        public void Execute(string commandStr)
        {
            var nameAndBody = BaseCommand.ParseCommand(commandStr);
            if (nameAndBody == null)
                return;

            var (name, body) = nameAndBody.Value;
            if (_consoleCommands.TryGetValue(name, out var command) || (UseGlobalCommands && GlobalConsoleCommands.TryGetValue(name, out command)))
            {
                try
                {
                    command.Execute(this, body);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                    PrintError($"An exception occurred while running the command: {exception.Message}");
                }
            }
            else
            {
                PrintError($"Unknown command: {name}");
            }
        }

        public BaseCommand[] GetCommands() => UseGlobalCommands 
            ? _consoleCommands.Values.Concat(GlobalConsoleCommands.Values).OrderBy(c => c.Name).ToArray() 
            : GetConsoleCommands();
        
        public BaseCommand[] GetConsoleCommands() => _consoleCommands.Values.OrderBy(c => c.Name).ToArray();

        public static BaseCommand[] GetGlobalCommands() => GlobalConsoleCommands.Values.OrderBy(c => c.Name).ToArray();

        public void AddConsoleCommand(BaseCommandWithParameters command)
        {
            _consoleCommands.Add(command.Name, command);
        }
        public void Print(string message)
        {
            OnPrintLine?.Invoke(message);
        }
        
        public void Print(Func<PrintBuilder, PrintBuilder> createMessage)
        {
            createMessage(_printBuilder + Format.TextColor);
            Print(_printBuilder.BuildStringAndClear());
        }
        
        public void Print(Func<PrintBuilder, PrintBuilder.PrintFormat, PrintBuilder> createMessage)
        {
            createMessage(_printBuilder + Format.TextColor, Format);
            Print(_printBuilder.BuildStringAndClear());
        }

        public void PrintWarning(string message) => Print(pb => pb + Format.WarningColor + message);

        public void PrintError(string message) => Print(pb => pb + Format.ErrorColor + message);

        private void OnUnityLogMessage(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    PrintError($"[Error] {condition}");
                    break;
                case LogType.Warning:
                    PrintWarning($"[Warning] {condition}");
                    break;
                case LogType.Log:
                    Print(p => p + "[Log] " + condition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        private static void LoadGlobalCommands()
        {
            if (_didLoadGlobalCommands)
                return;
            
            // Find commands
            var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .Where(t => t.IsClass &&
                                !t.IsAbstract &&
                                t.IsSubclassOf(typeof(BaseCommand)) &&
                                t.GetCustomAttributes(typeof(GlobalCommandAttribute), false).Length > 0 &&
                                t.GetConstructor(Type.EmptyTypes) != null));
            
            foreach (var commandType in commandTypes)
            {
                var command = (BaseCommand)Activator.CreateInstance(commandType);
                GlobalConsoleCommands.Add(command.Name, command);
            }
            
            // Find command attributes
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    .Where(m => m.GetParameters().Length == 0 || (m.GetParameters().Length == 2 && 
                                m.GetParameters()[0].ParameterType == typeof(Console) && 
                                m.GetParameters()[1].ParameterType == typeof(BaseCommandWithParameters.ParsedArguments)))
                    .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                    .ToArray());

            foreach (var method in methods)
            {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                var command = new Command(commandAttribute.Name, commandAttribute.Description, (console, args) =>
                {
                    var methodArgs = method.GetParameters().Length == 0
                        ? Array.Empty<object>()
                        : new object[] { console, args };
                    method.Invoke(null, methodArgs);
                });
                GlobalConsoleCommands.Add(command.Name, command);
            }

            _didLoadGlobalCommands = true;
        }
    }
}