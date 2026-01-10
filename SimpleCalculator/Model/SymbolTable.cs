using SimpleCalculator.Model.MathExpression;

namespace SimpleCalculator.Model
{
    public class SymbolTable
    {
        private readonly Dictionary<Constant, ValueExpression> _constants;
        private readonly Dictionary<Variable, ValueExpression> _variables;
        private readonly Dictionary<Function, ValueExpression> _functions;
        private readonly Dictionary<Operator, Expression> _operators;

        public IEnumerable<Operator> Operators
        {
            get { return _operators.Keys; }
        }
        public IEnumerable<Variable> Variables
        {
            get { return _variables.Keys; }
        }
        public IEnumerable<Function> Functions
        {
            get { return _functions.Keys; }
        }
        public IEnumerable<Constant> Constants
        {
            get { return _constants.Keys; }
        }

        public SymbolTable()
        {
            _constants = new Dictionary<Constant, ValueExpression>();
            _variables = new Dictionary<Variable, ValueExpression>();
            _functions = new Dictionary<Function, ValueExpression>();
            _operators = new Dictionary<Operator, Expression>();
        }

        public void Add(Constant symbol, ValueExpression symbolValue)
        {
            _constants.Add(symbol, symbolValue);
        }
        public void Add(Variable symbol, ValueExpression symbolValue)
        {
            _variables.Add(symbol, symbolValue);
        }
        public void Add(Function symbol, ValueExpression symbolValue)
        {
            _functions.Add(symbol, symbolValue);
        }
        public void Add(Operator symbol, Expression symbolValue)
        {
            _operators.Add(symbol, symbolValue);
        }

        public void Remove(Constant symbol)
        {
            _constants.Remove(symbol);
        }
        public void Remove(Variable symbol)
        {
            _variables.Remove(symbol);
        }
        public void Remove(Function symbol)
        {
            _functions.Remove(symbol);
        }
        public void Remove(Operator symbol)
        {
            _operators.Remove(symbol);
        }

        public ValueExpression GetValue(Constant symbol)
        {
            return _constants[symbol];
        }
        public ValueExpression GetValue(Variable symbol)
        {
            return _variables[symbol];
        }
        public ValueExpression GetValue(Function symbol)
        {
            return _functions[symbol];
        }
        public Expression GetValue(Operator symbol)
        {
            return _operators[symbol];
        }

        public bool IsDefined(Constant symbol)
        {
            return _constants.ContainsKey(symbol);
        }
        public bool IsDefined(Variable symbol)
        {
            return _variables.ContainsKey(symbol);
        }
        public bool IsDefined(Function symbol)
        {
            return _functions.ContainsKey(symbol);
        }
        public bool IsDefined(Operator symbol)
        {
            return _operators.ContainsKey(symbol);
        }
        public bool IsDefined(string symbol)
        {
            return _constants.Keys.Any(x => x.Symbol == symbol) ||
                   _variables.Keys.Any(x => x.Symbol == symbol) ||
                   _functions.Keys.Any(x => x.Symbol == symbol) ||
                   _operators.Keys.Any(x => x.Symbol == symbol);
        }
    }
}
