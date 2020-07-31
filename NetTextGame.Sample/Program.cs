using System;

namespace NetTextGame.Sample
{
    internal class Program
    {
        private static void Main(string[] args) => EngineBuilder.Create("SampleGameData.txt").Run();
    }
}
