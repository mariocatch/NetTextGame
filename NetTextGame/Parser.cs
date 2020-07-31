using System.Collections.Generic;
using System.IO;

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

            foreach (var line in File.ReadLines(_path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var step = new Step { Text = line.Substring(1) };

                for (int i = 0; i < line.Length; i++)
                {
                    var c = line[i];

                    if (c == '~')
                    {
                        step.StepType = StepType.Comment;
                        break;
                    }
                    else if (c == '>')
                        step.StepType = StepType.Output;
                    else if (c == '<')
                        step.StepType = StepType.Input;
                    else if (c == '#')
                        step.StepType = StepType.Chapter;
                    else if (c == '!')
                        step.StepType = StepType.Command;
                    else if (c == '?')
                        step.StepType = StepType.Flag;
                }

                steps.Add(step);
            }

            return steps;
        }
    }
}
