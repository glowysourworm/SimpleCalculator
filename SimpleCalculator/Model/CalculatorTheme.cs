using System.Windows.Media;

namespace SimpleCalculator.Model
{
    public class CalculatorTheme
    {
        public Color TitleForegroundColor { get; set; }
        public Color TitleBackgroundColor { get; set; }
        public Color CodeForegroundColor { get; set; }
        public Color CodeErrorForegroundColor { get; set; }
        public Color PrimaryBackgroundColor { get; set; }
        public Color PrimaryForegroundColor { get; set; }
        public Color SecondaryBackgroundColor { get; set; }

        public CalculatorTheme()
        {
            this.TitleForegroundColor = Color.FromArgb(0xFF, 0x88, 0x88, 0x88);
            this.TitleBackgroundColor = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);
            this.CodeForegroundColor = Color.FromArgb(0xFF, 0x88, 0x88, 0x88);
            this.CodeErrorForegroundColor = Colors.Red;
            this.PrimaryBackgroundColor = Colors.White;
            this.PrimaryForegroundColor = Color.FromArgb(0xFF, 0x55, 0x55, 0x55);
            this.SecondaryBackgroundColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
        }
    }
}
