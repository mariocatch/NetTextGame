using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetTextGame
{
    internal enum StepType
    {
        Unknown,
        Comment,
        Output,
        Input,
        Chapter,
        Command,
        Flag
    }

    internal class Parser
    {
        private readonly string _path;

        public Parser(string path) => _path = path;

        internal IEnumerable<Step> ParseSteps()
        {
            var steps = new List<Step>();

            foreach (var line in File.ReadLines(_path).Select(s => s.Trim()))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var step = new Step { Text = line.Substring(1).Trim() };
                step.StepType = line[0] switch
                {
                    '~' => StepType.Comment,
                    '>' => StepType.Output,
                    '<' => StepType.Input,
                    '#' => StepType.Chapter,
                    '!' => StepType.Command,
                    '?' => StepType.Flag,
                    _ => throw new InvalidOperationException($"Unsupported line prefix of {line[0]} was used")
                };

                steps.Add(step);
            }

            return steps;
        }
    }
}
