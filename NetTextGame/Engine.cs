using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTextGame
{
    public class Engine
    {
        private readonly IEnumerable<Step> _steps;

        internal Engine(IEnumerable<Step> steps) => _steps = steps;

        public void Run()
        {
            if (_steps == null || _steps.Count() == 0)
                throw new InvalidOperationException("No steps found in game data text file");

            foreach (var step in _steps)
            {
                switch (step.StepType)
                {
                    case StepType.Output:
                        Console.WriteLine(step.Text);
                        break;
                }
            }
        }
    }
}
