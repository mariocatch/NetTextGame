//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text.Json;

//namespace NetTextGame
//{
//    public class Engine
//    {
//        private readonly GameData _gameData;
//        private readonly Dictionary<string, string> _answers = new Dictionary<string, string>();

//        public Engine(string path)
//        {
//            var json = File.ReadAllText(path);
//            _gameData = JsonSerializer.Deserialize<GameData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//        }

//        public void StartGame()
//        {
//            foreach (var question in _gameData.Questions)
//            {
//                AskQuestion(question);
//                GetAnswer(question);
//            }
//        }

//        private void AskQuestion(Question question, ConsoleColor color = ConsoleColor.DarkGreen, bool reset = true)
//        {
//            Console.ForegroundColor = color;
//            Console.WriteLine(question.Text);
//            if (reset) Console.ResetColor();
//        }

//        private void GetAnswer(Question question, ConsoleColor color = ConsoleColor.DarkRed, bool reset = true)
//        {
//            Console.ForegroundColor = color;
//            Console.Write('\t');
//            _answers[question.Answer] = Console.ReadLine();
//            if (reset) Console.ResetColor();
//        }
//    }

//    public class GameData
//    {
//        public List<Variable> Variables { get; set; }
//        public List<Question> Questions { get; set; }
//    }

//    public class Variable
//    {
//        public string Name { get; set; }
//        public string Type { get; set; }
//    }

//    public class Question
//    {
//        public string Text { get; set; }
//        public string Answer { get; set; }
//    }
//}
