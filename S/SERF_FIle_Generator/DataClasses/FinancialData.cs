using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
	public class FinancialData : SerfFileBase
	{
        [Ignore]
        public int FinancialDataId { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(39)]
		public string LON05_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(7), LinePos(40)]
		public string LA_NSI_AGG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(47)]
		public string LA_PRI_CUR_AGG { get; set; }

		[FormatCode("S9"), Length(7), LinePos(56)]
		public string LA_R78_INT_MAX_AGG { get; set; }

		[FormatCode("S9"), Length(7), LinePos(63)]
		public string LA_R78_INT_PD_AGG { get; set; }

		[FormatCode("S9"), Length(7), LinePos(70)]
		public string LA_R78_INT_UPD_AGG { get; set; }

		[FormatCode("S9"), Length(7), LinePos(77)]
		public string LA_SIN_AGG { get; set; }

		[FormatCode("X"), Length(10), LinePos(84)]
		public string LD_NSI_ACR_THU_AGG { get; set; }

		[FormatCode("X"), Length(10), LinePos(94)]
		public string LD_NSI_PD_THU_AGG { get; set; }

		[FormatCode("X"), Length(10), LinePos(104)]
		public string LD_SIN_ACR_THU_AGG { get; set; }

		[FormatCode("X"), Length(10), LinePos(114)]
		public string LD_SIN_PD_THU_AGG { get; set; }

		[FormatCode("X"), Length(17), LinePos(124)]
		public string LF_BR_OWN_ACC_AGG { get; set; }

		[FormatCode("S9"), Length(4), LinePos(141)]
		public string LN_ACC_CVN_SEQ { get; set; }

		[FormatCode("X"), Length(1), LinePos(145)]
		public string LI_CAP_ATH_AGG { get; set; }

		[FormatCode("X"), Length(10), LinePos(146)]
		public string LD_PCV_LST_CAP_AGG { get; set; }

		[FormatCode("S9"), Length(7), LinePos(156)]
		public string LA_AGG_LTE_FEE_OTS { get; set; }

		[FormatCode("X"), Length(10), LinePos(163)]
		public string LD_AGG_LTE_FEE_WAV { get; set; }

		[FormatCode("S9"), Length(4), LinePos(173)]
		public string LN_RDC_PAY_PCV_AGG { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(177)]
		public string LON09_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(7), LinePos(178)]
		public string LA_R78_INT_UPD { get; set; }

		[FormatCode("D"), Length(10), LinePos(185)]
		public string LD_NPD_PCV { get; set; }

		[FormatCode("X"), Length(10), LinePos(195)]
		public string LD_NSI_LST_PD_PCV { get; set; }

		[FormatCode("X"), Length(1), LinePos(205)]
		public string LI_RBD_RGL_CAT { get; set; }

		[FormatCode("S9"), Length(7), LinePos(206)]
		public string LA_PT_PAY_PCV { get; set; }

		[FormatCode(" S9"), Length(9), LinePos(213)]
		public string  LA_TOT_INT_PAID_PCV { get; set; }

		[FormatCode("S9"), Length(9), LinePos(222)]
		public string LA_STD_STD_ISL_PCV { get; set; }

		[FormatCode("D"), Length(10), LinePos(231)]
		public string LD_25_YR_FGV_PCV { get; set; }

		[FormatCode("S9"), Length(3), LinePos(241)]
		public string LN_IBR_QLF_PAY_PCV { get; set; }

		[FormatCode("D"), Length(10), LinePos(244)]
		public string LD_BRW_LST_PMT_PCV { get; set; }

		[FormatCode("D"), Length(10), LinePos(254)]
		public string LD_LON_RHB_PCV { get; set; }

		[FormatCode("S9"), Length(9), LinePos(264)]
		public string LA_LON_RHB_PCV { get; set; }

		[FormatCode("S9"), Length(9), LinePos(273)]
		public string LA_TOT_PRI_PD_PCV { get; set; }

		[FormatCode("S9"), Length(9), LinePos(282)]
		public string LA_ORG_CAP_INT { get; set; }

		[FormatCode("9"), Length(5), LinePos(291)]
		public string LN_IBR_EHD_DFR_USE { get; set; }

		[FormatCode("X"), Length(16), LinePos(296)]
		public string IF_LON_SRV_DFL_LON { get; set; }

		[FormatCode("S9"), Length(9), LinePos(312)]
		public string LA_TOT_INT_OTS_RHB { get; set; }

		[FormatCode("S9"), Length(9), LinePos(321)]
		public string LA_TOT_COL_CST_RHB { get; set; }

		[FormatCode("S9"), Length(5), LinePos(330)]
		public string LN_PSV_FGV_PAY_CTR { get; set; }

		[FormatCode("S9"), Length(9), LinePos(335)]
		public string LA_PRI_BAL_RPY_BEG { get; set; }

		[FormatCode("S9"), Length(5), LinePos(344)]
		public string LN_IBR_FGV_MTH_CTR { get; set; }

		[FormatCode("S9"), Length(5), LinePos(349)]
		public string LN_ICR_FGV_MTH_CTR { get; set; }

		[FormatCode("S9"), Length(5), LinePos(354)]
		public string LN_ICR_ON_TME_PAY { get; set; }

		[FormatCode("D"), Length(10), LinePos(359)]
		public string LD_NEG_AMR_BEG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(369)]
		public string LA_NEG_AMR_PAY { get; set; }

		[FormatCode("S9"), Length(5), LinePos(378)]
		public string LN_ICR_NEG_AMR_MTH { get; set; }

		[FormatCode("D"), Length(10), LinePos(383)]
		public string LD_ICR_CAP_INT { get; set; }

		[FormatCode("X"), Length(1), LinePos(393)]
		public string LI_TEN_CAP_THD_RCH { get; set; }

		[FormatCode("S9"), Length(9), LinePos(394)]
		public string LA_CUM_NEG_INT_CAP { get; set; }

		[FormatCode("S9"), Length(9), LinePos(403)]
		public string LA_IBR_NEG_AMT_INT { get; set; }

		[FormatCode("S9"), Length(9), LinePos(412)]
		public string LA_ICR_INT_CAP_LTR { get; set; }

		[FormatCode("X"), Length(3), LinePos(421)]
		public string LC_SPS_INC_SRC { get; set; }

		[FormatCode("X"), Length(1), LinePos(424)]
		public string LC_CON_LON_DSB_PIO { get; set; }

		[FormatCode("X"), Length(2), LinePos(425)]
		public string LC_RPD_TYP { get; set; }

		[FormatCode("S9"), Length(3), LinePos(427)]
		public string LN_RPD_TRM_RMN { get; set; }

		[FormatCode("X"), Length(1), LinePos(430)]
		public string LI_RPD_IBR_DLQ_FOR { get; set; }

		[FormatCode("S9"), Length(3), LinePos(431)]
		public string LN_MTH_GRC_DFR_DLC { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(700)]
		public string LON10_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(701)]
		public string LN_SEQ { get; set; }

		[FormatCode("X"), Length(6), Required, LinePos(705)]
		public string IC_LON_PGM { get; set; }

		[FormatCode("X"), Length(8), LinePos(711)]
		public string IF_DOE_LDR { get; set; }

		[FormatCode("X"), Length(6), Required, LinePos(719)]
		public string IF_GTR { get; set; }

		[FormatCode("X"), Length(9), LinePos(725)]
		public string LF_STU_SSN { get; set; }

		[FormatCode("S9"), Length(8), LinePos(734)]
		public string LA_CUR_ILG { get; set; }

		[FormatCode("S9"), Length(8), LinePos(742)]
		public string LA_CUR_PRI { get; set; }

		[FormatCode("S9"), Length(8), LinePos(750)]
		public string LA_ILG { get; set; }

		[FormatCode("S9"), Length(8), Required, LinePos(758)]
		public string LA_LON_AMT_GTR { get; set; }

		[FormatCode("S9"), Length(7), Required, LinePos(766)]
		public string LA_NSI_OTS { get; set; }

		[FormatCode("S9"), Length(7), LinePos(773)]
		public string LA_R78_INT_MAX { get; set; }

		[FormatCode("S9"), Length(7), LinePos(780)]
		public string LA_R78_INT_PD { get; set; }

		[FormatCode("X"), Length(1), LinePos(787)]
		public string LI_1_TME_BR { get; set; }

		[FormatCode("X"), Length(1), LinePos(788)]
		public string LC_RPR_TYP { get; set; }

		[FormatCode("X"), Length(10), LinePos(789)]
		public string LD_CAP_LST_PIO_CVN { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(799)]
		public string LD_END_GRC_PRD { get; set; }

		[FormatCode("X"), Length(10), LinePos(809)]
		public string LD_GTE_LOS { get; set; }

		[FormatCode("X"), Length(10), LinePos(819)]
		public string LD_ILG_NTF { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(829)]
		public string LD_LON_GTR { get; set; }

		[FormatCode("X"), Length(10), Required, LinePos(839)]
		public string LD_NSI_ACR_THU { get; set; }

		[FormatCode("D"), Length(10), LinePos(849)]
		public string LD_PNT_SIG { get; set; }

		[FormatCode("X"), Length(10), LinePos(859)]
		public string LD_SCL_CLS_NTF { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(869)]
		public string LD_TRM_BEG { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(879)]
		public string LD_TRM_END { get; set; }

		[FormatCode("X"), Length(8), Required, LinePos(889)]
		public string LF_DOE_SCL_ORG { get; set; }

		[FormatCode("X"), Length(12), LinePos(897)]
		public string LF_GTR_RFR { get; set; }

		[FormatCode("X"), Length(1), LinePos(909)]
		public string LI_CAP_ALW { get; set; }

		[FormatCode("X"), Length(1), LinePos(910)]
		public string LI_ELG_SPA { get; set; }

		[FormatCode("X"), Length(1), LinePos(911)]
		public string LI_FGV_PGM { get; set; }

		[FormatCode("X"), Length(1), LinePos(912)]
		public string LI_GTR_NAT { get; set; }

		[FormatCode("S9"), Length(3), Required, LinePos(913)]
		public string LN_MTH_GRC_PRD_DSC { get; set; }

		[FormatCode("S9"), Length(7), LinePos(916)]
		public string LA_SIN_OTS_PCV { get; set; }

		[FormatCode("D"), Length(10), LinePos(923)]
		public string LD_SIN_ACR_THU_PCV { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(933)]
		public string LD_SIN_LST_PD_PCV { get; set; }

		[FormatCode("X"), Length(3), LinePos(943)]
		public string LC_STA_NEW_BR { get; set; }

		[FormatCode("X"), Length(2), LinePos(946)]
		public string LC_SCY_PGA { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(948)]
		public string LD_LON_1_DSB { get; set; }

		[FormatCode("X"), Length(2), LinePos(958)]
		public string LC_ACA_GDE_LEV { get; set; }

		[FormatCode("X"), Length(1), LinePos(960)]
		public string LC_SCY_PGA_PGM_YR { get; set; }

		[FormatCode("X"), Length(3), LinePos(961)]
		public string IC_HSP_CSE { get; set; }

		[FormatCode("X"), Length(1), LinePos(964)]
		public string LI_TL4_793_XCL_CON { get; set; }

		[FormatCode("X"), Length(1), LinePos(965)]
		public string LI_DFR_REQ_ON_APL { get; set; }

		[FormatCode("X"), Length(1), LinePos(966)]
		public string LI_LN_PT_COM_APL { get; set; }

		[FormatCode("S9"), Length(5), LinePos(967)]
		public string LR_WIR_CON_LON { get; set; }

		[FormatCode("X"), Length(1), LinePos(972)]
		public string LC_ELG_RDC_PGM { get; set; }

		[FormatCode("X"), Length(10), LinePos(973)]
		public string LD_ELG_RDC_PGM { get; set; }

		[FormatCode("X"), Length(1), LinePos(983)]
		public string LC_RPD_SLE { get; set; }

		[FormatCode("S9"), Length(5), LinePos(984)]
		public string LR_ITR_ORG { get; set; }

		[FormatCode("X"), Length(2), LinePos(989)]
		public string LC_ITR_TYP_ORG { get; set; }

		[FormatCode("X"), Length(1), LinePos(991)]
		public string LC_TIR_GRP { get; set; }

		[FormatCode("X"), Length(3), LinePos(992)]
		public string IF_TIR_PCE { get; set; }

		[FormatCode("S9"), Length(3), LinePos(995)]
		public string LN_RDC_PGM_PAY_PCV { get; set; }

		[FormatCode("X"), Length(1), LinePos(998)]
		public string LI_RTE_RDC_ELG { get; set; }

		[FormatCode("S9"), Length(7), LinePos(999)]
		public string LA_LTE_FEE_OTS { get; set; }

		[FormatCode("X"), Length(10), LinePos(1006)]
		public string LD_LON_LTE_FEE_WAV { get; set; }

		[FormatCode("X"), Length(3), LinePos(1016)]
		public string LC_CUR_RDC_PGM_NME { get; set; }

		[FormatCode("X"), Length(1), LinePos(1019)]
		public string LI_RIR_SCY_ELG { get; set; }

		[FormatCode("X"), Length(17), Required, LinePos(1020)]
		public string LF_LON_ALT { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(1037)]
		public string LN_LON_ALT_SEQ { get; set; }

		[FormatCode("X"), Length(1), LinePos(1041)]
		public string LI_LDR_LST_RST_DSB { get; set; }

		[FormatCode("D"), Length(10), Required, LinePos(1042)]
		public string LD_LON_APL_RCV { get; set; }

		[FormatCode("X"), Length(1), LinePos(1052)]
		public string LC_MPN_TYP { get; set; }

		[FormatCode("D"), Length(10), LinePos(1053)]
		public string LD_MPN_EXP { get; set; }

		[FormatCode("X"), Length(1), LinePos(1063)]
		public string LC_MPN_SRL_LON { get; set; }

		[FormatCode("X"), Length(2), LinePos(1064)]
		public string LC_MPN_REV_REA { get; set; }

		[FormatCode("X"), Length(8), LinePos(1066)]
		public string LF_ORG_RGN { get; set; }

		[FormatCode("X"), Length(10), LinePos(1074)]
		public string LD_AMR_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(1084)]
		public string LD_ORG_XPC_GRD { get; set; }

		[FormatCode("S9"), Length(6), LinePos(1094)]
		public string LR_SCL_SUB { get; set; }

		[FormatCode("X"), Length(1), LinePos(1100)]
		public string LI_LDR_BG_APL { get; set; }

		[FormatCode("X"), Length(1), LinePos(1101)]
		public string LI_ESG { get; set; }

		[FormatCode("S9"), Length(10), LinePos(1102)]
		public string LA_TOT_EDU_DET { get; set; }

		[FormatCode("X"), Length(11), LinePos(1112)]
		public string LF_MN_MST_NTE { get; set; }

		[FormatCode("S9"), Length(4), LinePos(1123)]
		public string LN_MN_MST_NTE_SEQ { get; set; }

		[FormatCode("X"), Length(7), LinePos(1127)]
		public string PC_PNT_YR { get; set; }

		[FormatCode("X"), Length(4), LinePos(1134)]
		public string LF_CRD_RTE_SRE { get; set; }

		[FormatCode("X"), Length(2), LinePos(1138)]
		public string LC_ST_BR_RSD_APL { get; set; }

		[FormatCode("S9"), Length(8), LinePos(1140)]
		public string LA_INT_FEE_URP_IRS { get; set; }

		[FormatCode("X"), Length(1), LinePos(1148)]
		public string LI_MN_PSD_BS { get; set; }

		[FormatCode("X"), Length(9), LinePos(1149)]
		public string LF_ESG_SRC { get; set; }

		[FormatCode("X"), Length(1), LinePos(1158)]
		public string LI_MNT_BIL_RCP { get; set; }

		[FormatCode("S9"), Length(6), LinePos(1159)]
		public string LA_BS_POI { get; set; }

		[FormatCode("X"), Length(1), LinePos(1165)]
		public string LC_ESG { get; set; }

		[FormatCode("X"), Length(1), LinePos(1166)]
		public string LC_UDL_DSB_COF { get; set; }

		[FormatCode("S9"), Length(3), LinePos(1167)]
		public string LN_BBS_PCV_PAY_MOT { get; set; }

		[FormatCode("X"), Length(1), LinePos(1170)]
		public string LI_BR_LT_HT   { get; set; }

		[FormatCode("X"), Length(2), LinePos(1171)]
		public string LC_ESP_RPD_OPT_SEL   { get; set; }

		[FormatCode("D"), Length(10), LinePos(1173)]
		public string LD_BBS_DSQ { get; set; }

		[FormatCode("X"), Length(2), LinePos(1183)]
		public string LC_BBS_DSQ_REA { get; set; }

		[FormatCode("X"), Length(1), LinePos(1185)]
		public string LC_ELG_95_SPA_BIL { get; set; }

		[FormatCode("X"), Length(23), LinePos(1186)]
		public string LF_GTR_RFR_XTN { get; set; }

		[FormatCode("X"), Length(2), LinePos(1209)]
		public string LC_SGM_COS_PRC { get; set; }

		[FormatCode("X"), Length(1), LinePos(1211)]
		public string LI_OO_PST_ENR_DFR { get; set; }

		[FormatCode("X"), Length(10), LinePos(1212)]
		public string LD_OO_PST_ENR_DFR { get; set; }

		[FormatCode("X"), Length(1), LinePos(1222)]
		public string LC_TL4_IBR_ELG         { get; set; }

		[FormatCode("S9"), Length(9), LinePos(1223)]
		public string LA_MSC_FEE_OTS { get; set; }

		[FormatCode("S9"), Length(9), LinePos(1232)]
		public string LA_MSC_FEE_PCV_OTS   { get; set; }

		[FormatCode("PIC X"), Length(6), LinePos(1241)]
		public string LF_FED_CLC_RSK         { get; set; }

		[FormatCode("PIC X"), Length(4), LinePos(1247)]
		public string LF_FED_FFY_1_DSB       { get; set; }

		[FormatCode("PIC X"), Length(3), LinePos(1251)]
		public string LC_FED_PGM_YR          { get; set; }

		[FormatCode("PIC X"), Length(6), LinePos(1254)]
		public string LF_PRV_GTR             { get; set; }

		[FormatCode("S9"), Length(9), LinePos(1260)]
		public string LA_INT_RCV_GOV       { get; set; }

		[FormatCode("X"), Length(4), LinePos(1269)]
		public string LC_VRS_ALT_APL         { get; set; }

		[FormatCode("X"), Length(2), LinePos(1273)]
		public string LF_LON_DCV_CLI { get; set; }

		[FormatCode("S9"), Length(4), LinePos(1275)]
		public string LN_LON_SEQ_DCV_CLI { get; set; }

		[FormatCode("D"), Length(10), LinePos(1279)]
		public string LD_LON_IBR_ENT { get; set; }

		[FormatCode("X"), Length(1), LinePos(1289)]
		public string LI_BR_DET_RPD_XTN { get; set; }

		[FormatCode("X"), Length(10), LinePos(1290)]
		public string LD_EFF_LBR_RTE { get; set; }

		[FormatCode("X"), Length(10), LinePos(1300)]
		public string LD_FAT_PRI_BAL_ZRO { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(1669)]
		public string LN_LON12_DAT_OCC_CNT { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(1697)]
		public string LON35_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(8), Required, LinePos(1698)]
		public string IF_OWN { get; set; }

		[FormatCode("X"), Length(8), Required, LinePos(1706)]
		public string IF_BND_ISS { get; set; }

		[FormatCode("X"), Length(7), LinePos(1714)]
		public string IF_LON_SLE { get; set; }

		[FormatCode("X"), Length(3), LinePos(1721)]
		public string LC_LOC_PNT { get; set; }

		[FormatCode("X"), Length(10), LinePos(1724)]
		public string LD_OWN_EFF_SR { get; set; }

		[FormatCode("X"), Length(17), LinePos(1734)]
		public string LF_BR_LON_OWN_ACC { get; set; }

		[FormatCode("X"), Length(20), LinePos(1751)]
		public string LF_CUR_POR { get; set; }

		[FormatCode("X"), Length(20), LinePos(1771)]
		public string LF_OWN_ORG_POR { get; set; }

		[FormatCode("X"), Length(3), LinePos(1791)]
		public string IF_TIR_PCE_1 { get; set; }

		[FormatCode("X"), Length(10), LinePos(1794)]
		public string LD_LON_IRL_SLE_TRF { get; set; }

		[FormatCode("X"), Length(8), LinePos(1804)]
		public string LF_OWN_EFT_RIR_ASN { get; set; }

		[FormatCode("X"), Length(7), LinePos(1812)]
		public string LA_LON_LVL_TRF_FEE { get; set; }

		[FormatCode("X"), Length(10), LinePos(1819)]
		public string LD_PRE_CVN_OWN_BEG { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(1832)]
		public string LN_LON32_DAT_OCC_CNT { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(1918)]
		public string LON72_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(1), LinePos(1919)]
		public string LC_ELG_SIN { get; set; }

		[FormatCode("X"), Length(2), LinePos(1920)]
		public string LC_ITR_TYP { get; set; }

		[FormatCode("X"), Length(1), LinePos(1922)]
		public string LC_STA_LON72 { get; set; }

		[FormatCode("D"), Length(10), LinePos(1923)]
		public string LD_STA_LON72 { get; set; }

		[FormatCode("X"), Length(1), LinePos(1933)]
		public string LI_SPC_ITR { get; set; }

		[FormatCode("S9"), Length(5), Required, LinePos(1934)]
		public string LR_ITR { get; set; }

		[FormatCode("S9"), Length(5), LinePos(1939)]
		public string LR_INT_RDC_PGM_ORG { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(1959)]
		public string LON84_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(3), LinePos(1960)]
		public string LC_RDC_PGM_NME { get; set; }

		[FormatCode("X"), Length(10), LinePos(1963)]
		public string LD_RDC_EFF_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(1973)]
		public string LD_RDC_EFF_END { get; set; }

		[FormatCode("X"), Length(1), LinePos(1983)]
		public string LC_STA_LON84 { get; set; }

		[FormatCode("S9"), Length(5), LinePos(1984)]
		public string LR_RDC { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(1989)]
		public string LON86_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(3), LinePos(1990)]
		public string LC_RDC_PGM_NME_1 { get; set; }

		[FormatCode("X"), Length(10), LinePos(1993)]
		public string LD_RDC_EFF_AGG_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(2003)]
		public string LD_RDC_EFF_AGG_END { get; set; }

		[FormatCode("S9"), Length(5), LinePos(2013)]
		public string LR_RDC_AGG { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(2018)]
		public string TAX_DAT_ENT_IND { get; set; }

		[FormatCode("S9"), Length(8), LinePos(2019)]
		public string TA_OID_PD_RPT { get; set; }

		[FormatCode("S9"), Length(8), LinePos(2027)]
		public string TA_CAP_ELG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(2035)]
		public string TA_GTR_OID_ELG { get; set; }

		[FormatCode("S9"), Length(9), LinePos(2044)]
		public string TA_GTR_CAP_ELG { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(2083)]
		public string LN_LON74_DAT_OCC_CNT { get; set; }

		[FormatCode("S9"), Length(4), Required, LinePos(2211)]
		public string LN_LON26_DAT_OCC_CNT { get; set; }

		[FormatCode("X"), Length(1), Required, LinePos(2257)]
		public string FS10_DAT_ENT_IND { get; set; }

		[FormatCode("X"), Length(18), LinePos(2258)]
		public string LF_FED_AWD { get; set; }

		[FormatCode("X"), Length(3), LinePos(2276)]
		public string LN_FED_AWD_SEQ { get; set; }

		[FormatCode("X"), Length(10), LinePos(2279)]
		public string LD_ITL_LN_SLD_DOE { get; set; }

		[FormatCode("X"), Length(9), LinePos(2289)]
		public string LF_ORG_DL_BR_SSN { get; set; }

		[FormatCode("X"), Length(10), LinePos(2298)]
		public string LD_ORG_DL_BR_DOB { get; set; }

		[FormatCode("X"), Length(9), LinePos(2308)]
		public string LF_ORG_DL_STU_SSN { get; set; }

		[FormatCode("X"), Length(10), LinePos(2317)]
		public string LD_ORG_DL_STU_DOB { get; set; }

		[FormatCode("X"), Length(9), LinePos(2327)]
		public string LF_ORG_DL_EDS_SSN { get; set; }

		[FormatCode("X"), Length(4), LinePos(2336)]
		public string LF_DL_FIN_AWD_YR { get; set; }

		[FormatCode("X"), Length(10), LinePos(2340)]
		public string LD_DL_STU_ACA_BEG { get; set; }

		[FormatCode("X"), Length(10), LinePos(2350)]
		public string LD_DL_STU_ACA_END { get; set; }

		[FormatCode("X"), Length(1), LinePos(2360)]
		public string LI_DL_BR_ELG_HPA { get; set; }

		[FormatCode("X"), Length(1), LinePos(2361)]
		public string LC_DL_STU_DEP_STA { get; set; }

		[FormatCode("X"), Length(2), LinePos(2362)]
		public string LF_ORG_DL_LON { get; set; }

		[FormatCode("X"), Length(1), LinePos(2364)]
		public string LC_DL_STA_MPN { get; set; }

		[FormatCode("X"), Length(21), LinePos(2365)]
		public string LF_DL_MPN { get; set; }

		[FormatCode("X"), Length(1), LinePos(2386)]
		public string LI_DL_XED_USB_LMT { get; set; }

		[FormatCode("X"), Length(1), LinePos(2387)]
		public string LI_FRC_ICR { get; set; }

		[FormatCode("X"), Length(2), LinePos(2388)]
		public string LC_DL_PRV_RPY_PLN { get; set; }

		[FormatCode("X"), Length(10), LinePos(2390)]
		public string LF_FED_LON_PR_GRP { get; set; }

		[Occurs(3), LinePos(1673)]
		public List<LON12_DAT> ListOfLON12_DAT { get; set; }
		public class LON12_DAT
		{
			[FormatCode("X"), Length(1), LinePos(0)]
			public string LC_LON_FEE { get; set; }

			[FormatCode("S9"), Length(7), LinePos(1)]
			public string LA_SUP_FEE_PCV { get; set; }

		}
		[Occurs(2), LinePos(1836)]
		public List<LON32_DAT> ListOfLON32_DAT { get; set; }
		public class LON32_DAT
		{
			[FormatCode("X"), Length(10), LinePos(0)]
			public string LD_SPA_STP { get; set; }

			[FormatCode("X"), Length(2), LinePos(10)]
			public string LC_SPA_REA_STP { get; set; }

			[FormatCode("X"), Length(10), LinePos(30)]
			public string LD_SPA_RTT { get; set; }

			[FormatCode("X"), Length(1), LinePos(40)]
			public string LC_STA_LON32 { get; set; }

		}
		[Occurs(4), LinePos(2087)]
		public List<LON74_DAT> ListOfLON74_DAT { get; set; }
		public class LON74_DAT
		{
			[FormatCode("S9"), Length(5), LinePos(0)]
			public string LR_LON_WIR { get; set; }

			[FormatCode("D"), Length(10), LinePos(5)]
			public string LD_LON_WIR_EFF_BEG { get; set; }

			[FormatCode("X"), Length(10), LinePos(15)]
			public string LD_LON_WIR_EFF_END { get; set; }

			[FormatCode("X"), Length(1), LinePos(25)]
			public string LI_LON_WIR_OVR { get; set; }

			[FormatCode("S9"), Length(5), LinePos(26)]
			public string LR_LON_WIR_PRV { get; set; }

		}
		[Occurs(3), LinePos(2215)]
		public List<LON26_DAT> ListOfLON26_DAT { get; set; }
		public class LON26_DAT
		{
			[FormatCode("S9"), Length(4), LinePos(0)]
			public string LN_LON_CRD_SEQ { get; set; }

			[FormatCode("X"), Length(6), LinePos(4)]
			public string IF_CRB { get; set; }

			[FormatCode("X"), Length(4), LinePos(10)]
			public string LF_CRD_SRE { get; set; }

		}
	}
}
