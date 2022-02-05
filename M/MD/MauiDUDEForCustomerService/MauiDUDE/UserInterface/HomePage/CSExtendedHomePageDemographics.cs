using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class CSExtendedHomePageDemographics : HomePageDemographics
    {
        public Demographics DemographicData
        {
            set
            {
                base.DemographicsData = value;
                //updating to use this so that if the user provides a positive validity indicator it overrides the other indicator
                bool altPhone2 = value.UPOther2Val ? value.UPOther2Val : value.OtherPhone2ValidityIndicator == "Y";
                //strike through any address parts taht are invalid and make other regular font
                var strikeThroughFont = new Font(textBoxAddress1.Font, FontStyle.Strikeout);
                var regularFont = new Font(textBoxAddress1.Font, FontStyle.Regular);
                //alt phone info
                if(!altPhone2 && value.OtherPhone2Num != "")
                {
                    //strike through text iff invalid home phone
                    textBoxAltPhone2.Font = strikeThroughFont;
                    textBoxAltExt2.Font = strikeThroughFont;
                }
                else
                {
                    //regular text if invalid home phone
                    textBoxAltPhone2.Font = regularFont;
                    textBoxAltExt2.Font = regularFont;
                }
            }
        }

        public CSExtendedHomePageDemographics()
        {
            InitializeComponent();
        }
    }
}
