using System;

namespace SubSystemShared
{
	public class KeyWordScope : IEquatable<KeyWordScope>
	{
		//Read-only properties to return the only valid values.
		public static KeyWordScope None { get { return new KeyWordScope("None"); } }
		public static KeyWordScope All { get { return new KeyWordScope("All"); } }
		public static KeyWordScope History { get { return new KeyWordScope("History"); } }
		public static KeyWordScope Issue { get { return new KeyWordScope("Issue"); } }
		public static KeyWordScope Subject { get { return new KeyWordScope("Subject"); } }

		private readonly string _scope;

		//Protected constructor prevents client code from creating an instance with an invalid value.
		protected KeyWordScope(string scope)
		{
			_scope = scope;
		}

		//Implement a bunch of methods to make the class convenient to use.
		public override string ToString()
		{
			return _scope;
		}

		public static bool operator ==(KeyWordScope a, KeyWordScope b)
		{
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null) 
				return false; 
			return a.ToString() == b.ToString();
		}

		public static bool operator !=(KeyWordScope a, KeyWordScope b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			KeyWordScope other = obj as KeyWordScope;
			if (other == null) 
				return false; 
			return base.Equals(obj) && this.ToString() == other.ToString();
		}

		public override int GetHashCode()
		{
			return _scope.GetHashCode();
		}

		#region IEquatable<KeywordScope> Members

		public bool Equals(KeyWordScope other)
		{
			return this.ToString() == other.ToString();
		}

		#endregion
	}
}