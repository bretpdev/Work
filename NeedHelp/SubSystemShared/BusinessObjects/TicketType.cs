using System;

namespace SubSystemShared
{
    public class TicketType : IEquatable<TicketType>
    {
		/// <summary>
		/// Full-text description of the ticket type (e.g., "Facilities Request" or "Financial Adjustment Request").
		/// </summary>
        public string Description { get; set; }

		/// <summary>
		/// Abbreviated text for the ticket type (e.g., "FAC" or "FAR").
		/// </summary>
        public string Abbreviation { get; set; }

		public TicketType()
		{
			Description = "";
			Abbreviation = "";
		}

		public override string ToString()
		{
			return Description;
		}

		public static bool operator ==(TicketType a, TicketType b)
		{
			if (ReferenceEquals(a, b)) 
				return true; 
			if (a is null || b is null) 
				return false; 
            return a.Abbreviation == b.Abbreviation;
		}

		public static bool operator !=(TicketType a, TicketType b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			TicketType other = obj as TicketType;
			if (other == null) 
				return false; 
			return base.Equals(obj) && this.Abbreviation == other.Abbreviation;
		}

		public override int GetHashCode()
		{
			if (Abbreviation == null) { return "".GetHashCode(); }
			return Abbreviation.GetHashCode();
		}

		#region IEquatable<TicketType> Members
		
		public bool Equals(TicketType other)
		{
			if (Abbreviation == null || other == null) { return false; }
			return Abbreviation.Equals(other.Abbreviation);
		}

		#endregion
	}
}