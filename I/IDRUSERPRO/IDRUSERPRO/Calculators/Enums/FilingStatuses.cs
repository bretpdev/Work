namespace IDRUSERPRO
{
    public enum FilingStatuses
    {
        Single,
        [EnumDescription("Married Filing Jointly")]
        MarriedFilingJointly,
        [EnumDescription("Married Filing Separately")]
        MarriedFilingSeparately,
        [EnumDescription("Head of Household")]
        HeadOfHousehold,
        [EnumDescription("Qualifying Widow(er) with Dependent Child")]
        QualifyingWidower,
        [EnumDescription("Not Applicable/No Tax Documents")]
        NotApplicable
    }
}