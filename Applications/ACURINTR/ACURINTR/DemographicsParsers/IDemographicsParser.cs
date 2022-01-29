using Uheaa.Common;

namespace ACURINTR.DemographicsParsers
{
    interface IDemographicsParser
    {
        QueueTask Parse();
    }
}
