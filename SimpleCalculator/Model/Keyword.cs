namespace SimpleCalculator.Model
{
    public class Keyword
    {
        public string Name { get; set; }
        public string HelpText { get; set; }
        public KeywordType Type { get; set; }

        public Keyword()
        {
            this.Name = string.Empty;
            this.HelpText = string.Empty;
            this.Type = KeywordType.Help;
        }
        public Keyword(string name, KeywordType type, string helpText)
        {
            this.Name = name;
            this.HelpText = helpText;
            this.Type = type;
        }
    }
}
