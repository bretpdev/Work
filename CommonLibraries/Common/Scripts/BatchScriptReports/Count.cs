using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
	public class Count : IEquatable<Count>
	{
		
		public event ValueChanged NewValue;
		public delegate void ValueChanged(Count c);

		private int EojCount;

		public Count(int initalValue)
		{
			EojCount = initalValue;
		}

		public void Increment()
		{
			EojCount++;
			if (NewValue != null)
			{
				NewValue(this);
			}
		}

        public static Count operator ++(Count c)
        {
            c.Increment();
            return c;
        }

        public int Value
        {
            get
            {
                return EojCount;
            }
        }

        public static implicit operator int(Count c)
        {
            return (int)c.Value;
        }

		public override bool Equals(object obj)
		{
			Count other = (Count)obj;
			if (other == null)
			{
				return false;
			}
			else
			{
				return base.Equals(obj) && EojCount == other.EojCount;
			}
		}

		public override string ToString()
		{
			return EojCount.ToString();
		}

		#region IEquatable<T> Members

		public bool Equals(Count other)
		{
			return EojCount.Equals(other.EojCount);
		}

		public override int GetHashCode()
		{
			return EojCount.GetHashCode();
		}

		#endregion
	}
}
