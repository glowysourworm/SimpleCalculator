using System.Text.RegularExpressions;

using SimpleCalculator.Model;

using SimpleWpf.Utilities;

namespace SimpleCalculator.Extension
{
    public static class MathStringExtension
    {
        private const string NON_ALPHA_CHARACTER = "[^a-zA-Z]";

        public static FunctionSignature? ReadAsFunctionSignature(this string signature)
        {
            // Matches:  f(x)  (1 time),  f(x  (0 times)
            //           f(xy) (1 time)
            //           f(x)g(y) (2 times)
            //           fine(xy) (1 time)
            //           fine(x,y) (1 time)
            //           fine(x,y,) (1 time)
            //
            var functionRegex = "[a-zA-Z]+\\([a-zA-Z|,]+\\)";

            var match = Regex.Match(signature, functionRegex);

            if (match.Success && match.Captures.Count == 1)
            {
                var symbol = new SubstringLocator(signature, CalculatorConfiguration.LeftParenthesis, false);

                var variables = signature.Split(new string[] {
                    symbol.GetSubString(),
                    CalculatorConfiguration.LeftParenthesis.ToString(),
                    CalculatorConfiguration.RightParenthesis.ToString(),
                    CalculatorConfiguration.FunctionVariableSeparator.ToString() },
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                if (variables.Length == 0)
                    return null;

                // NEW VARIABLE(S)
                var independentVariables = variables.Select(x => new Variable(x)).ToArray();

                return new FunctionSignature(new Variable(symbol.GetSubString()), independentVariables);
            }
            else
                return null;
        }

        /// <summary>
        /// Validates symbol expression for a function
        /// </summary>
        public static bool ValidateFunctionSymbol(this string symbolExpression)
        {
            // Empty or Null
            if (string.IsNullOrWhiteSpace(symbolExpression))
                return false;

            // White Space 
            if (symbolExpression.Contains(" "))
                return false;

            var containsParens = symbolExpression.Contains(CalculatorConfiguration.LeftParenthesis) ||
                                 symbolExpression.Contains(CalculatorConfiguration.RightParenthesis);

            // Argument List
            if (containsParens)
            {
                // Remove Argument List
                var symbol = new SubstringLocator(symbolExpression, CalculatorConfiguration.LeftParenthesis, false);

                // Non Alphabetical Character(s)
                if (StringHelpers.RegexMatchIC(NON_ALPHA_CHARACTER, symbol.GetSubString()))
                    return false;

                return true;
            }

            // No Argument List
            else
            {
                // Non Alphabetical Character(s)
                if (StringHelpers.RegexMatchIC(NON_ALPHA_CHARACTER, symbolExpression))
                    return false;

                return true;
            }
        }

        public static bool ValidateSymbol(this string symbol)
        {
            // Empty or Null
            if (string.IsNullOrWhiteSpace(symbol))
                return false;

            // Non Alphabetical Character(s)
            if (StringHelpers.RegexMatchIC(NON_ALPHA_CHARACTER, symbol))
                return false;

            // White Space 
            if (symbol.Contains(" "))
                return false;

            return true;
        }

        public static IEnumerable<string> RemoveOperatorsAndParens(this string statement, CalculatorConfiguration configuration)
        {
            var removes = configuration.SymbolTable.Operators.Select(x => x.Symbol).ToList();

            removes.Add(CalculatorConfiguration.LeftParenthesis.ToString());
            removes.Add(CalculatorConfiguration.RightParenthesis.ToString());


            return statement.Split(removes.ToArray(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public static string RemoveAllOuterParens(this string statement)
        {
            if (string.IsNullOrWhiteSpace(statement))
                throw new ArgumentException("Empty string. Cannot remove outer parens");

            while (statement.StartsWith(CalculatorConfiguration.LeftParenthesis) &&
                   statement.EndsWith(CalculatorConfiguration.RightParenthesis))
            {
                statement = statement.Substring(1, statement.Length - 2);
            }

            return statement;
        }

        public static string SurroundByParens(this string statement)
        {
            return CalculatorConfiguration.LeftParenthesis + statement + CalculatorConfiguration.RightParenthesis;
        }
    }
}
