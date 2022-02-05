using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class DisbData : SerfFileBase
	{
        [Ignore]
        public int DisbDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string LON15_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(6), LinePos(40)]
		public string IC_LON_PGM { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(46)]
		public string LN_SEQ { get; set; }

		[FormatCode("X"), Length(8), LinePos(50)]
		public string AF_DSB_RPT { get; set; }

		[FormatCode("S9"), Length(8), Required, LinePos(58)]
		public string LA_DSB { get; set; }

		[FormatCode("S9"), Length(8), LinePos(66)]
		public string LA_DSB_CAN { get; set; }

		[FormatCode("X"), Length(1), LinePos(74)]
		public string LC_DSB_CAN_TYP { get; set; }

		[FormatCode("X"), Length(1), LinePos(75)]
		public string LC_DSB_TYP { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(76)]
		public string LC_LDR_DSB_MDM { get; set; }

		[FormatCode("X"), Length(1), LinePos(77)]
		public string LC_RCP_CHK_DSB { get; set; }

		[FormatCode("X"), Length(10), LinePos(78)]
		public string LD_CAN_RPT_GTR { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(88)]
		public string LD_DSB { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(98)]
		public string LD_DSB_CAN { get; set; }

		[FormatCode("X"), Length(10), LinePos(108)]
		public string LD_DSB_ROS_PRT { get; set; }

		[FormatCode("X"), Length(10), LinePos(118)]
		public string LF_DSB_CHK { get; set; }

		[FormatCode("X"), Length(9), LinePos(128)]
		public string LF_RCP_DSB_CHK { get; set; }

		[FormatCode("X"), Length(1), LinePos(137)]
		public string LI_LTE_DSB_APV { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(138)]
		public string LN_BR_DSB_SEQ { get; set; }

		[FormatCode("S9"), Length(5), LinePos(142)]
		public string LR_DSB_ITR { get; set; }

		[FormatCode("X"), Length(1), LinePos(147)]
		public string LC_STA_LON15 { get; set; }

		[FormatCode("X"), Length(1), LinePos(148)]
		public string LC_LON_EXT_ORG_SRC { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(149)]
		public string LN_LON_DSB_SEQ { get; set; }

		[FormatCode("S9"), Length(8), LinePos(153)]
		public string LA_PCV_LDR_CHK { get; set; }

		[FormatCode("S9"), Length(8), LinePos(161)]
		public string LA_PRE_DSB_CAN { get; set; }

		[FormatCode("D"), Length(10), LinePos(169)]
		public string LD_PRE_DSB_CAN { get; set; }

		[FormatCode("X"), Length(4), LinePos(179)]
		public string LC_DSB_CAN_REA { get; set; }

		[FormatCode("X"), Length(9), LinePos(183)]
		public string AF_APL_ID { get; set; }

		[FormatCode("S9"), Length(4), LinePos(192)]
		public string AN_LC_APL_SEQ { get; set; }

		[FormatCode("S9"), Length(8), LinePos(196)]
		public string LA_DSB_CAN_PCV_RFD { get; set; }

		[FormatCode("S9"), Length(8), LinePos(204)]
		public string LA_DSB_CAN_RFD { get; set; }

		[FormatCode("D"), Length(10), LinePos(212)]
		public string LD_DSB_CAN_RFD { get; set; }

		[FormatCode("S9"), Length(12), LinePos(222)]
		public string LA_PRE_DSB_CAN_RPT { get; set; }

		[FormatCode("D"), Length(10), LinePos(234)]
		public string LD_PRE_DSB_CAN_RPT { get; set; }

		[FormatCode("S9"), Length(9), LinePos(244)]
		public string LA_DL_DSB_REB { get; set; }

		[FormatCode("S9"), Length(9), LinePos(253)]
		public string LA_DSB_REB_CAN { get; set; }

		[FormatCode("S9"), Length(4), LinePos(262)]
		public string LN_DL_DSB_PAY_SEQ { get; set; }

		[FormatCode("X"), Length(1), LinePos(291)]
		public string LI_DSB_CMNLN_ERR { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(292)]
		public string LN_LON18_DAT_OCC_CNT { get; set; }

		[Occurs(8), LinePos(296)]
		public List<LON18_DAT> ListOfLON18_DAT { get; set; }
		public class LON18_DAT
		{
			[FormatCode("S9"), Length(7), Required, LinePos(0)]
			public string LA_DSB_FEE { get; set; }

			[FormatCode("X"), Length(2), Required, LinePos(7)]
			public string LC_DSB_FEE { get; set; }

			[FormatCode("X"), Length(2), Required, LinePos(9)]
			public string LC_DSB_FEE_PAY { get; set; }

			[FormatCode("S9"), Length(5), LinePos(11)]
			public string LR_DSB_FEE { get; set; }

			[FormatCode("S9"), Length(7), LinePos(16)]
			public string LA_DSB_FEE_RFD_PCV { get; set; }

			[FormatCode("X"), Length(1), LinePos(23)]
			public string LC_FEE_COL_STA { get; set; }

			[FormatCode("X"), Length(10), LinePos(24)]
			public string LD_FEE_COL_STA { get; set; }

			[FormatCode("S9"), Length(7), LinePos(34)]
			public string LA_FEE_CAN { get; set; }

			[FormatCode("X"), Length(10), LinePos(41)]
			public string LD_FEE_CAN { get; set; }

			[FormatCode("S9"), Length(7), LinePos(51)]
			public string LA_FEE_RPT_PD { get; set; }

			[FormatCode("X"), Length(10), LinePos(58)]
			public string LD_FEE_RPT_PD { get; set; }

			[FormatCode("S9"), Length(7), LinePos(68)]
			public string LA_FEE_GTR_BAL { get; set; }

			[FormatCode("S9"), Length(7), LinePos(75)]
			public string LA_FEE_CAM_RPT { get; set; }

			[FormatCode("S9"), Length(7), LinePos(82)]
			public string LA_FEE_COL_AT_DSB { get; set; }

			[FormatCode("S9"), Length(7), LinePos(89)]
			public string LA_FEE_LDR_BAL { get; set; }

			[FormatCode("S9"), Length(7), LinePos(96)]
			public string LA_FEE_CAN_PCV_RFD { get; set; }

			[FormatCode("S9"), Length(7), LinePos(103)]
			public string LA_FEE_CAN_RFD { get; set; }

			[FormatCode("X"), Length(10), LinePos(110)]
			public string LD_FEE_CAN_RFD { get; set; }

		}
	}
}
