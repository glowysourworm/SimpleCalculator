using SimpleCalculator.Component;
using SimpleCalculator.Model.Interface;

namespace SimpleCalculator.Model
{
    public class Function : ISymbol
    {
        public string Symbol { get; }
        public SymbolType SymbolType { get; }

        public FunctionSignature Signature { get; private set; }
        public MathExpression Body { get; private set; }

        public Function(FunctionSignature signature, MathExpression bodyExpression)
        {
            this.Symbol = signature.Symbol;
            this.Signature = signature;
            this.Body = bodyExpression;
            this.SymbolType = SymbolType.Function;
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
            return this.Signature.ToString() + "=" + this.Body.ToString();
        }
    }
}
