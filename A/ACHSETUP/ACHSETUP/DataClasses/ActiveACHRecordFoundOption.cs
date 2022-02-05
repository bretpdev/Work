namespace ACHSETUP
{
    public class ActiveACHRecordFoundOption
    {
        public enum Option
        {
            Add,
            Change,
            Stop
        }

        public Option SelectedOption { get; set; }
    }
}