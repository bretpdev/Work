using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public interface IHomePage
    {
        Button SaveAndContinue { get; }
        Button UpdateDemographicsButton { get; }
        Button ReturnToMainMenuButtons { get; }
        bool IsVisible { get; }
        bool HasValidData { get; set; }

        void ShowCustom();
        void CloseAllForms();
        void Hide();

        void ReBindBorrower(Borrower borrower);

    }
}
