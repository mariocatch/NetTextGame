using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTextGame
{
    public class Engine
    {
        private readonly Dictionary<string, bool> _flags = new Dictionary<string, bool>();
        private readonly Dictionary<string, Input> _inputs = new Dictionary<string, Input>();
        private readonly IEnumerable<Step> _steps;

        internal Engine(IEnumerable<Step> steps) => _steps = steps;

        public void Run()
        {
            if (_steps == null || _steps.Count() == 0)
                throw new InvalidOperationException("No steps found in game data text file");

            foreach (var step in _steps)
            {
                string[] splitStepText = null;
                switch (step.StepType)
                {
                    case StepType.Output:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(step.Text);
                        Console.ResetColor();
                        break;
                    case StepType.Input:
                        splitStepText = step.Text.Split(';', StringSplitOptions.RemoveEmptyEntries);
                        var rawName = splitStepText[0];
                        var rawType = splitStepText[1];
                        var rawOptions = splitStepText.Length > 2 ? splitStepText[2] : null;
                        var splitRawOptions = rawOptions?.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        string rawInput = "";
                        int attempts = 0;
                        Type actualType = null;
                        while (attempts <= 5)
                        {
                            attempts += 1;

                            Console.Write("> ");
                            rawInput = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(rawInput) || rawInput == Environment.NewLine) continue;

                            if (splitRawOptions != null && splitRawOptions.Length > 0)
                            {
                                if (!splitRawOptions.Contains(rawInput)) continue;
                            }

                            actualType = rawType.ToUpper() switch
                            {
                                "NUMBER" => typeof(int),
                                "STRING" => typeof(string),
                                _ => throw new InvalidOperationException($"Unsupported input type of {rawType} was used")
                            };

                            try
                            {
                                _ = Convert.ChangeType(rawInput, actualType);
                            }
                            catch
                            {
                                continue;
                            }

                            break;
                        }

                        _inputs.Add(rawName, new Input
                        {
                            Name = rawName,
                            Type = actualType,
                            Value = rawInput
                        });
                        break;
                    case StepType.Chapter:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($"{Environment.NewLine}__ {step.Text} __{Environment.NewLine}");
                        Console.ResetColor();
                        break;
                    case StepType.Flag:
                        splitStepText = step.Text.Split('=', StringSplitOptions.RemoveEmptyEntries);
                        var flagName = splitStepText[0];
                        if (bool.TryParse(splitStepText[1], out var value)) _flags.Add(flagName, value);
                        else throw new InvalidOperationException($"Invalid flag value of {splitStepText[1]} was used");
                        break;
                    case StepType.Command:
                        splitStepText = step.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        var commandName = splitStepText[0];
                        break;
                }
            }
        }
    }

    internal class Input
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; }
    }
}
