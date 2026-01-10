using SimpleCalculator.Model;
using SimpleCalculator.Model.Calculation;

namespace SimpleCalculator.Component
{
    /// <summary>
    /// Some simple static methods for handling expressions
    /// </summary>
    public static class ExpressionCalculator
    {
        /// <summary>
        /// Returns locations of outer-most sub-statements enclosed by parentheses
        /// </summary>
        public static ParenthesesData GetOutermostParens(string statement)
        {
            var outermostStatements = new List<SubstringLocator>();
            var parenStack = new Stack<int>();

            for (int index = 0; index < statement.Length; index++)
            {
                // Start
                if (statement[index] == CalculatorConfiguration.LeftParenthesis)
                {
                    parenStack.Push(index);
                }

                // End
                else if (statement[index] == CalculatorConfiguration.RightParenthesis)
                {
                    var outerMost = parenStack.Count == 1;
                    var startIndex = parenStack.Pop();

                    if (outerMost)
                    {
                        // Treat this as a sub-statement
                        outermostStatements.Add(new SubstringLocator(statement, startIndex, index - startIndex + 1));
                    }
                }
            }

            return new ParenthesesData()
            {
                Outermost = outermostStatements
            };
        }

        /// <summary>
        /// Returns next operator location, respecting paren'd statements, and order of operations.
        /// </summary>
        public static int LocateNextOperator(CalculatorConfiguration configuration,
                                              string statement,
                                              List<SubstringLocator> outermostParenStatements,
                                              out Operator? resultOperator)
        {
            resultOperator = null;

            // Split on operators
            var subStatementPieces = statement.Split(configuration.SymbolTable
                                                                  .Operators
                                                                  .Select(x => x.Symbol)
                                                                  .ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            var operators = new Dictionary<int, Operator>();

            for (int index = 0; index < statement.Length; index++)
            {
                // Operator contained in sub-statement
                if (outermostParenStatements.Any(x => x.ContainsIndex(index)))
                    continue;

                var nextOperator = configuration.SymbolTable.Operators.FirstOrDefault(x => x.Symbol == statement[index].ToString());

                // Keep these to chose next in order of operations
                if (nextOperator != null)
                {
                    operators.Add(index, nextOperator);
                }
            }

            if (!operators.Any())
                return -1;

            // ORDER OF OPERATIONS!
            var resultIndex = operators.OrderBy(x => x.Value.Order).First().Key;

            // Set operator output parameter
            resultOperator = operators[resultIndex];

            return resultIndex;
        }
    }
}
