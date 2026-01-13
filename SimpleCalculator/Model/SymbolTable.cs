using SimpleCalculator.Model.Interface;

using SimpleWpf.Extensions.Collection;

namespace SimpleCalculator.Model
{
    public class SymbolTable
    {
        private readonly Dictionary<Constant, double> _constants;
        private readonly Dictionary<Variable, double> _variables;
        private readonly Dictionary<Function, Function> _functions;
        private readonly Dictionary<Operator, Operator> _operators;

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
            _constants = new Dictionary<Constant, double>();
            _variables = new Dictionary<Variable, double>();
            _functions = new Dictionary<Function, Function>();
            _operators = new Dictionary<Operator, Operator>();
        }

        public void Add(Constant symbol, double symbolValue)
        {
            _constants.Add(symbol, symbolValue);
        }
        public void Add(Variable symbol, double symbolValue)
        {
            _variables.Add(symbol, symbolValue);
        }
        public void Add(Function symbol, Function symbolValue)
        {
            _functions.Add(symbol, symbolValue);
        }
        public void Add(Operator symbol, Operator symbolValue)
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
        public void Remove(string symbol)
        {
            if (_constants.Any(x => x.Key.Symbol == symbol))
                _constants.Remove(x => x.Key.Symbol == symbol);

            if (_variables.Any(x => x.Key.Symbol == symbol))
                _variables.Remove(x => x.Key.Symbol == symbol);

            if (_functions.Any(x => x.Key.Symbol == symbol))
                _functions.Remove(x => x.Key.Symbol == symbol);

            if (_operators.Any(x => x.Key.Symbol == symbol))
                _operators.Remove(x => x.Key.Symbol == symbol);
        }

        public Operator GetAssignmentOperator()
        {
            return _operators.Keys.Single(x => x.Type == OperatorType.Assignment);
        }

        public ISymbol Get(string symbol)
        {
            if (!IsDefined(symbol))
                throw new Exception("Undefined symbol:  " + symbol);

            switch (GetSymbolType(symbol))
            {
                case SymbolType.Constant:
                    return _constants.First(x => x.Key.Symbol == symbol).Key;
                case SymbolType.Variable:
                    return _variables.First(x => x.Key.Symbol == symbol).Key;
                case SymbolType.Function:
                    return _functions.First(x => x.Key.Symbol == symbol).Key;
                case SymbolType.Operator:
                    return _operators.First(x => x.Key.Symbol == symbol).Key;
                default:
                    throw new Exception("Unhandled symbol type");
            }
        }

        public double GetValue(Constant symbol)
        {
            return _constants[symbol];
        }
        public double GetValue(Variable symbol)
        {
            return _variables[symbol];
        }
        public Function GetValue(Function symbol)
        {
            return _functions[symbol];
        }
        public Operator GetValue(Operator symbol)
        {
            return _operators[symbol];
        }

        public void SetValue(Constant symbol, double value)
        {
            _constants[symbol] = value;
        }
        public void SetValue(Variable symbol, double value)
        {
            _variables[symbol] = value;
        }
        public void SetValue(Operator oldOperator, Operator newOperator)
        {
            _operators[oldOperator] = newOperator;
        }
        public void SetValue(Function oldFunction, Function newFunction)
        {
            _functions[oldFunction] = newFunction;
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
        public bool IsDefined(ISymbol symbol)
        {
            return IsDefined(symbol.Symbol);
        }
        public bool IsDefined(string symbol)
        {
            return _constants.Keys.Any(x => x.Symbol == symbol) ||
                   _variables.Keys.Any(x => x.Symbol == symbol) ||
                   _functions.Keys.Any(x => x.Symbol == symbol) ||
                   _operators.Keys.Any(x => x.Symbol == symbol);
        }

        public SymbolType GetSymbolType(ISymbol symbol)
        {
            return GetSymbolType(symbol.Symbol);
        }
        public SymbolType GetSymbolType(string symbol)
        {
            if (_constants.Keys.Any(x => x.Symbol == symbol))
                return SymbolType.Constant;

            else if (_variables.Keys.Any(x => x.Symbol == symbol))
                return SymbolType.Variable;

            else if (_functions.Keys.Any(x => x.Symbol == symbol))
                return SymbolType.Function;

            else if (_operators.Keys.Any(x => x.Symbol == symbol))
                return SymbolType.Operator;

            else
                throw new Exception("Undefined symbol:  " + symbol);
        }
    }
}
