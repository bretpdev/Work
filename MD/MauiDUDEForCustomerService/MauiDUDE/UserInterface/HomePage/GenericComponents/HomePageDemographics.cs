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
    public partial class HomePageDemographics : UserControl
    {
        public Demographics DemographicsData
        {
            set
            {
                if(!string.IsNullOrWhiteSpace(value.HomePhoneForeignCountry))
                {
                    value.HomePhoneNum = value.HomePhoneForeignCountry + "-" + value.HomePhoneForeignCity + "-" + value.HomePhoneForeignLocalNumber;
                }
                if(!string.IsNullOrWhiteSpace(value.OtherPhoneForeignCountry))
                {
                    value.OtherPhoneNum = value.OtherPhoneForeignCountry + "-" + value.OtherPhoneForeignCity + "-" + value.OtherPhoneForeignLocalNumber;
                }
                if(!string.IsNullOrWhiteSpace(value.OtherPhone2ForeignCountry))
                {
                    value.OtherPhone2Num = value.OtherPhone2ForeignCountry + "-" + value.OtherPhone2ForeignCity + "-" + value.OtherPhone2ForeignLocalNumber;
                }
                if(!string.IsNullOrWhiteSpace(value.OtherPhone3ForeignCountry))
                {
                    value.OtherPhone3Num = value.OtherPhone3ForeignCountry + "-" + value.OtherPhone3ForeignCity + "-" + value.OtherPhone3ForeignLocalNumber;
                }
                demographicsBindingSource.DataSource = value;
                demographicsBindingSource.ResetBindings(false);
                //populate indicators
                bool addr = value.UPAddrVal;
                bool homePhone = value.UPPhoneVal;
                bool altPhone = value.UPOtherVal;
                bool email = value.UPEmailVal;

                //strike through any address parts that are invalid and make others regular font
                Font strikeThroughFont = new Font(textBoxAddress1.Font, FontStyle.Strikeout);
                Font regularFont = new Font(textBoxAddress1.Font, FontStyle.Regular);
                //address
                if(!addr)
                {
                    //strike through text if invalid address
                    textBoxAddress1.Font = strikeThroughFont;
                    textBoxAddress2.Font = strikeThroughFont;
                    textBoxCity.Font = strikeThroughFont;
                    textBoxState.Font = strikeThroughFont;
                    textBoxZIP.Font = strikeThroughFont;
                }
                else
                {
                    //regular text if invalid address
                    textBoxAddress1.Font = regularFont;
                    textBoxAddress2.Font = regularFont;
                    textBoxCity.Font = regularFont;
                    textBoxState.Font = regularFont;
                    textBoxZIP.Font = regularFont;
                }
                //home phone
                if(!homePhone && value.HomePhoneNum != "")
                {
                    //strike through text if invalid home phone
                    textBoxHomePhone.Font = strikeThroughFont;
                    textBoxHomeExt.Font = strikeThroughFont;
                }
                else
                {
                    //regular text if invalid home phone
                    textBoxHomePhone.Font = regularFont;
                    textBoxHomeExt.Font = regularFont;
                }

                //alt phone info
                if(!altPhone && value.OtherPhoneNum != "")
                {
                    //strike through text if invalid home phone
                    textBoxAltPhone.Font = strikeThroughFont;
                    textBoxAltExt.Font = strikeThroughFont;
                }
                else
                {
                    //regular text if invalid alt phone
                    textBoxAltPhone.Font = strikeThroughFont;
                    textBoxAltExt.Font = strikeThroughFont;
                }

                //email info
                if(!email && value.Email != "")
                {
                    //strike through text if invalid home phone
                    textBoxEmail.Font = strikeThroughFont;
                }
                else
                {
                    //regular text if invalid home phone
                    textBoxEmail.Font = regularFont;
                }

            }
        }

        public HomePageDemographics()
        {
            InitializeComponent();
        }
    }
}
