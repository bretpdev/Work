using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using MDIntermediary;

namespace MauiDUDE
{
    public partial class DeceasedACPQuestionsControl : UserControl, ILegalOtherControlValidator
    {
        public AcpDeceasedInfo ResponseData
        {
            get
            {
                if(comboBoxThirdPartyBorrower.Text.Length == 0)
                {
                    return null;
                }
                else
                {
                    return new AcpDeceasedInfo()
                    {
                        ThirdPartyKnowsBorrower = comboBoxThirdPartyBorrower.Text,
                        SpeakingTo = textBoxSpeakingTo.Text,
                        LetterOfPermissionOnFile = comboBoxPermission.Text,
                        DateOfDeath = maskedTextBoxDateOfDeath.Text.ToDate(),
                        DeathOccurredInformation = textBoxDeathOccuredInformation.Text,
                        AbleToSendOriginalDeathCertificate = comboBoxAbleToSendDeathCertificate.Text,
                        ClosestLivingRelativeInformation = textBoxLivingRelativeInformation.Text,
                        NameOfFuneralHome = textBoxFuneralHome.Text
                    };
                }
            }
        }

        public DeceasedACPQuestionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if data is valid
        /// </summary>
        public bool UserInputIsValid()
        {
            //check third party information
            if(comboBoxThirdPartyBorrower.Text.Length == 0)
            {
                return true; //if first question is not asnwered assume the control is just there and not populated
            }
            else if(textBoxSpeakingTo.Text.Length == 0)
            {
                WhoaDUDE.ShowWhoaDUDE("You must indicate who you are speaking to.", "Needed Deceased Information Missing");
                return false;
            }
            else if(comboBoxPermission.Text.Length == 0)
            {
                WhoaDUDE.ShowWhoaDUDE("You must indicate whether there is a letter of permission to speak to the 3rd party on file.", "Needed Deceased Information Missing");
                return false;
            }
            //check deceased information
            if (comboBoxAbleToSendDeathCertificate.Text.Length == 0)
            {
                WhoaDUDE.ShowWhoaDUDE("You must indicate whether the 3rd party can send an original death certificate.", "Needed Deceased Information Missing");
                return false;
            }
            else if (!maskedTextBoxDateOfDeath.Text.ToDateNullable().HasValue)
            {
                WhoaDUDE.ShowWhoaDUDE("You must provide a valid date of death.", "Needed Deceased Information Missing");
                return false;
            }
            else if (textBoxDeathOccuredInformation.Text.Length == 0)
            {
                WhoaDUDE.ShowWhoaDUDE("You must provide where the death occured.", "Needed Deceased Information Missing");
                return false;
            }
            else if (textBoxLivingRelativeInformation.Text.Length == 0)
            {
                WhoaDUDE.ShowWhoaDUDE("You must provide living relative information.", "Needed Deceased Information Missing");
                return false;
            }
            return true;
        }
    }
}
