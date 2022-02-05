using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    /// <summary>
    /// Calculated results for each type of Plan.
    /// </summary>
    public class LpcResults
    {
        public PlanResult LowestSuccessfulPlan
        {
            get
            {
                return SuccessfulPlansByMonthlyInstallmentDescending.LastOrDefault();
            }
        }
        public IEnumerable<PlanResult> SuccessfulPlansByMonthlyInstallmentDescending
        {
            get
            {
                var allPlans = planProperties.Select(o => (PlanResult)o.Value.GetMethod.Invoke(this, new object[] { }));
                return allPlans.Where(o => o.Status == ResultStatus.Successful).OrderByDescending(o => o.MonthlyInstallment);
            }
        }
        public PlanResult Ibr { get; set; }
        public PlanResult NewIbr { get; set; }
        public IndicatorsResult EligibilityIndicators { get; set; }
        public LpcInput Input { get; set; }

        public void SetPlanResults(RepaymentPlans repaymentPlan, PlanResult planResults)
        {
            planResults.Parent = this;
            planProperties[repaymentPlan].SetMethod.Invoke(this, new object[] { planResults });
        }
        static Dictionary<RepaymentPlans, PropertyInfo> planProperties = new Dictionary<RepaymentPlans, PropertyInfo>();
        static LpcResults()
        {
            var plans = Enum.GetNames(typeof(RepaymentPlans)).Select(o => o.ToLower());
            foreach (var prop in typeof(LpcResults).GetProperties())
            {
                var matchingPlan = plans.SingleOrDefault(o => o == prop.Name.ToLower());
                if (matchingPlan != null)
                {
                    var planEnum = (RepaymentPlans)Enum.Parse(typeof(RepaymentPlans), matchingPlan, true);
                    planProperties[planEnum] = prop;
                }
            }
        }
        public class PlanResult
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ResultStatus Status { get; set; } = ResultStatus.StillCalculating;
            [JsonConverter(typeof(DecimalJsonConverter))]
            public decimal MonthlyInstallment { get; set; }
            public string ErrorMessage { get; set; }
            [JsonIgnore]
            public LpcResults Parent { get; set; }
            [JsonIgnore]
            public bool IsLowestSuccessfulPlan
            {
                get
                {
                    if (Parent == null)
                        return false;
                    return Parent.LowestSuccessfulPlan == this;
                }
            }
            class DecimalJsonConverter : JsonConverter
            {
                public override bool CanConvert(Type objectType)
                {
                    return objectType == typeof(decimal);
                }
                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }
                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    writer.WriteRawValue(((decimal)value).ToString("F2", CultureInfo.InvariantCulture));
                }
            }
        }
        public enum ResultStatus
        {
            StillCalculating,
            Unsuccessful,
            Successful,
        }
    }
}
