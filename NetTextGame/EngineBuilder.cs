namespace NetTextGame
{
    public static class EngineBuilder
    {
        public static Engine Create(string path)
        {
            var steps = new Parser(path).ParseSteps();
            return new Engine(steps);
        }
    }
}
