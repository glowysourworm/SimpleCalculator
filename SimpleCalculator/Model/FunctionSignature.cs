using SimpleCalculator.Component;
using SimpleCalculator.Model.Interface;

using SimpleWpf.Extensions.Collection;

namespace SimpleCalculator.Model
{
    public class FunctionSignature : ExpressionBase, ISymbol
    {
        private readonly Variable _dependentVariable;
        private readonly Variable[] _independentVariables;

        public IEnumerable<Variable> IndependentVariables
        {
            get { return _independentVariables; }
        }

        /// <summary>
        /// Dependent variable for the function, which is treated like a variable; but defined in the
        /// symbol table as a function. The "Symbol" is the key to the symbol table, for the function.
        /// </summary>
        public Variable DependentVariable { get { return _dependentVariable; } }

        /// <summary>
        /// Symbol (not signature!) of the function:  f(x) (symbol is) f.
        /// </summary>
        public string Symbol { get { return _dependentVariable.Symbol; } }

        public SymbolType SymbolType { get; }

        public FunctionSignature(Variable dependentVariable, Variable[] independentVariables)
            : base(CreateSignature(dependentVariable, independentVariables))
        {
            _independentVariables = independentVariables;
            _dependentVariable = dependentVariable;

            this.SymbolType = SymbolType.Function;
        }

        private static string CreateSignature(Variable dependentVariable, Variable[] independentVariables)
        {
            return dependentVariable.Symbol +
                CalculatorConfiguration.LeftParenthesis +
                independentVariables.Join(",", x => x.Symbol) +
                CalculatorConfiguration.RightParenthesis;
        }

        public override bool Equals(object? obj)
        {
            return this.Symbol.Equals(((ISymbol)obj).Symbol);
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }

        public override string ToString()
        {
            return this.Symbol;
        }
    }
}
