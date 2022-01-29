
namespace Uheaa.Common
{
    public class ValidPercent : Percent
    {
        protected bool Validate(decimal percentage)
        {
            if (percentage < 0 || percentage > 1)
                throw new InvalidPercentageException(percentage);
            return true;
        }
        public ValidPercent(decimal percentage) : base(percentage) { }
        public ValidPercent(int percent): base(percent) { }
        public ValidPercent(decimal numerator, decimal denominator) : base(numerator, denominator) { }
        public static implicit operator ValidPercent(decimal p)
        {
            return new ValidPercent(p);
        }
        public static implicit operator ValidPercent(int i)
        {
            return new ValidPercent(i);
        }
    }
    public class Percent
    {
        #region Properties
        decimal value;
        public decimal DecimalValue
        {
            get { return value; }
        }
        public float FloatValue
        {
            get { return (float)value; }
        }
        public int Value
        {
            get { return (int)(value * 100); }
        }
        #endregion

        #region Constructors
        public Percent(decimal percentage)
        {
            value = percentage;
        }
        public Percent(int percent): this(percent / 100m) { }
        public Percent(decimal numerator, decimal denominator) : this(numerator / denominator) { }
        #endregion

        #region Implicit Conversions
        public static explicit operator decimal(Percent p)
        {
            return p.DecimalValue;
        }
        public static explicit operator float(Percent p)
        {
            return p.FloatValue;
        }
        public static explicit operator int(Percent p)
        {
            return p.Value;
        }
        public static implicit operator Percent(decimal p)
        {
            return new Percent(p);
        }
        public static implicit operator Percent(int i)
        {
            return new Percent(i);
        }
        #endregion

        #region Overloaded Operators
        public static int operator *(Percent p, int i)
        {
            return (int)(p.DecimalValue * i);
        }
        public static Percent operator *(Percent one, Percent two)
        {
            return new Percent(one.DecimalValue * two.DecimalValue);
        }
        public static Percent operator /(Percent one, Percent two)
        {
            return new Percent(one.DecimalValue / two.DecimalValue);
        }
        public static Percent operator +(Percent one, Percent two)
        {
            return new Percent(one.DecimalValue + two.DecimalValue);
        }
        public static Percent operator -(Percent one, Percent two)
        {
            return new Percent(one.DecimalValue - two.DecimalValue);
        }
        public static bool operator >(Percent one, Percent two)
        {
            return one.DecimalValue > two.DecimalValue;
        }
        public static bool operator >=(Percent one, Percent two)
        {
            return one.DecimalValue >= two.DecimalValue;
        }
        public static bool operator <(Percent one, Percent two)
        {
            return one.DecimalValue < two.DecimalValue;
        }
        public static bool operator <=(Percent one, Percent two)
        {
            return one.DecimalValue <= two.DecimalValue;
        }
        public override bool Equals(object obj)
        {
            if (obj is Percent)
                return ((Percent)obj).DecimalValue == this.DecimalValue;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return DecimalValue.GetHashCode();
        }
        #endregion

        public Percent Invert()
        {
            return new Percent(1 - value);
        }

        public override string ToString()
        {
            return DecimalValue.ToPercent();
        }
    }
}