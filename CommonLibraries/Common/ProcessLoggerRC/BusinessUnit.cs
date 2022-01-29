namespace Uheaa.Common.ProcessLoggerRC
{
    public class BusinessUnit
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                BusinessUnit objBus = (BusinessUnit)obj;
                return objBus.ID == this.ID && objBus.Name == this.Name;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}