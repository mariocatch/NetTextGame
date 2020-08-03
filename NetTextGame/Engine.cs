using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTextGame
{
    internal enum ParseMode
    {
        LineByLine,
        Switch,
        SkipToEndIf
    }

    public class Engine
    {
        private readonly Dictionary<string, bool> _flags = new Dictionary<string, bool>();
        private readonly Dictionary<string, Input> _inputs = new Dictionary<string, Input>();
        private readonly List<string> _activeEffects = new List<string>();
        private int _experience;
        private ParseMode _parseMode = ParseMode.LineByLine;
        private bool _onCorrectCase;
        private string _activeSwitch;
        private readonly IEnumerable<Step> _steps;

        internal Engine(IEnumerable<Step> steps) => _steps = steps;

        public void Run()
        {
            if (_steps == null || _steps.Count() == 0)
                throw new InvalidOperationException("No steps found in game data text file");

            foreach (var step in _steps)
            {
                if (_parseMode == ParseMode.SkipToEndIf)
                {
                    if (!(step is CommandStep) || step.Text.ToUpper() != "ENDIF") continue;
                }

                string[] splitStepText = null;
                switch (step)
                {
                    case OutputStep:
                        if (_parseMode != ParseMode.LineByLine &&
                            (_parseMode != ParseMode.Switch || !_onCorrectCase))
                        {
                            continue;
                        }

                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(step.Text);
                        Console.ResetColor();
                        break;
                    case InputStep:
                        if (_parseMode != ParseMode.LineByLine &&
                            (_parseMode != ParseMode.Switch || !_onCorrectCase))
                        {
                            continue;
                        }
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
                    case ChapterStep:
                        if (_parseMode != ParseMode.LineByLine &&
                            (_parseMode != ParseMode.Switch || !_onCorrectCase))
                        {
                            continue;
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($"{Environment.NewLine}__ {step.Text} __{Environment.NewLine}");
                        Console.ResetColor();
                        break;
                    case FlagStep:
                        if (_parseMode != ParseMode.LineByLine &&
                            (_parseMode != ParseMode.Switch || !_onCorrectCase))
                        {
                            continue;
                        }
                        splitStepText = step.Text.Split('=', StringSplitOptions.RemoveEmptyEntries);
                        var flagName = splitStepText[0];
                        if (bool.TryParse(splitStepText[1], out var value)) _flags.Add(flagName, value);
                        else throw new InvalidOperationException($"Invalid flag value of {splitStepText[1]} was used");
                        break;
                    case CommandStep:
                        splitStepText = step.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        var commandName = splitStepText[0];
                        string commandParameter;
                        switch (commandName.ToUpper())
                        {
                            case "SWITCH":
                                commandParameter = splitStepText[1];
                                _activeSwitch = commandParameter;
                                _parseMode = ParseMode.Switch;
                                break;
                            case "ENDSWITCH":
                                _parseMode = ParseMode.LineByLine;
                                break;
                            case "CASE":
                                commandParameter = splitStepText[1];
                                _onCorrectCase = commandParameter == _inputs[_activeSwitch].Value;
                                break;
                            case "EFFECT":
                                if (_parseMode != ParseMode.LineByLine &&
                                    (_parseMode != ParseMode.Switch || !_onCorrectCase))
                                {
                                    continue;
                                }
                                var effectName = splitStepText[1];
                                _activeEffects.Add(effectName);
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"You gain effect: {effectName}");
                                Console.ResetColor();
                                break;
                            case "GIVE":
                                var giveName = splitStepText[1];
                                if (giveName == "experience")
                                {
                                    var amount = int.Parse(splitStepText[2]);
                                    _experience += amount;
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine($"You gain {amount} experience");
                                    Console.ResetColor();
                                }
                                break;
                            case "IF":
                                commandParameter = splitStepText[1];
                                var splitCommandParameter = commandParameter.Split("==", StringSplitOptions.RemoveEmptyEntries);
                                // get flag and bool
                                // check flags for flag
                                // if doesnt exist skip to endif
                                // if exists check value
                                // if value doesnt match skip to endif
                                var ifFlag = splitCommandParameter[0];
                                var ifValue = bool.Parse(splitCommandParameter[1]);
                                if (!_flags.ContainsKey(ifFlag) || _flags[ifFlag] != ifValue)
                                {
                                    _parseMode = ParseMode.SkipToEndIf;
                                }
                                break;
                            case "endif":
                                _parseMode = ParseMode.LineByLine;
                                break;
                        }
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
