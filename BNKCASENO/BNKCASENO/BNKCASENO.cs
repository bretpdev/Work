using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;
using System.IO;

namespace BNKCASENO
{
    public class BNKCASENO : BatchScriptBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri"></param>
        public BNKCASENO(ReflectionInterface ri)
            : base(ri,"BNKCASENO")
        {
        }

        //main starting point for processing 
        public override void Main()
        {
            if (CalledByMasterBatchScript() == false)
            {
                if (MessageBox.Show("This is the Bankruptcy Case Number Script. It loads the Doc ID database with SAS file ULW104.LW104R2.","Bankruptcy Case Number Script",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    EndDLLScript();
                }
            }

            SplashScreen splashForm = new SplashScreen();
            splashForm.Show();
            splashForm.Refresh();

            string ftpFolder = TestMode(string.Empty).FtpFolder;
            string sas = string.Empty;

            try 
            {
                sas = DeleteOldFilesReturnMostCurrent(ftpFolder,"ULWI04.LWI04R2*",Common.FileOptions.ErrorOnMissing | Common.FileOptions.ErrorOnEmpty);
            }
            catch (FileEmptyException)
            {
                MessageBox.Show("The SAS file is empty.  Please contact System Support.", "SAS File Is Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                splashForm.Close();
                EndDLLScript();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("SAS file ULWI04.LWI04R2* is missing. Contact System Support.","Missing File",MessageBoxButtons.OK,MessageBoxIcon.Error);
                splashForm.Close();
                EndDLLScript();
            }
            
            DataAccess.DeleteAllDataFromTable(TestModeProperty);
            VbaStyleFileOpen(sas,1,Common.MSOpenMode.Input);
            while (VbaStyleEOF(1) == false)
            {
                BankruptcyRecordData record = new BankruptcyRecordData();
                record.AccountNumber = VbaStyleFileInput(1);
                record.CaseNumber = VbaStyleFileInput(1);
                record.FirstName = VbaStyleFileInput(1).Replace("'","''");
                record.LastName = VbaStyleFileInput(1).Replace("'","''");
                DataAccess.InsertRecordIntoTable(TestModeProperty,record);
            }
            VbaStyleFileClose(1);
            if (File.Exists(sas))
            {
                File.Delete(sas);
            }
            splashForm.Close();
            ProcessingComplete();
    
        }

    }
}
