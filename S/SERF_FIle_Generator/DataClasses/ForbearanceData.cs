using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class ForbearanceData : SerfFileBase
	{
        [Ignore]
        public int ForbearanceDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string FOR10_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(3), LinePos(40)]
		public string LC_FOR_REQ_COR { get; set; }

		[FormatCode("X"), Length(1), LinePos(43)]
		public string LC_FOR_STA { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(44)]
		public string LC_FOR_SUB_TYP { get; set; }

		[FormatCode("X"), Length(2), Required, LinePos(45)]
		public string LC_FOR_TYP { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(47)]
		public string LD_FOR_REQ_BEG { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(57)]
		public string LD_FOR_REQ_END { get; set; }

		[FormatCode("X"), Length(3), LinePos(67)]
		public string LF_FOR_CTL_NUM { get; set; }

		[FormatCode("X"), Length(1), LinePos(70)]
		public string LI_CAP_FOR_INT_REQ { get; set; }

		[FormatCode("S9"), Length(7), LinePos(71)]
		public string LA_REQ_RDC_PAY { get; set; }

		[FormatCode("X"), Length(2), LinePos(78)]
		public string LC_FORNEW_SUB_TYP { get; set; }

		[FormatCode("X"), Length(2), LinePos(80)]
		public string LC_FOR_XCP_DCR_TYP { get; set; }

		[FormatCode("X"), Length(10), LinePos(82)]
		public string LD_FOR_INF_CER { get; set; }

		[FormatCode("X"), Length(8), LinePos(92)]
		public string LF_DOE_SCL_FOR { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(101)]
		public string LON60_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(102)]
		public string LN_SEQ { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(106)]
		public string LD_FOR_BEG { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(116)]
		public string LD_FOR_END { get; set; }

		[FormatCode("X"), Length(10), LinePos(126)]
		public string LD_FOR_APL { get; set; }

		[FormatCode("S9"), Length(7), LinePos(136)]
		public string LA_ACL_RDC_PAY { get; set; }

		[FormatCode("X"), Length(1), LinePos(143)]
		public string LC_LON_LEV_FOR_CAP { get; set; }

		[FormatCode("S9"), Length(7), LinePos(294)]
		public string LA_BR_PAY_CHK_JOB { get; set; }

		[FormatCode("X"), Length(2), LinePos(303)]
		public string LC_BR_PAY_CHK_FRQ { get; set; }

		[FormatCode("D"), Length(10), LinePos(305)]
		public string LD_FOR_BR_REQ_BEG { get; set; }

		[FormatCode("D"), Length(10), LinePos(315)]
		public string LD_FOR_BR_REQ_END { get; set; }

		[FormatCode("X"), Length(3), LinePos(325)]
		public string LC_FOR_DNL_USR_ENT { get; set; }

		[FormatCode("X"), Length(1), LinePos(328)]
		public string LI_FOR_SPT_DOC_ACP { get; set; }

		[FormatCode("S9"), Length(7), LinePos(329)]
		public string LA_BR_MTH_IRL_ISL { get; set; }

		[FormatCode("S9"), Length(7), LinePos(338)]
		public string LA_BR_MTH_EXT_ISL { get; set; }

		[FormatCode("X"), Length(1), LinePos(347)]
		public string LI_BRQ_TMP_DNL_FOR { get; set; }

		[FormatCode("X"), Length(1), LinePos(348)]
		public string LI_BRQ_TMP_FOR_DLQ { get; set; }

	}
}
