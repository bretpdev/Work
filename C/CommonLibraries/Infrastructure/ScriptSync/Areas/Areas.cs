using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace Uheaa
{
    public class Areas
    {
        public CommonArea CommonFed { get; internal set; }
        public CommonArea CommonFfel { get; internal set; }

        public TempArea TempFed { get; internal set; }
        public TempArea TempFfel { get; internal set; }

        public QArea QFed { get; internal set; }
        public QArea QFfel { get; internal set; }

        private static Areas live;
        public static Areas Live
        {
            get
            {
                if (live == null) live = GetArea(DataAccessHelper.Mode.Live);
                return live;
            }
        }

        private static Areas test;
        public static Areas Test
        {
            get
            {
                if (test == null) test = GetArea(DataAccessHelper.Mode.Test);
                return test;
            }
        }
        public static Areas Current
        {
            get
            {
                return DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? Live : Test;
            }
        }

        private static Areas GetArea(DataAccessHelper.Mode mode)
        {
            Areas a = new Areas();
            string networkFed = NetworkLocations.Fed(mode);
            string localFed = LocalLocations.FedScripts(mode);

            string networkFfel = NetworkLocations.Ffel(mode);
            string localFfel = LocalLocations.FfelScripts(mode);

            a.CommonFed = new CommonArea(networkFed, localFed, networkFed, localFed, true);
            a.CommonFfel = new CommonArea(networkFfel, localFfel, networkFfel, localFfel, false);

            a.TempFed = new TempArea(networkFed, localFed, networkFed, localFed, true);
            a.TempFfel = new TempArea(networkFfel, localFfel, networkFfel, localFfel, false);

            a.QFed = new QArea(networkFed, localFed, networkFed, localFed, true);
            a.QFfel = new QArea(networkFfel, localFfel, networkFfel, localFfel, false);

            return a;
        }
    }
}
