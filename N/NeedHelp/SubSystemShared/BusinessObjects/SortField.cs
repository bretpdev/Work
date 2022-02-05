using System;

namespace SubSystemShared
{
	public class SortField : IEquatable<SortField>
	{
		//Read-only properties to return the only valid values.
		public static SortField None { get { return new SortField(""); } }
		public static SortField LastUpdateDate { get { return new SortField("Last Update Date"); } }
		public static SortField Priority { get { return new SortField("Priority"); } }
		public static SortField Status { get { return new SortField("Status"); } }

		private readonly string _field;

		//Protected constructor prevents client code from creating an instance with an invalid value.
		protected SortField(string field)
		{
			_field = field;
		}

		//Implement a bunch of methods to make the class convenient to use.
		public override string ToString()
		{
			return _field;
		}

		public static bool operator ==(SortField a, SortField b)
		{
			if (ReferenceEquals(a, b)) 
				return true; 
			if (a is null || b is null) 
				return false; 
			return a.ToString() == b.ToString();
		}

		public static bool operator !=(SortField a, SortField b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			SortField other = obj as SortField;
			if (other == null) 
				return false; 
			return base.Equals(obj) && this.ToString() == other.ToString();
		}

		public override int GetHashCode()
		{
			return _field.GetHashCode();
		}

		#region IEquatable<SortField> Members

		public bool Equals(SortField other)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}