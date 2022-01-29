using Q;
using System;
using System.IO;
using System.Windows.Forms;

namespace SPLTRCAMP
{
    public class CampaignProcessor : ScriptSessionBase
    {

        private CampaignData _data;
        private string _scriptID;
        private Recovery _recovery;
        private string _testFile = string.Format("{0}Special Letter Campaign Test File.txt",DataAccessBase.PersonalDataDirectory);
        public string ErrorLog { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public CampaignProcessor(ReflectionInterface ri, string scriptID, CampaignData data)
            : base(ri)
        {
            _data = data;
            _scriptID = scriptID;
            ErrorLog = string.Format("{0}Special Letter Campaign Error Log {1}.txt", DataAccessBase.PersonalDataDirectory,DateTime.Now.ToString("MM-dd-yyyy hhmmss"));
            _recovery = new Recovery(TestModeProperty);
            _recovery.RecoveryLog = string.Format("{0}Special Letter Campaign Log.txt", TestMode("").LogFolder);
            if (File.Exists(_recovery.RecoveryLog))
            {
                if (MessageBox.Show("The script detected a recovery log.  Do you wish to recover?  Click Yes to recover or No to process without recovering.","In Recovery", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ErrorLog = _recovery.GetLogContents();
                }
            }
        }

        /// <summary>
        /// Main processing point.
        /// </summary>
        public void RunCampaign()
        {
            if (_data.ActionSelected == CampaignData.Action.Test)
            {
                //create test file for the test process to process against
                CreateTestFile();
                //actual processing is done against the CalculatedDataFile variable (in this case the test file)
                _data.CalculatedDataFile = _testFile;
            }
            else
            {
                //actual processing is done against the CalculatedDataFile variable
                _data.CalculatedDataFile = _data.DataFile; 
            }
            //create letters
            string newLetterDataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(_data.CalculatedDataFile, "ACCOUNT_NUMBER", _data.LetterIDFromLTS, true, ((DocumentHandling.Barcode2DLetterRecipient)_data.Recipient), TestModeProperty);
            DocumentHandling.CostCenterPrinting(_data.LetterDescriptionForCCCCoverSheet, ((DocumentHandling.DestinationOrPageCount)_data.PageCountOrDestination), _data.LetterIDFromLTS, newLetterDataFile, "CCC", "STATE", DateTime.Now.ToShortDateString(), _scriptID, TestModeProperty, _recovery.InRecovery, true);
            //do comments
            if (_data.AddCommentsToCompass || _data.AddCommentsToOneLINK)
            {
                AddComments();
            }
            //delete recovery log
            if (File.Exists(_recovery.RecoveryLog))
            {
                File.Delete(_recovery.RecoveryLog);
            }
        }

        //adds comments for data file
        private void AddComments()
        {
            CampaignRecordData recordData;
            //open file 
            VbaStyleFileOpen(_data.CalculatedDataFile,1,Common.MSOpenMode.Input);
            //header row
            recordData = GetRecord();
            if (_recovery.InRecovery)
            {
                //recover if in recovery
                while (recordData.AccountNumber != _recovery.AccountNumber)
                {
                    recordData = GetRecord();
                }
            }
            while (!VbaStyleEOF(1))
            {
                //input data
                recordData = GetRecord();
                AddCommentsForRecord(recordData);
                //update recovery log
                _recovery.UpdateLogContents(recordData.AccountNumber, ErrorLog);
            }
            VbaStyleFileClose(1);
        }

        //gets next record in data file
        private CampaignRecordData GetRecord()
        {
            CampaignRecordData recordData = new CampaignRecordData();
            recordData.ACSKeyline = VbaStyleFileInput(1);
            recordData.FName = VbaStyleFileInput(1);
            recordData.LName = VbaStyleFileInput(1);
            recordData.Addr1 = VbaStyleFileInput(1);
            recordData.Addr2 = VbaStyleFileInput(1);
            recordData.City = VbaStyleFileInput(1);
            recordData.State = VbaStyleFileInput(1);
            recordData.Zip = VbaStyleFileInput(1);
            recordData.Country = VbaStyleFileInput(1);
            recordData.AccountNumber = VbaStyleFileInput(1);
            recordData.CCC = VbaStyleFileInput(1);
            recordData.Gen1 = VbaStyleFileInput(1);
            recordData.Gen2 = VbaStyleFileInput(1);
            recordData.Gen3 = VbaStyleFileInput(1);
            return recordData;
        }

        //adds comments for a record in the data file.
        private void AddCommentsForRecord(CampaignRecordData recordData)
        {
            try
            {
                //add Compass comments if applicable
                if (_data.AddCommentsToCompass)
                {
                    //translate to SSN
                    recordData.SSN = GetDemographicsFromTX1J(recordData.AccountNumber).SSN;
                    if (ATD22ByBalance(recordData.SSN, _data.ARC, _data.Comment, _scriptID, false) == false)
                    {
                        if (ATD37FirstLoan(recordData.SSN, _data.ARC, _data.Comment, _scriptID, false) != Common.CompassCommentScreenResults.CommentAddedSuccessfully)
                        {
                            //if comments couldn't be left on either TD22 or TD37 then write out to error log
                            UpdateErrorLog(recordData, "Compass");
                        }
                    }
                }
            }
            catch (Exception)
            {
                UpdateErrorLog(recordData, "Compass");
            }
            try
            {
                //add OneLINK comments if applicable
                if (_data.AddCommentsToOneLINK)
                {
                    if (recordData.SSN == string.Empty)
                    {
                        recordData.SSN = GetDemographicsFromLP22(recordData.AccountNumber).SSN;
                    }
                    if (AddCommentInLP50(recordData.SSN, _data.ActionCode, _scriptID, "LT", "03", _data.Comment) == false)
                    {
                        UpdateErrorLog(recordData, "OneLINK");
                    }
                }
            }
            catch (Exception)
            {
                UpdateErrorLog(recordData, "OneLINK");
            }
        }

        //create error log
        private void UpdateErrorLog(CampaignRecordData recordData, string system)
        {
            VbaStyleFileOpen(ErrorLog, 2, Common.MSOpenMode.Append);
            VbaStyleFileWriteLine(2, recordData.ACSKeyline, recordData.FName, recordData.LName, recordData.Addr1, 
                                    recordData.Addr2, recordData.City, recordData.State, recordData.Zip, recordData.Country, 
                                    recordData.AccountNumber, recordData.CCC, recordData.Gen1, recordData.Gen2, 
                                    recordData.Gen3, system);
            VbaStyleFileClose(2);
        }

        //creates test file
        private void CreateTestFile()
        {
            int lineCount = 0;
            VbaStyleFileOpen(_data.DataFile, 1, Common.MSOpenMode.Input);
            VbaStyleFileOpen(_testFile,2,Common.MSOpenMode.Output);
            //transfer header row to test file
            CampaignRecordData recordData = GetRecord();
            VbaStyleFileWriteLine(2, recordData.ACSKeyline, recordData.FName, recordData.LName, recordData.Addr1,
                                    recordData.Addr2, recordData.City, recordData.State, recordData.Zip, recordData.Country,
                                    recordData.AccountNumber, recordData.CCC, recordData.Gen1, recordData.Gen2,
                                    recordData.Gen3);
            //transfer over 5 records or the rest of the file to test file
            while (!VbaStyleEOF(1) && lineCount < 5)
            {
                recordData = GetRecord();
                VbaStyleFileWriteLine(2, recordData.ACSKeyline, recordData.FName, recordData.LName, recordData.Addr1,
                                    recordData.Addr2, recordData.City, recordData.State, recordData.Zip, recordData.Country,
                                    recordData.AccountNumber, recordData.CCC, recordData.Gen1, recordData.Gen2,
                                    recordData.Gen3);
                lineCount++;
            }
            VbaStyleFileClose(2);
            VbaStyleFileClose(1);
        }

    }
}
