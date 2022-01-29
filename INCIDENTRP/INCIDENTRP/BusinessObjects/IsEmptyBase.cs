namespace INCIDENTRP
{
    public abstract class IsEmptyBase
    {
        /// <summary>
        /// Returns true if all String and Boolean properties of this object are false/empty.
        /// </summary>
        public bool IsEmpty()
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    if (!string.IsNullOrEmpty((string)prop.GetValue(this)))
                        return false;
                if (prop.PropertyType == typeof(bool))
                    if ((bool)prop.GetValue(this))
                        return false;
            }
            return true;
        }
    }
}