using SimpleCalculator.Component;

namespace SimpleCalculator.Model.Calculation
{
    public class SemanticTree
    {
        readonly private SemanticTreeNode _root;

        /// <summary>
        /// Initiailizes a semantic tree with an already parsed, completed,
        /// node tree which is ready to be evaluated.
        /// </summary>
        public SemanticTree(SemanticTreeNode root)
        {
            _root = root;
        }

        public SemanticTreeResult Execute(SymbolTable symbolTable)
        {
            var result = _root.Evaluate(new CalculationData(symbolTable));

            return new SemanticTreeResult(result.ErrorMessage == null ?
                                            SemanticTreeResultStatus.Success :
                                            SemanticTreeResultStatus.ExecutionError,
                                          result.ErrorMessage ?? string.Empty,
                                          result.Value);
        }

        public SemanticTreeNode GetRoot()
        {
            return _root;
        }
    }
}
