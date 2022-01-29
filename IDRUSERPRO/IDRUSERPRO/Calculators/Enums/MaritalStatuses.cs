namespace IDRUSERPRO
{
    public enum MaritalStatuses
    {
        Single,
        Married,
        [EnumDescription("Married but separated")]
        MarriedSeparated,
        [EnumDescription("Married No Spouse Info")]
        MarriedNoSpouseInfo
    }
}