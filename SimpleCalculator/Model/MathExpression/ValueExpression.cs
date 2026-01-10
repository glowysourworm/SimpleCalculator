using SimpleCalculator.Component;

namespace SimpleCalculator.Model.MathExpression
{
    /// <summary>
    /// Class that represents a value expression assigned to some symbol. This could be a: number, constant, 
    /// function, or variable. The numeric value must evaluated. For number types, the value is parsed during
    /// the constructor.
    /// </summary>
    public class ValueExpression : ExpressionBase
    {
        /// <summary>
        /// Type of symbol represented by the expression.
        /// </summary>
        public ValueExpressionType Type { get; private set; }

        // Numeric value used as the primary value expression's value. This is essentially the symbol's cached value.
        private double _numericValue;
        private bool _hasNumericValue;

        public ValueExpression(string expression, ValueExpressionType type) : base(expression)
        {
            this.Type = type;

            _numericValue = 0;
            _hasNumericValue = false;
        }
        public ValueExpression(string expression, ValueExpressionType type, double numericValue) : base(expression)
        {
            this.Type = type;

            _numericValue = numericValue;
            _hasNumericValue = true;
        }

        public bool HasNumericValue()
        {
            return _hasNumericValue;
        }

        public double GetNumericValue()
        {
            if (this.Type == ValueExpressionType.Evaluated)
                throw new Exception("Cannot get numeric value of an evaluated ValueExpression! It must be assembled using a SemanticTree");

            return _numericValue;
        }

        public void SetNumericValue(double value)
        {
            if (this.Type == ValueExpressionType.Evaluated)
                throw new Exception("Cannot set numeric value of an evaluated ValueExpression! It must be assembled using a SemanticTree");

            if (!_hasNumericValue)
                throw new Exception("Numeric value has not yet been set for this value expression");

            _numericValue = value;
            _hasNumericValue = true;
        }

        public void ClearValue()
        {
            _numericValue = 0;
            _hasNumericValue = false;
        }
    }
}
