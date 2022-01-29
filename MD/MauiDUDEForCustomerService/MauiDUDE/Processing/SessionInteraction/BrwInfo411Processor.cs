using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{ 
    class BrwInfo411Processor
    {
        //TODO The use of staic in this class worries me since it could mean that borrower information is not being properly cleaned up.
        private static BrwInfo411 _frm;
        private static Borrower _uheaaBorrower;
        private static Borrower _borrower;
        private static bool _info411Changed;
        private ReflectionInterface RI;

        public static bool Info411Changed { get { return _info411Changed; } }

        public static void Show411Form(Borrower uheaaBorrower, bool userRequested)
        {
            _uheaaBorrower = uheaaBorrower;
            _borrower = _uheaaBorrower;

            if(_borrower.Info411.Length > 0)
            {
                if(_uheaaBorrower != null)
                {
                    _uheaaBorrower.AddAlert(Borrower.AlertTypes.Has411);
                }
            }

            ShowForm(userRequested);
        }

        public static void Show411Form(bool userRequested)
        {
            //if(_borrower.Info411.Length > 0)
            //{
            //    _uheaaBorrower.AddAlert(Borrower.AlertTypes.Has411);
            //}

            ShowForm(userRequested);
        }

        private static void ShowForm(bool userRequested)
        {
            if (_borrower.Info411.Length == 0 && !userRequested)
            {
                return;
            }

            _frm = new BrwInfo411();
            _frm.ShowDialog(_borrower, userRequested);
        }

        public static void SaveChangesToSystems()
        {
            BrwInfo411Processor processor = new BrwInfo411Processor();
            processor.RI = SessionInteractionComponents.RI;
            bool results = false;

            if(_uheaaBorrower != null)
            {
                processor.RI.Stup(DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
                results = processor.RI.Atd22AllLoans(_uheaaBorrower.SSN, "M1411", _uheaaBorrower.Info411, "", SessionInteractionComponents.ScriptId, false);
            }

            if(results == false)
            {
                WhoaDUDE.ShowWhoaDUDE("For some reason an activity comment using the M1411 ARC could not be added.  Please notifiy the System Support Help Desk.", "Activity Comment Add Error");
            }

            _info411Changed = false;
        }

        public static void SaveChanges(string commentText)
        {
            _info411Changed = true;

            if(_uheaaBorrower != null)
            {
                _uheaaBorrower.Info411 = commentText;
            }
        }

        /// <summary>
        /// closes the 411 screen
        /// </summary>
        public static void Close411Form()
        {
            if(_frm != null)
            {
                _frm.Close();
                _frm = null;
            }
        }
    }
}
