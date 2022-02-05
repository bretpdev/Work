using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace XmlGeneratorECorr
{
    public class Letters
    {
        [Hidden, PrimaryKey]
        public int LetterId { get; set; }
        [Required, MaxLength(10)]
        public string Letter { get; set; }
        [Manual]
        public int LetterTypeId { get; set; }
        [Required, MaxLength(10)]
        public string DocId { get; set; }
        [Label("Viewable"), DbIgnore]
        public bool ViewableBool
        {
            get
            {
                return Viewable == "Y";
            }
            set
            {
                Viewable = value ? "Y" : "N";
            }

        }
        [Hidden,]
        public string Viewable { get; set; }
        [Required, MaxLength(60)]
        public string ReportDescription { get; set; }
        [Required, MaxLength(17)]
        public string ReportName { get; set; }
        [Label("Viewed"), DbIgnore]
        public bool ViewedBool
        {
            get
            {
                return Viewed == "Y";
            }
            set
            {
                Viewed = value ? "Y" : "N";
            }
        }
        [Hidden]
        public string Viewed { get; set; }
        [Required, MaxLength(8)]
        public string MainFrameRegion { get; set; }
        [Required, MaxLength(50)]
        public string SubjectLine { get; set; }
        [Required, MaxLength(10)]
        public string DocSource { get; set; }
        [TextBoxLines(3), Required, MaxLength(255)]
        public string DocComment { get; set; }
        [Label("Work Flow"), DbIgnore]
        public bool WorkFlowBool
        {
            get
            {
                return WorkFlow == "Y";
            }
            set
            {
                WorkFlow = value ? "Y" : "N";
            }
        }
        [Hidden]
        public string WorkFlow { get; set; }
        [Label("Doc Delete"), DbIgnore]
        public bool DocDeleteBool
        {
            get
            {
                return DocDelete == "Y";
            }
            set
            {
                DocDelete = value ? "Y" : "N";
            }
        }
        [Hidden]
        public string DocDelete { get; set; }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "UpdateLetter")]
        public static void UpdateDb(Letters letter)
        {
            DataAccessHelper.Execute("UpdateLetter", DataAccessHelper.Database.ECorrFed, SqlParams.Update(letter));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "InsertLetter")]
        public static void AddRecordDb(Letters letter)
        {
            DataAccessHelper.Execute("InsertLetter", DataAccessHelper.Database.ECorrFed, SqlParams.Insert(letter));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "InactiveLetterRecord")]
        public static void InactiveRecord(int letterId)
        {
            DataAccessHelper.Execute("InactiveLetterRecord", DataAccessHelper.Database.ECorrFed, new SqlParameter("LetterId", letterId));
        }
    }
}
