using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class DefermentData : SerfFileBase
	{
        [Ignore]
        public int DefermentDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string DFR10_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(9), Required, LinePos(40)]
		public string LF_STU_SSN { get; set; }

		[FormatCode("X"), Length(3), LinePos(49)]
		public string LC_DFR_REQ_COR { get; set; }

		[FormatCode("X"), Length(1), LinePos(52)]
		public string LC_DFR_STA { get; set; }

		[FormatCode("X"), Length(2), LinePos(53)]
		public string LC_DFR_TYP { get; set; }

		[FormatCode("X"), Length(1), LinePos(55)]
		public string LC_ENR_STA { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(56)]
		public string LD_DFR_REQ_BEG { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(66)]
		public string LD_DFR_REQ_END { get; set; }

		[FormatCode("X"), Length(3), LinePos(76)]
		public string LF_DFR_CTL_NUM { get; set; }

		[FormatCode("X"), Length(8), LinePos(79)]
		public string LF_DOE_SCL_DFR { get; set; }

		[FormatCode("X"), Length(1), LinePos(87)]
		public string LI_CAP_DFR_INT_REQ { get; set; }

		[FormatCode("X"), Length(10), LinePos(88)]
		public string LD_DFR_CER { get; set; }

		[FormatCode("X"), Length(2), LinePos(98)]
		public string LC_DFR_SUB_TYP { get; set; }

		[FormatCode("X"), Length(10), LinePos(100)]
		public string LD_BR_REQ_DFR_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(110)]
		public string LD_DFR_SPT_DOC_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(120)]
		public string LD_DFR_SPT_DOC_END { get; set; }

		[FormatCode("X"), Length(1), LinePos(130)]
		public string LI_DFR_SPT_DOC_ACP { get; set; }

		[FormatCode("X"), Length(3), LinePos(131)]
		public string LC_DFR_DNL_USR_ENT { get; set; }

		[FormatCode("X"), Length(1), LinePos(134)]
		public string LI_DFR_DOC_SPT_REQ { get; set; }

		[FormatCode("X"), Length(1), LinePos(135)]
		public string LI_REQ_PST_DFR_DFR { get; set; }

		[FormatCode("X"), Length(1), LinePos(136)]
		public string LI_REQ_IN_SCL_DFR { get; set; }

		[FormatCode("X"), Length(10), LinePos(137)]
		public string LD_STP_ENR_MIN_HT { get; set; }

		[FormatCode("X"), Length(3), LinePos(147)]
		public string LC_ARA_TSH { get; set; }

		[FormatCode("X"), Length(10), LinePos(150)]
		public string LD_NTF_DFR_END { get; set; }

		[FormatCode("X"), Length(10), LinePos(160)]
		public string LD_DFR_INF_CER { get; set; }

		[FormatCode("S9"), Length(7), LinePos(170)]
		public string LA_BR_PAY_CHK_JOB { get; set; }

		[FormatCode("X"), Length(2), LinePos(182)]
		public string LC_BR_PAY_CHK_FRQ { get; set; }

		[FormatCode("X"), Length(1), LinePos(184)]
		public string LC_BR_EMP_STA { get; set; }

		[FormatCode("S9"), Length(3), LinePos(185)]
		public string LN_BR_FAM_SIZ { get; set; }

		[FormatCode("X"), Length(2), LinePos(188)]
		public string LC_FED_POV_GID_ST { get; set; }

		[FormatCode("S9"), Length(7), LinePos(200)]
		public string LA_MTH_FED_MIN_WGE { get; set; }

		[FormatCode("S9"), Length(7), LinePos(212)]
		public string LA_BR_CLC_POV { get; set; }

		[FormatCode("X"), Length(1), LinePos(224)]
		public string LC_SEL_EHD_DFR_TYP { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(235)]
		public string LON50_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(236)]
		public string LN_SEQ { get; set; }

		[FormatCode("X"), Length(3), Required, LinePos(240)]
		public string LC_DFR_RSP { get; set; }

		[FormatCode("X"), Length(10), LinePos(243)]
		public string LD_DFR_APL { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(253)]
		public string LD_DFR_BEG { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(263)]
		public string LD_DFR_END { get; set; }

		[FormatCode("X"), Length(10), LinePos(273)]
		public string LD_DFR_GRC_END { get; set; }

		[FormatCode("X"), Length(1), LinePos(283)]
		public string LC_LON_LEV_DFR_CAP { get; set; }

		[FormatCode("X"), Length(1), LinePos(284)]
		public string LI_DLQ_CAP { get; set; }

	}
}
