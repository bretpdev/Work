using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class StudentData : SerfFileBase
	{
        [Ignore]
        public int StudentDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string PDEM_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(1), LinePos(40)]
		public string DC_LAG_FGN { get; set; }

		[FormatCode("X"), Length(1), LinePos(41)]
		public string DC_SEX { get; set; }

		[FormatCode("X"), Length(2), LinePos(42)]
		public string DC_ST_DRV_LIC { get; set; }

		[FormatCode("D"), Length(10), LinePos(44)]
		public string DD_BRT { get; set; }

		[FormatCode("X"), Length(10), LinePos(54)]
		public string DD_DRV_LIC_REN { get; set; }

		[FormatCode("D"), Length(10), LinePos(64)]
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

		[FormatCode("X"), Length(9), Required, LinePos(892)]
		public string LF_STU_SSN { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(901)]
		public string STU10_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(2), LinePos(902)]
		public string LC_REA_SCL_SPR { get; set; }

		[FormatCode("X"), Length(2), LinePos(904)]
		public string LC_SCR_SCL_SPR { get; set; }

		[FormatCode("X"), Length(1), LinePos(906)]
		public string LC_STA_STU10 { get; set; }

		[FormatCode("D"), Length(10), LinePos(907)]
		public string LD_NTF_SCL_SPR { get; set; }

		[FormatCode("D"), Length(10), LinePos(917)]
		public string LD_SCL_SPR { get; set; }

		[FormatCode("D"), Length(10), LinePos(927)]
		public string LD_STA_STU10 { get; set; }

		[FormatCode("X"), Length(6), LinePos(937)]
		public string IF_HSP { get; set; }

		[FormatCode("X"), Length(8), LinePos(943)]
		public string LF_DOE_SCL_ENR_CUR { get; set; }

		[FormatCode("S9"), Length(4), LinePos(951)]
		public string LN_STU_SPR_SEQ { get; set; }

		[FormatCode("D"), Length(10), LinePos(955)]
		public string LD_SCL_CER_STU_STA { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(995)]
		public string STU20_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(1), LinePos(996)]
		public string LC_STA_STU_ENR { get; set; }

		[FormatCode("D"), Length(10), LinePos(997)]
		public string LD_ENR_BEG { get; set; }

		[FormatCode("D"), Length(10), LinePos(1007)]
		public string LD_ENR_END { get; set; }

		[FormatCode("S9"), Length(4), LinePos(1017)]
		public string LN_STU_ENR_SEQ { get; set; }

		[FormatCode("X"), Length(6), LinePos(1021)]
		public string IF_HSP_1 { get; set; }

		[FormatCode("X"), Length(8), LinePos(1027)]
		public string IF_DOE_SCL { get; set; }

		[FormatCode("X"), Length(1), LinePos(1035)]
		public string LON13_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(4), LinePos(1036)]
		public string LN_SEQ { get; set; }

		[FormatCode("S9"), Length(4), LinePos(1040)]
		public string LN_STU_SPR_SEQ_1 { get; set; }

		[FormatCode("X"), Length(10), LinePos(1044)]
		public string LD_END_GRC_PRD_ALI { get; set; }

		[FormatCode("X"), Length(1), LinePos(1054)]
		public string LON03_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(4), LinePos(1055)]
		public string LN_STU_SPR_SEQ_2 { get; set; }

		[FormatCode("S9"), Length(3), LinePos(1059)]
		public string LN_MTH_GRC_PRD_AGG { get; set; }

		[FormatCode("X"), Length(10), LinePos(1062)]
		public string LD_END_GRC_PRD_AGG { get; set; }

	}
}
