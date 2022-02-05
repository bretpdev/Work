using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class RepaymentData : SerfFileBase
	{
        [Ignore]
        public int RepaymentDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string RPST10_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(1), LinePos(40)]
		public string LC_FRQ_PAY { get; set; }

		[FormatCode("X"), Length(1), LinePos(41)]
		public string LC_RPD_DIS { get; set; }

		[FormatCode("X"), Length(1), LinePos(42)]
		public string LC_STA_RPST10 { get; set; }

		[FormatCode("D"), Length(10), LinePos(43)]
		public string LD_RPS_1_PAY_DU { get; set; }

		[FormatCode("X"), Length(10), LinePos(53)]
		public string LD_RTN_RPD_DIS { get; set; }

		[FormatCode("D"), Length(10), LinePos(63)]
		public string LD_SNT_RPD_DIS { get; set; }

		[FormatCode("D"), Length(10), LinePos(73)]
		public string LD_STA_RPST10 { get; set; }

		[FormatCode("X"), Length(1), LinePos(83)]
		public string LI_SIG_RPD_DIS { get; set; }

		[FormatCode("S9"), Length(4), LinePos(84)]
		public string LN_RPS_SEQ { get; set; }

		[FormatCode("X"), Length(1), LinePos(88)]
		public string LC_RPY_FIX_TRM_AMT { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(118)]
		public string LON06_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(9), LinePos(119)]
		public string LA_CPI_RPD_DIS_AGG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(128)]
		public string LA_IC_RPD_DIS_AGG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(137)]
		public string LA_RPD_INT_DIS_AGG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(146)]
		public string LA_TOT_RPD_DIS_AGG { get; set; }

		[FormatCode("X"), Length(1), LinePos(155)]
		public string LC_STA_LON06 { get; set; }

		[FormatCode("S9"), Length(5), LinePos(156)]
		public string LR_INT_RPD_DIS_AGG { get; set; }

		[FormatCode("X"), Length(2), LinePos(161)]
		public string LC_TYP_SCH_DIS { get; set; }

		[FormatCode("S9"), Length(7), LinePos(163)]
		public string LA_ACR_INT_RPD_AGG { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(190)]
		public string LN_LON07_DAT_OCC_CNT { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(474)]
		public string LON65_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(475)]
		public string LN_SEQ { get; set; }

		[FormatCode("S9"), Length(8), LinePos(479)]
		public string LA_CPI_RPD_DIS { get; set; }

		[FormatCode("S9"), Length(8), LinePos(487)]
		public string LA_RPD_INT_DIS { get; set; }

		[FormatCode("S9"), Length(8), LinePos(495)]
		public string LA_TOT_RPD_DIS { get; set; }

		[FormatCode("X"), Length(1), LinePos(503)]
		public string LC_STA_LON65 { get; set; }

		[FormatCode("S9"), Length(5), LinePos(504)]
		public string LR_INT_RPD_DIS { get; set; }

		[FormatCode("S9"), Length(7), LinePos(509)]
		public string LA_ANT_CAP { get; set; }

		[FormatCode("X"), Length(2), LinePos(516)]
		public string LC_TYP_SCH_DIS_1 { get; set; }

		[FormatCode("S9"), Length(7), LinePos(518)]
		public string LA_ACR_INT_RPD { get; set; }

		[FormatCode("X"), Length(10), LinePos(525)]
		public string LD_RPD_MAX_TRM_SR { get; set; }

		[FormatCode("S9"), Length(3), LinePos(535)]
		public string LN_RPD_MAX_TRM_REQ { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(568)]
		public string LN_LON66_DAT_OCC_CNT { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(772)]
		public string LN_LON04_DAT_OCC_CNT { get; set; }

		[FormatCode("X"), Length(1), LinePos(822)]
		public string RS05_DAT_ENT_IND        { get; set; }

		[FormatCode("D"), Length(10), LinePos(823)]
		public string BD_CRT_RS05           { get; set; }

		[FormatCode("S9"), Length(4), LinePos(833)]
		public string BN_IBR_SEQ          { get; set; }

		[FormatCode("X"), Length(9), LinePos(837)]
		public string BF_SSN_SPO { get; set; }

		[FormatCode("X"), Length(3), LinePos(846)]
		public string BC_IBR_INF_SRC_VER { get; set; }

		[FormatCode("X"), Length(1), LinePos(849)]
		public string BC_IRS_TAX_FIL_STA { get; set; }

		[FormatCode("S9"), Length(12), LinePos(850)]
		public string BA_AGI { get; set; }

		[FormatCode("X"), Length(2), LinePos(862)]
		public string BN_MEM_HSE_HLD { get; set; }

		[FormatCode("X"), Length(1), LinePos(864)]
		public string BI_JNT_BR_SPO_RPY { get; set; }

		[FormatCode("X"), Length(1), LinePos(897)]
		public string RS20_DAT_ENT_IND        { get; set; }

		[FormatCode("S9"), Length(12), LinePos(898)]
		public string LA_PFH_PAY          { get; set; }

		[FormatCode("S9"), Length(12), LinePos(910)]
		public string LA_PMN_STD_PAY { get; set; }

		[Occurs(20), LinePos(194)]
		public List<LON07_DAT> ListOfLON07_DAT { get; set; }
		public class LON07_DAT
		{
			[FormatCode("S9"), Length(7), LinePos(0)]
			public string LA_RPS_ISL_AGG { get; set; }

			[FormatCode("S9"), Length(4), LinePos(7)]
			public string LN_GRD_RPS_SEQ_AGG { get; set; }

			[FormatCode("S9"), Length(3), LinePos(11)]
			public string LN_RPS_TRM_AGG { get; set; }

		}
		[Occurs(20), LinePos(572)]
		public List<LON66_DAT> ListOfLON66_DAT { get; set; }
		public class LON66_DAT
		{
			[FormatCode("S9"), Length(7), LinePos(0)]
			public string LA_RPS_ISL { get; set; }

			[FormatCode("S9"), Length(3), LinePos(7)]
			public string LN_RPS_TRM { get; set; }

		}
		[Occurs(2), LinePos(776)]
		public List<LON04_DAT> ListOfLON04_DAT { get; set; }
		public class LON04_DAT
		{
			[FormatCode("D"), Length(10), LinePos(0)]
			public string LD_PCB_NPD { get; set; }

			[FormatCode("X"), Length(1), LinePos(10)]
			public string LI_PSB_TOT_RPD_AGG { get; set; }

			[FormatCode("S9"), Length(4), LinePos(11)]
			public string LN_AGG_RPD_SEQ { get; set; }

			[FormatCode("X"), Length(1), LinePos(15)]
			public string LC_TYP_PCV_BIL_DAT { get; set; }

			[FormatCode("S9"), Length(7), LinePos(16)]
			public string LA_PT_PAY_PCV_AGG { get; set; }

		}
	}
}
