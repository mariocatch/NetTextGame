using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetTextGame
{

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

                var text = line.Substring(1).Trim();
#pragma warning disable IDE0007 // Use implicit type
                Step step = line[0] switch
#pragma warning restore IDE0007 // Use implicit type
                {
                    '~' => new CommentStep(text),
                    '>' => new OutputStep(text),
                    '<' => new InputStep(text),
                    '#' => new ChapterStep(text),
                    '!' => new CommandStep(text),
                    '?' => new FlagStep(text),
                    _ => throw new InvalidOperationException($"Unsupported line prefix of {line[0]} was used")
                };

                steps.Add(step);
            }

            return steps;
        }
    }
}
