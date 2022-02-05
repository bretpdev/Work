using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class Hygiene : Form
    {
        public static DialogResult ShowHygiene()
        {
            using(Hygiene hygiene = new Hygiene())
            {
                return hygiene.ShowDialog();
            }
        }

        public Hygiene()
        {
            InitializeComponent();

            InitializeAbbreviations();
        }

        public void InitializeAbbreviations()
        {
            textBoxRules.BackColor = BackColor;
            textBoxRules.ForeColor = ForeColor;
            textBoxRules2.BackColor = BackColor;
            textBoxRules2.ForeColor = ForeColor;
            textBoxSecondary.BackColor = BackColor;
            textBoxSecondary.ForeColor = ForeColor;
            textBoxSecondary2.BackColor = BackColor;
            textBoxSecondary2.ForeColor = ForeColor;

            textBoxRules.Text = "Alley\n" +
                                "Avenue\n" +
                                "Boulevard\n" +
                                "Center\n" +
                                "Circle\n" +
                                "Cove\n" +
                                "Court\n" +
                                "Drive\n" +
                                "Lane\n" +
                                "Park\n" +
                                "Parkway\n" +
                                "Place\n" +
                                "Ridge\n" +
                                "Road\n" +
                                "Street\n" +
                                "Summit\n" +
                                "Terrace\n" +
                                "Trailer\n" +
                                "Valley\n" +
                                "Village\n" +
                                "Vista\n" +
                                "Way";

            textBoxRules2.Text = "ALY\n" +
                                "AVE\n" +
                                "BLVD\n" +
                                "CTR\n" +
                                "CIR\n" +
                                "CV\n" +
                                "CT\n" +
                                "DR\n" +
                                "LN\n" +
                                "PARK\n" +
                                "PKY\n" +
                                "PL\n" +
                                "RDG\n" +
                                "RD\n" +
                                "ST\n" +
                                "SMT\n" +
                                "TER\n" +
                                "TRL\n" +
                                "VLY\n" +
                                "VLG\n" +
                                "VIS\n" +
                                "WAY";

            textBoxSecondary.Text = "Apartment\n" +
                                    "Basement\n" +
                                    "Building\n" +
                                    "Lot\n" +
                                    "Lower\n" +
                                    "Office\n" +
                                    "Penthouse\n" +
                                    "Suite\n" +
                                    "Trailer\n" +
                                    "Unit\n" +
                                    "Upper";

            textBoxSecondary2.Text = "APT\n" +
                                     "BSMT\n" +
                                     "BLDG\n" +
                                     "LOT\n" +
                                     "LOWR\n" +
                                     "OFC\n" +
                                     "PH\n" +
                                     "STE\n" +
                                     "TRLR\n" +
                                     "UNIT\n" +
                                     "UPPR";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}