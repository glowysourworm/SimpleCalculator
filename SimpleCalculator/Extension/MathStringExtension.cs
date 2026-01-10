using SimpleCalculator.Model;

namespace SimpleCalculator.Extension
{
    public static class MathStringExtension
    {
        public static IEnumerable<string> RemoveOperatorsAndParens(this string statement, CalculatorConfiguration configuration)
        {
            var removes = configuration.SymbolTable.Operators.Select(x => x.Symbol).ToList();

            removes.Add(CalculatorConfiguration.LeftParenthesis.ToString());
            removes.Add(CalculatorConfiguration.RightParenthesis.ToString());


            return statement.Split(removes.ToArray(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public static string SurroundByParens(this string statement)
        {
            return CalculatorConfiguration.LeftParenthesis + statement + CalculatorConfiguration.RightParenthesis;
        }
    }
}
