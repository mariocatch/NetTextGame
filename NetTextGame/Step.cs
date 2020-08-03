namespace NetTextGame
{
    internal abstract class Step
    {
        public string Text { get; }

        protected Step(string text) => Text = text;
    }

    internal class CommentStep : Step
    {
        public CommentStep(string text) : base(text)
        {
        }
    }

    internal class OutputStep : Step
    {
        public OutputStep(string text) : base(text)
        {
        }
    }

    internal class InputStep : Step
    {
        public InputStep(string text) : base(text)
        {
        }
    }

    internal class ChapterStep : Step
    {
        public ChapterStep(string text) : base(text)
        {
        }
    }

    internal class CommandStep : Step
    {
        public CommandStep(string text) : base(text)
        {
        }
    }

    internal class FlagStep : Step
    {
        public FlagStep(string text) : base(text)
        {
        }
    }
}
