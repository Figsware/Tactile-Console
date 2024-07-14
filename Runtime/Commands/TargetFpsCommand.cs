using Tactile.Console.Parameters;
using UnityEngine;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class TargetFpsCommand : BaseCommandWithParameters<int>
    {
        public TargetFpsCommand() : base("fps", "Sets the target fps.", new IntegerParameter("fps", "The desired FPS", true))
        {
        }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
            Application.targetFrameRate = arguments.Arg1;
        }
    }
}