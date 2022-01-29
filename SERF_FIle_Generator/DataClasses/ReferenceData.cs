﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class ReferenceData : SerfFileBase
	{
        [Ignore]
        public int ReferenceDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string PDEM_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(1), LinePos(40)]
		public string DC_LAG_FGN { get; set; }

		[FormatCode("X"), Length(1), LinePos(41)]
		public string DC_SEX { get; set; }

		[FormatCode("X"), Length(2), LinePos(42)]
		public string DC_ST_DRV_LIC { get; set; }

		[FormatCode("X"), Length(10), LinePos(44)]
		public string DD_BRT { get; set; }

		[FormatCode("X"), Length(10), LinePos(54)]
		public string DD_DRV_LIC_REN { get; set; }

		[FormatCode("X"), Length(10), LinePos(64)]
		public string DD_NME_VER_LST { get; set; }

		[FormatCode("X"), Length(9), LinePos(74)]
		public string DF_ALN_RGS { get; set; }

		[FormatCode("X"), Length(20), LinePos(83)]
		public string DF_DRV_LIC { get; set; }

		[FormatCode("X"), Length(9), Required, LinePos(103)]
		public string DF_PRS_ID { get; set; }

		[FormatCode("X"), Length(1), LinePos(112)]
		public string DI_US_CTZ { get; set; }

		[FormatCode("X"), Length(13), Required, LinePos(113)]
		public string DM_PRS_1 { get; set; }

		[FormatCode("X"), Length(23), Required, LinePos(126)]
		public string DM_PRS_LST { get; set; }

		[FormatCode("X"), Length(4), LinePos(149)]
		public string DM_PRS_LST_SFX { get; set; }

		[FormatCode("X"), Length(13), LinePos(153)]
		public string DM_PRS_MID { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(216)]
		public string DC_ADR { get; set; }

		[FormatCode("X"), Length(2), LinePos(217)]
		public string DC_SRC_ADR { get; set; }

		[FormatCode("X"), Length(2), Required, LinePos(219)]
		public string DC_DOM_ST { get; set; }

		[FormatCode("D"), Length(10), LinePos(221)]
		public string DD_STA_PDEM30 { get; set; }

		[FormatCode("D"), Length(10), LinePos(231)]
		public string DD_VER_ADR { get; set; }

		[FormatCode("X"), Length(17), Required, LinePos(241)]
		public string DF_ZIP_CDE { get; set; }

		[FormatCode("X"), Length(1), LinePos(258)]
		public string DI_VLD_ADR { get; set; }

		[FormatCode("X"), Length(20), Required, LinePos(259)]
		public string DM_CT { get; set; }

		[FormatCode("X"), Length(25), LinePos(279)]
		public string DM_FGN_CNY { get; set; }

		[FormatCode("X"), Length(2), LinePos(304)]
		public string DC_FGN_CNY { get; set; }

		[FormatCode("X"), Length(15), LinePos(306)]
		public string DM_FGN_ST { get; set; }

		[FormatCode("X"), Length(30), Required, LinePos(321)]
		public string DX_STR_ADR_1 { get; set; }

		[FormatCode("X"), Length(30), LinePos(351)]
		public string DX_STR_ADR_2 { get; set; }

		[FormatCode("X"), Length(30), LinePos(381)]
		public string DX_STR_ADR_3 { get; set; }

		[FormatCode("X"), Length(10), LinePos(411)]
		public string DD_DSB_ADR_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(421)]
		public string DD_DSB_ADR_END { get; set; }

		[FormatCode("X"), Length(1), LinePos(511)]
		public string DC_PHN { get; set; }

		[FormatCode("D"), Length(10), LinePos(512)]
		public string DD_PHN_VER { get; set; }

		[FormatCode("X"), Length(1), LinePos(522)]
		public string DI_PHN_VLD { get; set; }

		[FormatCode("X"), Length(1), LinePos(523)]
		public string DI_PHN_WTS { get; set; }

		[FormatCode("X"), Length(3), LinePos(524)]
		public string DN_DOM_PHN_ARA { get; set; }

		[FormatCode("X"), Length(4), LinePos(527)]
		public string DN_DOM_PHN_LCL { get; set; }

		[FormatCode("X"), Length(3), LinePos(531)]
		public string DN_DOM_PHN_XCH { get; set; }

		[FormatCode("X"), Length(3), LinePos(534)]
		public string DN_FGN_PHN_CNY { get; set; }

		[FormatCode("X"), Length(5), LinePos(537)]
		public string DN_FGN_PHN_CT { get; set; }

		[FormatCode("X"), Length(3), LinePos(542)]
		public string DN_FGN_PHN_INL { get; set; }

		[FormatCode("X"), Length(11), LinePos(545)]
		public string DN_FGN_PHN_LCL { get; set; }

		[FormatCode("X"), Length(5), LinePos(556)]
		public string DN_PHN_XTN { get; set; }

		[FormatCode("X"), Length(8), LinePos(561)]
		public string DT_PHN_BST_CL { get; set; }

		[FormatCode("X"), Length(5), LinePos(569)]
		public string DX_PHN_TME_ZNE { get; set; }

		[FormatCode("X"), Length(1), LinePos(574)]
		public string DC_ALW_ADL_PHN { get; set; }

		[FormatCode("X"), Length(1), LinePos(604)]
		public string DC_ADR_EML { get; set; }

		[FormatCode("D"), Length(10), LinePos(605)]
		public string DD_VER_ADR_EML { get; set; }

		[FormatCode("X"), Length(2), LinePos(615)]
		public string DC_SRC_ADR_EML { get; set; }

		[FormatCode("X"), Length(1), LinePos(617)]
		public string DI_VLD_ADR_EML { get; set; }

		[FormatCode("X"), Length(254), LinePos(618)]
		public string DX_ADR_EML_TXT { get; set; }

		[FormatCode("X"), Length(1), LinePos(872)]
		public string DC_DL_COR_TYP { get; set; }

		[FormatCode("X"), Length(2), Required, LinePos(892)]
		public string BC_RFR_REL_BR { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(894)]
		public string BC_RFR_TYP { get; set; }

		[FormatCode("D"), Length(10), LinePos(895)]
		public string BD_EFF_RFR { get; set; }

		[FormatCode("X"), Length(9), LinePos(905)]
		public string BF_RFR { get; set; }

		[FormatCode("X"), Length(1), LinePos(914)]
		public string BI_ATH_3_PTY { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(915)]
		public string BN_SEQ_RFR { get; set; }

		[FormatCode("X"), Length(1), LinePos(919)]
		public string BC_STA_REFR10 { get; set; }

	}
}
