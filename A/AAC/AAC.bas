'Version 1.0.0.4680
'which file is the script trying to process
Private Enum FileTypeInProcess
    rehab = 0
    Repurch = 1
    tilp = 2
End Enum

Private Type RehabRec
    'borrower level
    BF_SSN As String
    DM_PRS_1 As String
    DM_PRS_MID As String
    DM_PRS_LST As String
    DD_BRT As String
    DX_STR_ADR_1 As String
    DX_STR_ADR_2 As String
    DM_CT As String
    DC_DOM_ST As String
    DF_ZIP As String
    DM_FGN_CNY As String
    DI_VLD_ADR As String
    DN_PHN As String
    DI_PHN_VLD As String
    DN_ALT_PHN As String
    DI_ALT_PHN_VLD As String
    
    'loan level
    AC_LON_TYP As String
    SUBSIDY As String
    AD_PRC As String
    AF_ORG_APL_OPS_LDR As String
    AF_APL_ID As String
    AF_APL_ID_SFX As String
    AD_IST_TRM_BEG As String
    AD_IST_TRM_END As String
    AA_GTE_LON_AMT As String
    AF_APL_OPS_SCL As String
    AD_BR_SIG As String
    LD_LFT_SCL As String
    PR_RPD_FOR_ITR As String
    LC_INT_TYP As String
    LD_TRX_EFF As String
    LA_TRX As String
    LD_RHB As String
    LA_PRI As String
    LA_INT As String
    IF_OPS_SCL_RPT As String
    LC_STU_ENR_TYP As String
    LD_ENR_CER As String
    LD_LDR_NTF As String
    AR_CON_ITR As String
    AD_APL_RCV As String
    AC_STU_DFR_REQ As String
    
    'disbursment info loan level
    AN_DISB_1 As String 'disb seq num
    AC_DISB_1 As String 'disb adj code
    AD_DISB_1 As String 'Disb Date
    AA_DISB_1 As String 'Disb Amt
    ORG_1 As String     'origination Fee
    CD_DISB_1 As String 'disb cancelled date
    CA_DISB_1 As String 'disb cancelled amount
    GTE_1 As String
    AN_DISB_2 As String
    AC_DISB_2 As String
    AD_DISB_2 As String
    AA_DISB_2 As String
    ORG_2 As String
    CD_DISB_2 As String
    CA_DISB_2 As String
    GTE_2 As String
    AN_DISB_3 As String
    AC_DISB_3 As String
    AD_DISB_3 As String
    AA_DISB_3 As String
    ORG_3 As String
    CD_DISB_3 As String
    CA_DISB_3 As String
    GTE_3 As String
    AN_DISB_4 As String
    AC_DISB_4 As String
    AD_DISB_4 As String
    AA_DISB_4 As String
    ORG_4 As String
    CD_DISB_4 As String
    CA_DISB_4 As String
    GTE_4 As String
    AA_TOT_EDU_DET_PNT As String
    
    'deferment info loan level
    LC_DFR_TYP1 As String 'type
    LC_DFR_TYP2 As String
    LC_DFR_TYP3 As String
    LC_DFR_TYP4 As String
    LC_DFR_TYP5 As String
    LC_DFR_TYP6 As String
    LC_DFR_TYP7 As String
    LC_DFR_TYP8 As String
    LC_DFR_TYP9 As String
    LC_DFR_TYP10 As String
    LC_DFR_TYP11 As String
    LC_DFR_TYP12 As String
    LC_DFR_TYP13 As String
    LC_DFR_TYP14 As String
    LC_DFR_TYP15 As String
    LD_DFR_BEG1 As String 'begin date
    LD_DFR_BEG2 As String
    LD_DFR_BEG3 As String
    LD_DFR_BEG4 As String
    LD_DFR_BEG5 As String
    LD_DFR_BEG6 As String
    LD_DFR_BEG7 As String
    LD_DFR_BEG8 As String
    LD_DFR_BEG9 As String
    LD_DFR_BEG10 As String
    LD_DFR_BEG11 As String
    LD_DFR_BEG12 As String
    LD_DFR_BEG13 As String
    LD_DFR_BEG14 As String
    LD_DFR_BEG15 As String
    LD_DFR_END1 As String 'end date
    LD_DFR_END2 As String
    LD_DFR_END3 As String
    LD_DFR_END4 As String
    LD_DFR_END5 As String
    LD_DFR_END6 As String
    LD_DFR_END7 As String
    LD_DFR_END8 As String
    LD_DFR_END9 As String
    LD_DFR_END10 As String
    LD_DFR_END11 As String
    LD_DFR_END12 As String
    LD_DFR_END13 As String
    LD_DFR_END14 As String
    LD_DFR_END15 As String
    LF_DOE_SCL_DFR1 As String 'school code
    LF_DOE_SCL_DFR2 As String
    LF_DOE_SCL_DFR3 As String
    LF_DOE_SCL_DFR4 As String
    LF_DOE_SCL_DFR5 As String
    LF_DOE_SCL_DFR6 As String
    LF_DOE_SCL_DFR7 As String
    LF_DOE_SCL_DFR8 As String
    LF_DOE_SCL_DFR9 As String
    LF_DOE_SCL_DFR10 As String
    LF_DOE_SCL_DFR11 As String
    LF_DOE_SCL_DFR12 As String
    LF_DOE_SCL_DFR13 As String
    LF_DOE_SCL_DFR14 As String
    LF_DOE_SCL_DFR15 As String
    LD_DFR_INF_CER1 As String 'cert date
    LD_DFR_INF_CER2 As String
    LD_DFR_INF_CER3 As String
    LD_DFR_INF_CER4 As String
    LD_DFR_INF_CER5 As String
    LD_DFR_INF_CER6 As String
    LD_DFR_INF_CER7 As String
    LD_DFR_INF_CER8 As String
    LD_DFR_INF_CER9 As String
    LD_DFR_INF_CER10 As String
    LD_DFR_INF_CER11 As String
    LD_DFR_INF_CER12 As String
    LD_DFR_INF_CER13 As String
    LD_DFR_INF_CER14 As String
    LD_DFR_INF_CER15 As String
    AC_LON_STA_REA1 As String 'type OL
    AC_LON_STA_REA2 As String
    AC_LON_STA_REA3 As String
    AC_LON_STA_REA4 As String
    AC_LON_STA_REA5 As String
    AC_LON_STA_REA6 As String
    AC_LON_STA_REA7 As String
    AC_LON_STA_REA8 As String
    AC_LON_STA_REA9 As String
    AC_LON_STA_REA10 As String
    AC_LON_STA_REA11 As String
    AC_LON_STA_REA12 As String
    AC_LON_STA_REA13 As String
    AC_LON_STA_REA14 As String
    AC_LON_STA_REA15 As String
    AD_DFR_BEG1 As String 'begin date OL
    AD_DFR_BEG2 As String
    AD_DFR_BEG3 As String
    AD_DFR_BEG4 As String
    AD_DFR_BEG5 As String
    AD_DFR_BEG6 As String
    AD_DFR_BEG7 As String
    AD_DFR_BEG8 As String
    AD_DFR_BEG9 As String
    AD_DFR_BEG10 As String
    AD_DFR_BEG11 As String
    AD_DFR_BEG12 As String
    AD_DFR_BEG13 As String
    AD_DFR_BEG14 As String
    AD_DFR_BEG15 As String
    AD_DFR_END1 As String 'end date OL
    AD_DFR_END2 As String
    AD_DFR_END3 As String
    AD_DFR_END4 As String
    AD_DFR_END5 As String
    AD_DFR_END6 As String
    AD_DFR_END7 As String
    AD_DFR_END8 As String
    AD_DFR_END9 As String
    AD_DFR_END10 As String
    AD_DFR_END11 As String
    AD_DFR_END12 As String
    AD_DFR_END13 As String
    AD_DFR_END14 As String
    AD_DFR_END15 As String
    IF_OPS_SCL_RPT1 As String 'school code OL
    IF_OPS_SCL_RPT2 As String
    IF_OPS_SCL_RPT3 As String
    IF_OPS_SCL_RPT4 As String
    IF_OPS_SCL_RPT5 As String
    IF_OPS_SCL_RPT6 As String
    IF_OPS_SCL_RPT7 As String
    IF_OPS_SCL_RPT8 As String
    IF_OPS_SCL_RPT9 As String
    IF_OPS_SCL_RPT10 As String
    IF_OPS_SCL_RPT11 As String
    IF_OPS_SCL_RPT12 As String
    IF_OPS_SCL_RPT13 As String
    IF_OPS_SCL_RPT14 As String
    IF_OPS_SCL_RPT15 As String
    LD_ENR_CER1 As String 'cert date OL
    LD_ENR_CER2 As String
    LD_ENR_CER3 As String
    LD_ENR_CER4  As String
    LD_ENR_CER5 As String
    LD_ENR_CER6 As String
    LD_ENR_CER7 As String
    LD_ENR_CER8 As String
    LD_ENR_CER9 As String
    LD_ENR_CER10 As String
    LD_ENR_CER11 As String
    LD_ENR_CER12 As String
    LD_ENR_CER13 As String
    LD_ENR_CER14 As String
    LD_ENR_CER15 As String
    
    'student info at loan level
    STU_SSN As String
    STU_DM_PRS_1 As String
    STU_DM_PRS_MID As String
    STU_DM_PRS_LST As String
    STU_DD_BRT As String
    STU_DX_STR_ADR_1 As String
    STU_DX_STR_ADR_2 As String
    STU_DM_CT As String
    STU_DC_DOM_ST As String
    STU_DF_ZIP As String
    STU_DM_FGN_CNY As String
    STU_DI_VLD_ADR As String
    STU_DN_PHN As String
    STU_DI_PHN_VLD As String
    STU_DN_ALT_PHN As String
    STU_DI_ALT_PHN_VLD As String
    
    'endorser info at loan level
    EDSR_SSN As String
    EDSR_DM_PRS_1 As String
    EDSR_DM_PRS_MID As String
    EDSR_DM_PRS_LST As String
    EDSR_DD_BRT As String
    EDSR_DX_STR_ADR_1 As String
    EDSR_DX_STR_ADR_2 As String
    EDSR_DM_CT As String
    EDSR_DC_DOM_ST As String
    EDSR_DF_ZIP As String
    EDSR_DM_FGN_CNY As String
    EDSR_DI_VLD_ADR As String
    EDSR_DN_PHN As String
    EDSR_DI_PHN_VLD As String
    EDSR_DN_ALT_PHN As String
    EDSR_DI_ALT_PHN_VLD As String
    AC_EDS_TYP As String
    
    'Reference info at borrower level
    REF_IND As String
    BM_RFR_1_1 As String
    BM_RFR_MID_1 As String
    BM_RFR_LST_1 As String
    BX_RFR_STR_ADR_1_1 As String
    BX_RFR_STR_ADR_2_1 As String
    BM_RFR_CT_1 As String
    BC_RFR_ST_1 As String
    BF_RFR_ZIP_1 As String
    BM_RFR_FGN_CNY_1 As String
    BI_VLD_ADR_1 As String
    BN_RFR_DOM_PHN_1 As String
    BI_DOM_PHN_VLD_1 As String
    BN_RFR_ALT_PHN_1 As String
    BI_ALT_PHN_VLD_1 As String
    BC_RFR_REL_BR_1 As String
    BM_RFR_1_2 As String
    BM_RFR_MID_2 As String
    BM_RFR_LST_2  As String
    BX_RFR_STR_ADR_1_2 As String
    BX_RFR_STR_ADR_2_2 As String
    BM_RFR_CT_2 As String
    BC_RFR_ST_2 As String
    BF_RFR_ZIP_2 As String
    BM_RFR_FGN_CNY_2 As String
    BI_VLD_ADR_2 As String
    BN_RFR_DOM_PHN_2 As String
    BI_DOM_PHN_VLD_2 As String
    BN_RFR_ALT_PHN_2 As String
    BI_ALT_PHN_VLD_2 As String
    BC_RFR_REL_BR_2 As String
    
    'loan level
    BondID As String
    AVE_REHB_PAY_AMT As String
    
    'batch data
    BAT_ID As String
    BAT_BR_CT As String
    BAT_LN_CT As String
    BAT_TOT_SUM As String
    
    'IBR info (borrower level)
    BR_ELIG_IND As String
    LD_IBR_25Y_FGV_BEG As String
    LD_IBR_RPD_SR As String
    LA_IBR_STD_STD_PAY As String
    LN_IBR_QLF_FGV_MTH As String
    
    'interest rate data
    ORG_INT_RATE As String
    
    'Rehab
    BA_STD_PAY As String
    LA_PAY_XPC As String
    BL_LA_PAY_XPC As String
    PRI_COST As String
    
End Type

Private Type RepurchaseRec
    'borrower level
    BF_SSN  As String
    DM_PRS_1 As String
    DM_PRS_MID As String
    DM_PRS_LST As String
    DD_BRT As String
    DX_STR_ADR_1 As String
    DX_STR_ADR_2 As String
    DM_CT As String
    DC_DOM_ST As String
    DF_ZIP As String
    DM_FGN_CNY As String
    DI_VLD_ADR As String
    DN_PHN As String
    DI_PHN_VLD As String
    DN_ALT_PHN As String
    DI_ALT_PHN_VLD As String
    
    'loan level
    AC_LON_TYP As String
    SUBSIDY As String
    AD_PRC As String
    AF_ORG_APL_OPS_LDR As String
    AF_APL_ID As String
    AF_APL_ID_SFX As String
    AD_IST_TRM_BEG As String
    AD_IST_TRM_END As String
    AA_GTE_LON_AMT As String
    AF_APL_OPS_SCL As String
    AD_BR_SIG As String
    LD_LFT_SCL As String
    PR_RPD_FOR_ITR As String
    LC_INT_TYP As String
    Bal As String
    DT_REPUR As String
    IF_OPS_SCL_RPT As String
    LC_STU_ENR_TYP As String
    LD_ENR_CER As String
    LD_LDR_NTF As String
    AR_CON_ITR As String
    AD_APL_RCV As String
    AC_STU_DFR_REQ As String
    
    'disbursment info loan level
    AN_DISB_1 As String 'disb seq num
    AC_DISB_1 As String 'disb adj code
    AD_DISB_1 As String 'Disb Date
    AA_DISB_1 As String 'Disb Amt
    ORG_1 As String     'origination Fee
    CD_DISB_1 As String 'disb cancelled date
    CA_DISB_1 As String 'disb cancelled amount
    GTE_1 As String
    AN_DISB_2 As String
    AC_DISB_2 As String
    AD_DISB_2 As String
    AA_DISB_2 As String
    ORG_2 As String
    CD_DISB_2 As String
    CA_DISB_2 As String
    GTE_2 As String
    AN_DISB_3 As String
    AC_DISB_3 As String
    AD_DISB_3 As String
    AA_DISB_3 As String
    ORG_3 As String
    CD_DISB_3 As String
    CA_DISB_3 As String
    GTE_3 As String
    AN_DISB_4 As String
    AC_DISB_4 As String
    AD_DISB_4 As String
    AA_DISB_4 As String
    ORG_4 As String
    CD_DISB_4 As String
    CA_DISB_4 As String
    GTE_4 As String
    AA_TOT_EDU_DET_PNT As String
    
    'deferment info loan level
    LC_DFR_TYP1 As String
    LC_DFR_TYP2 As String
    LC_DFR_TYP3 As String
    LC_DFR_TYP4 As String
    LC_DFR_TYP5 As String
    LC_DFR_TYP6 As String
    LC_DFR_TYP7 As String
    LC_DFR_TYP8 As String
    LC_DFR_TYP9 As String
    LC_DFR_TYP10 As String
    LC_DFR_TYP11 As String
    LC_DFR_TYP12 As String
    LC_DFR_TYP13 As String
    LC_DFR_TYP14 As String
    LC_DFR_TYP15 As String
    LD_DFR_BEG1 As String
    LD_DFR_BEG2 As String
    LD_DFR_BEG3 As String
    LD_DFR_BEG4 As String
    LD_DFR_BEG5 As String
    LD_DFR_BEG6 As String
    LD_DFR_BEG7 As String
    LD_DFR_BEG8 As String
    LD_DFR_BEG9 As String
    LD_DFR_BEG10 As String
    LD_DFR_BEG11 As String
    LD_DFR_BEG12 As String
    LD_DFR_BEG13 As String
    LD_DFR_BEG14 As String
    LD_DFR_BEG15 As String
    LD_DFR_END1 As String
    LD_DFR_END2 As String
    LD_DFR_END3 As String
    LD_DFR_END4 As String
    LD_DFR_END5 As String
    LD_DFR_END6 As String
    LD_DFR_END7 As String
    LD_DFR_END8 As String
    LD_DFR_END9 As String
    LD_DFR_END10 As String
    LD_DFR_END11 As String
    LD_DFR_END12 As String
    LD_DFR_END13 As String
    LD_DFR_END14 As String
    LD_DFR_END15 As String
    LF_DOE_SCL_DFR1 As String 'school code
    LF_DOE_SCL_DFR2 As String
    LF_DOE_SCL_DFR3 As String
    LF_DOE_SCL_DFR4 As String
    LF_DOE_SCL_DFR5 As String
    LF_DOE_SCL_DFR6 As String
    LF_DOE_SCL_DFR7 As String
    LF_DOE_SCL_DFR8 As String
    LF_DOE_SCL_DFR9 As String
    LF_DOE_SCL_DFR10 As String
    LF_DOE_SCL_DFR11 As String
    LF_DOE_SCL_DFR12 As String
    LF_DOE_SCL_DFR13 As String
    LF_DOE_SCL_DFR14 As String
    LF_DOE_SCL_DFR15 As String
    LD_DFR_INF_CER1 As String 'cert date
    LD_DFR_INF_CER2 As String
    LD_DFR_INF_CER3 As String
    LD_DFR_INF_CER4 As String
    LD_DFR_INF_CER5 As String
    LD_DFR_INF_CER6 As String
    LD_DFR_INF_CER7 As String
    LD_DFR_INF_CER8 As String
    LD_DFR_INF_CER9 As String
    LD_DFR_INF_CER10 As String
    LD_DFR_INF_CER11 As String
    LD_DFR_INF_CER12 As String
    LD_DFR_INF_CER13 As String
    LD_DFR_INF_CER14 As String
    LD_DFR_INF_CER15 As String
    AC_LON_STA_REA1 As String 'type OL
    AC_LON_STA_REA2 As String
    AC_LON_STA_REA3 As String
    AC_LON_STA_REA4 As String
    AC_LON_STA_REA5 As String
    AC_LON_STA_REA6 As String
    AC_LON_STA_REA7 As String
    AC_LON_STA_REA8 As String
    AC_LON_STA_REA9 As String
    AC_LON_STA_REA10 As String
    AC_LON_STA_REA11 As String
    AC_LON_STA_REA12 As String
    AC_LON_STA_REA13 As String
    AC_LON_STA_REA14 As String
    AC_LON_STA_REA15 As String
    AD_DFR_BEG1 As String 'begin date OL
    AD_DFR_BEG2 As String
    AD_DFR_BEG3 As String
    AD_DFR_BEG4 As String
    AD_DFR_BEG5 As String
    AD_DFR_BEG6 As String
    AD_DFR_BEG7 As String
    AD_DFR_BEG8 As String
    AD_DFR_BEG9 As String
    AD_DFR_BEG10 As String
    AD_DFR_BEG11 As String
    AD_DFR_BEG12 As String
    AD_DFR_BEG13 As String
    AD_DFR_BEG14 As String
    AD_DFR_BEG15 As String
    AD_DFR_END1 As String 'end date OL
    AD_DFR_END2 As String
    AD_DFR_END3 As String
    AD_DFR_END4 As String
    AD_DFR_END5 As String
    AD_DFR_END6 As String
    AD_DFR_END7 As String
    AD_DFR_END8 As String
    AD_DFR_END9 As String
    AD_DFR_END10 As String
    AD_DFR_END11 As String
    AD_DFR_END12 As String
    AD_DFR_END13 As String
    AD_DFR_END14 As String
    AD_DFR_END15 As String
    IF_OPS_SCL_RPT1 As String 'school code OL
    IF_OPS_SCL_RPT2 As String
    IF_OPS_SCL_RPT3 As String
    IF_OPS_SCL_RPT4 As String
    IF_OPS_SCL_RPT5 As String
    IF_OPS_SCL_RPT6 As String
    IF_OPS_SCL_RPT7 As String
    IF_OPS_SCL_RPT8 As String
    IF_OPS_SCL_RPT9 As String
    IF_OPS_SCL_RPT10 As String
    IF_OPS_SCL_RPT11 As String
    IF_OPS_SCL_RPT12 As String
    IF_OPS_SCL_RPT13 As String
    IF_OPS_SCL_RPT14 As String
    IF_OPS_SCL_RPT15 As String
    LD_ENR_CER1 As String 'cert date OL
    LD_ENR_CER2 As String
    LD_ENR_CER3 As String
    LD_ENR_CER4  As String
    LD_ENR_CER5 As String
    LD_ENR_CER6 As String
    LD_ENR_CER7 As String
    LD_ENR_CER8 As String
    LD_ENR_CER9 As String
    LD_ENR_CER10 As String
    LD_ENR_CER11 As String
    LD_ENR_CER12 As String
    LD_ENR_CER13 As String
    LD_ENR_CER14 As String
    LD_ENR_CER15 As String
    
    'student info at loan level
    STU_SSN As String
    STU_DM_PRS_1 As String
    STU_DM_PRS_MID As String
    STU_DM_PRS_LST As String
    STU_DD_BRT As String
    STU_DX_STR_ADR_1 As String
    STU_DX_STR_ADR_2 As String
    STU_DM_CT As String
    STU_DC_DOM_ST As String
    STU_DF_ZIP As String
    STU_DM_FGN_CNY As String
    STU_DI_VLD_ADR As String
    STU_DN_PHN As String
    STU_DI_PHN_VLD As String
    STU_DN_ALT_PHN As String
    STU_DI_ALT_PHN_VLD As String
    
    'endorser info at loan level
    EDSR_SSN As String
    EDSR_DM_PRS_1 As String
    EDSR_DM_PRS_MID As String
    EDSR_DM_PRS_LST As String
    EDSR_DD_BRT As String
    EDSR_DX_STR_ADR_1 As String
    EDSR_DX_STR_ADR_2 As String
    EDSR_DM_CT As String
    EDSR_DC_DOM_ST As String
    EDSR_DF_ZIP As String
    EDSR_DM_FGN_CNY As String
    EDSR_DI_VLD_ADR As String
    EDSR_DN_PHN As String
    EDSR_DI_PHN_VLD As String
    EDSR_DN_ALT_PHN As String
    EDSR_DI_ALT_PHN_VLD As String
    AC_EDS_TYP As String
    
    'Reference info at borrower level
    REF_IND As String
    BM_RFR_1_1 As String
    BM_RFR_MID_1 As String
    BM_RFR_LST_1 As String
    BX_RFR_STR_ADR_1_1 As String
    BX_RFR_STR_ADR_2_1 As String
    BM_RFR_CT_1 As String
    BC_RFR_ST_1 As String
    BF_RFR_ZIP_1 As String
    BM_RFR_FGN_CNY_1 As String
    BI_VLD_ADR_1 As String
    BN_RFR_DOM_PHN_1 As String
    BI_DOM_PHN_VLD_1 As String
    BN_RFR_ALT_PHN_1 As String
    BI_ALT_PHN_VLD_1 As String
    BC_RFR_REL_BR_1 As String
    BM_RFR_1_2 As String
    BM_RFR_MID_2 As String
    BM_RFR_LST_2 As String
    BX_RFR_STR_ADR_1_2 As String
    BX_RFR_STR_ADR_2_2 As String
    BM_RFR_CT_2 As String
    BC_RFR_ST_2 As String
    BF_RFR_ZIP_2 As String
    BM_RFR_FGN_CNY_2 As String
    BI_VLD_ADR_2 As String
    BN_RFR_DOM_PHN_2 As String
    BI_DOM_PHN_VLD_2 As String
    BN_RFR_ALT_PHN_2 As String
    BI_ALT_PHN_VLD_2 As String
    BC_RFR_REL_BR_2 As String
    
    'loan level
    BondID As String
    
    'batch data
    BAT_ID As String
    BAT_BR_CT As String
    BAT_LN_CT As String
    BAT_TOT_SUM As String
    
    'extended data
    LC_FOR_TYP1 As String
    LC_FOR_TYP2 As String
    LC_FOR_TYP3 As String
    LC_FOR_TYP4  As String
    LC_FOR_TYP5  As String
    LC_FOR_TYP6  As String
    LC_FOR_TYP7  As String
    LC_FOR_TYP8  As String
    LC_FOR_TYP9  As String
    LC_FOR_TYP10  As String
    LC_FOR_TYP11  As String
    LC_FOR_TYP12  As String
    LC_FOR_TYP13 As String
    LC_FOR_TYP14 As String
    LC_FOR_TYP15 As String
    LD_FOR_BEG1 As String
    LD_FOR_BEG2 As String
    LD_FOR_BEG3 As String
    LD_FOR_BEG4 As String
    LD_FOR_BEG5 As String
    LD_FOR_BEG6 As String
    LD_FOR_BEG7 As String
    LD_FOR_BEG8 As String
    LD_FOR_BEG9 As String
    LD_FOR_BEG10 As String
    LD_FOR_BEG11 As String
    LD_FOR_BEG12 As String
    LD_FOR_BEG13 As String
    LD_FOR_BEG14 As String
    LD_FOR_BEG15 As String
    LD_FOR_END1 As String
    LD_FOR_END2 As String
    LD_FOR_END3 As String
    LD_FOR_END4 As String
    LD_FOR_END5 As String
    LD_FOR_END6 As String
    LD_FOR_END7 As String
    LD_FOR_END8 As String
    LD_FOR_END9 As String
    LD_FOR_END10 As String
    LD_FOR_END11 As String
    LD_FOR_END12 As String
    LD_FOR_END13 As String
    LD_FOR_END14 As String
    LD_FOR_END15 As String
    LI_CAP_FOR_INT_REQ1 As String
    LI_CAP_FOR_INT_REQ2 As String
    LI_CAP_FOR_INT_REQ3 As String
    LI_CAP_FOR_INT_REQ4 As String
    LI_CAP_FOR_INT_REQ5 As String
    LI_CAP_FOR_INT_REQ6 As String
    LI_CAP_FOR_INT_REQ7 As String
    LI_CAP_FOR_INT_REQ8 As String
    LI_CAP_FOR_INT_REQ9 As String
    LI_CAP_FOR_INT_REQ10 As String
    LI_CAP_FOR_INT_REQ11 As String
    LI_CAP_FOR_INT_REQ12 As String
    LI_CAP_FOR_INT_REQ13 As String
    LI_CAP_FOR_INT_REQ14 As String
    LI_CAP_FOR_INT_REQ15 As String
    OL_FRB_BEG1 As String
    OL_FRB_BEG2 As String
    OL_FRB_BEG3 As String
    OL_FRB_BEG4 As String
    OL_FRB_BEG5 As String
    OL_FRB_BEG6 As String
    OL_FRB_BEG7 As String
    OL_FRB_BEG8 As String
    OL_FRB_BEG9 As String
    OL_FRB_BEG10 As String
    OL_FRB_BEG11 As String
    OL_FRB_BEG12 As String
    OL_FRB_BEG13 As String
    OL_FRB_BEG14 As String
    OL_FRB_BEG15 As String
    OL_FRB_END1 As String
    OL_FRB_END2 As String
    OL_FRB_END3 As String
    OL_FRB_END4 As String
    OL_FRB_END5 As String
    OL_FRB_END6 As String
    OL_FRB_END7 As String
    OL_FRB_END8 As String
    OL_FRB_END9 As String
    OL_FRB_END10 As String
    OL_FRB_END11 As String
    OL_FRB_END12 As String
    OL_FRB_END13 As String
    OL_FRB_END14 As String
    OL_FRB_END15 As String

    'IBR info (borrower level)
    BR_ELIG_IND As String
    LD_IBR_25Y_FGV_BEG As String
    LD_IBR_RPD_SR As String
    LA_IBR_STD_STD_PAY As String
    LN_IBR_QLF_FGV_MTH As String
    
    'interest rate data
    ORG_INT_RATE As String
    
    LA_PRI As String
    LA_INT As String
End Type

Private Type TILPRec
    'borrower level
    BF_SSN As String
    DM_PRS_1 As String
    DM_PRS_MID As String
    DM_PRS_LST As String
    DD_BRT As String
    DX_STR_ADR_1 As String
    DX_STR_ADR_2 As String
    DM_CT As String
    DC_DOM_ST As String
    DF_ZIP As String
    DM_FGN_CNY As String
    DI_VLD_ADR As String
    DN_PHN As String
    DI_PHN_VLD As String
    DN_ALT_PHN As String
    DI_ALT_PHN_VLD As String
    
    'loan level
    AC_LON_TYP As String
    SUBSIDY As String
    AD_PRC As String
    AF_ORG_APL_OPS_LDR As String
    AF_APL_ID As String
    AF_APL_ID_SFX As String
    AD_IST_TRM_BEG As String
    AD_IST_TRM_END As String
    AA_GTE_LON_AMT As String
    AF_APL_OPS_SCL As String
    AD_BR_SIG As String
    LD_LFT_SCL As String
    PR_RPD_FOR_ITR As String
    LC_INT_TYP As String
    LD_TRX_EFF As String
    LA_TRX As String
    IF_OPS_SCL_RPT As String
    LC_STU_ENR_TYP As String
    LD_ENR_CER As String
    LD_LDR_NTF As String
    AR_CON_ITR As String
    AD_APL_RCV As String
    AC_STU_DFR_REQ As String
    
    'disbursment info loan level
    AN_DISB_1 As String 'disb seq num
    AC_DISB_1 As String 'disb adj code
    AD_DISB_1 As String 'Disb Date
    AA_DISB_1 As String 'Disb Amt
    ORG_1 As String     'origination Fee
    CD_DISB_1 As String 'disb cancelled date
    CA_DISB_1 As String 'disb cancelled amount
    GTE_1 As String
    AN_DISB_2 As String
    AC_DISB_2 As String
    AD_DISB_2 As String
    AA_DISB_2 As String
    ORG_2 As String
    CD_DISB_2 As String
    CA_DISB_2 As String
    GTE_2 As String
    AN_DISB_3 As String
    AC_DISB_3 As String
    AD_DISB_3 As String
    AA_DISB_3 As String
    ORG_3 As String
    CD_DISB_3 As String
    CA_DISB_3 As String
    GTE_3 As String
    AN_DISB_4 As String
    AC_DISB_4 As String
    AD_DISB_4 As String
    AA_DISB_4 As String
    ORG_4 As String
    CD_DISB_4 As String
    CA_DISB_4 As String
    GTE_4 As String
    AA_TOT_EDU_DET_PNT As String
    
    'deferment info loan level
    LC_DFR_TYP1 As String 'type
    LC_DFR_TYP2 As String
    LC_DFR_TYP3 As String
    LC_DFR_TYP4 As String
    LC_DFR_TYP5 As String
    LC_DFR_TYP6 As String
    LC_DFR_TYP7 As String
    LC_DFR_TYP8 As String
    LC_DFR_TYP9 As String
    LC_DFR_TYP10 As String
    LC_DFR_TYP11 As String
    LC_DFR_TYP12 As String
    LC_DFR_TYP13 As String
    LC_DFR_TYP14 As String
    LC_DFR_TYP15 As String
    LD_DFR_BEG1 As String 'begin date
    LD_DFR_BEG2 As String
    LD_DFR_BEG3 As String
    LD_DFR_BEG4 As String
    LD_DFR_BEG5 As String
    LD_DFR_BEG6 As String
    LD_DFR_BEG7 As String
    LD_DFR_BEG8 As String
    LD_DFR_BEG9 As String
    LD_DFR_BEG10 As String
    LD_DFR_BEG11 As String
    LD_DFR_BEG12 As String
    LD_DFR_BEG13 As String
    LD_DFR_BEG14 As String
    LD_DFR_BEG15 As String
    LD_DFR_END1 As String 'end date
    LD_DFR_END2 As String
    LD_DFR_END3 As String
    LD_DFR_END4 As String
    LD_DFR_END5 As String
    LD_DFR_END6 As String
    LD_DFR_END7 As String
    LD_DFR_END8 As String
    LD_DFR_END9 As String
    LD_DFR_END10 As String
    LD_DFR_END11 As String
    LD_DFR_END12 As String
    LD_DFR_END13 As String
    LD_DFR_END14 As String
    LD_DFR_END15 As String
    LF_DOE_SCL_DFR1 As String 'school code
    LF_DOE_SCL_DFR2 As String
    LF_DOE_SCL_DFR3 As String
    LF_DOE_SCL_DFR4 As String
    LF_DOE_SCL_DFR5 As String
    LF_DOE_SCL_DFR6 As String
    LF_DOE_SCL_DFR7 As String
    LF_DOE_SCL_DFR8 As String
    LF_DOE_SCL_DFR9 As String
    LF_DOE_SCL_DFR10 As String
    LF_DOE_SCL_DFR11 As String
    LF_DOE_SCL_DFR12 As String
    LF_DOE_SCL_DFR13 As String
    LF_DOE_SCL_DFR14 As String
    LF_DOE_SCL_DFR15 As String
    LD_DFR_INF_CER1 As String 'cert date
    LD_DFR_INF_CER2 As String
    LD_DFR_INF_CER3 As String
    LD_DFR_INF_CER4 As String
    LD_DFR_INF_CER5 As String
    LD_DFR_INF_CER6 As String
    LD_DFR_INF_CER7 As String
    LD_DFR_INF_CER8 As String
    LD_DFR_INF_CER9 As String
    LD_DFR_INF_CER10 As String
    LD_DFR_INF_CER11 As String
    LD_DFR_INF_CER12 As String
    LD_DFR_INF_CER13 As String
    LD_DFR_INF_CER14 As String
    LD_DFR_INF_CER15 As String
    AC_LON_STA_REA1 As String 'type OL
    AC_LON_STA_REA2 As String
    AC_LON_STA_REA3 As String
    AC_LON_STA_REA4 As String
    AC_LON_STA_REA5 As String
    AC_LON_STA_REA6 As String
    AC_LON_STA_REA7 As String
    AC_LON_STA_REA8 As String
    AC_LON_STA_REA9 As String
    AC_LON_STA_REA10 As String
    AC_LON_STA_REA11 As String
    AC_LON_STA_REA12 As String
    AC_LON_STA_REA13 As String
    AC_LON_STA_REA14 As String
    AC_LON_STA_REA15 As String
    AD_DFR_BEG1 As String 'begin date OL
    AD_DFR_BEG2 As String
    AD_DFR_BEG3 As String
    AD_DFR_BEG4 As String
    AD_DFR_BEG5 As String
    AD_DFR_BEG6 As String
    AD_DFR_BEG7 As String
    AD_DFR_BEG8 As String
    AD_DFR_BEG9 As String
    AD_DFR_BEG10 As String
    AD_DFR_BEG11 As String
    AD_DFR_BEG12 As String
    AD_DFR_BEG13 As String
    AD_DFR_BEG14 As String
    AD_DFR_BEG15 As String
    AD_DFR_END1 As String 'end date OL
    AD_DFR_END2 As String
    AD_DFR_END3 As String
    AD_DFR_END4 As String
    AD_DFR_END5 As String
    AD_DFR_END6 As String
    AD_DFR_END7 As String
    AD_DFR_END8 As String
    AD_DFR_END9 As String
    AD_DFR_END10 As String
    AD_DFR_END11 As String
    AD_DFR_END12 As String
    AD_DFR_END13 As String
    AD_DFR_END14 As String
    AD_DFR_END15 As String
    IF_OPS_SCL_RPT1 As String 'school code OL
    IF_OPS_SCL_RPT2 As String
    IF_OPS_SCL_RPT3 As String
    IF_OPS_SCL_RPT4 As String
    IF_OPS_SCL_RPT5 As String
    IF_OPS_SCL_RPT6 As String
    IF_OPS_SCL_RPT7 As String
    IF_OPS_SCL_RPT8 As String
    IF_OPS_SCL_RPT9 As String
    IF_OPS_SCL_RPT10 As String
    IF_OPS_SCL_RPT11 As String
    IF_OPS_SCL_RPT12 As String
    IF_OPS_SCL_RPT13 As String
    IF_OPS_SCL_RPT14 As String
    IF_OPS_SCL_RPT15 As String
    LD_ENR_CER1 As String 'cert date OL
    LD_ENR_CER2 As String
    LD_ENR_CER3 As String
    LD_ENR_CER4  As String
    LD_ENR_CER5 As String
    LD_ENR_CER6 As String
    LD_ENR_CER7 As String
    LD_ENR_CER8 As String
    LD_ENR_CER9 As String
    LD_ENR_CER10 As String
    LD_ENR_CER11 As String
    LD_ENR_CER12 As String
    LD_ENR_CER13 As String
    LD_ENR_CER14 As String
    LD_ENR_CER15 As String
    
    'student info at loan level
    STU_SSN As String
    STU_DM_PRS_1 As String
    STU_DM_PRS_MID As String
    STU_DM_PRS_LST As String
    STU_DD_BRT As String
    STU_DX_STR_ADR_1 As String
    STU_DX_STR_ADR_2 As String
    STU_DM_CT As String
    STU_DC_DOM_ST As String
    STU_DF_ZIP As String
    STU_DM_FGN_CNY As String
    STU_DI_VLD_ADR As String
    STU_DN_PHN As String
    STU_DI_PHN_VLD As String
    STU_DN_ALT_PHN As String
    STU_DI_ALT_PHN_VLD As String
    
    'endorser info at loan level
    EDSR_SSN As String
    EDSR_DM_PRS_1 As String
    EDSR_DM_PRS_MID As String
    EDSR_DM_PRS_LST As String
    EDSR_DD_BRT As String
    EDSR_DX_STR_ADR_1 As String
    EDSR_DX_STR_ADR_2 As String
    EDSR_DM_CT As String
    EDSR_DC_DOM_ST As String
    EDSR_DF_ZIP As String
    EDSR_DM_FGN_CNY As String
    EDSR_DI_VLD_ADR As String
    EDSR_DN_PHN As String
    EDSR_DI_PHN_VLD As String
    EDSR_DN_ALT_PHN As String
    EDSR_DI_ALT_PHN_VLD As String
    AC_EDS_TYP As String
    
    'Reference info at borrower level
    REF_IND As String
    BM_RFR_1_1 As String
    BM_RFR_MID_1 As String
    BM_RFR_LST_1 As String
    BX_RFR_STR_ADR_1_1 As String
    BX_RFR_STR_ADR_2_1 As String
    BM_RFR_CT_1 As String
    BC_RFR_ST_1 As String
    BF_RFR_ZIP_1 As String
    BM_RFR_FGN_CNY_1 As String
    BI_VLD_ADR_1 As String
    BN_RFR_DOM_PHN_1 As String
    BI_DOM_PHN_VLD_1 As String
    BN_RFR_ALT_PHN_1 As String
    BI_ALT_PHN_VLD_1 As String
    BC_RFR_REL_BR_1 As String
    BM_RFR_1_2 As String
    BM_RFR_MID_2 As String
    BM_RFR_LST_2  As String
    BX_RFR_STR_ADR_1_2 As String
    BX_RFR_STR_ADR_2_2 As String
    BM_RFR_CT_2 As String
    BC_RFR_ST_2 As String
    BF_RFR_ZIP_2 As String
    BM_RFR_FGN_CNY_2 As String
    BI_VLD_ADR_2 As String
    BN_RFR_DOM_PHN_2 As String
    BI_DOM_PHN_VLD_2 As String
    BN_RFR_ALT_PHN_2 As String
    BI_ALT_PHN_VLD_2 As String
    BC_RFR_REL_BR_2 As String
    
    'loan level
    BondID As String
    AVE_REHB_PAY_AMT As String
    
    'unique TILP data
    RPY_LETTER_DT As String
    RPY_INIT_BAL As String
    RPY_INT_RATE As String
    RPY_PAY_AMT As String
    RPY_PAY_TERM As String
    RPY_FIRST_DUE_DT As String
    TOT_INT_TO_REPAY As String
    TOT_REPAY_AMT As String
    RPY_LAST_PAY_DT As String
    FIT_INT_AMT As String
    FIT_FEE_AMT As String
    STH_STATUS As String
    STH_EFF_DT As String
    CACTUS_ID As String
    RPY_NEXT_DUE_DT As String
    RPY_OVERPAY As String
    INT_ACCRUE_START_DT As String
    BAT_ITR As String
    BAT_FEE As String
    
    'batch data
    BAT_ID As String
    BAT_BR_CT As String
    BAT_LN_CT As String
    BAT_TOT_SUM As String
End Type

'data needed to create a batch
Private Type BatchInformation
    EffLoanAddDt As String
    ConversionType As String
    SubIntLastCollection As String
    OriginationFee As String
    BondIssue As String
    Guarantor As String
    Program As String
    Lender As String
    PrevOwner As String
    RepurchaseType As String
    Owner As String
End Type

Private Type Disb
    DisbSeqNum As String 'disb seq num
    DisbAdjCode As String 'disb adj code
    DisbDate As String 'Disb Date
    DisbAmt As String 'Disb Amt
    OFee As String     'origination Fee
    DisbCancelDt As String 'disb cancelled date
    DisbCancelAmt As String 'disb cancelled amount
    DisbMedium As String
    DisbCancelReason As String
    GTE As String
End Type

Private Type Def
    BeginDate As String
    EndDate As String
    Type As String
    SchoolCode As String
    CertDate As String
    CapInt As String
End Type

Private Type Frb
    BeginDate As String
    EndDate As String
    Type As String
    CapInt As String
End Type

'loan level information
Private Type Loan
    BondID As String
    Guarantor As String
    LoanPrg As String
    OrigLender As String
    RepType As String
    AppRcvdDt As String
    DtNoteSigned As String
    DtGuaranteed As String
    AmountGuaranteed As String
    clid As String
    TermBg As String
    TermEd As String
    OrigSchool As String
    SubsidyCd As String
    SpallElig As String
    PriorTo7_93 As String
    WtdAveInt As String
    UndrlyDisb As String
    CurrSchool As String
    SchSepDate As String
    SepReason As String
    SepSource As String
    CertDt As String
    NtfRecdDt As String
    GraceMonth As String
    GracePeriodEnd As String
    RepayStartDt As String
    LD_TRX_EFF_Less9Months As String
    LD_TRX_EFF As String
    LD_RHB As String
    LA_PRI As String
    LA_INT As String
    OriginalIntRate As String
    IntRate As String
    IntType As String
    SpecRate As String
    TotIndbt As String
    Principal As String
    IntLastCapped As String
    AC_LON_TYP As String
    SystemSeqNum As String
    AC_STU_DFR_REQ As String
    
    'endorser info
    ESSN As String
    EFirstName As String
    EMID As String
    ELastName As String
    EDOB As String
    EAddr1 As String
    EAddr2 As String
    ECity As String
    EState As String
    EZip As String
    ECountry As String
    EAddrInd As String
    EPhone As String
    EPhoneInd As String
    EAltPhone As String
    EAltPhoneInd As String
    AC_EDS_TYP As String
    
    'student info
    SSSN As String
    SFirstName As String
    SMID As String
    SLastName As String
    SDOB As String
    SAddr1 As String
    SAddr2 As String
    SCity As String
    SState As String
    SZip As String
    SCountry As String
    SAddrInd As String
    SPhone As String
    SPhoneInd As String
    SAltPhone As String
    SAltPhoneInd As String
    
    'disbursment info
    Disbursements() As Disb
    
    'deferement info
    Deferments() As Def
    Forbearances() As Frb
    
    'Rehab
    BA_STD_PAY As String
    LA_PAY_XPC As String
    BL_LA_PAY_XPC As String
    PRI_COST As String
End Type

'borrower level data
Private Type Borrower
    'borrower
    BSSN As String
    BLastName As String
    BFirstName As String
    BMID As String
    BAddr1 As String
    BAddr2 As String
    BCity As String
    BState As String
    BZip As String
    BAddrInd As String
    BPhone As String
    BPhoneInd As String
    BDOB As String
    BPhoneSource As String
    
    'misc
    loans() As Loan
    SkipAACProcessingBecauseOfErrors As Boolean
    
    'reference1
    R1LastName As String
    R1FirstName As String
    R1MID As String
    R1Relation As String
    R1Addr1 As String
    R1Addr2 As String
    R1City As String
    R1State As String
    R1Zip As String
    R1AddrInd As String
    R1Phone As String
    R1PhoneInd As String
    
    
    'reference2
    R2LastName As String
    R2FirstName As String
    R2MID As String
    R2Relation As String
    R2Addr1 As String
    R2Addr2 As String
    R2City As String
    R2State As String
    R2Zip As String
    R2AddrInd As String
    R2Phone As String
    R2PhoneInd As String
    
    'TILP Only fields
    TILPOnlyFees As String
    TILPOnlyInterest As String
    TILPOnlysth_end_date As String
    TILPOnlyDefExists As Boolean
    TILPOnlyLD_LFT_SCLIsGTLD_DFR_BEG As Boolean
    TILPOnlyIntAccrueStartDt As String
    TILPOnlyRpy_init_bal As String
    TILPOnlyRpy_first_due_dt As String
    TILPOnlyRpy_pay_term As String
    TILPOnlyRpy_pay_amt As String
    TILPOnlyRpy_next_due_dt As String
    TILPOnlyFIT_INT_AMT As String
    TILPOnlyFileCreateDate As String
    TILPOnlyLD_LFT_SCL As String
    TILPOnlyLD_LFT_SCLIsGTsth_eff_dt As Boolean
    TILPOnlyLD_LFT_SCLIsLTAnyDisbDt As Boolean
    TILPOnlyBNKLnStatusFound As Boolean
    TILPOnlyBatchLvlFees As String
    
    'IBR
    IBREligibleInd As String
    IBRForgivenessStartDate As String
    IBRCreatDate As String
    IBRStndStndPayAmount As String
    IBRQualifyingPayments As String
  
End Type

Private Type RecoveryData
    batchId As String
    ssn As String
    clid As String
    Phase As Integer
    logFile As String
    LnSeq As String
End Type

Private BatchInfo As BatchInformation
Private Borrs() As Borrower
Private FTPDir As String
Private LogDir As String
Private SaveInDir As String
Private RecvInfo As RecoveryData
Private FileTypeInProc As FileTypeInProcess
Private RepaymentFile As String
Private ValidationErrFound As Boolean

Public Sub Main()
    Dim FilesArr() As String
    Dim FileSearchStr As Variant
    Dim FileInProc As String
    Dim FileToProcFound As Boolean
    
    If MsgBox("This script does AAC processing for Rehab, Repurchase and TILP.  Click OK to continue or Cancel to end.", vbOKCancel, "AAC Processing") <> vbOK Then End
    
    'setup test mode vars
    SaveInDir = "X:\PADD\AccountServices\"
    SP.Common.TestMode FTPDir, SaveInDir, LogDir
    RepaymentFile = FTPDir & "REHRPTSCHD." & Format(Now, "MM-DD-YY Hh.Nn.Ss AM/PM") & ".txt"
    RecvInfo.logFile = LogDir & "AAC Recovery Log.txt"
    
    'check for recovery
    If Dir(RecvInfo.logFile) <> "" Then
        'load recovery variables
        Open RecvInfo.logFile For Input As #20
        Input #20, RecvInfo.batchId, RecvInfo.ssn, RecvInfo.clid, RecvInfo.Phase, RecvInfo.LnSeq
        Close #20
    Else
        Open "T:\AACLenderServicer.txt" For Output As #90
        Close #90
    End If
    
    Dim rehabfc As Integer: rehabfc = 0
    Dim repurfc As Integer: repurfc = 0
    Dim tilpfc As Integer: tilpfc = 0
    'setup file name array for processing
    ReDim FilesArr(2)
    FilesArr(0) = "ULWRH1.LWRH1R8.*"
    FilesArr(1) = "ULWRP1.LWRP1R26.*"
    FilesArr(2) = "TILPAAC*"
    'cycle through all files to process and make sure at least one exists
    For Each FileSearchStr In FilesArr
        Dim Filename As String
        Filename = Dir(FTPDir & CStr(FileSearchStr))
        'check for file existence
        If Filename <> "" Then
        
            If InStr(1, Filename, "LWRH1R8") <> 0 Then
                rehabfc = 1
            ElseIf InStr(1, Filename, "LWRP1R26") <> 0 Then
                repurfc = 1
            ElseIf InStr(1, UCase(Filename), "TILPAAC") <> 0 Then
                tilpfc = 1
            End If
                
            Dim count As Integer
            count = 1
            Dim lastFile As String
            Do While Filename <> ""
             If lastFile = Filename Then
                    Exit Do
                End If
                lastFile = Filename
                
                count = count + 1
                Filename = Dir()
            Loop
            If count > 2 Then
                If MsgBox("There are multiple " & FileSearchStr & " Do you want to continue processing?", vbOKCancel, "AAC Processing") <> vbOK Then End
            End If
            FileToProcFound = True
        End If
    Next
    'if one of the file types to be processed weren't found then prompt user and end the script
    If FileToProcFound = False Then
        MsgBox "The script couldn't find any files to process.  Please contact Systems Support.", vbOKOnly + vbCritical, "Files Not Found"
        End
    End If
    
    If rehabfc + repurfc + tilpfc <> 1 Then
        Dim message As String: message = "The following files were found. "
        
        If rehabfc <> 0 Then
            message = message & "Rehab, "
        End If
        If repurfc <> 0 Then
            message = message & "Repurchase, "
        End If
        If tilpfc <> 0 Then
            message = message & "TILP, "
        End If
        
        message = message & "Please remove the files so there is only 1 to process.  The script will now end."
        
        MsgBox message, vbOKOnly + vbCritical, "Error"
        End
    End If
    
    'process all file types that can be found
    For Each FileSearchStr In FilesArr
        'check for file existence
        FileInProc = Dir(FTPDir & CStr(FileSearchStr))
        If FileInProc <> "" Then
            'check that file has data
            If FileLen(FTPDir & FileInProc) = 0 Then
                'if file has no data then skip any processing for file, delete file and search for another
                Kill FTPDir & FileInProc
            Else
                'mark enum for future processing reference
                If FileSearchStr = "ULWRH1.LWRH1R8.*" Then
                    FileTypeInProc = rehab
                ElseIf FileSearchStr = "ULWRP1.LWRP1R26.*" Then
                    FileTypeInProc = Repurch
                ElseIf FileSearchStr = "TILPAAC*" Then
                    FileTypeInProc = tilp
                End If
                If RecvInfo.Phase < 1 Then 'check for recovery
                    'if file is rehab file then do pre processing file spliting
                    If FileTypeInProc = rehab Then CreateREHRPTSCHDFile FileInProc
                    UpdateRecvLog "", "", "", 1, ""
                End If
                If RecvInfo.Phase < 2 Then
                    'documents Batch level information in Master Tracking File
                    CreateMasterTrackingFile FileInProc
                    'set up batch info for batch creation
                    SetUpBatchInfo FileTypeInProc, FileInProc
                    'create batches
                    CreateAACBatches FileTypeInProc
                    UpdateRecvLog "", "", "", 2, ""
                End If
                'update system
                UpdateSys FileTypeInProc, FileInProc
                If RecvInfo.Phase < 13 Then 'recovery check
                    'process error file
                    CreateErrorQTsks FileInProc
                    CreateQTsks
                    UpdateRecvLog "", "", "", 13, ""
                End If
                Kill FTPDir & FileInProc 'delete file just processed
                If Dir(RepaymentFile) <> "" Then Kill RepaymentFile
            End If
        End If
        Wait "1" 'there is some functionality the requires the time date stamp to be unique for every iteration this is here just to be sure they are unique
    Next
    
    If Dir(RecvInfo.logFile) <> "" Then Kill RecvInfo.logFile 'delete recovery log file
    If Dir("T:\AACLenderServicer.txt") <> "" Then Kill "T:\AACLenderServicer.txt"
    
    MsgBox "Processing Complete!", vbOKOnly + vbInformation
    End
End Sub

'this sub updates the recovery log file
Private Sub UpdateRecvLog(BID As String, ssn As String, clid As String, Phase As Integer, LnSeq As String)
    Open RecvInfo.logFile For Output As #20
    Write #20, BID, ssn, clid, Phase, LnSeq
    Close #20
    RecvInfo.Phase = 0 'if the log is updated then the script has recovered
End Sub

'this sub processes the error file which was created during processing
Private Sub CreateErrorQTsks(FIP As String)
    Dim UID As String
    Dim LD_TRX As String
    Dim ssn As String
    Dim ErrorMsg As String
    Dim Queue As String
    Dim StrTranslation As String
    Dim QueuesPopulated As String
    Dim FileSaveFlag As Boolean
    
    DeDupFile "T:\AACErrorQueueTaskData.txt"
    If Dir("T:\AACErrorQueueTaskData.txt") <> "" Then
        Open "T:\AACErrorQueueTaskData.txt" For Input As #10
        While Not EOF(10)
            'SSN, ErrorMsg, Queue, StrTranslation
            Input #10, ssn, ErrorMsg, Queue, StrTranslation
            If InStr(1, QueuesPopulated, Queue) = False Then
                If QueuesPopulated = "" Then
                    QueuesPopulated = "- " & Queue
                Else
                    QueuesPopulated = QueuesPopulated & vbLf & "- " & Queue
                End If
            End If
            If StrTranslation = "True" Then
                FileSaveFlag = True
            End If
            SP.Common.AddLP9O ssn, Queue, "", ErrorMsg
        Wend
        Close #10
        'save data file if flag is set to true
        If FileSaveFlag Then
            FileCopy FTPDir & FIP, SaveInDir & FIP
        End If
        Kill "T:\AACErrorQueueTaskData.txt"
        If SP.Common.SendMail(SP.Common.BSYSRecips("AAC Script Errors"), "AACScript@utahsbr.edu", "AAC Queue Tasks Created", "Queues populated:" & vbLf & QueuesPopulated, , , , , SP.Common.TestMode()) = False Then
            MsgBox "An error occured while trying to send an email.  Please contact Systems Support."
        End If
    End If
    
    'Update lender servicer info
    If Dir("T:\AACLenderServicer.txt") <> "" Then
        Open "T:\AACLenderServicer.txt" For Input As #91
            Do While Not EOF(91)
                Input #91, ssn, UID, LD_TRX
                'If FileTypeInProc = Rehab Then LenderAndServicer ssn, UID, LD_TRX
            Loop
        Close #91
    End If
End Sub

Private Sub CreateQTsks()
    Dim UID As String
    Dim LD_TRX As String
    Dim ssn As String
    Dim ErrorMsg As String
    Dim Queue As String
    Dim StrTranslation As String
    Dim QueuesPopulated As String
    Dim FileSaveFlag As Boolean
    
    DeDupFile "T:\AACQueueTaskData.txt"
    If Dir("T:\AACQueueTaskData.txt") <> "" Then
        Open "T:\AACQueueTaskData.txt" For Input As #10
        While Not EOF(10)
            Input #10, ssn, ErrorMsg, Queue, StrTranslation
            SP.Common.AddLP9O ssn, Queue, "", ErrorMsg
        Wend
        Close #10
        Kill "T:\AACQueueTaskData.txt"
    End If
End Sub

Private Sub DeDupFile(file As String)
    Dim x As Integer
    Dim Y As Integer
    Dim ssn As String
    Dim ErrorMsg As String
    Dim Queue As String
    Dim StrTranslation As String
    Dim Arr() As String
    
    ReDim Arr(4, 0)
    If Dir(file) = "" Then Exit Sub
    Open file For Input As #10
        While Not EOF(10)
            Input #10, ssn, ErrorMsg, Queue, StrTranslation
            Arr(0, UBound(Arr, 2)) = ssn
            Arr(1, UBound(Arr, 2)) = ErrorMsg
            Arr(2, UBound(Arr, 2)) = Queue
            Arr(3, UBound(Arr, 2)) = StrTranslation
            ReDim Preserve Arr(4, UBound(Arr, 2) + 1)
        Wend
    Close #10
    ReDim Preserve Arr(4, UBound(Arr, 2) - 1)
    Open file For Output As #10
    Close #10
    For x = 0 To UBound(Arr, 2)
        If Arr(4, x) <> "X" Then
            'add to file
            Open file For Append As #10
                Write #10, Arr(0, x), Arr(1, x), Arr(2, x), Arr(3, x)
            Close #10
            'mark all matching ssn and queues
            If x < UBound(Arr, 2) Then
                For Y = x + 1 To UBound(Arr, 2)
                    If Arr(0, x) = Arr(0, Y) And Arr(2, x) = Arr(2, Y) Then
                        Arr(4, Y) = "X"
                    End If
                Next Y
            End If
        End If
    Next x
End Sub

'this sub orchestrates system updates
Private Sub UpdateSys(FTIP As FileTypeInProcess, FIP As String)
    Dim HeaderRow As String
    Dim RHRec As RehabRec
    Dim RPRec As RepurchaseRec
    Dim TPRec As TILPRec
    Dim ProcCounter As Integer
    Dim LoanProcCounter As Integer
    Dim BatchInProc As String
    
    Open FTPDir & FIP For Input As #1
    Line Input #1, HeaderRow
    While Not EOF(1)
        ValidationErrFound = False
        If FTIP = rehab Then
            RHRec.BF_SSN = ""
            BatchInProc = RHObjectsLoaded(FTIP, RHRec)
        ElseIf FTIP = Repurch Then
            RPRec.BF_SSN = ""
            BatchInProc = RPObjectsLoaded(FTIP, RPRec)
        ElseIf FTIP = tilp Then
            TPRec.BF_SSN = ""
            BatchInProc = TILPObjectsLoaded(FTIP, TPRec, FIP)
        End If
        'recovery processing to align batch ID
        If RecvInfo.batchId <> "" Then
            'search for batch the script was on when it stopped
            While RecvInfo.batchId <> BatchInProc
                If FTIP = rehab Then
                    BatchInProc = RHObjectsLoaded(FTIP, RHRec)
                ElseIf FTIP = Repurch Then
                    BatchInProc = RPObjectsLoaded(FTIP, RPRec)
                ElseIf FTIP = tilp Then
                    BatchInProc = TILPObjectsLoaded(FTIP, TPRec, FIP)
                End If
            Wend
            RecvInfo.batchId = "" 'reset to nothing so script doesn't try and recover to the correct batch ID again
        Else
            'not in recovery or if the script has recovered then update log
            If RecvInfo.Phase = 0 Then UpdateRecvLog BatchInProc, "", "", 2, ""
        End If
        
        'process everything collected for the batch
        While ProcCounter < UBound(Borrs) + 1
            'recovery processing to align SSN
            If RecvInfo.ssn <> "" Then
                'search for the SSN the script was on
                While RecvInfo.ssn <> Borrs(ProcCounter).BSSN
                    ProcCounter = ProcCounter + 1
                Wend
                RecvInfo.ssn = "" 'reset to nothing so script doesn't try and recovery to the correct SSN again
            Else
                'not in recovery or if the script has recovered then update log
                If RecvInfo.Phase = 0 Then UpdateRecvLog BatchInProc, Borrs(ProcCounter).BSSN, "", 2, ""
            End If
            If RecvInfo.Phase < 3 Then 'recovery check
                'verify borrower and reference demographic info
                VerifyPersonsExist BatchInProc, FTIP, "B", ProcCounter
                UpdateRecvLog BatchInProc, Borrs(ProcCounter).BSSN, "", 3, ""
            End If
            If RecvInfo.Phase <= 4 Then 'recovery check
                'update all student information
                LoanProcCounter = 0
                While LoanProcCounter < UBound(Borrs(ProcCounter).loans)
                    'recovery processing to align loan
                    If RecvInfo.clid <> "" Then
                        While RecvInfo.clid <> Borrs(ProcCounter).loans(LoanProcCounter).clid
                            LoanProcCounter = LoanProcCounter + 1
                        Wend
                        LoanProcCounter = LoanProcCounter + 1
                        RecvInfo.clid = "" 'reset to nothing so script doesn't try and recovery to the correct CLID again
                    End If
                    'only process if recovery doesn't take pass last loan for borrower
                    If LoanProcCounter < UBound(Borrs(ProcCounter).loans) Then
                        'only process student if PLUS loan
                        If Borrs(ProcCounter).loans(LoanProcCounter).AC_LON_TYP = "PL" Then
                            VerifyPersonsExist BatchInProc, FTIP, "S", ProcCounter, LoanProcCounter
                        End If
                        UpdateRecvLog BatchInProc, Borrs(ProcCounter).BSSN, Borrs(ProcCounter).loans(LoanProcCounter).clid, 4, ""
                    End If
                    LoanProcCounter = LoanProcCounter + 1
                Wend
                UpdateRecvLog BatchInProc, Borrs(ProcCounter).BSSN, "", 5, ""
            End If
            If RecvInfo.Phase < 12 Then
                WorkQueueTask ProcCounter, BatchInProc, FTIP
            End If
            ProcCounter = ProcCounter + 1
        Wend
        ProcCounter = 0
        If RecvInfo.Phase < 12 Then 'recovery check
            If ValidationErrFound = False Then LockBatch BatchInProc, Borrs(0).TILPOnlyBatchLvlFees
            UpdateRecvLog BatchInProc, "", "", 12, ""
        End If
        UnassignQueueTsks 'check if unassign Q tasks is needed
    Wend
    Close #1
End Sub

'this sub locks the batch
Private Sub LockBatch(BatchInProc As String, LateFees As String)
    Dim FieldErrorStr As String
    If LateFees <> "" Then
        If CDbl(LateFees) <> 0 Then
            ModFastPath "CTA0L" & BatchInProc & ";" & "00001"
            puttext 18, 66, GetText(20, 66, 10), "Enter"
        End If
    End If
    ModFastPath "CTA0H" & BatchInProc & ";" & "00001"
    If Check4Text(12, 18, " 0") = False Or Check4Text(12, 31, " 0") = False Or Check4Text(20, 18, " 0.00") = False Or Check4Text(20, 54, "     ") = False Then
        If Check4Text(12, 18, " 0") = False Then FieldErrorStr = "Borrowers"
        If Check4Text(12, 31, " 0") = False Then
            If FieldErrorStr = "" Then
                FieldErrorStr = "Loans"
            Else
                FieldErrorStr = FieldErrorStr & "," & "Loans"
            End If
        End If
        If Check4Text(20, 18, " 0.00") = False Then
            If FieldErrorStr = "" Then
                FieldErrorStr = "Principal"
            Else
                FieldErrorStr = FieldErrorStr & "," & "Principal"
            End If
        End If
        EntryIntoErrorFile Borrs(0).BSSN, "Batch: " & BatchInProc & "  The following fields had a variance: " & FieldErrorStr & "  Variance exists on TA0H. Review.", "AACVLERR"
    Else
        Hit "Enter"
        puttext 21, 27, "Y", "Enter"
    End If
End Sub

'this sub gets the script into AAC
Private Sub WorkQueueTask(PC As Integer, batchId As String, FTIP As FileTypeInProcess)
    If Borrs(PC).SkipAACProcessingBecauseOfErrors = False Then
        'skip AAC processing if fatal errors were found earlier
        FastPath "TX3ZITX6T;;" & batchId
        'select only option
        puttext 21, 18, "1", "Enter"
        'Check to see if the user has two tasked assigned to them
        If Check4Text(23, 2, "01848") = True Then
            MsgBox "You are assigned to multiple tasks. Unassign one and restart the script."
            End
        End If
        
        EnterLoanInformation PC, batchId, FTIP
        If RecvInfo.Phase < 10 Then 'recovery check
            'enter deferment data if there is any
            If UBound(Borrs(PC).loans(0).Deferments) > 0 Then
                EnterDefermentInformation PC, 0, batchId, FTIP
            End If
            'do forbearance thing if file isn't TILP
            If FTIP <> tilp Then
                'Enter Forbearance Data
                'If UBound(Borrs(PC).Loans(0).Forbearances) > 0 Then
                EnterForbearanceInformation PC, 0, batchId, FTIP
                'End If
            Else
                'for TILP
                TILPRepaymentSchedule PC
            End If
            UpdateRecvLog batchId, Borrs(PC).BSSN, "", 10, ""
        End If
        If RecvInfo.Phase < 11 Then 'recovery check
            'Validation PC, batchId, FTIP
            UpdateRecvLog batchId, Borrs(PC).BSSN, "", 11, ""
        End If
    End If
End Sub

'this sub validates the borrower's loan level information
Private Sub Validation(PC As Integer, BID As String, FTIP As FileTypeInProcess)
    Dim Row As Integer
    Dim TargetRow As Integer
    Dim ErrorFound As Boolean
    Dim FoundWarning As Boolean
    Dim FoundReject As Boolean
    Dim FoundFatal As Boolean
    Dim Found00001 As Boolean
    Dim Found00002 As Boolean
    Dim Found00003 As Boolean
    Dim ErrorStr As String
    
    ModFastPath "CTA0G" & Borrs(PC).BSSN
    If Check4Text(1, 72, "TAX0T") = False Then
        Row = 9
        'selection screen
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            'select record
            If Len(GetText(Row, 3, 3)) = 1 Then
                puttext 22, 12, GetText(Row, 3, 3)
                Hit "End"
            Else
                puttext 22, 12, GetText(Row, 3, 3)
            End If
            Hit "Enter"
            
            TargetRow = 7
            ErrorFound = False
            FoundWarning = False
            FoundReject = False
            FoundFatal = False
            Found00001 = False
            Found00002 = False
            Found00003 = False
            While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And ErrorFound = False
                If Check4Text(TargetRow, 46, "FATAL") Or Check4Text(TargetRow, 46, "REJECT") Then
                    If Check4Text(TargetRow, 8, "00001") = False And Check4Text(TargetRow, 8, "00002") = False And Check4Text(TargetRow, 8, "00003") = False Then
                        EntryIntoErrorFile Borrs(PC).BSSN, "Batch: " & BID & " LnSeq: " & GetText(4, 16, 3) & " ErrorNum: " & GetText(TargetRow, 8, 5) & "  Validation error received  Review TA0G to determine how to fix error.", "AACVLERR"
                        ErrorFound = True 'flag to get out of the loop
                    End If
                Else
                    'set flags for warning cases
                    If Check4Text(TargetRow, 46, "FATAL") Then
                        FoundFatal = True
                    ElseIf Check4Text(TargetRow, 46, "REJECT") Then
                        FoundReject = True
                    ElseIf Check4Text(TargetRow, 46, "WARNING") Then
                        FoundWarning = True
                    End If
                    If Check4Text(TargetRow, 8, "00001") Then
                        Found00001 = True
                        ErrorStr = ErrorStr & " " & SP.Q.GetText(TargetRow, 8, 5)
                    ElseIf Check4Text(TargetRow, 8, "00002") Then
                        Found00002 = True
                        ErrorStr = ErrorStr & " " & SP.Q.GetText(TargetRow, 8, 5)
                    ElseIf Check4Text(TargetRow, 8, "00003") Then
                        Found00003 = True
                        ErrorStr = ErrorStr & " " & SP.Q.GetText(TargetRow, 8, 5)
                    End If
                End If
                TargetRow = TargetRow + 2
                'check 4 page forward
                If Check4Text(TargetRow, 46, " ") Or TargetRow = 23 Then
                    TargetRow = 7
                    Hit "F8"
                End If
            Wend
            If FoundWarning = False And FoundReject = False And FoundFatal And (Found00001 Or Found00002 Or Found00003) Then
                EntryIntoErrorFile Borrs(PC).BSSN, "Batch: " & BID & " LnSeq: " & GetText(4, 16, 3) & " ErrorNum:" & ErrorStr & "  Validation error received  Review TA0G to determine how to fix error.", "AACVLERR"
            End If
            Hit "F12"
            Row = Row + 1
            If Check4Text(Row, 4, " ") Then
                Row = 9
                Hit "F8"
            End If
        Wend
    Else
        'target screen
        TargetRow = 7
        ErrorFound = False
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And ErrorFound = False
            If Check4Text(TargetRow, 46, "FATAL") Or Check4Text(TargetRow, 46, "REJECT") Then
                If Check4Text(TargetRow, 8, "00001") = False And Check4Text(TargetRow, 8, "00002") = False And Check4Text(TargetRow, 8, "00003") = False Then
                    EntryIntoErrorFile Borrs(PC).BSSN, "Batch: " & BID & " LnSeq: " & GetText(4, 16, 3) & " ErrorNum: " & GetText(TargetRow, 8, 5) & "  Validation error received  Review TA0G to determine how to fix error.", "AACVLERR"
                    ErrorFound = True 'flag to get out of the loop
                End If
            End If
            TargetRow = TargetRow + 2
            'check 4 page forward
            If Check4Text(TargetRow, 46, " ") Or TargetRow = 23 Then
                TargetRow = 7
                Hit "F8"
            End If
        Wend
    End If
End Sub

'this function enters the loan level information
Private Sub EnterLoanInformation(PC As Integer, BID As String, FTIP As FileTypeInProcess)
    Dim LC As Integer
    While LC < UBound(Borrs(PC).loans)
        If RecvInfo.clid <> "" Then
            While Borrs(PC).loans(LC).clid <> RecvInfo.clid
                LC = LC + 1
            Wend
            'if recovery phase is = 9 then the previous loan was completed and the script needs to go on to the next one
            If RecvInfo.Phase = 9 Then
                LC = LC + 1
                RecvInfo.Phase = 0
            End If
            RecvInfo.clid = "" 'reset to nothing so script doesn't try and recovery to the correct CLID again
        End If
        'only process if recovery hasn't take counter past last loan for borrower
        If LC < UBound(Borrs(PC).loans) Then
            If RecvInfo.Phase < 6 Then 'recovery check
                If FTIP = rehab Then AddToLenderServicerNoDup Borrs(PC).BSSN, Borrs(PC).loans(LC).clid, Borrs(PC).loans(LC).LD_TRX_EFF
                ModFastPath "ATA3X" & Borrs(PC).BSSN & ";"
                puttext 4, 16, Borrs(PC).loans(LC).BondID
                puttext 4, 43, Borrs(PC).loans(LC).Guarantor
                puttext 4, 66, Borrs(PC).loans(LC).LoanPrg
                puttext 5, 16, Borrs(PC).loans(LC).OrigLender
                puttext 5, 66, Borrs(PC).loans(LC).RepType
                If Borrs(PC).loans(LC).AppRcvdDt <> "" Then puttext 9, 16, Format(CDate(Borrs(PC).loans(LC).AppRcvdDt), "MMDDYY")
                If Borrs(PC).loans(LC).DtNoteSigned <> "" Then puttext 9, 70, Format(CDate(Borrs(PC).loans(LC).DtNoteSigned), "MMDDYY")
                If Borrs(PC).loans(LC).DtGuaranteed <> "" Then puttext 12, 38, Format(CDate(Borrs(PC).loans(LC).DtGuaranteed), "MMDDYY")
                puttext 12, 70, Borrs(PC).loans(LC).AmountGuaranteed
                puttext 11, 52, Borrs(PC).loans(LC).clid
                If Borrs(PC).loans(LC).TermBg <> "" Then puttext 13, 16, Format(CDate(Borrs(PC).loans(LC).TermBg), "MMDDYY")
                If Borrs(PC).loans(LC).TermEd <> "" Then puttext 13, 38, Format(CDate(Borrs(PC).loans(LC).TermEd), "MMDDYY")
                puttext 15, 43, Borrs(PC).loans(LC).OrigSchool
                puttext 16, 16, Borrs(PC).loans(LC).SubsidyCd
                puttext 16, 43, Borrs(PC).loans(LC).SpallElig
                puttext 10, 70, Borrs(PC).loans(LC).AC_STU_DFR_REQ
                Hit "F8" 'move to next screen
                puttext 10, 16, Borrs(PC).loans(LC).PriorTo7_93
                puttext 10, 43, Borrs(PC).loans(LC).WtdAveInt
                puttext 11, 45, Borrs(PC).loans(LC).UndrlyDisb
                If Borrs(PC).loans(LC).UndrlyDisb = "A" Then
                    EntryIntoErrorFile Borrs(PC).BSSN, "CLID: " & Borrs(PC).loans(LC).clid & " Batch: " & BID & "  Loan Guaranteed after 07/01/01.  Verify underlying loan code in COMPASS TS5O.", "RVWCONSL"
                End If
                puttext 15, 16, Borrs(PC).loans(LC).SSSN
                If (FTIP = tilp) And ((Borrs(PC).TILPOnlyLD_LFT_SCL = "") Or (Borrs(PC).TILPOnlyDefExists And Borrs(PC).TILPOnlyLD_LFT_SCLIsGTLD_DFR_BEG) Or Borrs(PC).TILPOnlyLD_LFT_SCLIsGTsth_eff_dt) Or (Borrs(PC).TILPOnlyLD_LFT_SCLIsLTAnyDisbDt) Then
                    'skip logic in else statement if conditional statement is true
                Else
                    'enter enrollment information
                    puttext 15, 43, Borrs(PC).loans(LC).CurrSchool
                    If Borrs(PC).loans(LC).SchSepDate <> "" Then puttext 15, 71, Format(CDate(Borrs(PC).loans(LC).SchSepDate), "MMDDYY")
                    puttext 16, 16, Borrs(PC).loans(LC).SepReason
                    puttext 16, 43, Borrs(PC).loans(LC).SepSource
                    If Borrs(PC).loans(LC).CertDt <> "" Then puttext 16, 71, Format(CDate(Borrs(PC).loans(LC).CertDt), "MMDDYY")
                    If Borrs(PC).loans(LC).NtfRecdDt <> "" Then puttext 17, 16, Format(CDate(Borrs(PC).loans(LC).NtfRecdDt), "MMDDYY")
                    puttext 17, 43, Borrs(PC).loans(LC).GraceMonth
                    If Borrs(PC).loans(LC).GracePeriodEnd <> "" Then puttext 17, 71, Format(CDate(Borrs(PC).loans(LC).GracePeriodEnd), "MMDDYY")
                End If
                If (FTIP = rehab) Then
                    If Borrs(PC).loans(LC).LD_RHB <> "" Then puttext 18, 43, Format(CDate(Borrs(PC).loans(LC).LD_RHB), "MMDDYY")
                    If Borrs(PC).loans(LC).PRI_COST <> "" Then puttext 18, 66, Borrs(PC).loans(LC).PRI_COST
                     If Borrs(PC).loans(LC).LA_INT <> "" And Borrs(PC).loans(LC).LA_INT <> "0.00" Then puttext 19, 13, Borrs(PC).loans(LC).LA_INT
					'Rehab sch
                    If (Borrs(PC).loans(LC).BA_STD_PAY = "" Or Borrs(PC).loans(LC).BL_LA_PAY_XPC = "") Then
                        puttext 20, 14, ""
                    Else
                        If CDbl(Borrs(PC).loans(LC).BA_STD_PAY) > CDbl(Borrs(PC).loans(LC).BL_LA_PAY_XPC) Then puttext 20, 14, "N"
                        If CDbl(Borrs(PC).loans(LC).BA_STD_PAY) <= CDbl(Borrs(PC).loans(LC).BL_LA_PAY_XPC) Then puttext 20, 14, "S"
                    End If
                    puttext 20, 48, Borrs(PC).loans(LC).LA_PAY_XPC
                End If
                Hit "F8"
                If FTIP <> tilp Then
                    puttext 10, 16, Borrs(PC).loans(LC).OriginalIntRate
                    puttext 10, 40, MID(Borrs(PC).loans(LC).OriginalIntRate, 1, 1)
                End If
                puttext 12, 10, Borrs(PC).loans(LC).IntRate
                puttext 12, 19, Borrs(PC).loans(LC).IntType
                puttext 12, 65, Borrs(PC).loans(LC).SpecRate
                If Borrs(PC).IBRForgivenessStartDate <> "" And Borrs(PC).IBRCreatDate <> "" Then
                    puttext 13, 12, Borrs(PC).IBREligibleInd
                    puttext 13, 35, Format(Borrs(PC).IBRForgivenessStartDate, "MMDDYY")
                    puttext 13, 66, Borrs(PC).IBRStndStndPayAmount
                    puttext 14, 16, Borrs(PC).IBRQualifyingPayments
                    puttext 15, 16, Format(Borrs(PC).IBRCreatDate, "MM")
                    puttext 15, 19, "01"
                    puttext 15, 22, Format(Borrs(PC).IBRCreatDate, "YY")
                End If

                Hit "F10"
                If Check4Text(23, 2, "03970") Then Hit "F10"
                If Check4Text(23, 2, "01004") = False Then
                    'problem was found create error record and skip the rest of the processing
                    EntryIntoErrorFile Borrs(PC).BSSN, "CLID: " & Borrs(PC).loans(LC).clid & " Batch: " & BID & "  " & GetText(23, 2, 75), "AACLNERR", True
                    Borrs(PC).SkipAACProcessingBecauseOfErrors = True
                    Exit Sub
                End If
                'get loan seq#
                Borrs(PC).loans(LC).SystemSeqNum = GetText(6, 43, 3)
                UpdateRecvLog BID, Borrs(PC).BSSN, Borrs(PC).loans(LC).clid, 6, Borrs(PC).loans(LC).SystemSeqNum
            Else
                'if script is in recovery for a point beyond this one then the sequence number needs to be noted for future processing
                Borrs(PC).loans(LC).SystemSeqNum = RecvInfo.LnSeq
            End If
            'verify that screen is CTA3X
            If Check4Text(1, 4, "CTA3X") = False Then
                'if not on CTA3X then go there
                ModFastPath "CTA3X" & Borrs(PC).BSSN & ";" & Borrs(PC).loans(LC).SystemSeqNum
            End If
            If RecvInfo.Phase < 7 Then 'recovery check
                'check if endorser info exists
                If Borrs(PC).loans(LC).ESSN <> "" And Replace(Replace(SP.Q.GetText(20, 16, 11), "_", ""), " ", "") = "" Then
                    'check if needed endorser demographic information is populated
                    If Borrs(PC).loans(LC).EAddr1 <> "" And Borrs(PC).loans(LC).ECity <> "" And Borrs(PC).loans(LC).EState <> "" And Borrs(PC).loans(LC).EZip <> "" Then
                        'if needed demographic data is present
                        puttext 20, 16, Borrs(PC).loans(LC).ESSN, "Enter"
                        If Check4Text(23, 2, "11106 NO ENDORSER INFORMATION FOUND") Then
                            'endorser wasn't found in the system or not linked to the loan
                            Session.MoveCursor 20, 16
                            Hit "F4"
                            VerifyPersonsExist BID, FTIP, "E", PC, LC
                            'return back to AAC
                            Hit "F12"
                        End If
                    Else
                        'if needed demographic info doesn't exists
                        EntryIntoErrorFile Borrs(PC).BSSN, "CLID: " & Borrs(PC).loans(LC).clid & " Batch: " & BID & "  Missing Key Endorser Demographic Info", "AACLNERR", True
                    End If
                End If
              
                UpdateRecvLog BID, Borrs(PC).BSSN, Borrs(PC).loans(LC).clid, 7, Borrs(PC).loans(LC).SystemSeqNum
            End If
            If RecvInfo.Phase < 8 Then 'recovery check
                Hit "F2"
                Hit "F4"
                'enter disbursment data
                EnterDisbursementInformation PC, LC, BID, FTIP
                UpdateRecvLog BID, Borrs(PC).BSSN, Borrs(PC).loans(LC).clid, 8, Borrs(PC).loans(LC).SystemSeqNum
            End If
            If RecvInfo.Phase < 9 Then 'recovery check
                'enter financial data
                If FTIP = tilp Then
                    EnterTILPLoanFinancialInformation PC, LC
                Else
                    EnterLoanFinancialInformation PC, LC, BID, FTIP
                End If
                UpdateRecvLog BID, Borrs(PC).BSSN, Borrs(PC).loans(LC).clid, 9, Borrs(PC).loans(LC).SystemSeqNum
            End If
        End If
        LC = LC + 1
    Wend
End Sub

'this sub enters deferment information
Private Sub EnterDefermentInformation(PC As Integer, LC As Integer, BID As String, FTIP As FileTypeInProcess)
    Dim DC As Integer
    Dim Row As Integer
    Dim Page As Integer
    Dim PageI As Integer
    Dim rowI As Integer
    Dim FigurePageCounter As Integer
    Dim FigurePageCounterI As Integer
    
    Row = 11
    'enter deferment information
    ModFastPath "ATA0B" & Borrs(PC).BSSN
    While DC < UBound(Borrs(PC).loans(LC).Deferments)
        puttext Row, 7, Format(CDate(Borrs(PC).loans(LC).Deferments(DC).BeginDate), "MMDDYY")
        If Borrs(PC).loans(LC).Deferments(DC).EndDate <> "" Then puttext Row, 18, Format(CDate(Borrs(PC).loans(LC).Deferments(DC).EndDate), "MMDDYY")
        puttext Row, 30, Borrs(PC).loans(LC).Deferments(DC).Type
        puttext Row, 44, Borrs(PC).loans(LC).Deferments(DC).SchoolCode
        If Borrs(PC).loans(LC).Deferments(DC).CertDate <> "" Then puttext Row, 55, Format(CDate(Borrs(PC).loans(LC).Deferments(DC).CertDate), "MMDDYY")
        puttext Row, 66, Borrs(PC).loans(LC).Deferments(DC).CapInt
        Row = Row + 1
        If Row = 22 Then
            'if there are more to add then submit the current additions and get ready to add the rest
            If (DC + 1) <> UBound(Borrs(PC).loans(LC).Deferments) Then
                Hit "Enter"
                Row = 11
                ModFastPath "ATA0B" & Borrs(PC).BSSN
            End If
        End If
        DC = DC + 1
    Wend
    Hit "Enter" 'submit additions
    
    'switch to change mode for "Rel" field update
    ModFastPath "CTA0B" & Borrs(PC).BSSN
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        Row = 11
        While Check4Text(Row, 3, "_")
            puttext Row, 3, "X"
            Row = Row + 1
        Wend
        Hit "F10"
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            'update screen
            rowI = 11
            While Check4Text(rowI, 3, " ") = False
                If Check4Text(rowI, 3, "Y") = False Then
                    puttext rowI, 3, "Y"
                End If
                rowI = rowI + 1
            Wend
            Hit "Enter"
            're-enter selection screen
            ModFastPath "CTA0B" & Borrs(PC).BSSN
            'page forward to correct selection screen
            FigurePageCounter = 0
            While Page > FigurePageCounter
                Hit "F8"
                FigurePageCounter = FigurePageCounter + 1
            Wend
            'select everything on selection screen
            Row = 11
            While Check4Text(Row, 3, "_")
                puttext Row, 3, "X"
                Row = Row + 1
            Wend
            Hit "F10" 'move to target screen
            PageI = PageI + 1
            FigurePageCounterI = 0
            'page to the correct target screen
            While PageI > FigurePageCounterI
                FigurePageCounterI = FigurePageCounterI + 1
                Hit "F8"
            Wend
        Wend
        Page = Page + 1 'increment page counter
        ModFastPath "CTA0B" & Borrs(PC).BSSN
        FigurePageCounter = 0
        While Page > FigurePageCounter
            Hit "F8"
            FigurePageCounter = FigurePageCounter + 1
        Wend
    Wend
End Sub

'this sub enters financial information for rehab and repurchase files
Private Sub EnterLoanFinancialInformation(PC As Integer, LC As Integer, BID As String, FTIP As FileTypeInProcess)
    Dim Finder As Integer
    Dim PaidThruDate As String
    Dim TPaidThruDate As String
    Dim x As Integer
    
    ModFastPath "CTA05" & Borrs(PC).BSSN
    If Check4Text(1, 72, "TAX0X") Then
        'if selection screen
        Finder = 9
        'find seq number
        While GetText(Finder, 6, 3) <> CStr(Val(Borrs(PC).loans(LC).SystemSeqNum))
            Finder = Finder + 1
            If Finder = 22 Then
                Hit "F8"
                Finder = 9
            End If
        Wend
        'select record
        puttext 22, 12, GetText(Finder, 3, 3), "Enter"
    End If
    'Tot Indbt
    If Borrs(PC).loans(LC).AC_LON_TYP = "CL" Then puttext 10, 13, Borrs(PC).loans(LC).TotIndbt
    '81) Principal
    If FTIP = Repurch Then
        puttext 11, 13, Borrs(PC).loans(LC).LA_PRI
    End If
    If FTIP = rehab Then
        puttext 11, 13, Borrs(PC).loans(LC).PRI_COST
    End If
    'Int Last Capped
    'If Borrs(PC).loans(LC).IntLastCapped <> "" Then PutText 11, 46, Format(CDate(Borrs(PC).loans(LC).IntLastCapped), "MMDDYY")
    'Subsidized Interest
    If Borrs(PC).loans(LC).AC_LON_TYP = "SF" Then
        For x = 0 To UBound(Borrs(PC).loans(LC).Deferments)
            If Borrs(PC).loans(LC).Deferments(x).EndDate <> "" Then
                If PaidThruDate = "" Then
                    PaidThruDate = Borrs(PC).loans(LC).Deferments(x).EndDate
                Else
                    If CDate(PaidThruDate) < CDate(Borrs(PC).loans(LC).Deferments(x).EndDate) Then
                        PaidThruDate = Borrs(PC).loans(LC).Deferments(x).EndDate
                    End If
                End If
            End If
        Next x
    
        If PaidThruDate = "" Then
            If Borrs(PC).loans(LC).GracePeriodEnd <> "" Then SP.Q.puttext 16, 48, Format(CDate(Borrs(PC).loans(LC).GracePeriodEnd), "MMDDYY")
        Else
            SP.Q.puttext 16, 48, Format(CDate(PaidThruDate), "MMDDYY")
        End If
    End If
    
    'Non-Subsidized Interest
    puttext 17, 32, Borrs(PC).loans(LC).LA_INT, "END"
    puttext 17, 67, Format(CDate(Borrs(PC).loans(LC).IntLastCapped), "MMDDYY")
    Hit "enter"
End Sub

Private Sub EnterForbearanceInformation(PC As Integer, LC As Integer, BID As String, FTIP As FileTypeInProcess)
    Dim x As Integer
    Dim Bdt As Date 'begining date
    Dim Edt As Date 'ending date
    Dim TempBdt As Date 'begining date
    Dim TempEdt As Date 'ending date
    Dim AddCnt As Integer
    Dim x2 As Integer
    
    ModFastPath "ATA0C" & Borrs(PC).BSSN
    If FileTypeInProc = rehab Then
    
        If UBound(Borrs(PC).loans(0).Deferments) > 0 Then
            
            Bdt = CDate(Borrs(PC).loans(LC).RepayStartDt)
            Edt = CDate(Borrs(PC).loans(LC).LD_TRX_EFF_Less9Months)
            For x = 0 To UBound(Borrs(PC).loans(LC).Deferments)
                If Borrs(PC).loans(LC).Deferments(x).BeginDate <> "" Then
                    TempBdt = CDate(Borrs(PC).loans(LC).Deferments(x).BeginDate)
                    TempEdt = CDate(Borrs(PC).loans(LC).Deferments(x).EndDate)
                    If TempBdt > Bdt And TempEdt < Edt Then
                        AddForbearance Bdt, TempBdt - 1, "11"
                        AddCnt = AddCnt + 1
                        Bdt = TempEdt + 1
                    ElseIf TempBdt > Bdt And TempEdt > Edt Then
                        Edt = TempBdt - 1
                    End If
                End If
            Next x
            AddForbearance Bdt, Edt, "11"
            AddCnt = AddCnt + 1
        
            SP.Q.Hit "ENTER"
             x = 1
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                'select forbearance
                For x2 = 11 To 22
                    If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "X"
                Next x2
                
                SP.Q.Hit "F10"
                'select loans
                Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    For x2 = 11 To 22
                        If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "Y", "ENTER"
                    Next x2
                    SP.Q.Hit "F8"
                Loop
                
                SP.Q.Hit "F12"
                'clear Xs so you can move to next page
                For x2 = 11 To 22
                    If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "", "END"
                Next x2
                SP.Q.Hit "F8"
            Loop
        Else
            'no deferment exits
            Bdt = CDate(Borrs(PC).loans(LC).RepayStartDt)
            Edt = CDate(Borrs(PC).loans(LC).LD_TRX_EFF_Less9Months)
            AddForbearance Bdt, Edt, "11"
            SP.Q.Hit "ENTER"
            SP.Q.puttext 11, 3, "X"
            SP.Q.Hit "F10"
            'select loans
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x2 = 11 To 22
                    If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "Y", "ENTER"
                Next x2
                SP.Q.Hit "F8"
            Loop
        End If
    ElseIf FileTypeInProc = Repurch Then
        If UBound(Borrs(PC).loans(0).Forbearances) > 0 Then
            For x = 1 To UBound(Borrs(PC).loans(LC).Forbearances)
                If x Mod 11 = 1 And x > 1 Then
                    SP.Q.Hit "ENTER"
                    ModFastPath "ATA0C" & Borrs(PC).BSSN
                End If
                AddForbearance CDate(Borrs(PC).loans(LC).Forbearances(x).BeginDate), CDate(Borrs(PC).loans(LC).Forbearances(x).EndDate), "09"
            Next x
            SP.Q.Hit "ENTER"
            x = 1
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                'select forbearance
                For x2 = 11 To 22
                    If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "X"
                Next x2
                
                SP.Q.Hit "F10"
                'select loans
                Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    For x2 = 11 To 22
                        If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "Y", "ENTER"
                    Next x2
                    SP.Q.Hit "F8"
                Loop
                
                SP.Q.Hit "F12"
                'clear Xs so you can move to next page
                For x2 = 11 To 22
                    If SP.Q.GetText(x2, 3, 1) <> "" Then SP.Q.puttext x2, 3, "", "END"
                Next x2
                SP.Q.Hit "F8"
            Loop

        End If
        'Validation PC, BID, Repurch
    End If
End Sub

Private Sub RepaymentSchedule(PC As Integer, BID As String)
    Dim tSSN As String
    Dim CLUID As String
    Dim Amt As String
    Dim CurPrin As String
    Dim DisbDt1 As String
    Dim TotalAmt As Double
    Dim IntType As String
    Dim IntRate As String
    Dim x As Integer
    Dim xx As Integer
    Dim InstallmentAmt As String
    Dim RepaymentTerms As String
    Dim ReqArr() As String
    
    Dim DoSTFFRD As Boolean 'Has consol loan type should loop again for that loan type
    Dim DoCNS As Boolean
    Dim DoSPC As Boolean
    Dim DoCNSLDN As Boolean
    Dim DoSLS As Boolean
    Dim DoPLUS As Boolean
    
    Dim CurrentLoanType As String
    Dim ProcessedIntTypeRate As String 'This is a coma delimited list of interest types and rates already processed.
    Dim NumberOfRunsNeeded As Integer 'this is the number of unique interest types and rates
    Dim RunCount As Integer 'number of times ran
    
    DoSTFFRD = False
    DoCNS = False
    DoSPC = False
    DoCNSLDN = False
    DoSLS = False
    DoPLUS = False
    
    ReDim ReqArr(3, 0)
    Open RepaymentFile For Input As #92
        Do While Not EOF(92)
            Input #92, tSSN, CLUID, Amt, CurPrin, DisbDt1
            If Amt = "" Then Amt = 0
            If tSSN = Borrs(PC).BSSN Then
                ReqArr(0, UBound(ReqArr, 2)) = CLUID
                ReqArr(1, UBound(ReqArr, 2)) = Amt
                ReqArr(2, UBound(ReqArr, 2)) = CurPrin
                ReqArr(3, UBound(ReqArr, 2)) = DisbDt1
                ReDim Preserve ReqArr(3, UBound(ReqArr, 2) + 1)
            End If
        Loop
    Close #92
    
    'go to validation sub first
    'Validation PC, BID, rehab
    'GET LOAN TYPES TO PROCESS
    ModFastPath "CTA3X" & Borrs(PC).BSSN
    If SP.Q.Check4Text(1, 72, "TAX3Z") = False Then
        Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For x = 9 To 21
                If SP.Q.GetText(x, 21, 6) <> "" Then
                    If SP.Q.Check4Text(x, 21, "STFFRD") Or SP.Q.Check4Text(x, 21, "UNSTFD") Then DoSTFFRD = True
                    If SP.Q.Check4Text(x, 21, "SUBCNS") Or SP.Q.Check4Text(x, 21, "UNCNS") Then DoCNS = True
                    If SP.Q.Check4Text(x, 21, "SUBSPC") Or SP.Q.Check4Text(x, 21, "UNSPC") Then DoSPC = True
                    If SP.Q.Check4Text(x, 21, "CNSLDN") Then DoCNSLDN = True
                    If SP.Q.Check4Text(x, 21, "SLS") Then DoSLS = True
                    If SP.Q.Check4Text(x, 21, "PLUS") Then DoPLUS = True
                Else
                    Exit For
                End If
            Next x
            SP.Q.Hit "F8"
        Loop
    End If
    Do
        TotalAmt = 0
        'Validation PC, BID, rehab
        ModFastPath "CTA3X" & Borrs(PC).BSSN
        If SP.Q.Check4Text(1, 72, "TAX3Z") = False Then
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x = 9 To 21
                    If DoSTFFRD = True Then
                        If SP.Q.Check4Text(x, 21, "STFFRD") Or SP.Q.Check4Text(x, 21, "UNSTFD") Then
                            SP.Q.puttext 22, 12, Format(SP.Q.GetText(x, 3, 2), "00"), "ENTER"
                            CurrentLoanType = "STFFRD"
                            'DoSTFFRD = True
                            Exit Do
                        End If
                    ElseIf DoCNS = True Then
                        If SP.Q.Check4Text(x, 21, "SUBCNS") Or SP.Q.Check4Text(x, 21, "UNCNS") Then
                            SP.Q.puttext 22, 12, Format(SP.Q.GetText(x, 3, 2), "00"), "ENTER"
                            CurrentLoanType = "SUBCNS"
                            'DoCNS = False
                            Exit Do
                        End If
                    ElseIf DoSPC = True Then
                        If SP.Q.Check4Text(x, 21, "SUBSPC") Or SP.Q.Check4Text(x, 21, "UNSPC") Then
                            SP.Q.puttext 22, 12, Format(SP.Q.GetText(x, 3, 2), "00"), "ENTER"
                            CurrentLoanType = "SUBSPC"
                            'DoSPC = False
                            Exit Do
                        End If
                    ElseIf DoCNSLDN = True Then
                        If SP.Q.Check4Text(x, 21, "CNSLDN") Then
                            SP.Q.puttext 22, 12, Format(SP.Q.GetText(x, 3, 2), "00"), "ENTER"
                            CurrentLoanType = "CNSLDN"
                            'DoCNSLDN = False
                            Exit Do
                        End If
                    ElseIf DoSLS = True Then
                        If SP.Q.Check4Text(x, 21, "SLS") Then
                            SP.Q.puttext 22, 12, Format(SP.Q.GetText(x, 3, 2), "00"), "ENTER"
                            CurrentLoanType = "SLS"
                            'DoSLS = False
                            Exit Do
                        End If
                    ElseIf DoPLUS = True Then
                        If SP.Q.Check4Text(x, 21, "PLUS") Then
                            SP.Q.puttext 22, 12, Format(SP.Q.GetText(x, 3, 2), "00"), "ENTER"
                            CurrentLoanType = "PLUS"
                            'DoPLUS = False
                            Exit Do
                        End If
                    End If
                    
                Next x
                SP.Q.Hit "F8"
            Loop
        End If
        SP.Q.Hit "F2"
        SP.Q.Hit "F11"
        If SP.Q.Check4Text(1, 72, "TSX0S") Then SP.Q.Hit "ENTER"
        
        If SP.Q.Check4Text(23, 2, "03612 ALL LOANS BYPASSED, FULL REVIEW OF ACCOUNT IS REQUIRED") = False Then
            Dim str As String
            'get number of runs needed
            If NumberOfRunsNeeded = 0 Then
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x = 10 To 22
                    IntType = SP.Q.GetText(x, 39, 2)
                    IntRate = SP.Q.GetText(x, 43, 4)
                    If IntType = "" Then Exit For
                    If InStr(str, IntType & IntRate & ",") = 0 Then
                        str = str & IntType & IntRate & ","
                        NumberOfRunsNeeded = NumberOfRunsNeeded + 1
                    End If
                Next x
                SP.Q.Hit "F8"
            Loop
            End If
            SP.Q.Hit "F12"
            If SP.Q.Check4Text(1, 72, "TSX0S") Then SP.Q.Hit "ENTER" Else SP.Q.Hit "F11"
            'get the next unprocessed interest type and rate
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x = 10 To 22
                    IntType = SP.Q.GetText(x, 39, 2)
                    IntRate = SP.Q.GetText(x, 43, 4)
                    If InStr(ProcessedIntTypeRate, IntType & IntRate & ",") = 0 Then
                        Exit Do
                    End If
                Next x
                SP.Q.Hit "F8"
            Loop
            ProcessedIntTypeRate = ProcessedIntTypeRate & IntType & IntRate & "," 'add to processed list
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x = 10 To 22
                    'find matching interest type and rate
                    If SP.Q.Check4Text(x, 39, IntType) And SP.Q.Check4Text(x, 43, IntRate) Then
                        SP.Q.puttext x, 3, "X"
                        'find in array and add amt to total
                        For xx = 0 To UBound(ReqArr, 2) - 1
                            If SP.Q.GetText(x, 16, 10) = Replace(FormatCurrency(ReqArr(2, xx), 2), "$", "") And _
                            SP.Q.GetText(x, 5, 10) = Format(CDate(ReqArr(3, xx)), "MM/DD/YYYY") Then
                                TotalAmt = TotalAmt + ReqArr(1, xx)
                                Exit For
                            End If
                        Next xx
                    End If
                Next x
                SP.Q.Hit "F8"
            Loop
            SP.Q.Hit "ENTER"
            SP.Q.puttext 8, 14, "L", "ENTER"
            InstallmentAmt = SP.Q.GetText(22, 68, 10)
            Dim decrement As Integer
            'decrement = 1
            Do While InstallmentAmt < TotalAmt
                RepaymentTerms = SP.Q.GetText(9, 23, 3)
                SP.Q.Hit "F12"
                SP.Q.Hit "ENTER"
                SP.Q.puttext 8, 14, "L"
                SP.Q.puttext 9, 23, Format(CInt(RepaymentTerms) - 1, "000"), "ENTER"
                InstallmentAmt = SP.Q.GetText(22, 68, 10)
            Loop
            If NumberOfRunsNeeded <= UBound(Split(ProcessedIntTypeRate, ",")) Then
                If CurrentLoanType = "STFFRD" Then
                    DoSTFFRD = False
                ElseIf CurrentLoanType = "SUBSPC" Then
                    DoSPC = False
                ElseIf CurrentLoanType = "CNSLDN" Then
                    DoCNSLDN = False
                ElseIf CurrentLoanType = "SLS" Then
                    DoSLS = False
                ElseIf CurrentLoanType = "PLUS" Then
                    DoPLUS = False
                End If
                ProcessedIntTypeRate = ""
            End If
                
            SP.Q.Hit "F4"
            SP.Q.Hit "F12"
            SP.Q.Hit "F12"
            If SP.Q.Check4Text(1, 72, "TSX0S") Then SP.Q.Hit "F12"
            SP.Q.Hit "F2"
            SP.Q.Hit "F2"
            SP.Q.Hit "F11"
            SP.Q.Hit "ENTER"
            If SP.Q.Check4Text(23, 2, "02895") Then
                'Validation PC, BID, rehab
                If DoSTFFRD = False And DoCNS = False And DoSPC = False And DoCNSLDN = False And DoSLS = False And DoPLUS = False Then Exit Do
            End If
        Else
            Dim tamount As Double
            tamount = 0
            Open RepaymentFile For Input As #92
            Do While Not EOF(92)
                Input #92, tSSN, CLUID, Amt, CurPrin, DisbDt1
                If Amt = "" Then Amt = 0
                If tSSN = Borrs(PC).BSSN Then
                    tamount = tamount + CDbl(Amt)
                End If
            Loop
            Close #92
            EntryIntoQueueFile Borrs(PC).BSSN, "AACVLERR", FormatCurrency(tamount, 2)
            Exit Do
        End If
    Loop
End Sub

Private Sub AddForbearance(Bdt As Date, Edt As Date, Ftype As String)
    Dim x As Integer
    For x = 11 To 22
        If SP.Q.GetText(x, 8, 1) = "_" Then
            found = True
            Exit For
        End If
    Next x
    
    puttext x, 8, Format(Bdt, "MMDDYY")
    puttext x, 19, Format(Edt, "MMDDYY")
    puttext x, 30, Ftype
    puttext x, 47, "3"
    puttext x, 63, "N"
End Sub

'this sub enters disbursment level information
Private Sub EnterDisbursementInformation(PC As Integer, LC As Integer, BID As String, FTIP As FileTypeInProcess)
    Dim DC As Integer
    While DC < UBound(Borrs(PC).loans(LC).Disbursements())
        If Borrs(PC).loans(LC).Disbursements(DC).DisbDate <> "" Then puttext 8, 69, Format(CDate(Borrs(PC).loans(LC).Disbursements(DC).DisbDate), "MMDDYY")
        puttext 9, 17, Borrs(PC).loans(LC).Disbursements(DC).DisbAmt
        puttext 11, 15, Borrs(PC).loans(LC).Disbursements(DC).DisbMedium
        puttext 10, 17, Borrs(PC).loans(LC).Disbursements(DC).DisbCancelAmt
        If Borrs(PC).loans(LC).Disbursements(DC).DisbCancelDt <> "" Then puttext 10, 44, Format(CDate(Borrs(PC).loans(LC).Disbursements(DC).DisbCancelDt), "MMDDYY")
        puttext 12, 15, Borrs(PC).loans(LC).Disbursements(DC).DisbCancelReason
        'check if origination fee
        If Val(Borrs(PC).loans(LC).Disbursements(DC).OFee) > 0 Then
            puttext 21, 26, Borrs(PC).loans(LC).Disbursements(DC).OFee 'enter o fee info
        Else
            puttext 21, 9, "", "End" 'clear o fee as a fee type
        End If
        'check if GTE
        If Val(Borrs(PC).loans(LC).Disbursements(DC).GTE) > 0 Then
            puttext 22, 26, Borrs(PC).loans(LC).Disbursements(DC).GTE 'enter GTE fee info
        Else
            puttext 22, 9, "", "End" 'clear GTE as a fee type
        End If
        'clear other fee types and options
        puttext 20, 9, "", "End"
        Hit "Enter"
        DC = DC + 1
        'if there is another disbursement to enter then hit F6 to get another screen to enter it in
        If DC <> UBound(Borrs(PC).loans(LC).Disbursements()) Then Hit "F6"
    Wend
End Sub

'this function verifies that the borrower and other vital persons exist on the system
'if they don't then it will orchestrate the adding them to the system
Private Sub VerifyPersonsExist(BatchInProc As String, FTIP As FileTypeInProcess, PType As String, BorrowerIndx As Integer, Optional LoanIndx As Integer = 0)
    'The system won't accept "FC" as a state code, so we'll need to watch for that and replace it with something useful.
    Dim borrowerState As String
    If Borrs(BorrowerIndx).BState = "FC" Then borrowerState = "__" Else borrowerState = Borrs(BorrowerIndx).BState
    Dim endorserState As String
    If Borrs(BorrowerIndx).loans(LoanIndx).EState = "FC" Then endorserState = "__" Else endorserState = Borrs(BorrowerIndx).loans(LoanIndx).EState
    Dim reference1State As String
    If Borrs(BorrowerIndx).R1State = "FC" Then reference1State = "__" Else reference1State = Borrs(BorrowerIndx).R1State
    Dim reference2State As String
    If Borrs(BorrowerIndx).R2State = "FC" Then reference2State = "__" Else reference2State = Borrs(BorrowerIndx).R2State
    Dim studentState As String
    If Borrs(BorrowerIndx).loans(LoanIndx).SState = "FC" Then studentState = "__" Else studentState = Borrs(BorrowerIndx).loans(LoanIndx).SState
    
    'check for borrower on TX1J
    If PType = "B" Then
        FastPath "TX3ZITX1JB;" & Borrs(BorrowerIndx).BSSN
    ElseIf PType = "S" Then
        FastPath "TX3ZITX1JS;" & Borrs(BorrowerIndx).loans(LoanIndx).SSSN
    End If
    If PType = "B" Or PType = "S" Then
        If Check4Text(1, 72, "TXX1K") Then
            'person doesn't exist on system
            If PType = "B" Then
                'add borrower and both references
                If Borrs(BorrowerIndx).BAddr1 <> "" And Borrs(BorrowerIndx).BCity <> "" And Borrs(BorrowerIndx).BState <> "" And Borrs(BorrowerIndx).BZip <> "" Then
                    'if needed demographic info exists
                    EnterPersonInfo FTIP, "B", Borrs(BorrowerIndx).BLastName, Borrs(BorrowerIndx).BFirstName, Borrs(BorrowerIndx).BMID, Borrs(BorrowerIndx).BAddr1, Borrs(BorrowerIndx).BAddr2, Borrs(BorrowerIndx).BCity, borrowerState, Borrs(BorrowerIndx).BZip, Borrs(BorrowerIndx).BAddrInd, Borrs(BorrowerIndx).BPhone, Borrs(BorrowerIndx).BPhoneInd, Borrs(BorrowerIndx).BSSN, Borrs(BorrowerIndx).BPhoneSource, , , , Borrs(BorrowerIndx).BDOB
                Else
                    'if needed demographic info doesn't exists
                    EntryIntoErrorFile Borrs(BorrowerIndx).BSSN, "Batch: " & BatchInProc & "  Missing Key Borrower Demographic Info", "AACLNERR", True
                    Borrs(BorrowerIndx).SkipAACProcessingBecauseOfErrors = True
                End If
                If Borrs(BorrowerIndx).R1Addr1 <> "" And Borrs(BorrowerIndx).R1City <> "" And Borrs(BorrowerIndx).R1State <> "" And Borrs(BorrowerIndx).R1Zip <> "" Then EnterPersonInfo FTIP, "R", Borrs(BorrowerIndx).R1LastName, Borrs(BorrowerIndx).R1FirstName, Borrs(BorrowerIndx).R1MID, Borrs(BorrowerIndx).R1Addr1, Borrs(BorrowerIndx).R1Addr2, Borrs(BorrowerIndx).R1City, reference1State, Borrs(BorrowerIndx).R1Zip, Borrs(BorrowerIndx).R1AddrInd, Borrs(BorrowerIndx).R1Phone, Borrs(BorrowerIndx).R1PhoneInd, Borrs(BorrowerIndx).BSSN, Borrs(BorrowerIndx).BPhoneSource, , Borrs(BorrowerIndx).R1Relation
                If Borrs(BorrowerIndx).R2Addr1 <> "" And Borrs(BorrowerIndx).R2City <> "" And Borrs(BorrowerIndx).R2State <> "" And Borrs(BorrowerIndx).R2Zip <> "" Then EnterPersonInfo FTIP, "R", Borrs(BorrowerIndx).R2LastName, Borrs(BorrowerIndx).R2FirstName, Borrs(BorrowerIndx).R2MID, Borrs(BorrowerIndx).R2Addr1, Borrs(BorrowerIndx).R2Addr2, Borrs(BorrowerIndx).R2City, reference2State, Borrs(BorrowerIndx).R2Zip, Borrs(BorrowerIndx).R2AddrInd, Borrs(BorrowerIndx).R2Phone, Borrs(BorrowerIndx).R2PhoneInd, Borrs(BorrowerIndx).BSSN, Borrs(BorrowerIndx).BPhoneSource, , Borrs(BorrowerIndx).R2Relation
            ElseIf PType = "S" Then
                'if the ptype is student
                If Borrs(BorrowerIndx).loans(LoanIndx).SAddr1 <> "" And Borrs(BorrowerIndx).loans(LoanIndx).SCity <> "" And Borrs(BorrowerIndx).loans(LoanIndx).SState <> "" And Borrs(BorrowerIndx).loans(LoanIndx).SZip <> "" Then
                    'if needed demographic info exists
                    EnterPersonInfo FTIP, "S", Borrs(BorrowerIndx).loans(LoanIndx).SLastName, Borrs(BorrowerIndx).loans(LoanIndx).SFirstName, Borrs(BorrowerIndx).loans(LoanIndx).SMID, Borrs(BorrowerIndx).loans(LoanIndx).SAddr1, Borrs(BorrowerIndx).loans(LoanIndx).SAddr2, Borrs(BorrowerIndx).loans(LoanIndx).SCity, studentState, Borrs(BorrowerIndx).loans(LoanIndx).SZip, Borrs(BorrowerIndx).loans(LoanIndx).SAddrInd, Borrs(BorrowerIndx).loans(LoanIndx).SPhone, Borrs(BorrowerIndx).loans(LoanIndx).SPhoneInd, Borrs(BorrowerIndx).loans(LoanIndx).SSSN, Borrs(BorrowerIndx).BPhoneSource, , , , Borrs(BorrowerIndx).loans(LoanIndx).SDOB
                Else
                    'if needed demographic info doesn't exists
                    EntryIntoErrorFile Borrs(BorrowerIndx).BSSN, "CLID: " & Borrs(BorrowerIndx).loans(LoanIndx).clid & " Batch: " & BatchInProc & "  Missing Key Student Demographic Info", "AACLNERR", True
                    Borrs(BorrowerIndx).SkipAACProcessingBecauseOfErrors = True
                End If
            End If
        Else
            'person exists on system
            If PType = "B" Then
                'if the ptype is borrower
                If Borrs(BorrowerIndx).BAddr1 <> "" And Borrs(BorrowerIndx).BCity <> "" And Borrs(BorrowerIndx).BState <> "" And Borrs(BorrowerIndx).BZip <> "" Then
                    'if needed demographic info exists
                    VerifyPersonInfo "B", Borrs(BorrowerIndx).BAddr1, Borrs(BorrowerIndx).BAddr2, Borrs(BorrowerIndx).BCity, borrowerState, Borrs(BorrowerIndx).BZip, Borrs(BorrowerIndx).BPhone, Borrs(BorrowerIndx).BPhoneSource, Borrs(BorrowerIndx).BDOB
                Else
                    'if needed demographic info doesn't exists
                    Borrs(BorrowerIndx).SkipAACProcessingBecauseOfErrors = True
                    EntryIntoErrorFile Borrs(BorrowerIndx).BSSN, "Batch: " & BatchInProc & "  Missing Key Borrower Demographic Info", "AACLNERR", True
                End If
                Hit "F2"
                Hit "F4"
                If Check4Text(23, 2, "01121") = False Then
                    'some reference info found
                    If Borrs(BorrowerIndx).R1Addr1 <> "" And Borrs(BorrowerIndx).R1City <> "" And Borrs(BorrowerIndx).R1State <> "" And Borrs(BorrowerIndx).R1Zip <> "" Then
                        VerifyPersonInfo "R", Borrs(BorrowerIndx).R1Addr1, Borrs(BorrowerIndx).R1Addr2, Borrs(BorrowerIndx).R1City, reference1State, Borrs(BorrowerIndx).R1Zip, Borrs(BorrowerIndx).R1Phone, Borrs(BorrowerIndx).BPhoneSource, Borrs(BorrowerIndx).BDOB, Borrs(BorrowerIndx).BSSN, FTIP, Borrs(BorrowerIndx).R1FirstName, Borrs(BorrowerIndx).R1LastName, Borrs(BorrowerIndx).R1MID, Borrs(BorrowerIndx).R1Relation, Borrs(BorrowerIndx).R1AddrInd, Borrs(BorrowerIndx).R1PhoneInd
                    End If
                    FastPath "TX3ZITX1JB;" & Borrs(BorrowerIndx).BSSN
                    Hit "F2"
                    Hit "F4"
                    If Borrs(BorrowerIndx).R2Addr1 <> "" And Borrs(BorrowerIndx).R2City <> "" And Borrs(BorrowerIndx).R2State <> "" And Borrs(BorrowerIndx).R2Zip <> "" Then
                        VerifyPersonInfo "R", Borrs(BorrowerIndx).R2Addr1, Borrs(BorrowerIndx).R2Addr2, Borrs(BorrowerIndx).R2City, reference2State, Borrs(BorrowerIndx).R2Zip, Borrs(BorrowerIndx).R2Phone, Borrs(BorrowerIndx).BPhoneSource, Borrs(BorrowerIndx).BDOB, Borrs(BorrowerIndx).BSSN, FTIP, Borrs(BorrowerIndx).R2FirstName, Borrs(BorrowerIndx).R2LastName, Borrs(BorrowerIndx).R2MID, Borrs(BorrowerIndx).R2Relation, Borrs(BorrowerIndx).R2AddrInd, Borrs(BorrowerIndx).R2PhoneInd
                    End If
                Else
                    'no reference info at all
                    If Borrs(BorrowerIndx).R1Addr1 <> "" And Borrs(BorrowerIndx).R1City <> "" And Borrs(BorrowerIndx).R1State <> "" And Borrs(BorrowerIndx).R1Zip <> "" Then
                        EnterPersonInfo FTIP, "R", Borrs(BorrowerIndx).R1LastName, Borrs(BorrowerIndx).R1FirstName, Borrs(BorrowerIndx).R1MID, Borrs(BorrowerIndx).R1Addr1, Borrs(BorrowerIndx).R1Addr2, Borrs(BorrowerIndx).R1City, reference1State, Borrs(BorrowerIndx).R1Zip, Borrs(BorrowerIndx).R1AddrInd, Borrs(BorrowerIndx).R1Phone, Borrs(BorrowerIndx).R1PhoneInd, Borrs(BorrowerIndx).BSSN, Borrs(BorrowerIndx).BPhoneSource, , Borrs(BorrowerIndx).R1Relation
                    End If
                    If Borrs(BorrowerIndx).R2Addr1 <> "" And Borrs(BorrowerIndx).R2City <> "" And Borrs(BorrowerIndx).R2State <> "" And Borrs(BorrowerIndx).R2Zip <> "" Then
                        EnterPersonInfo FTIP, "R", Borrs(BorrowerIndx).R2LastName, Borrs(BorrowerIndx).R2FirstName, Borrs(BorrowerIndx).R2MID, Borrs(BorrowerIndx).R2Addr1, Borrs(BorrowerIndx).R2Addr2, Borrs(BorrowerIndx).R2City, reference2State, Borrs(BorrowerIndx).R2Zip, Borrs(BorrowerIndx).R2AddrInd, Borrs(BorrowerIndx).R2Phone, Borrs(BorrowerIndx).R2PhoneInd, Borrs(BorrowerIndx).BSSN, Borrs(BorrowerIndx).BPhoneSource, , Borrs(BorrowerIndx).R2Relation
                    End If
                End If
            ElseIf PType = "S" Then
                'if the ptype is student
                If Borrs(BorrowerIndx).loans(LoanIndx).SAddr1 <> "" And Borrs(BorrowerIndx).loans(LoanIndx).SCity <> "" And Borrs(BorrowerIndx).loans(LoanIndx).SState <> "" And Borrs(BorrowerIndx).loans(LoanIndx).SZip <> "" Then
                    'if needed demographic info exists
                    VerifyPersonInfo "S", Borrs(BorrowerIndx).loans(LoanIndx).SAddr1, Borrs(BorrowerIndx).loans(LoanIndx).SAddr2, Borrs(BorrowerIndx).loans(LoanIndx).SCity, studentState, Borrs(BorrowerIndx).loans(LoanIndx).SZip, Borrs(BorrowerIndx).loans(LoanIndx).SPhone, Borrs(BorrowerIndx).BPhoneSource, Borrs(BorrowerIndx).BDOB
                Else
                    'if needed demographic info doesn't exists
                    EntryIntoErrorFile Borrs(BorrowerIndx).BSSN, "CLID: " & Borrs(BorrowerIndx).loans(LoanIndx).clid & " Batch: " & BatchInProc & "  Missing Key Student Demographic Info", "AACLNERR", True
                    Borrs(BorrowerIndx).SkipAACProcessingBecauseOfErrors = True
                End If
            End If
        End If
    ElseIf PType = "E" Then
        'check if last name is blank
        If Check4Text(4, 6, "_") = False Then
            'endorser already exists
            If Borrs(BorrowerIndx).loans(LoanIndx).AC_EDS_TYP = "S" Or Borrs(BorrowerIndx).loans(LoanIndx).AC_EDS_TYP = "C" Then
                puttext 8, 11, "M"
            ElseIf Borrs(BorrowerIndx).loans(LoanIndx).AC_EDS_TYP = "E" Then
                puttext 8, 11, "S"
            End If
            If Borrs(BorrowerIndx).loans(LoanIndx).AC_LON_TYP <> "CL" Then
                puttext 8, 36, "01"
            Else
                puttext 8, 36, "06"
            End If
            Hit "Enter"
            VerifyPersonInfo "E", Borrs(BorrowerIndx).loans(LoanIndx).EAddr1, Borrs(BorrowerIndx).loans(LoanIndx).EAddr2, Borrs(BorrowerIndx).loans(LoanIndx).ECity, endorserState, Borrs(BorrowerIndx).loans(LoanIndx).EZip, Borrs(BorrowerIndx).loans(LoanIndx).EPhone, Borrs(BorrowerIndx).BPhoneSource, Borrs(BorrowerIndx).BDOB
        Else
            'endorser doesn't exist
            EnterPersonInfo FTIP, "E", Borrs(BorrowerIndx).loans(LoanIndx).ELastName, Borrs(BorrowerIndx).loans(LoanIndx).EFirstName, Borrs(BorrowerIndx).loans(LoanIndx).EMID, Borrs(BorrowerIndx).loans(LoanIndx).EAddr1, Borrs(BorrowerIndx).loans(LoanIndx).EAddr2, Borrs(BorrowerIndx).loans(LoanIndx).ECity, endorserState, Borrs(BorrowerIndx).loans(LoanIndx).EZip, Borrs(BorrowerIndx).loans(LoanIndx).EAddrInd, Borrs(BorrowerIndx).loans(LoanIndx).EPhone, Borrs(BorrowerIndx).loans(LoanIndx).EPhoneInd, Borrs(BorrowerIndx).loans(LoanIndx).ESSN, Borrs(BorrowerIndx).BPhoneSource, Borrs(BorrowerIndx).loans(LoanIndx).AC_EDS_TYP, , Borrs(BorrowerIndx).loans(LoanIndx).AC_LON_TYP, Borrs(BorrowerIndx).loans(LoanIndx).EDOB
        End If
    End If
End Sub

'this sub verifies that a person's information is still the same, if it isn't then it updates it
Private Sub VerifyPersonInfo(PType As String, Addr1 As String, Addr2 As String, city As String, state As String, zip As String, Phn As String, PhnSource As String, ByVal dateOfBirth As String, Optional ssn As String, Optional FTIP As FileTypeInProcess, Optional FN As String, Optional ln As String, Optional MI As String, Optional Relationship As String, Optional AddrVld As String, Optional PhnVld As String, Optional LP22Proc As Boolean = False)
    Dim ChangedToChangeMode As Boolean
    Dim Row As Integer
    Dim RefName As String
    Dim RefNameSplit() As String
    Dim FoundRefMatch As Boolean
    
    If LP22Proc Then
        'onelink
        'compare address
        If Check4Text(10, 9, MID(Addr1, 1, 4)) = False Then
            ChangedToChangeMode = True
            puttext 1, 7, "C", "enter"
            'update address info
            puttext 10, 9, Addr1
            If Len(Addr1) < 35 Then Hit "end"
            puttext 11, 9, Addr2
            If Len(Addr2) < 35 Then Hit "end"
            puttext 12, 9, city
            If Len(city) < 30 Then Hit "end"
            puttext 12, 52, state
            puttext 12, 60, zip
            If Len(zip) < 9 Then Hit "end"
            puttext 10, 57, "Y"
        End If
        'check phone info
        If GetText(13, 12, 10) <> MID(Phn, 1, 10) Then
            If ChangedToChangeMode = False Then
                puttext 1, 7, "C", "Enter"
                ChangedToChangeMode = True
            End If
            puttext 13, 12, Phn
            puttext 13, 38, "Y"
        End If
        'Check birthdate.
        If Check4Text(4, 72, Replace(dateOfBirth, "/", "")) = False Then
            If ChangedToChangeMode = False Then
                puttext 1, 7, "C", "Enter"
                ChangedToChangeMode = True
            End If
            puttext 4, 72, Replace(dateOfBirth, "/", "")
        End If
        'if anything was changed then hit enter and F6 to post the changes
        If ChangedToChangeMode Then
            puttext 3, 9, "A"
            Hit "Enter"
            Hit "F6"
        End If
    Else
        'compass
        If PType = "R" Then
            If Check4Text(1, 74, "TXX1Y") Then
                'selection screen
                Row = 10
                While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And FoundRefMatch = False
                    If Check4Text(Row, 5, "R") Then
                        RefName = GetText(Row, 23, 23) 'get full ref name
                        RefNameSplit = Split(RefName, ",") 'split up into name parts
                        'check if first four characters of last and first name are the same
                        If Len(RefNameSplit(0)) < 4 Then
                            'if length of last name is < 4
                            If MID(RefNameSplit(0), 1, Len(RefNameSplit(0))) = MID(ln, 1, Len(ln)) Then
                                'if last names match
                                If Len(RefNameSplit(1)) < 4 Then
                                    'if length of first name is less than 4 characters
                                    If MID(RefNameSplit(1), 1, Len(RefNameSplit(1))) = MID(FN, 1, Len(FN)) Then
                                        'if first names match then match is found, select ref record and set flag
                                        puttext 22, 12, GetText(Row, 2, 3)
                                        If Len(GetText(Row, 2, 3)) = 1 Then Hit "End" 'blank the rest of the field if needed
                                        Hit "enter" 'select ref
                                        FoundRefMatch = True
                                    End If
                                Else
                                    'if length of first name is >= 4
                                    If MID(RefNameSplit(1), 1, 4) = MID(FN, 1, 4) Then
                                        'if first names match then match is found, select ref record and set flag
                                        puttext 22, 12, GetText(Row, 2, 3)
                                        If Len(GetText(Row, 2, 3)) = 1 Then Hit "End" 'blank the rest of the field if needed
                                        Hit "enter" 'select ref
                                        FoundRefMatch = True
                                    End If
                                End If
                            End If
                        Else
                            'if length of last name is >= 4
                            If MID(RefNameSplit(0), 1, 4) = MID(ln, 1, 4) Then
                                'if last names match
                                If Len(RefNameSplit(1)) < 4 Then
                                    'if length of first name is less than 4 characters
                                    If MID(RefNameSplit(1), 1, Len(RefNameSplit(1))) = MID(FN, 1, Len(FN)) Then
                                        'if first names match then match is found, select ref record and set flag
                                        puttext 22, 12, GetText(Row, 2, 3)
                                        If Len(GetText(Row, 2, 3)) = 1 Then Hit "End" 'blank the rest of the field if needed
                                        Hit "enter" 'select ref
                                        FoundRefMatch = True
                                    End If
                                Else
                                    'if length of first name is > 4
                                    If MID(RefNameSplit(1), 1, 4) = MID(FN, 1, 4) Then
                                        'if first names match then match is found, select ref record and set flag
                                        puttext 22, 12, GetText(Row, 2, 3)
                                        If Len(GetText(Row, 2, 3)) = 1 Then Hit "End" 'blank the rest of the field if needed
                                        Hit "enter" 'select ref
                                        FoundRefMatch = True
                                    End If
                                End If
                            End If
                        End If
                    End If
                    Row = Row + 1
                    If FoundRefMatch = False And Row = 22 Then
                        'move forward to next page of selections
                        Row = 10
                        Hit "F8"
                    End If
                Wend
            Else
                'target screen
                'check if first four characters of last and first name are the same
                If Len(GetText(4, 6, 5)) < 4 Then
                    'if length of last name is < 4
                    If MID(GetText(4, 6, 5), 1, Len(GetText(4, 6, 5))) = MID(ln, 1, Len(ln)) Then
                        'if last names match
                        If Len(GetText(4, 34, 5)) < 4 Then
                            'if length of first name is less than 4 characters
                            If MID(GetText(4, 34, 5), 1, Len(GetText(4, 34, 5))) = MID(FN, 1, Len(FN)) Then
                                'if first names match then match is found, select ref record and set flag
                                FoundRefMatch = True
                            End If
                        Else
                            'if length of first name is >= 4
                            If MID(GetText(4, 34, 5), 1, 4) = MID(FN, 1, 4) Then
                                'if first names match then match is found, select ref record and set flag
                                FoundRefMatch = True
                            End If
                        End If
                    End If
                Else
                    'if length of last name is >= 4
                    If MID(GetText(4, 6, 5), 1, 4) = MID(ln, 1, 4) Then
                        'if last names match
                        If Len(GetText(4, 34, 5)) < 4 Then
                            'if length of first name is less than 4 characters
                            If MID(GetText(4, 34, 5), 1, Len(GetText(4, 34, 5))) = MID(FN, 1, Len(FN)) Then
                                'if first names match then match is found, select ref record and set flag
                                FoundRefMatch = True
                            End If
                        Else
                            'if length of first name is > 4
                            If MID(GetText(4, 34, 5), 1, 4) = MID(FN, 1, 4) Then
                                'if first names match then match is found, select ref record and set flag
                                FoundRefMatch = True
                            End If
                        End If
                    End If
                End If
            End If
            If FoundRefMatch = False Then
                'add to system
                EnterPersonInfo FTIP, "R", ln, FN, MI, Addr1, Addr2, city, state, zip, AddrVld, Phn, PhnVld, ssn, PhnSource, , Relationship
                Exit Sub
            End If
        End If
        'if borrower, endorser, student or (ref with match from above code)
        'check address info
        'compare last name
        If Check4Text(4, 6, MID(ln, 1, 4)) = False Then
            ChangedToChangeMode = True
            puttext 1, 4, "C", "enter"
            puttext 4, 6, ln, "ENTER"
            EntryIntoQueueFile ssn, "AACNMCHG"
        End If
        'compare first name
        If Check4Text(4, 34, MID(FN, 1, 4)) = False Then
            ChangedToChangeMode = True
            puttext 1, 4, "C", "enter"
            puttext 4, 34, FN, "ENTER"
            EntryIntoQueueFile ssn, "AACNMCHG"
        End If
        'compare address
        If Check4Text(11, 10, MID(Addr1, 1, 4)) = False Then
            ChangedToChangeMode = True
            puttext 1, 4, "C", "enter"
            Hit "F6"
            Hit "F6"
            'update address info
            puttext 11, 10, Addr1
            If Len(Addr1) < 31 Then Hit "end"
            puttext 12, 10, Addr2
            If Len(Addr2) < 31 Then Hit "end"
            puttext 14, 8, city
            If Len(city) < 20 Then Hit "end"
            puttext 14, 32, state
            puttext 14, 40, zip
            If Len(zip) < 17 Then Hit "end"
            puttext 11, 55, "Y"
            puttext 10, 32, Format(Date, "MMDDYY")
            If PType = "B" Then 'if borrower then add source code
                If FTIP = tilp Then
                    puttext 8, 18, "04"
                Else
                    puttext 8, 18, "56"
                End If
            End If
            Hit "Enter"
        End If
        'check phone info
        If Check4Text(1, 71, "TXX1R-03") Or Check4Text(1, 71, "TXX1R-02") Or Check4Text(1, 71, "TXX1R-04") Then
            'TXX1R-03 and TXX1R-02
            If Replace(Replace(GetText(17, 14, 3) & GetText(17, 23, 3) & GetText(17, 31, 4) & GetText(17, 40, 5), "_", ""), " ", "") <> Phn Then
                If ChangedToChangeMode Then
                    Hit "F6"
                Else
                    puttext 1, 4, "C", "Enter"
                    Hit "F6"
                    Hit "F6"
                    Hit "F6"
                End If
                puttext 17, 14, Phn
                puttext 17, 54, "Y"
                If FTIP = tilp Then
                    puttext 19, 14, "04"
                Else
                    puttext 19, 14, "56"
                End If
                puttext 16, 45, Format(Date, "MMDDYY")
                Hit "Enter"
            End If
        Else
            'TXX1R-01
            If Replace(Replace(GetText(17, 14, 3) & GetText(17, 23, 3) & GetText(17, 31, 4) & GetText(17, 40, 5), "_", ""), " ", "") <> Phn Then
                If ChangedToChangeMode Then
                    Hit "F6"
                Else
                    puttext 1, 4, "C", "Enter"
                    Hit "F6"
                    Hit "F6"
                    Hit "F6"
                End If
                puttext 17, 14, Phn
                puttext 17, 54, "Y"
                If FTIP = tilp Then
                    puttext 19, 14, "04"
                Else
                    puttext 19, 14, "56"
                End If
                puttext 16, 45, Format(Date, "MMDDYY")
                Hit "Enter"
            End If
        End If
        'Check DOB.
        If Check4Text(20, 6, Replace(dateOfBirth, "/", " ")) = False Then
            ChangedToChangeMode = True
            puttext 1, 4, "C", "enter"
            puttext 20, 6, MID(dateOfBirth, 1, 2)
            puttext 20, 9, MID(dateOfBirth, 4, 2)
            puttext 20, 12, MID(dateOfBirth, 7, 4)
            Hit "ENTER"
        End If
    End If
End Sub

'this sub adds a person to the system
Private Sub EnterPersonInfo(FTIP As FileTypeInProcess, PType As String, ln As String, FN As String, MID As String, Addr1 As String, Addr2 As String, city As String, state As String, zip As String, AddrVld As String, Phn As String, PhnVld As String, ssn As String, PhnSource As String, Optional AC_EDS_TYPE_E As String = "", Optional Relationship_R As String = "", Optional AC_LON_TYP_E As String = "", Optional DOB_BSE As String = "", Optional LP22Proc As Boolean = False)
    If LP22Proc Then
        'add borrower to OneLINK if from TILP file
        FastPath "LP22A" & ssn
        If Check4Text(22, 3, "48002") Then Exit Sub
        puttext 3, 9, "A"
        puttext 4, 5, ln
        puttext 4, 44, FN
        puttext 4, 60, MID
        If DOB_BSE <> "" Then puttext 4, 72, DOB_BSE
        puttext 10, 9, Addr1
        puttext 11, 9, Addr2
        puttext 12, 9, city
        puttext 12, 52, state
        puttext 12, 60, zip
        puttext 13, 12, Phn
        puttext 10, 57, AddrVld
        puttext 13, 68, "T"
        puttext 13, 38, PhnVld
        Hit "Enter"
        Hit "F6"
        'Address history comment
        Dim UID As String
        UID = SP.Common.GetUserID
    Else
        'add person to COMPASS
        If PType = "B" Or PType = "S" Then
            FastPath "TX3ZATX1J" & PType & ";" & ssn
        ElseIf PType = "R" Then
            FastPath "TX3ZATX1J" & PType
        End If
        puttext 4, 6, ln
        puttext 4, 34, FN
        puttext 4, 53, MID
        puttext 11, 10, Addr1
        puttext 12, 10, Addr2
        puttext 14, 8, city
        puttext 11, 55, AddrVld
        puttext 14, 32, state
        puttext 14, 40, zip
        puttext 16, 20, "U"
        puttext 16, 30, "N"
        puttext 17, 14, Phn
        puttext 17, 54, PhnVld
        puttext 19, 14, PhnSource
        If PType = "B" Then
            puttext 7, 18, "Y"
            If DOB_BSE <> "" Then puttext 20, 6, Format(CDate(DOB_BSE), "MMDDYYYY")
            If FTIP = tilp Then
                puttext 19, 14, "04"
                puttext 8, 18, "04"
            Else
                puttext 19, 14, "56"
                puttext 8, 18, "56"
            End If
        ElseIf PType = "R" Then
            puttext 7, 11, ssn
            If FTIP = tilp Then
                puttext 19, 14, "04"
                puttext 8, 15, "01"
            Else
                puttext 19, 14, "56"
                'repurchase or rehab
                If Relationship_R = "F" Or Relationship_R = "FR" Then puttext 8, 15, "11"
                If Relationship_R = "G" Or Relationship_R = "GU" Then puttext 8, 15, "12"
                If Relationship_R = "M" Or Relationship_R = "SP" Then puttext 8, 15, "06"
                If Relationship_R = "NE" Then puttext 8, 15, "09"
                If Relationship_R = "O" Or Relationship_R = "OT" Then puttext 8, 15, "01"
                If Relationship_R = "P" Or Relationship_R = "PA" Then puttext 8, 15, "02"
                If Relationship_R = "R" Or Relationship_R = "RE" Then puttext 8, 15, "03"
                If Relationship_R = "RM" Then puttext 8, 15, "08"
                If Relationship_R = "S" Or Relationship_R = "SI" Then puttext 8, 15, "07"
            End If
        ElseIf PType = "E" Then
            If DOB_BSE <> "" Then puttext 20, 6, Format(CDate(DOB_BSE), "MMDDYYYY")
            If AC_EDS_TYPE_E = "S" Or AC_EDS_TYPE_E = "C" Then
                puttext 8, 11, "M"
            ElseIf AC_EDS_TYPE_E = "E" Then
                puttext 8, 11, "S"
            End If
            puttext 19, 14, "56"
            If FTIP = tilp Then
                puttext 8, 15, "01"
            Else
                'repurchase or rehab
                If AC_LON_TYP_E = "CL" Then
                    puttext 8, 36, "01"
                Else
                    puttext 8, 36, "06"
                End If
            End If
        ElseIf PType = "S" Then
            puttext 19, 14, "56"
            If DOB_BSE <> "" Then puttext 20, 6, Format(CDate(DOB_BSE), "MMDDYYYY")
        End If
        Hit "Enter" 'submit request to add
        If Check4Text(23, 2, "01079") Then Hit "Enter"
        If PType = "E" Then
            'being called from the add loan part of code
            Hit "F12"
            Hit "F2"
        End If
    End If
End Sub


'this sub blanks out the current borrower/loan data and loads new borrower/loan data
Private Function RHObjectsLoaded(FTIP As FileTypeInProcess, RHRec As RehabRec) As String
    Dim LastSSNProcessed As String
    Dim Batch As String
    Dim TrackingTemp As String
    Dim LoansInBatch As String
    
    'blank current borrower data and init loan level info arrays
    ReDim Borrs(0)
    ReDim Borrs(UBound(Borrs)).loans(0)
    If RHRec.BF_SSN = "" Then
        'get priming read
        Input #1, RHRec.BF_SSN, RHRec.DM_PRS_1, RHRec.DM_PRS_MID, RHRec.DM_PRS_LST, RHRec.DD_BRT, RHRec.DX_STR_ADR_1, RHRec.DX_STR_ADR_2, RHRec.DM_CT, RHRec.DC_DOM_ST, RHRec.DF_ZIP, RHRec.DM_FGN_CNY, RHRec.DI_VLD_ADR, RHRec.DN_PHN, RHRec.DI_PHN_VLD, RHRec.DN_ALT_PHN, RHRec.DI_ALT_PHN_VLD, _
        RHRec.AC_LON_TYP, RHRec.SUBSIDY, RHRec.AD_PRC, RHRec.AF_ORG_APL_OPS_LDR, RHRec.AF_APL_ID, RHRec.AF_APL_ID_SFX, RHRec.AD_IST_TRM_BEG, RHRec.AD_IST_TRM_END, RHRec.AA_GTE_LON_AMT, RHRec.AF_APL_OPS_SCL, RHRec.AD_BR_SIG, RHRec.LD_LFT_SCL, RHRec.PR_RPD_FOR_ITR, RHRec.LC_INT_TYP, RHRec.LD_TRX_EFF, RHRec.LA_TRX, RHRec.LD_RHB, RHRec.LA_PRI, RHRec.LA_INT, RHRec.IF_OPS_SCL_RPT, RHRec.LC_STU_ENR_TYP, RHRec.LD_ENR_CER, RHRec.LD_LDR_NTF, RHRec.AR_CON_ITR, RHRec.AD_APL_RCV, RHRec.AC_STU_DFR_REQ, _
        RHRec.AN_DISB_1, RHRec.AC_DISB_1, RHRec.AD_DISB_1, RHRec.AA_DISB_1, RHRec.ORG_1, RHRec.CD_DISB_1, RHRec.CA_DISB_1, RHRec.GTE_1, RHRec.AN_DISB_2, RHRec.AC_DISB_2, RHRec.AD_DISB_2, RHRec.AA_DISB_2, RHRec.ORG_2, RHRec.CD_DISB_2, RHRec.CA_DISB_2, RHRec.GTE_2, RHRec.AN_DISB_3, RHRec.AC_DISB_3, RHRec.AD_DISB_3, RHRec.AA_DISB_3, RHRec.ORG_3, RHRec.CD_DISB_3, RHRec.CA_DISB_3, RHRec.GTE_3, RHRec.AN_DISB_4, RHRec.AC_DISB_4, RHRec.AD_DISB_4, RHRec.AA_DISB_4, RHRec.ORG_4, RHRec.CD_DISB_4, RHRec.CA_DISB_4, RHRec.GTE_4, RHRec.AA_TOT_EDU_DET_PNT, RHRec.LC_DFR_TYP1, RHRec.LC_DFR_TYP2, RHRec.LC_DFR_TYP3, RHRec.LC_DFR_TYP4, RHRec.LC_DFR_TYP5, RHRec.LC_DFR_TYP6, RHRec.LC_DFR_TYP7, RHRec.LC_DFR_TYP8, RHRec.LC_DFR_TYP9, RHRec.LC_DFR_TYP10, RHRec.LC_DFR_TYP11, RHRec.LC_DFR_TYP12, RHRec.LC_DFR_TYP13, RHRec.LC_DFR_TYP14, RHRec.LC_DFR_TYP15, RHRec.LD_DFR_BEG1, RHRec.LD_DFR_BEG2, RHRec.LD_DFR_BEG3, RHRec.LD_DFR_BEG4, RHRec.LD_DFR_BEG5, _
        RHRec.LD_DFR_BEG6, RHRec.LD_DFR_BEG7, RHRec.LD_DFR_BEG8, RHRec.LD_DFR_BEG9, RHRec.LD_DFR_BEG10, RHRec.LD_DFR_BEG11, RHRec.LD_DFR_BEG12, RHRec.LD_DFR_BEG13, RHRec.LD_DFR_BEG14, RHRec.LD_DFR_BEG15, RHRec.LD_DFR_END1, RHRec.LD_DFR_END2, RHRec.LD_DFR_END3, RHRec.LD_DFR_END4, RHRec.LD_DFR_END5, RHRec.LD_DFR_END6, RHRec.LD_DFR_END7, RHRec.LD_DFR_END8, RHRec.LD_DFR_END9, RHRec.LD_DFR_END10, RHRec.LD_DFR_END11, RHRec.LD_DFR_END12, RHRec.LD_DFR_END13, RHRec.LD_DFR_END14, RHRec.LD_DFR_END15, RHRec.LF_DOE_SCL_DFR1, RHRec.LF_DOE_SCL_DFR2, RHRec.LF_DOE_SCL_DFR3, RHRec.LF_DOE_SCL_DFR4, RHRec.LF_DOE_SCL_DFR5, RHRec.LF_DOE_SCL_DFR6, RHRec.LF_DOE_SCL_DFR7, RHRec.LF_DOE_SCL_DFR8, RHRec.LF_DOE_SCL_DFR9, RHRec.LF_DOE_SCL_DFR10, RHRec.LF_DOE_SCL_DFR11, RHRec.LF_DOE_SCL_DFR12, RHRec.LF_DOE_SCL_DFR13, RHRec.LF_DOE_SCL_DFR14, RHRec.LF_DOE_SCL_DFR15, RHRec.LD_DFR_INF_CER1, _
        RHRec.LD_DFR_INF_CER2, RHRec.LD_DFR_INF_CER3, RHRec.LD_DFR_INF_CER4, RHRec.LD_DFR_INF_CER5, RHRec.LD_DFR_INF_CER6, RHRec.LD_DFR_INF_CER7, RHRec.LD_DFR_INF_CER8, RHRec.LD_DFR_INF_CER9, RHRec.LD_DFR_INF_CER10, RHRec.LD_DFR_INF_CER11, RHRec.LD_DFR_INF_CER12, RHRec.LD_DFR_INF_CER13, RHRec.LD_DFR_INF_CER14, RHRec.LD_DFR_INF_CER15, RHRec.AC_LON_STA_REA1, RHRec.AC_LON_STA_REA2, RHRec.AC_LON_STA_REA3, RHRec.AC_LON_STA_REA4, RHRec.AC_LON_STA_REA5, RHRec.AC_LON_STA_REA6, RHRec.AC_LON_STA_REA7, RHRec.AC_LON_STA_REA8, RHRec.AC_LON_STA_REA9, RHRec.AC_LON_STA_REA10, RHRec.AC_LON_STA_REA11, RHRec.AC_LON_STA_REA12, RHRec.AC_LON_STA_REA13, RHRec.AC_LON_STA_REA14, _
        RHRec.AC_LON_STA_REA15, RHRec.AD_DFR_BEG1, RHRec.AD_DFR_BEG2, RHRec.AD_DFR_BEG3, RHRec.AD_DFR_BEG4, RHRec.AD_DFR_BEG5, RHRec.AD_DFR_BEG6, RHRec.AD_DFR_BEG7, RHRec.AD_DFR_BEG8, RHRec.AD_DFR_BEG9, RHRec.AD_DFR_BEG10, RHRec.AD_DFR_BEG11, RHRec.AD_DFR_BEG12, RHRec.AD_DFR_BEG13, RHRec.AD_DFR_BEG14, RHRec.AD_DFR_BEG15, RHRec.AD_DFR_END1, RHRec.AD_DFR_END2, RHRec.AD_DFR_END3, RHRec.AD_DFR_END4, RHRec.AD_DFR_END5, RHRec.AD_DFR_END6, RHRec.AD_DFR_END7, RHRec.AD_DFR_END8, RHRec.AD_DFR_END9, RHRec.AD_DFR_END10, RHRec.AD_DFR_END11, RHRec.AD_DFR_END12, RHRec.AD_DFR_END13, RHRec.AD_DFR_END14, RHRec.AD_DFR_END15, RHRec.IF_OPS_SCL_RPT1, RHRec.IF_OPS_SCL_RPT2, RHRec.IF_OPS_SCL_RPT3, _
        RHRec.IF_OPS_SCL_RPT4, RHRec.IF_OPS_SCL_RPT5, RHRec.IF_OPS_SCL_RPT6, RHRec.IF_OPS_SCL_RPT7, RHRec.IF_OPS_SCL_RPT8, RHRec.IF_OPS_SCL_RPT9, RHRec.IF_OPS_SCL_RPT10, RHRec.IF_OPS_SCL_RPT11, RHRec.IF_OPS_SCL_RPT12, RHRec.IF_OPS_SCL_RPT13, RHRec.IF_OPS_SCL_RPT14, RHRec.IF_OPS_SCL_RPT15, RHRec.LD_ENR_CER1, RHRec.LD_ENR_CER2, RHRec.LD_ENR_CER3, RHRec.LD_ENR_CER4, RHRec.LD_ENR_CER5, RHRec.LD_ENR_CER6, RHRec.LD_ENR_CER7, RHRec.LD_ENR_CER8, RHRec.LD_ENR_CER9, RHRec.LD_ENR_CER10, RHRec.LD_ENR_CER11, RHRec.LD_ENR_CER12, RHRec.LD_ENR_CER13, RHRec.LD_ENR_CER14, RHRec.LD_ENR_CER15, _
        RHRec.STU_SSN, RHRec.STU_DM_PRS_1, RHRec.STU_DM_PRS_MID, RHRec.STU_DM_PRS_LST, RHRec.STU_DD_BRT, RHRec.STU_DX_STR_ADR_1, RHRec.STU_DX_STR_ADR_2, RHRec.STU_DM_CT, RHRec.STU_DC_DOM_ST, RHRec.STU_DF_ZIP, RHRec.STU_DM_FGN_CNY, RHRec.STU_DI_VLD_ADR, RHRec.STU_DN_PHN, RHRec.STU_DI_PHN_VLD, RHRec.STU_DN_ALT_PHN, RHRec.STU_DI_ALT_PHN_VLD, _
        RHRec.EDSR_SSN, RHRec.EDSR_DM_PRS_1, RHRec.EDSR_DM_PRS_MID, RHRec.EDSR_DM_PRS_LST, RHRec.EDSR_DD_BRT, RHRec.EDSR_DX_STR_ADR_1, RHRec.EDSR_DX_STR_ADR_2, RHRec.EDSR_DM_CT, RHRec.EDSR_DC_DOM_ST, RHRec.EDSR_DF_ZIP, RHRec.EDSR_DM_FGN_CNY, RHRec.EDSR_DI_VLD_ADR, RHRec.EDSR_DN_PHN, RHRec.EDSR_DI_PHN_VLD, RHRec.EDSR_DN_ALT_PHN, RHRec.EDSR_DI_ALT_PHN_VLD, RHRec.AC_EDS_TYP, _
        RHRec.REF_IND, RHRec.BM_RFR_1_1, RHRec.BM_RFR_MID_1, RHRec.BM_RFR_LST_1, RHRec.BX_RFR_STR_ADR_1_1, RHRec.BX_RFR_STR_ADR_2_1, RHRec.BM_RFR_CT_1, RHRec.BC_RFR_ST_1, RHRec.BF_RFR_ZIP_1, RHRec.BM_RFR_FGN_CNY_1, RHRec.BI_VLD_ADR_1, RHRec.BN_RFR_DOM_PHN_1, RHRec.BI_DOM_PHN_VLD_1, RHRec.BN_RFR_ALT_PHN_1, RHRec.BI_ALT_PHN_VLD_1, RHRec.BC_RFR_REL_BR_1, _
        RHRec.BM_RFR_1_2, RHRec.BM_RFR_MID_2, RHRec.BM_RFR_LST_2, RHRec.BX_RFR_STR_ADR_1_2, RHRec.BX_RFR_STR_ADR_2_2, RHRec.BM_RFR_CT_2, RHRec.BC_RFR_ST_2, RHRec.BF_RFR_ZIP_2, RHRec.BM_RFR_FGN_CNY_2, RHRec.BI_VLD_ADR_2, RHRec.BN_RFR_DOM_PHN_2, RHRec.BI_DOM_PHN_VLD_2, RHRec.BN_RFR_ALT_PHN_2, RHRec.BI_ALT_PHN_VLD_2, RHRec.BC_RFR_REL_BR_2, _
        RHRec.BondID, RHRec.AVE_REHB_PAY_AMT, RHRec.BAT_ID, RHRec.BAT_BR_CT, RHRec.BAT_LN_CT, RHRec.BAT_TOT_SUM, RHRec.BR_ELIG_IND, RHRec.LD_IBR_25Y_FGV_BEG, RHRec.LD_IBR_RPD_SR, RHRec.LA_IBR_STD_STD_PAY, RHRec.LN_IBR_QLF_FGV_MTH, RHRec.ORG_INT_RATE, RHRec.BA_STD_PAY, RHRec.LA_PAY_XPC, RHRec.BL_LA_PAY_XPC, RHRec.PRI_COST
    End If
    'take note of batch ID and only process the current batch ID
    If Batch = "" Then Batch = RHRec.BAT_ID
    'get associated system batch ID
    Open "T:\AACMasterTracking.txt" For Input As #2
    '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch
    While Not EOF(2)
        Line Input #2, TrackingTemp
        If Replace(Split(TrackingTemp, ",")(0), """", "") = Batch Then
            RHObjectsLoaded = Replace(Split(TrackingTemp, ",")(4), """", "")
            LoansInBatch = Replace(Split(TrackingTemp, ",")(2), """", "")
        End If
    Wend
    Close #2
    Dim LineCount As Integer
    LineCount = 1
    'load new borrower data (batches of 5)
    Do While Not EOF(1)
        'check if batch is the same if not then exit function with data collected
        If Batch <> RHRec.BAT_ID Then
            Exit Function
        End If
        
        'create new borrower or loan for current borrower as needed
        If LastSSNProcessed = RHRec.BF_SSN Then
            'add loan to existing borrower
            LoadRehabDat Borrs(UBound(Borrs) - 1), RHRec, False
        Else
            'create new borrower
            LastSSNProcessed = RHRec.BF_SSN
            LoadRehabDat Borrs(UBound(Borrs)), RHRec, True
            'init borrower and loan level info arrays
            ReDim Preserve Borrs(UBound(Borrs) + 1)
            ReDim Borrs(UBound(Borrs)).loans(0)
        End If
        'init SSN flag for switching between SSNs
        If LastSSNProcessed = "" Then LastSSNProcessed = RHRec.BF_SSN
        Input #1, RHRec.BF_SSN, RHRec.DM_PRS_1, RHRec.DM_PRS_MID, RHRec.DM_PRS_LST, RHRec.DD_BRT, RHRec.DX_STR_ADR_1, RHRec.DX_STR_ADR_2, RHRec.DM_CT, RHRec.DC_DOM_ST, RHRec.DF_ZIP, RHRec.DM_FGN_CNY, RHRec.DI_VLD_ADR, RHRec.DN_PHN, RHRec.DI_PHN_VLD, RHRec.DN_ALT_PHN, RHRec.DI_ALT_PHN_VLD, _
        RHRec.AC_LON_TYP, RHRec.SUBSIDY, RHRec.AD_PRC, RHRec.AF_ORG_APL_OPS_LDR, RHRec.AF_APL_ID, RHRec.AF_APL_ID_SFX, RHRec.AD_IST_TRM_BEG, RHRec.AD_IST_TRM_END, RHRec.AA_GTE_LON_AMT, RHRec.AF_APL_OPS_SCL, RHRec.AD_BR_SIG, RHRec.LD_LFT_SCL, RHRec.PR_RPD_FOR_ITR, RHRec.LC_INT_TYP, RHRec.LD_TRX_EFF, RHRec.LA_TRX, RHRec.LD_RHB, RHRec.LA_PRI, RHRec.LA_INT, RHRec.IF_OPS_SCL_RPT, RHRec.LC_STU_ENR_TYP, RHRec.LD_ENR_CER, RHRec.LD_LDR_NTF, RHRec.AR_CON_ITR, RHRec.AD_APL_RCV, RHRec.AC_STU_DFR_REQ, _
        RHRec.AN_DISB_1, RHRec.AC_DISB_1, RHRec.AD_DISB_1, RHRec.AA_DISB_1, RHRec.ORG_1, RHRec.CD_DISB_1, RHRec.CA_DISB_1, RHRec.GTE_1, RHRec.AN_DISB_2, RHRec.AC_DISB_2, RHRec.AD_DISB_2, RHRec.AA_DISB_2, RHRec.ORG_2, RHRec.CD_DISB_2, RHRec.CA_DISB_2, RHRec.GTE_2, RHRec.AN_DISB_3, RHRec.AC_DISB_3, RHRec.AD_DISB_3, RHRec.AA_DISB_3, RHRec.ORG_3, RHRec.CD_DISB_3, RHRec.CA_DISB_3, RHRec.GTE_3, RHRec.AN_DISB_4, RHRec.AC_DISB_4, RHRec.AD_DISB_4, RHRec.AA_DISB_4, RHRec.ORG_4, RHRec.CD_DISB_4, RHRec.CA_DISB_4, RHRec.GTE_4, RHRec.AA_TOT_EDU_DET_PNT, RHRec.LC_DFR_TYP1, RHRec.LC_DFR_TYP2, RHRec.LC_DFR_TYP3, RHRec.LC_DFR_TYP4, RHRec.LC_DFR_TYP5, RHRec.LC_DFR_TYP6, RHRec.LC_DFR_TYP7, RHRec.LC_DFR_TYP8, RHRec.LC_DFR_TYP9, RHRec.LC_DFR_TYP10, RHRec.LC_DFR_TYP11, RHRec.LC_DFR_TYP12, RHRec.LC_DFR_TYP13, RHRec.LC_DFR_TYP14, RHRec.LC_DFR_TYP15, RHRec.LD_DFR_BEG1, RHRec.LD_DFR_BEG2, RHRec.LD_DFR_BEG3, RHRec.LD_DFR_BEG4, RHRec.LD_DFR_BEG5, _
        RHRec.LD_DFR_BEG6, RHRec.LD_DFR_BEG7, RHRec.LD_DFR_BEG8, RHRec.LD_DFR_BEG9, RHRec.LD_DFR_BEG10, RHRec.LD_DFR_BEG11, RHRec.LD_DFR_BEG12, RHRec.LD_DFR_BEG13, RHRec.LD_DFR_BEG14, RHRec.LD_DFR_BEG15, RHRec.LD_DFR_END1, RHRec.LD_DFR_END2, RHRec.LD_DFR_END3, RHRec.LD_DFR_END4, RHRec.LD_DFR_END5, RHRec.LD_DFR_END6, RHRec.LD_DFR_END7, RHRec.LD_DFR_END8, RHRec.LD_DFR_END9, RHRec.LD_DFR_END10, RHRec.LD_DFR_END11, RHRec.LD_DFR_END12, RHRec.LD_DFR_END13, RHRec.LD_DFR_END14, RHRec.LD_DFR_END15, RHRec.LF_DOE_SCL_DFR1, RHRec.LF_DOE_SCL_DFR2, RHRec.LF_DOE_SCL_DFR3, RHRec.LF_DOE_SCL_DFR4, RHRec.LF_DOE_SCL_DFR5, RHRec.LF_DOE_SCL_DFR6, RHRec.LF_DOE_SCL_DFR7, RHRec.LF_DOE_SCL_DFR8, RHRec.LF_DOE_SCL_DFR9, RHRec.LF_DOE_SCL_DFR10, RHRec.LF_DOE_SCL_DFR11, RHRec.LF_DOE_SCL_DFR12, RHRec.LF_DOE_SCL_DFR13, RHRec.LF_DOE_SCL_DFR14, RHRec.LF_DOE_SCL_DFR15, RHRec.LD_DFR_INF_CER1, _
        RHRec.LD_DFR_INF_CER2, RHRec.LD_DFR_INF_CER3, RHRec.LD_DFR_INF_CER4, RHRec.LD_DFR_INF_CER5, RHRec.LD_DFR_INF_CER6, RHRec.LD_DFR_INF_CER7, RHRec.LD_DFR_INF_CER8, RHRec.LD_DFR_INF_CER9, RHRec.LD_DFR_INF_CER10, RHRec.LD_DFR_INF_CER11, RHRec.LD_DFR_INF_CER12, RHRec.LD_DFR_INF_CER13, RHRec.LD_DFR_INF_CER14, RHRec.LD_DFR_INF_CER15, RHRec.AC_LON_STA_REA1, RHRec.AC_LON_STA_REA2, RHRec.AC_LON_STA_REA3, RHRec.AC_LON_STA_REA4, RHRec.AC_LON_STA_REA5, RHRec.AC_LON_STA_REA6, RHRec.AC_LON_STA_REA7, RHRec.AC_LON_STA_REA8, RHRec.AC_LON_STA_REA9, RHRec.AC_LON_STA_REA10, RHRec.AC_LON_STA_REA11, RHRec.AC_LON_STA_REA12, RHRec.AC_LON_STA_REA13, RHRec.AC_LON_STA_REA14, _
        RHRec.AC_LON_STA_REA15, RHRec.AD_DFR_BEG1, RHRec.AD_DFR_BEG2, RHRec.AD_DFR_BEG3, RHRec.AD_DFR_BEG4, RHRec.AD_DFR_BEG5, RHRec.AD_DFR_BEG6, RHRec.AD_DFR_BEG7, RHRec.AD_DFR_BEG8, RHRec.AD_DFR_BEG9, RHRec.AD_DFR_BEG10, RHRec.AD_DFR_BEG11, RHRec.AD_DFR_BEG12, RHRec.AD_DFR_BEG13, RHRec.AD_DFR_BEG14, RHRec.AD_DFR_BEG15, RHRec.AD_DFR_END1, RHRec.AD_DFR_END2, RHRec.AD_DFR_END3, RHRec.AD_DFR_END4, RHRec.AD_DFR_END5, RHRec.AD_DFR_END6, RHRec.AD_DFR_END7, RHRec.AD_DFR_END8, RHRec.AD_DFR_END9, RHRec.AD_DFR_END10, RHRec.AD_DFR_END11, RHRec.AD_DFR_END12, RHRec.AD_DFR_END13, RHRec.AD_DFR_END14, RHRec.AD_DFR_END15, RHRec.IF_OPS_SCL_RPT1, RHRec.IF_OPS_SCL_RPT2, RHRec.IF_OPS_SCL_RPT3, _
        RHRec.IF_OPS_SCL_RPT4, RHRec.IF_OPS_SCL_RPT5, RHRec.IF_OPS_SCL_RPT6, RHRec.IF_OPS_SCL_RPT7, RHRec.IF_OPS_SCL_RPT8, RHRec.IF_OPS_SCL_RPT9, RHRec.IF_OPS_SCL_RPT10, RHRec.IF_OPS_SCL_RPT11, RHRec.IF_OPS_SCL_RPT12, RHRec.IF_OPS_SCL_RPT13, RHRec.IF_OPS_SCL_RPT14, RHRec.IF_OPS_SCL_RPT15, RHRec.LD_ENR_CER1, RHRec.LD_ENR_CER2, RHRec.LD_ENR_CER3, RHRec.LD_ENR_CER4, RHRec.LD_ENR_CER5, RHRec.LD_ENR_CER6, RHRec.LD_ENR_CER7, RHRec.LD_ENR_CER8, RHRec.LD_ENR_CER9, RHRec.LD_ENR_CER10, RHRec.LD_ENR_CER11, RHRec.LD_ENR_CER12, RHRec.LD_ENR_CER13, RHRec.LD_ENR_CER14, RHRec.LD_ENR_CER15, _
        RHRec.STU_SSN, RHRec.STU_DM_PRS_1, RHRec.STU_DM_PRS_MID, RHRec.STU_DM_PRS_LST, RHRec.STU_DD_BRT, RHRec.STU_DX_STR_ADR_1, RHRec.STU_DX_STR_ADR_2, RHRec.STU_DM_CT, RHRec.STU_DC_DOM_ST, RHRec.STU_DF_ZIP, RHRec.STU_DM_FGN_CNY, RHRec.STU_DI_VLD_ADR, RHRec.STU_DN_PHN, RHRec.STU_DI_PHN_VLD, RHRec.STU_DN_ALT_PHN, RHRec.STU_DI_ALT_PHN_VLD, _
        RHRec.EDSR_SSN, RHRec.EDSR_DM_PRS_1, RHRec.EDSR_DM_PRS_MID, RHRec.EDSR_DM_PRS_LST, RHRec.EDSR_DD_BRT, RHRec.EDSR_DX_STR_ADR_1, RHRec.EDSR_DX_STR_ADR_2, RHRec.EDSR_DM_CT, RHRec.EDSR_DC_DOM_ST, RHRec.EDSR_DF_ZIP, RHRec.EDSR_DM_FGN_CNY, RHRec.EDSR_DI_VLD_ADR, RHRec.EDSR_DN_PHN, RHRec.EDSR_DI_PHN_VLD, RHRec.EDSR_DN_ALT_PHN, RHRec.EDSR_DI_ALT_PHN_VLD, RHRec.AC_EDS_TYP, _
        RHRec.REF_IND, RHRec.BM_RFR_1_1, RHRec.BM_RFR_MID_1, RHRec.BM_RFR_LST_1, RHRec.BX_RFR_STR_ADR_1_1, RHRec.BX_RFR_STR_ADR_2_1, RHRec.BM_RFR_CT_1, RHRec.BC_RFR_ST_1, RHRec.BF_RFR_ZIP_1, RHRec.BM_RFR_FGN_CNY_1, RHRec.BI_VLD_ADR_1, RHRec.BN_RFR_DOM_PHN_1, RHRec.BI_DOM_PHN_VLD_1, RHRec.BN_RFR_ALT_PHN_1, RHRec.BI_ALT_PHN_VLD_1, RHRec.BC_RFR_REL_BR_1, _
        RHRec.BM_RFR_1_2, RHRec.BM_RFR_MID_2, RHRec.BM_RFR_LST_2, RHRec.BX_RFR_STR_ADR_1_2, RHRec.BX_RFR_STR_ADR_2_2, RHRec.BM_RFR_CT_2, RHRec.BC_RFR_ST_2, RHRec.BF_RFR_ZIP_2, RHRec.BM_RFR_FGN_CNY_2, RHRec.BI_VLD_ADR_2, RHRec.BN_RFR_DOM_PHN_2, RHRec.BI_DOM_PHN_VLD_2, RHRec.BN_RFR_ALT_PHN_2, RHRec.BI_ALT_PHN_VLD_2, RHRec.BC_RFR_REL_BR_2, _
        RHRec.BondID, RHRec.AVE_REHB_PAY_AMT, RHRec.BAT_ID, RHRec.BAT_BR_CT, RHRec.BAT_LN_CT, RHRec.BAT_TOT_SUM, RHRec.BR_ELIG_IND, RHRec.LD_IBR_25Y_FGV_BEG, RHRec.LD_IBR_RPD_SR, RHRec.LA_IBR_STD_STD_PAY, RHRec.LN_IBR_QLF_FGV_MTH, RHRec.ORG_INT_RATE, RHRec.BA_STD_PAY, RHRec.LA_PAY_XPC, RHRec.BL_LA_PAY_XPC, RHRec.PRI_COST
        LineCount = LineCount + 1
        If LineCount = CInt(LoansInBatch) Then
            Exit Do
        End If
    Loop
    'create new borrower or loan for current borrower as needed
        If LastSSNProcessed = RHRec.BF_SSN Then
            'add loan to existing borrower
            LoadRehabDat Borrs(UBound(Borrs) - 1), RHRec, False
        Else
            'create new borrower
            LoadRehabDat Borrs(UBound(Borrs)), RHRec, True
            'init borrower and loan level info arrays
            ReDim Preserve Borrs(UBound(Borrs) + 1)
            ReDim Borrs(UBound(Borrs)).loans(0)
        End If
End Function

'this sub blanks out the current borrower/loan data and loads new borrower/loan data
Private Function RPObjectsLoaded(FTIP As FileTypeInProcess, Rec As RepurchaseRec) As String
    Dim LastSSNProcessed As String
    Dim Batch As String
    Dim TrackingTemp As String
    Dim LoansInBatch As String
    
    'blank current borrower data and init loan level info arrays
    ReDim Borrs(0)
    ReDim Borrs(UBound(Borrs)).loans(0)
    If Rec.BF_SSN = "" Then
        'get priming read
        Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.Bal, Rec.DT_REPUR, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, _
        Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, _
        Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, _
        Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, _
        Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, _
        Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, _
        Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, _
        Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, _
        Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, Rec.BondID, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, _
        Rec.LC_FOR_TYP1, Rec.LC_FOR_TYP2, Rec.LC_FOR_TYP3, Rec.LC_FOR_TYP4, Rec.LC_FOR_TYP5, Rec.LC_FOR_TYP6, Rec.LC_FOR_TYP7, Rec.LC_FOR_TYP8, Rec.LC_FOR_TYP9, Rec.LC_FOR_TYP10, Rec.LC_FOR_TYP11, Rec.LC_FOR_TYP12, Rec.LC_FOR_TYP13, Rec.LC_FOR_TYP14, Rec.LC_FOR_TYP15, Rec.LD_FOR_BEG1, Rec.LD_FOR_BEG2, Rec.LD_FOR_BEG3, Rec.LD_FOR_BEG4, Rec.LD_FOR_BEG5, Rec.LD_FOR_BEG6, Rec.LD_FOR_BEG7, Rec.LD_FOR_BEG8, Rec.LD_FOR_BEG9, Rec.LD_FOR_BEG10, Rec.LD_FOR_BEG11, Rec.LD_FOR_BEG12, Rec.LD_FOR_BEG13, Rec.LD_FOR_BEG14, Rec.LD_FOR_BEG15, _
        Rec.LD_FOR_END1, Rec.LD_FOR_END2, Rec.LD_FOR_END3, Rec.LD_FOR_END4, Rec.LD_FOR_END5, Rec.LD_FOR_END6, Rec.LD_FOR_END7, Rec.LD_FOR_END8, Rec.LD_FOR_END9, Rec.LD_FOR_END10, Rec.LD_FOR_END11, Rec.LD_FOR_END12, Rec.LD_FOR_END13, Rec.LD_FOR_END14, Rec.LD_FOR_END15, Rec.LI_CAP_FOR_INT_REQ1, Rec.LI_CAP_FOR_INT_REQ2, Rec.LI_CAP_FOR_INT_REQ3, Rec.LI_CAP_FOR_INT_REQ4, Rec.LI_CAP_FOR_INT_REQ5, Rec.LI_CAP_FOR_INT_REQ6, Rec.LI_CAP_FOR_INT_REQ7, Rec.LI_CAP_FOR_INT_REQ8, Rec.LI_CAP_FOR_INT_REQ9, Rec.LI_CAP_FOR_INT_REQ10, Rec.LI_CAP_FOR_INT_REQ11, Rec.LI_CAP_FOR_INT_REQ12, Rec.LI_CAP_FOR_INT_REQ13, Rec.LI_CAP_FOR_INT_REQ14, Rec.LI_CAP_FOR_INT_REQ15, Rec.OL_FRB_BEG1, Rec.OL_FRB_BEG2, Rec.OL_FRB_BEG3, Rec.OL_FRB_BEG4, Rec.OL_FRB_BEG5, Rec.OL_FRB_BEG6, Rec.OL_FRB_BEG7, Rec.OL_FRB_BEG8, Rec.OL_FRB_BEG9, Rec.OL_FRB_BEG10, Rec.OL_FRB_BEG11, Rec.OL_FRB_BEG12, Rec.OL_FRB_BEG13, _
        Rec.OL_FRB_BEG14, Rec.OL_FRB_BEG15, Rec.OL_FRB_END1, Rec.OL_FRB_END2, Rec.OL_FRB_END3, Rec.OL_FRB_END4, Rec.OL_FRB_END5, Rec.OL_FRB_END6, Rec.OL_FRB_END7, Rec.OL_FRB_END8, Rec.OL_FRB_END9, Rec.OL_FRB_END10, Rec.OL_FRB_END11, Rec.OL_FRB_END12, Rec.OL_FRB_END13, Rec.OL_FRB_END14, Rec.OL_FRB_END15, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.LA_PRI, Rec.LA_INT
    End If
    'take note of batch ID and only process the current batch ID
    If Batch = "" Then Batch = Rec.BAT_ID
    'get associated system batch ID
    Open "T:\AACMasterTracking.txt" For Input As #2
    '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
    While Not EOF(2)
        Line Input #2, TrackingTemp
        If Replace(Split(TrackingTemp, ",")(0), """", "") = Batch Then
            RPObjectsLoaded = Replace(Split(TrackingTemp, ",")(4), """", "")
            LoansInBatch = Replace(Split(TrackingTemp, ",")(2), """", "")
        End If
    Wend
    Close #2
    Dim LineCount As Integer
    LineCount = 1
    'load new borrower data (batches of 5)
    Do While Not EOF(1)
        'check if batch is the same if not then exit function with data collected
        If Batch <> Rec.BAT_ID Then
            Exit Function
        End If
        'create new borrower or loan for current borrower as needed
        If LastSSNProcessed = Rec.BF_SSN Then
            'add loan to existing borrower
            LoadPurchDat Borrs(UBound(Borrs) - 1), Rec, False
        Else
            'create new borrower
            LastSSNProcessed = Rec.BF_SSN
            LoadPurchDat Borrs(UBound(Borrs)), Rec, True
            'init borrower and loan level info arrays
            ReDim Preserve Borrs(UBound(Borrs) + 1)
            ReDim Borrs(UBound(Borrs)).loans(0)
        End If
        'init SSN flag for switching between SSNs
        If LastSSNProcessed = "" Then LastSSNProcessed = Rec.BF_SSN
        Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.Bal, Rec.DT_REPUR, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, _
        Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, _
        Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, _
        Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, _
        Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, _
        Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, _
        Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, _
        Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, _
        Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, Rec.BondID, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, _
        Rec.LC_FOR_TYP1, Rec.LC_FOR_TYP2, Rec.LC_FOR_TYP3, Rec.LC_FOR_TYP4, Rec.LC_FOR_TYP5, Rec.LC_FOR_TYP6, Rec.LC_FOR_TYP7, Rec.LC_FOR_TYP8, Rec.LC_FOR_TYP9, Rec.LC_FOR_TYP10, Rec.LC_FOR_TYP11, Rec.LC_FOR_TYP12, Rec.LC_FOR_TYP13, Rec.LC_FOR_TYP14, Rec.LC_FOR_TYP15, Rec.LD_FOR_BEG1, Rec.LD_FOR_BEG2, Rec.LD_FOR_BEG3, Rec.LD_FOR_BEG4, Rec.LD_FOR_BEG5, Rec.LD_FOR_BEG6, Rec.LD_FOR_BEG7, Rec.LD_FOR_BEG8, Rec.LD_FOR_BEG9, Rec.LD_FOR_BEG10, Rec.LD_FOR_BEG11, Rec.LD_FOR_BEG12, Rec.LD_FOR_BEG13, Rec.LD_FOR_BEG14, Rec.LD_FOR_BEG15, _
        Rec.LD_FOR_END1, Rec.LD_FOR_END2, Rec.LD_FOR_END3, Rec.LD_FOR_END4, Rec.LD_FOR_END5, Rec.LD_FOR_END6, Rec.LD_FOR_END7, Rec.LD_FOR_END8, Rec.LD_FOR_END9, Rec.LD_FOR_END10, Rec.LD_FOR_END11, Rec.LD_FOR_END12, Rec.LD_FOR_END13, Rec.LD_FOR_END14, Rec.LD_FOR_END15, Rec.LI_CAP_FOR_INT_REQ1, Rec.LI_CAP_FOR_INT_REQ2, Rec.LI_CAP_FOR_INT_REQ3, Rec.LI_CAP_FOR_INT_REQ4, Rec.LI_CAP_FOR_INT_REQ5, Rec.LI_CAP_FOR_INT_REQ6, Rec.LI_CAP_FOR_INT_REQ7, Rec.LI_CAP_FOR_INT_REQ8, Rec.LI_CAP_FOR_INT_REQ9, Rec.LI_CAP_FOR_INT_REQ10, Rec.LI_CAP_FOR_INT_REQ11, Rec.LI_CAP_FOR_INT_REQ12, Rec.LI_CAP_FOR_INT_REQ13, Rec.LI_CAP_FOR_INT_REQ14, Rec.LI_CAP_FOR_INT_REQ15, Rec.OL_FRB_BEG1, Rec.OL_FRB_BEG2, Rec.OL_FRB_BEG3, Rec.OL_FRB_BEG4, Rec.OL_FRB_BEG5, Rec.OL_FRB_BEG6, Rec.OL_FRB_BEG7, Rec.OL_FRB_BEG8, Rec.OL_FRB_BEG9, Rec.OL_FRB_BEG10, Rec.OL_FRB_BEG11, Rec.OL_FRB_BEG12, Rec.OL_FRB_BEG13, _
        Rec.OL_FRB_BEG14, Rec.OL_FRB_BEG15, Rec.OL_FRB_END1, Rec.OL_FRB_END2, Rec.OL_FRB_END3, Rec.OL_FRB_END4, Rec.OL_FRB_END5, Rec.OL_FRB_END6, Rec.OL_FRB_END7, Rec.OL_FRB_END8, Rec.OL_FRB_END9, Rec.OL_FRB_END10, Rec.OL_FRB_END11, Rec.OL_FRB_END12, Rec.OL_FRB_END13, Rec.OL_FRB_END14, Rec.OL_FRB_END15, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.LA_PRI, Rec.LA_INT
        LineCount = LineCount + 1
        If LineCount = CInt(LoansInBatch) Then
            Exit Do
        End If
    Loop
    'create new borrower or loan for current borrower as needed
    If LastSSNProcessed = Rec.BF_SSN Then
        'add loan to existing borrower
        LoadPurchDat Borrs(UBound(Borrs) - 1), Rec, False
    Else
        'create new borrower
        LoadPurchDat Borrs(UBound(Borrs)), Rec, True
        'init borrower and loan level info arrays
        ReDim Preserve Borrs(UBound(Borrs) + 1)
        ReDim Borrs(UBound(Borrs)).loans(0)
    End If
End Function

'this sub uses the master tracking file to create system batches
Private Sub CreateAACBatches(FTIP As FileTypeInProcess)
    Dim TrackingTemp() As String
    Dim i As Integer
    Dim CRBRptingDt As String
    Dim PreviousMonth As Integer
    
    'calculate what the last day of the next month is
    CRBRptingDt = CStr(DateAdd("m", 2, Date))
    PreviousMonth = Month(CDate(CRBRptingDt))
    While Month(CDate(CRBRptingDt)) = PreviousMonth
        CRBRptingDt = CStr(DateAdd("d", -1, CRBRptingDt))
    Wend
    ReDim TrackingTemp(11, 0)
    Open "T:\AACMasterTracking.txt" For Input As #1
    '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
    'read file into an array
    While Not EOF(1)

        If FTIP = tilp Then
            Input #1, TrackingTemp(0, UBound(TrackingTemp, 2)), TrackingTemp(1, UBound(TrackingTemp, 2)), TrackingTemp(2, UBound(TrackingTemp, 2)), TrackingTemp(3, UBound(TrackingTemp, 2)), TrackingTemp(4, UBound(TrackingTemp, 2)), TrackingTemp(5, UBound(TrackingTemp, 2)), TrackingTemp(6, UBound(TrackingTemp, 2)), TrackingTemp(7, UBound(TrackingTemp, 2)), TrackingTemp(8, UBound(TrackingTemp, 2)), TrackingTemp(9, UBound(TrackingTemp, 2)), TrackingTemp(10, UBound(TrackingTemp, 2)), TrackingTemp(11, UBound(TrackingTemp, 2))
        Else
            Input #1, TrackingTemp(0, UBound(TrackingTemp, 2)), TrackingTemp(1, UBound(TrackingTemp, 2)), TrackingTemp(2, UBound(TrackingTemp, 2)), TrackingTemp(3, UBound(TrackingTemp, 2)), TrackingTemp(4, UBound(TrackingTemp, 2)), TrackingTemp(5, UBound(TrackingTemp, 2)), TrackingTemp(6, UBound(TrackingTemp, 2)), TrackingTemp(7, UBound(TrackingTemp, 2)), TrackingTemp(8, UBound(TrackingTemp, 2)), TrackingTemp(9, UBound(TrackingTemp, 2)), TrackingTemp(10, UBound(TrackingTemp, 2))
        End If
        ReDim Preserve TrackingTemp(11, UBound(TrackingTemp, 2) + 1)
    Wend
    Close #1
    'create batches for all files
    While i < UBound(TrackingTemp, 2)
        FastPath "TX3ZATA0I;"
        puttext 5, 10, BatchInfo.Owner
        puttext 5, 64, Format(CDate(BatchInfo.EffLoanAddDt), "MMDDYY") 'effective date
        puttext 6, 11, "TERI VIG" 'contact
        puttext 6, 64, Format(Date, "MMDDYY") ' Actl Loan Add Date
        puttext 7, 19, BatchInfo.ConversionType 'conversion
        puttext 7, 64, Format(DateAdd("d", -1, CDate(BatchInfo.EffLoanAddDt)), "MMDDYY") 'owner cut-off date
        If BatchInfo.SubIntLastCollection <> "" Then puttext 8, 26, Format(CDate(BatchInfo.SubIntLastCollection), "MMDDYY") 'sub Int Last collected
        puttext 9, 74, BatchInfo.OriginationFee 'origination fee
        puttext 10, 62, "S" 'Cnv Packaging Rspb
        puttext 11, 22, "SVT" 'Prom Note location
        puttext 17, 24, "N" 'Conversion Ltr Print
        If BatchInfo.Guarantor <> "" Then puttext 20, 36, BatchInfo.Guarantor 'guarantor
        If BatchInfo.Program <> "" Then puttext 20, 54, BatchInfo.Program 'program
        puttext 20, 71, BatchInfo.Lender 'lender
        puttext 21, 14, BatchInfo.PrevOwner
        If BatchInfo.RepurchaseType <> "" Then puttext 21, 42, BatchInfo.RepurchaseType 'repurchase type
        puttext 10, 28, Format(Now, "MMDDYY") 'borrower file delivery date
        puttext 16, 23, Format(CDate(CRBRptingDt), "MMDDYY") 'CRB Reporting
        Hit "enter"
        'get batch ID
        TrackingTemp(4, i) = GetText(3, 23, 10) 'example "2007012001"
        '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
        FastPath "TX3ZCTA0J" & TrackingTemp(4, i)
        puttext 6, 10, "AD" 'DE Que
        puttext 6, 27, "A1" 'DE Sub Que
        puttext 6, 45, "AQ" 'QA Que
        puttext 6, 63, "A1" 'QA Sub Que
        puttext 7, 62, "S" 'Conv Sub Type
        puttext 12, 8, TrackingTemp(1, i) 'exp # bor
        puttext 12, 20, TrackingTemp(2, i) 'exp # loans
        puttext 14, 70, "1" 'Mnr Bch Xpc
        puttext 18, 6, TrackingTemp(3, i) 'exp cur prin
        puttext 18, 44, TrackingTemp(10, i) 'interest
        puttext 18, 61, TrackingTemp(11, i) 'fees
        Hit "Enter"
        '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
        FastPath "TX3ZATA0L" & TrackingTemp(4, i) & ";00001"
        puttext 12, 6, TrackingTemp(1, i) 'exp # bor
        puttext 12, 19, TrackingTemp(2, i) 'exp # loans
        puttext 18, 6, TrackingTemp(3, i) 'exp cur prin
        puttext 18, 45, TrackingTemp(10, i) 'interest
        puttext 18, 64, TrackingTemp(11, i) 'fees
        Hit "Enter"
        FastPath "TX3ZATA0M" & TrackingTemp(4, i) & ";00001"
        '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
        If TrackingTemp(5, i) <> "" Then puttext 10, 8, TrackingTemp(5, i)
        If TrackingTemp(6, i) <> "" Then puttext 11, 8, TrackingTemp(6, i)
        If TrackingTemp(7, i) <> "" Then puttext 12, 8, TrackingTemp(7, i)
        If TrackingTemp(8, i) <> "" Then puttext 13, 8, TrackingTemp(8, i)
        If TrackingTemp(9, i) <> "" Then puttext 14, 8, TrackingTemp(9, i)
        Hit "enter"
        i = i + 1
    Wend
    i = 0
    'once all batches are created then write data back out to MasterTracking file and write them out to MajorBatchIDs file
    Open "T:\AACMasterTracking.txt" For Output As #1
    Open FTPDir & "MajorBatchIDs.txt" For Append As #2
    While i < UBound(TrackingTemp, 2)
        '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
        If TrackingTemp(5, i) <> "" Then puttext 10, 8, TrackingTemp(5, i)
        If FTIP = tilp Then
            Write #1, TrackingTemp(0, i), TrackingTemp(1, i), TrackingTemp(2, i), TrackingTemp(3, i), TrackingTemp(4, i), TrackingTemp(5, i), TrackingTemp(6, i), TrackingTemp(7, i), TrackingTemp(8, i), TrackingTemp(9, i), TrackingTemp(10, i), TrackingTemp(11, i)
        Else
            Write #1, TrackingTemp(0, i), TrackingTemp(1, i), TrackingTemp(2, i), TrackingTemp(3, i), TrackingTemp(4, i), TrackingTemp(5, i), TrackingTemp(6, i), TrackingTemp(7, i), TrackingTemp(8, i), TrackingTemp(9, i), TrackingTemp(10, i)
        End If
        Write #2, TrackingTemp(4, i)
        i = i + 1
    Wend
    Close #1
    Close #2
End Sub

'this function gets the effective date and Bond ID from a rehab file
Private Function GetRehabDat(FIP As String, EffDt As String, BondID As String) As String
    Dim Rec As RehabRec
    Open FTPDir & FIP For Input As #1
    'header row
    Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.LD_TRX_EFF, Rec.LA_TRX, Rec.LD_RHB, Rec.LA_PRI, Rec.LA_INT, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, _
    Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, _
    Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, _
    Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, _
    Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, _
    Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, _
    Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, _
    Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, _
    Rec.BondID, Rec.AVE_REHB_PAY_AMT, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.BA_STD_PAY, Rec.LA_PAY_XPC, Rec.BL_LA_PAY_XPC, Rec.PRI_COST
    'first data row
    Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, EffDt, Rec.LA_TRX, Rec.LD_RHB, Rec.LA_PRI, Rec.LA_INT, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, _
    Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, _
    Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, _
    Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, _
    Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, _
    Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, _
    Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, _
    Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, _
    BondID, Rec.AVE_REHB_PAY_AMT, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.BA_STD_PAY, Rec.LA_PAY_XPC, Rec.BL_LA_PAY_XPC, Rec.PRI_COST
    Close #1
End Function

'this function gets the effective date and Bond ID from a repurchase file
Private Function GetRepurchDat(FIP As String, EffDt As String, BondID As String) As String
    Dim Rec As RepurchaseRec
    Open FTPDir & FIP For Input As #1
    'header row
    Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.Bal, Rec.DT_REPUR, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, _
    Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, _
    Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, _
    Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, _
    Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, _
    Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, _
    Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, _
    Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, _
    Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, Rec.BondID, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, _
    Rec.LC_FOR_TYP1, Rec.LC_FOR_TYP2, Rec.LC_FOR_TYP3, Rec.LC_FOR_TYP4, Rec.LC_FOR_TYP5, Rec.LC_FOR_TYP6, Rec.LC_FOR_TYP7, Rec.LC_FOR_TYP8, Rec.LC_FOR_TYP9, Rec.LC_FOR_TYP10, Rec.LC_FOR_TYP11, Rec.LC_FOR_TYP12, Rec.LC_FOR_TYP13, Rec.LC_FOR_TYP14, Rec.LC_FOR_TYP15, Rec.LD_FOR_BEG1, Rec.LD_FOR_BEG2, Rec.LD_FOR_BEG3, Rec.LD_FOR_BEG4, Rec.LD_FOR_BEG5, Rec.LD_FOR_BEG6, Rec.LD_FOR_BEG7, Rec.LD_FOR_BEG8, Rec.LD_FOR_BEG9, Rec.LD_FOR_BEG10, Rec.LD_FOR_BEG11, Rec.LD_FOR_BEG12, Rec.LD_FOR_BEG13, Rec.LD_FOR_BEG14, Rec.LD_FOR_BEG15, _
    Rec.LD_FOR_END1, Rec.LD_FOR_END2, Rec.LD_FOR_END3, Rec.LD_FOR_END4, Rec.LD_FOR_END5, Rec.LD_FOR_END6, Rec.LD_FOR_END7, Rec.LD_FOR_END8, Rec.LD_FOR_END9, Rec.LD_FOR_END10, Rec.LD_FOR_END11, Rec.LD_FOR_END12, Rec.LD_FOR_END13, Rec.LD_FOR_END14, Rec.LD_FOR_END15, Rec.LI_CAP_FOR_INT_REQ1, Rec.LI_CAP_FOR_INT_REQ2, Rec.LI_CAP_FOR_INT_REQ3, Rec.LI_CAP_FOR_INT_REQ4, Rec.LI_CAP_FOR_INT_REQ5, Rec.LI_CAP_FOR_INT_REQ6, Rec.LI_CAP_FOR_INT_REQ7, Rec.LI_CAP_FOR_INT_REQ8, Rec.LI_CAP_FOR_INT_REQ9, Rec.LI_CAP_FOR_INT_REQ10, Rec.LI_CAP_FOR_INT_REQ11, Rec.LI_CAP_FOR_INT_REQ12, Rec.LI_CAP_FOR_INT_REQ13, Rec.LI_CAP_FOR_INT_REQ14, Rec.LI_CAP_FOR_INT_REQ15, Rec.OL_FRB_BEG1, Rec.OL_FRB_BEG2, Rec.OL_FRB_BEG3, Rec.OL_FRB_BEG4, Rec.OL_FRB_BEG5, Rec.OL_FRB_BEG6, Rec.OL_FRB_BEG7, Rec.OL_FRB_BEG8, Rec.OL_FRB_BEG9, Rec.OL_FRB_BEG10, Rec.OL_FRB_BEG11, Rec.OL_FRB_BEG12, Rec.OL_FRB_BEG13, _
    Rec.OL_FRB_BEG14, Rec.OL_FRB_BEG15, Rec.OL_FRB_END1, Rec.OL_FRB_END2, Rec.OL_FRB_END3, Rec.OL_FRB_END4, Rec.OL_FRB_END5, Rec.OL_FRB_END6, Rec.OL_FRB_END7, Rec.OL_FRB_END8, Rec.OL_FRB_END9, Rec.OL_FRB_END10, Rec.OL_FRB_END11, Rec.OL_FRB_END12, Rec.OL_FRB_END13, Rec.OL_FRB_END14, Rec.OL_FRB_END15, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.LA_PRI, Rec.LA_INT
    'get data
    Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.Bal, EffDt, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, _
    Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, _
    Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, _
    Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, _
    Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, _
    Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, _
    Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, _
    Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, _
    Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, BondID, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, _
    Rec.LC_FOR_TYP1, Rec.LC_FOR_TYP2, Rec.LC_FOR_TYP3, Rec.LC_FOR_TYP4, Rec.LC_FOR_TYP5, Rec.LC_FOR_TYP6, Rec.LC_FOR_TYP7, Rec.LC_FOR_TYP8, Rec.LC_FOR_TYP9, Rec.LC_FOR_TYP10, Rec.LC_FOR_TYP11, Rec.LC_FOR_TYP12, Rec.LC_FOR_TYP13, Rec.LC_FOR_TYP14, Rec.LC_FOR_TYP15, Rec.LD_FOR_BEG1, Rec.LD_FOR_BEG2, Rec.LD_FOR_BEG3, Rec.LD_FOR_BEG4, Rec.LD_FOR_BEG5, Rec.LD_FOR_BEG6, Rec.LD_FOR_BEG7, Rec.LD_FOR_BEG8, Rec.LD_FOR_BEG9, Rec.LD_FOR_BEG10, Rec.LD_FOR_BEG11, Rec.LD_FOR_BEG12, Rec.LD_FOR_BEG13, Rec.LD_FOR_BEG14, Rec.LD_FOR_BEG15, _
    Rec.LD_FOR_END1, Rec.LD_FOR_END2, Rec.LD_FOR_END3, Rec.LD_FOR_END4, Rec.LD_FOR_END5, Rec.LD_FOR_END6, Rec.LD_FOR_END7, Rec.LD_FOR_END8, Rec.LD_FOR_END9, Rec.LD_FOR_END10, Rec.LD_FOR_END11, Rec.LD_FOR_END12, Rec.LD_FOR_END13, Rec.LD_FOR_END14, Rec.LD_FOR_END15, Rec.LI_CAP_FOR_INT_REQ1, Rec.LI_CAP_FOR_INT_REQ2, Rec.LI_CAP_FOR_INT_REQ3, Rec.LI_CAP_FOR_INT_REQ4, Rec.LI_CAP_FOR_INT_REQ5, Rec.LI_CAP_FOR_INT_REQ6, Rec.LI_CAP_FOR_INT_REQ7, Rec.LI_CAP_FOR_INT_REQ8, Rec.LI_CAP_FOR_INT_REQ9, Rec.LI_CAP_FOR_INT_REQ10, Rec.LI_CAP_FOR_INT_REQ11, Rec.LI_CAP_FOR_INT_REQ12, Rec.LI_CAP_FOR_INT_REQ13, Rec.LI_CAP_FOR_INT_REQ14, Rec.LI_CAP_FOR_INT_REQ15, Rec.OL_FRB_BEG1, Rec.OL_FRB_BEG2, Rec.OL_FRB_BEG3, Rec.OL_FRB_BEG4, Rec.OL_FRB_BEG5, Rec.OL_FRB_BEG6, Rec.OL_FRB_BEG7, Rec.OL_FRB_BEG8, Rec.OL_FRB_BEG9, Rec.OL_FRB_BEG10, Rec.OL_FRB_BEG11, Rec.OL_FRB_BEG12, Rec.OL_FRB_BEG13, _
    Rec.OL_FRB_BEG14, Rec.OL_FRB_BEG15, Rec.OL_FRB_END1, Rec.OL_FRB_END2, Rec.OL_FRB_END3, Rec.OL_FRB_END4, Rec.OL_FRB_END5, Rec.OL_FRB_END6, Rec.OL_FRB_END7, Rec.OL_FRB_END8, Rec.OL_FRB_END9, Rec.OL_FRB_END10, Rec.OL_FRB_END11, Rec.OL_FRB_END12, Rec.OL_FRB_END13, Rec.OL_FRB_END14, Rec.OL_FRB_END15, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.LA_PRI, Rec.LA_INT
    Close #1
End Function

'this formulates the master tracking file which is used throught the rest ot the processing
Private Sub CreateMasterTrackingFile(FileInProc As String)
    Dim Rec As String
    Dim Flds() As String
    Dim SSNs As String
    Dim LastBatchNum As String
    Dim LastBorrCount As String
    Dim LastLoanCount As String
    Dim LastSumTotal As String
    Dim LastIntTotal As String
    Dim LastFeeTotal As String
    Dim FillInCommasCounter As Integer
    Dim offSet As Integer
    Dim BatchPrincipal As Double
    Dim BatchInterest As Double
    
    'master file layout 0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
    Open "T:\AACMasterTracking.txt" For Output As #1
    Open FTPDir & FileInProc For Input As #2
    Line Input #2, Rec 'get header row
    If FileTypeInProc = rehab Then
        offSet = 10
    ElseIf FileTypeInProc = Repurch Then
        offSet = 98
    ElseIf FileTypeInProc = tilp Then
        offSet = 2
    End If
    While Not EOF(2)
        'get record from SAS file
        Line Input #2, Rec
        Flds = Split(Rec, ",")
        If LastBatchNum = "" Then
            'init vars if first time through loop
            SSNs = Flds(0) 'get SSN
            LastBatchNum = Flds(UBound(Flds) - (3 + offSet)) 'get SAS batch ID
            LastBorrCount = Flds(UBound(Flds) - (2 + offSet)) 'get borrower count for batch
            LastLoanCount = Flds(UBound(Flds) - (1 + offSet)) 'get loan count for batch
            If FileTypeInProc = rehab Then
                LastSumTotal = Flds(UBound(Flds)) 'get Exp Curr Prin
                LastIntTotal = Flds(UBound(Flds) - 270)
            End If
            If FileTypeInProc = Repurch Then
                LastSumTotal = Flds(UBound(Flds) - 1) 'get Exp Curr Prin
                LastIntTotal = Flds(UBound(Flds))
            End If
            If FileTypeInProc = tilp Then
                LastSumTotal = Flds(UBound(Flds) - (offSet))
                LastIntTotal = Flds(UBound(Flds) - (offSet - 1))  'get interest total
                LastFeeTotal = Flds(UBound(Flds) - (offSet - 2))  'get fee total
            End If
        Else
            'if the script has already looped then check if still on same SAS batch num
            If LastBatchNum = Flds(UBound(Flds) - (3 + offSet)) Then
                If InStr(1, SSNs, Flds(0)) = False Then SSNs = SSNs & "," & Flds(0) 'get SSN
            If FileTypeInProc = Repurch Then 'Continue adding for each record in the batch
                BatchPrincipal = CDbl(LastSumTotal) + Flds(UBound(Flds) - 1) 'get Exp Curr Prin
                LastSumTotal = BatchPrincipal
                BatchInterest = CDbl(LastIntTotal) + Flds(UBound(Flds))
                LastIntTotal = BatchInterest
            End If
            If FileTypeInProc = rehab Then
                BatchPrincipal = CDbl(LastSumTotal) + Flds(UBound(Flds)) 'get Exp Curr Prin
                LastSumTotal = BatchPrincipal
                BatchInterest = CDbl(LastIntTotal) + Flds(UBound(Flds) - 270)
                LastIntTotal = BatchInterest
            End If
            Else
                'if batch ids aren't the same then add info to master file and start collecting for the next batch
                If FileTypeInProc = tilp Then
                    Print #1, LastBatchNum & "," & LastBorrCount & "," & LastLoanCount & "," & LastSumTotal & ",," & SSNs & "," & LastIntTotal & "," & LastFeeTotal
                Else
                    Print #1, LastBatchNum & "," & LastBorrCount & "," & LastLoanCount & "," & LastSumTotal & ",," & SSNs & "," & LastIntTotal
                End If
                BatchPrincipal = 0
                SSNs = Flds(0) 'get SSN
                LastBatchNum = Flds(UBound(Flds) - (3 + offSet)) 'get SAS batch ID
                LastBorrCount = Flds(UBound(Flds) - (2 + offSet)) 'get borrower count for batch
                LastLoanCount = Flds(UBound(Flds) - (1 + offSet)) 'get loan count for batch
                If FileTypeInProc = rehab Then
                    LastSumTotal = Flds(UBound(Flds)) 'get Exp Curr Prin
                    LastIntTotal = Flds(UBound(Flds) - 270)
                End If
                If FileTypeInProc = Repurch Then
                    LastSumTotal = Flds(UBound(Flds) - 1) 'get Exp Curr Prin
                    LastIntTotal = Flds(UBound(Flds))
                End If
                If FileTypeInProc = tilp Then
                    LastSumTotal = Flds(UBound(Flds) - (offSet))
                    LastIntTotal = Flds(UBound(Flds) - (offSet - 1))  'get interest total
                    LastFeeTotal = Flds(UBound(Flds) - (offSet - 2))  'get fee total
                End If
            End If
        End If
    Wend
    'the last rec may not have all SSNs filled in; add commas for empty SSN flds
    FillInCommasCounter = CInt(LastBorrCount) - 1
    While FillInCommasCounter < 4
        SSNs = SSNs & ","
        FillInCommasCounter = FillInCommasCounter + 1
    Wend
    'print last record
    If FileTypeInProc = tilp Then
        Print #1, LastBatchNum & "," & LastBorrCount & "," & LastLoanCount & "," & LastSumTotal & ",," & SSNs & "," & LastIntTotal & "," & LastFeeTotal
    Else
        Print #1, LastBatchNum & "," & LastBorrCount & "," & LastLoanCount & "," & LastSumTotal & ",," & SSNs & "," & LastIntTotal
    End If
    Close #2
    Close #1
End Sub

'this sub creates the REHRPTSCHD in X:\PADD\FTP from the rehab file
Private Sub CreateREHRPTSCHDFile(FileInProc As String)
    Dim Rec As RehabRec
    Open FTPDir & FileInProc For Input As #1
    Open RepaymentFile For Output As #2
    'get header record
    Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.LD_TRX_EFF, Rec.LA_TRX, Rec.LD_RHB, Rec.LA_PRI, Rec.LA_INT, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, _
    Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, _
    Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, _
    Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, _
    Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, _
    Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, _
    Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, _
    Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, _
    Rec.BondID, Rec.AVE_REHB_PAY_AMT, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.BA_STD_PAY, Rec.LA_PAY_XPC, Rec.BL_LA_PAY_XPC, Rec.PRI_COST
    While Not EOF(1)
        Input #1, Rec.BF_SSN, Rec.DM_PRS_1, Rec.DM_PRS_MID, Rec.DM_PRS_LST, Rec.DD_BRT, Rec.DX_STR_ADR_1, Rec.DX_STR_ADR_2, Rec.DM_CT, Rec.DC_DOM_ST, Rec.DF_ZIP, Rec.DM_FGN_CNY, Rec.DI_VLD_ADR, Rec.DN_PHN, Rec.DI_PHN_VLD, Rec.DN_ALT_PHN, Rec.DI_ALT_PHN_VLD, Rec.AC_LON_TYP, Rec.SUBSIDY, Rec.AD_PRC, Rec.AF_ORG_APL_OPS_LDR, Rec.AF_APL_ID, Rec.AF_APL_ID_SFX, Rec.AD_IST_TRM_BEG, Rec.AD_IST_TRM_END, Rec.AA_GTE_LON_AMT, Rec.AF_APL_OPS_SCL, Rec.AD_BR_SIG, Rec.LD_LFT_SCL, Rec.PR_RPD_FOR_ITR, Rec.LC_INT_TYP, Rec.LD_TRX_EFF, Rec.LA_TRX, Rec.LD_RHB, Rec.LA_PRI, Rec.LA_INT, Rec.IF_OPS_SCL_RPT, Rec.LC_STU_ENR_TYP, Rec.LD_ENR_CER, Rec.LD_LDR_NTF, Rec.AR_CON_ITR, Rec.AD_APL_RCV, Rec.AC_STU_DFR_REQ, Rec.AN_DISB_1, Rec.AC_DISB_1, Rec.AD_DISB_1, Rec.AA_DISB_1, _
        Rec.ORG_1, Rec.CD_DISB_1, Rec.CA_DISB_1, Rec.GTE_1, Rec.AN_DISB_2, Rec.AC_DISB_2, Rec.AD_DISB_2, Rec.AA_DISB_2, Rec.ORG_2, Rec.CD_DISB_2, Rec.CA_DISB_2, Rec.GTE_2, Rec.AN_DISB_3, Rec.AC_DISB_3, Rec.AD_DISB_3, Rec.AA_DISB_3, Rec.ORG_3, Rec.CD_DISB_3, Rec.CA_DISB_3, Rec.GTE_3, Rec.AN_DISB_4, Rec.AC_DISB_4, Rec.AD_DISB_4, Rec.AA_DISB_4, Rec.ORG_4, Rec.CD_DISB_4, Rec.CA_DISB_4, Rec.GTE_4, Rec.AA_TOT_EDU_DET_PNT, Rec.LC_DFR_TYP1, Rec.LC_DFR_TYP2, Rec.LC_DFR_TYP3, Rec.LC_DFR_TYP4, Rec.LC_DFR_TYP5, Rec.LC_DFR_TYP6, Rec.LC_DFR_TYP7, Rec.LC_DFR_TYP8, Rec.LC_DFR_TYP9, Rec.LC_DFR_TYP10, Rec.LC_DFR_TYP11, Rec.LC_DFR_TYP12, Rec.LC_DFR_TYP13, Rec.LC_DFR_TYP14, Rec.LC_DFR_TYP15, Rec.LD_DFR_BEG1, Rec.LD_DFR_BEG2, Rec.LD_DFR_BEG3, Rec.LD_DFR_BEG4, Rec.LD_DFR_BEG5, _
        Rec.LD_DFR_BEG6, Rec.LD_DFR_BEG7, Rec.LD_DFR_BEG8, Rec.LD_DFR_BEG9, Rec.LD_DFR_BEG10, Rec.LD_DFR_BEG11, Rec.LD_DFR_BEG12, Rec.LD_DFR_BEG13, Rec.LD_DFR_BEG14, Rec.LD_DFR_BEG15, Rec.LD_DFR_END1, Rec.LD_DFR_END2, Rec.LD_DFR_END3, Rec.LD_DFR_END4, Rec.LD_DFR_END5, Rec.LD_DFR_END6, Rec.LD_DFR_END7, Rec.LD_DFR_END8, Rec.LD_DFR_END9, Rec.LD_DFR_END10, Rec.LD_DFR_END11, Rec.LD_DFR_END12, Rec.LD_DFR_END13, Rec.LD_DFR_END14, Rec.LD_DFR_END15, Rec.LF_DOE_SCL_DFR1, Rec.LF_DOE_SCL_DFR2, Rec.LF_DOE_SCL_DFR3, Rec.LF_DOE_SCL_DFR4, Rec.LF_DOE_SCL_DFR5, Rec.LF_DOE_SCL_DFR6, Rec.LF_DOE_SCL_DFR7, Rec.LF_DOE_SCL_DFR8, Rec.LF_DOE_SCL_DFR9, Rec.LF_DOE_SCL_DFR10, Rec.LF_DOE_SCL_DFR11, Rec.LF_DOE_SCL_DFR12, Rec.LF_DOE_SCL_DFR13, Rec.LF_DOE_SCL_DFR14, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER1, _
        Rec.LD_DFR_INF_CER2, Rec.LD_DFR_INF_CER3, Rec.LD_DFR_INF_CER4, Rec.LD_DFR_INF_CER5, Rec.LD_DFR_INF_CER6, Rec.LD_DFR_INF_CER7, Rec.LD_DFR_INF_CER8, Rec.LD_DFR_INF_CER9, Rec.LD_DFR_INF_CER10, Rec.LD_DFR_INF_CER11, Rec.LD_DFR_INF_CER12, Rec.LD_DFR_INF_CER13, Rec.LD_DFR_INF_CER14, Rec.LD_DFR_INF_CER15, Rec.AC_LON_STA_REA1, Rec.AC_LON_STA_REA2, Rec.AC_LON_STA_REA3, Rec.AC_LON_STA_REA4, Rec.AC_LON_STA_REA5, Rec.AC_LON_STA_REA6, Rec.AC_LON_STA_REA7, Rec.AC_LON_STA_REA8, Rec.AC_LON_STA_REA9, Rec.AC_LON_STA_REA10, Rec.AC_LON_STA_REA11, Rec.AC_LON_STA_REA12, Rec.AC_LON_STA_REA13, Rec.AC_LON_STA_REA14, _
        Rec.AC_LON_STA_REA15, Rec.AD_DFR_BEG1, Rec.AD_DFR_BEG2, Rec.AD_DFR_BEG3, Rec.AD_DFR_BEG4, Rec.AD_DFR_BEG5, Rec.AD_DFR_BEG6, Rec.AD_DFR_BEG7, Rec.AD_DFR_BEG8, Rec.AD_DFR_BEG9, Rec.AD_DFR_BEG10, Rec.AD_DFR_BEG11, Rec.AD_DFR_BEG12, Rec.AD_DFR_BEG13, Rec.AD_DFR_BEG14, Rec.AD_DFR_BEG15, Rec.AD_DFR_END1, Rec.AD_DFR_END2, Rec.AD_DFR_END3, Rec.AD_DFR_END4, Rec.AD_DFR_END5, Rec.AD_DFR_END6, Rec.AD_DFR_END7, Rec.AD_DFR_END8, Rec.AD_DFR_END9, Rec.AD_DFR_END10, Rec.AD_DFR_END11, Rec.AD_DFR_END12, Rec.AD_DFR_END13, Rec.AD_DFR_END14, Rec.AD_DFR_END15, Rec.IF_OPS_SCL_RPT1, Rec.IF_OPS_SCL_RPT2, Rec.IF_OPS_SCL_RPT3, _
        Rec.IF_OPS_SCL_RPT4, Rec.IF_OPS_SCL_RPT5, Rec.IF_OPS_SCL_RPT6, Rec.IF_OPS_SCL_RPT7, Rec.IF_OPS_SCL_RPT8, Rec.IF_OPS_SCL_RPT9, Rec.IF_OPS_SCL_RPT10, Rec.IF_OPS_SCL_RPT11, Rec.IF_OPS_SCL_RPT12, Rec.IF_OPS_SCL_RPT13, Rec.IF_OPS_SCL_RPT14, Rec.IF_OPS_SCL_RPT15, Rec.LD_ENR_CER1, Rec.LD_ENR_CER2, Rec.LD_ENR_CER3, Rec.LD_ENR_CER4, Rec.LD_ENR_CER5, Rec.LD_ENR_CER6, Rec.LD_ENR_CER7, Rec.LD_ENR_CER8, Rec.LD_ENR_CER9, Rec.LD_ENR_CER10, Rec.LD_ENR_CER11, Rec.LD_ENR_CER12, Rec.LD_ENR_CER13, Rec.LD_ENR_CER14, Rec.LD_ENR_CER15, Rec.STU_SSN, Rec.STU_DM_PRS_1, Rec.STU_DM_PRS_MID, Rec.STU_DM_PRS_LST, Rec.STU_DD_BRT, _
        Rec.STU_DX_STR_ADR_1, Rec.STU_DX_STR_ADR_2, Rec.STU_DM_CT, Rec.STU_DC_DOM_ST, Rec.STU_DF_ZIP, Rec.STU_DM_FGN_CNY, Rec.STU_DI_VLD_ADR, Rec.STU_DN_PHN, Rec.STU_DI_PHN_VLD, Rec.STU_DN_ALT_PHN, Rec.STU_DI_ALT_PHN_VLD, Rec.EDSR_SSN, Rec.EDSR_DM_PRS_1, Rec.EDSR_DM_PRS_MID, Rec.EDSR_DM_PRS_LST, Rec.EDSR_DD_BRT, Rec.EDSR_DX_STR_ADR_1, Rec.EDSR_DX_STR_ADR_2, Rec.EDSR_DM_CT, Rec.EDSR_DC_DOM_ST, Rec.EDSR_DF_ZIP, Rec.EDSR_DM_FGN_CNY, Rec.EDSR_DI_VLD_ADR, Rec.EDSR_DN_PHN, Rec.EDSR_DI_PHN_VLD, Rec.EDSR_DN_ALT_PHN, Rec.EDSR_DI_ALT_PHN_VLD, Rec.AC_EDS_TYP, Rec.REF_IND, Rec.BM_RFR_1_1, _
        Rec.BM_RFR_MID_1, Rec.BM_RFR_LST_1, Rec.BX_RFR_STR_ADR_1_1, Rec.BX_RFR_STR_ADR_2_1, Rec.BM_RFR_CT_1, Rec.BC_RFR_ST_1, Rec.BF_RFR_ZIP_1, Rec.BM_RFR_FGN_CNY_1, Rec.BI_VLD_ADR_1, Rec.BN_RFR_DOM_PHN_1, Rec.BI_DOM_PHN_VLD_1, Rec.BN_RFR_ALT_PHN_1, Rec.BI_ALT_PHN_VLD_1, Rec.BC_RFR_REL_BR_1, Rec.BM_RFR_1_2, Rec.BM_RFR_MID_2, Rec.BM_RFR_LST_2, Rec.BX_RFR_STR_ADR_1_2, Rec.BX_RFR_STR_ADR_2_2, Rec.BM_RFR_CT_2, Rec.BC_RFR_ST_2, Rec.BF_RFR_ZIP_2, Rec.BM_RFR_FGN_CNY_2, Rec.BI_VLD_ADR_2, Rec.BN_RFR_DOM_PHN_2, Rec.BI_DOM_PHN_VLD_2, Rec.BN_RFR_ALT_PHN_2, Rec.BI_ALT_PHN_VLD_2, Rec.BC_RFR_REL_BR_2, _
        Rec.BondID, Rec.AVE_REHB_PAY_AMT, Rec.BAT_ID, Rec.BAT_BR_CT, Rec.BAT_LN_CT, Rec.BAT_TOT_SUM, Rec.BR_ELIG_IND, Rec.LD_IBR_25Y_FGV_BEG, Rec.LD_IBR_RPD_SR, Rec.LA_IBR_STD_STD_PAY, Rec.LN_IBR_QLF_FGV_MTH, Rec.ORG_INT_RATE, Rec.BA_STD_PAY, Rec.LA_PAY_XPC, Rec.BL_LA_PAY_XPC, Rec.PRI_COST
        Write #2, Rec.BF_SSN, Rec.AF_APL_ID & Rec.AF_APL_ID_SFX, Rec.AVE_REHB_PAY_AMT, Rec.LA_TRX, Rec.AD_DISB_1
    Wend
    Close #1
    Close #2
End Sub

'this function figures out what the last day of the previous quarter was
Private Function LastDayOfPreviousQuarter() As String
    If Month(Date) = 1 Or Month(Date) = 2 Or Month(Date) = 3 Then
        LastDayOfPreviousQuarter = "12/31/" & (Year(Date) - 1)
    ElseIf Month(Date) = 4 Or Month(Date) = 5 Or Month(Date) = 6 Then
        LastDayOfPreviousQuarter = "3/31/" & Year(Date)
    ElseIf Month(Date) = 7 Or Month(Date) = 8 Or Month(Date) = 9 Then
        LastDayOfPreviousQuarter = "6/30/" & Year(Date)
    ElseIf Month(Date) = 10 Or Month(Date) = 11 Or Month(Date) = 12 Then
        LastDayOfPreviousQuarter = "9/30/" & Year(Date)
    End If
End Function

'this sub pre populates all vars for creating a batch
Private Sub SetUpBatchInfo(FTIP As FileTypeInProcess, FIP As String)
    Dim EffDt As String
    Dim BondID As String
    'get needed batch info from data file
    If FTIP = rehab Then
        GetRehabDat FIP, EffDt, BondID
    ElseIf FTIP = Repurch Then
        GetRepurchDat FIP, EffDt, BondID
    ElseIf FTIP = tilp Then
        GetTILPDat EffDt, FIP
    End If
    'owner
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.Owner = "828476"
    Else
        BatchInfo.Owner = "971357"
    End If
    'effective date
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.EffLoanAddDt = EffDt
    Else
        BatchInfo.EffLoanAddDt = Format(DateAdd("d", 1, CDate(EffDt)), "MM/DD/YYYY")
    End If
    'Conversion Type
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.ConversionType = "R"
    Else
        BatchInfo.ConversionType = "M"
    End If
    'Sub Int Last Collected
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.SubIntLastCollection = LastDayOfPreviousQuarter()
    Else
        BatchInfo.SubIntLastCollection = ""
    End If
    'Origination Fee
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.OriginationFee = "C"
    Else
        BatchInfo.OriginationFee = "N"
    End If
    'bond issue
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.BondIssue = BondID
    Else
        BatchInfo.BondIssue = ""
    End If
    'guarantor
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.Guarantor = "000749"
    Else
        BatchInfo.Guarantor = "I00059"
    End If
    'program
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.Program = ""
    Else
        BatchInfo.Program = "TILP"
    End If
    'Lender
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.Lender = "828476"
    Else
        BatchInfo.Lender = "971357"
    End If
    'Prev Owner
    If FTIP = rehab Or FTIP = Repurch Then
        BatchInfo.PrevOwner = "813894"
    Else
        BatchInfo.PrevOwner = ""
    End If
    'repurchase type
    If FTIP = rehab Then
        BatchInfo.RepurchaseType = "R"
    ElseIf FTIP = Repurch Then
        BatchInfo.RepurchaseType = "N"
    Else
        BatchInfo.RepurchaseType = ""
    End If
End Sub

Private Sub EntryIntoErrorFile(ssn As String, ErrorMsg As String, Queue As String, Optional MarkToSaveFile As Boolean = False)
    Dim StrTranslation As String
    If MarkToSaveFile Then
        StrTranslation = "True"
    Else
        StrTranslation = "False"
    End If
    ValidationErrFound = True
    Open "T:\AACErrorQueueTaskData.txt" For Append As #10
    Write #10, ssn, ErrorMsg, Queue, StrTranslation
    Close #10
End Sub

Private Sub EntryIntoQueueFile(ssn As String, Queue As String, Optional comment As String)
    Dim StrTranslation As String
    If MarkToSaveFile Then
        StrTranslation = "True"
    Else
        StrTranslation = "False"
    End If
    Open "T:\AACErrorQueueTaskData.txt" For Append As #10
    Write #10, ssn, comment, Queue, ""
    Close #10
End Sub

'this function loads data from the file to the processing objects
Private Sub LoadRehabDat(ByRef Borr As Borrower, Rec As RehabRec, LoadBorrLvlDat As Boolean)
    Dim UseCOMPASSDeferInfo As Boolean
    If LoadBorrLvlDat Then
        'borrower level data
        'borrower
        Borr.BAddr1 = Rec.DX_STR_ADR_1
        Borr.BAddr2 = Rec.DX_STR_ADR_2
        Borr.BAddrInd = Rec.DI_VLD_ADR
        Borr.BCity = Rec.DM_CT
        Borr.BDOB = Rec.DD_BRT
        Borr.BFirstName = Rec.DM_PRS_1
        Borr.BLastName = Rec.DM_PRS_LST
        Borr.BMID = Rec.DM_PRS_MID
        Borr.BPhone = Rec.DN_PHN
        Borr.BPhoneInd = Rec.DI_PHN_VLD
        Borr.BPhoneSource = "56"
        Borr.BSSN = Rec.BF_SSN
        Borr.BState = Rec.DC_DOM_ST
        Borr.BZip = Rec.DF_ZIP
        'ref1
        Borr.R1Addr1 = Rec.BX_RFR_STR_ADR_1_1
        Borr.R1Addr2 = Rec.BX_RFR_STR_ADR_2_1
        Borr.R1AddrInd = Rec.BI_VLD_ADR_1
        Borr.R1City = Rec.BM_RFR_CT_1
        Borr.R1FirstName = Rec.BM_RFR_1_1
        Borr.R1LastName = Rec.BM_RFR_LST_1
        Borr.R1MID = Rec.BM_RFR_MID_1
        Borr.R1Phone = Rec.BN_RFR_DOM_PHN_1
        Borr.R1PhoneInd = Rec.BI_DOM_PHN_VLD_1
        Borr.R1Relation = Rec.BC_RFR_REL_BR_1
        Borr.R1State = Rec.BC_RFR_ST_1
        Borr.R1Zip = Rec.BF_RFR_ZIP_1
        'ref2
        Borr.R2Addr1 = Rec.BX_RFR_STR_ADR_1_2
        Borr.R2Addr2 = Rec.BX_RFR_STR_ADR_2_2
        Borr.R2AddrInd = Rec.BI_VLD_ADR_2
        Borr.R2City = Rec.BM_RFR_CT_2
        Borr.R2FirstName = Rec.BM_RFR_1_2
        Borr.R2LastName = Rec.BM_RFR_LST_2
        Borr.R2MID = Rec.BM_RFR_MID_2
        Borr.R2Phone = Rec.BN_RFR_DOM_PHN_2
        Borr.R2PhoneInd = Rec.BI_DOM_PHN_VLD_2
        Borr.R2Relation = Rec.BC_RFR_REL_BR_2
        Borr.R2State = Rec.BC_RFR_ST_2
        Borr.R2Zip = Rec.BF_RFR_ZIP_2
        'IBR
        Borr.IBREligibleInd = Rec.BR_ELIG_IND
        If Rec.LD_IBR_25Y_FGV_BEG <> "" Then
            Borr.IBRForgivenessStartDate = MID(Rec.LD_IBR_25Y_FGV_BEG, 1, 3) & "01/" & MID(Rec.LD_IBR_25Y_FGV_BEG, 7, 4)
        End If
        If Rec.LD_IBR_RPD_SR <> "" Then
            Borr.IBRCreatDate = MID(Rec.LD_IBR_RPD_SR, 1, 3) & "01/" & MID(Rec.LD_IBR_RPD_SR, 7, 4)
        End If
        Borr.IBRStndStndPayAmount = Rec.LA_IBR_STD_STD_PAY
        Borr.IBRQualifyingPayments = Rec.LN_IBR_QLF_FGV_MTH
    End If
    'loan lvl data
    Borr.loans(UBound(Borr.loans)).BondID = Rec.BondID
    Borr.loans(UBound(Borr.loans)).Guarantor = "000749"
    Borr.loans(UBound(Borr.loans)).AC_LON_TYP = Rec.AC_LON_TYP
    'loan prg
    If Rec.AC_LON_TYP = "SF" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "STFFRD"
    ElseIf Rec.AC_LON_TYP = "SU" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "UNSTFD"
    ElseIf Rec.AC_LON_TYP = "PL" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "PLUS"
    ElseIf Rec.AC_LON_TYP = "SL" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "SLS"
    ElseIf Rec.AC_LON_TYP = "GB" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "PLUSGB"
    ElseIf Rec.AC_LON_TYP = "CL" And Rec.AD_PRC <= CDate("07/01/1997") Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "CNSLDN"
    ElseIf Rec.AC_LON_TYP = "CL" Then
        If Rec.SUBSIDY = "Y" And Rec.EDSR_SSN <> "" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "SUBSPC"
        ElseIf Rec.SUBSIDY = "N" And Rec.EDSR_SSN <> "" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "UNSPC"
        ElseIf Rec.SUBSIDY = "Y" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "SUBCNS"
        ElseIf Rec.SUBSIDY = "N" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "UNCNS"
        Else
            MsgBox "Unexpected Value For Loan Program."
            End
        End If
    Else
        MsgBox "Unexpected Value For Loan Program."
        End
    End If
    Borr.loans(UBound(Borr.loans)).OrigLender = Rec.AF_ORG_APL_OPS_LDR
    Borr.loans(UBound(Borr.loans)).RepType = "R"
    If CDate(Rec.AD_APL_RCV) > CDate(Rec.AD_DISB_1) Then
        Borr.loans(UBound(Borr.loans)).AppRcvdDt = Rec.AD_DISB_1
    Else
        Borr.loans(UBound(Borr.loans)).AppRcvdDt = Rec.AD_APL_RCV
    End If
    Borr.loans(UBound(Borr.loans)).DtNoteSigned = Rec.AD_BR_SIG
    Borr.loans(UBound(Borr.loans)).DtGuaranteed = Rec.AD_PRC
    Borr.loans(UBound(Borr.loans)).AmountGuaranteed = Rec.AA_GTE_LON_AMT
    Borr.loans(UBound(Borr.loans)).clid = Rec.AF_APL_ID & Rec.AF_APL_ID_SFX
    'if SLS loan
    If Rec.AC_LON_TYP = "SL" Then
        If Rec.AC_STU_DFR_REQ = "" Then
            Borr.loans(UBound(Borr.loans)).AC_STU_DFR_REQ = "N"
        Else
            Borr.loans(UBound(Borr.loans)).AC_STU_DFR_REQ = Rec.AC_STU_DFR_REQ
        End If
    End If
    'if not consolidation
    If Rec.AC_LON_TYP <> "CL" Then
        Borr.loans(UBound(Borr.loans)).TermBg = Rec.AD_IST_TRM_BEG
        Borr.loans(UBound(Borr.loans)).TermEd = Rec.AD_IST_TRM_END
        Borr.loans(UBound(Borr.loans)).OrigSchool = Rec.AF_APL_OPS_SCL
    End If
    'subsidy
    If Rec.AC_LON_TYP = "SF" Then
        Borr.loans(UBound(Borr.loans)).SubsidyCd = "Y"
    ElseIf Rec.AC_LON_TYP = "CL" And Rec.AD_PRC <= CDate("07/01/1997") Then
        Borr.loans(UBound(Borr.loans)).SubsidyCd = "N"
    ElseIf Rec.AC_LON_TYP = "CL" Then
        Borr.loans(UBound(Borr.loans)).SubsidyCd = Rec.SUBSIDY
    Else
        Borr.loans(UBound(Borr.loans)).SubsidyCd = "N"
    End If
    Borr.loans(UBound(Borr.loans)).SpallElig = "Y"
    'consolidation only
    If Rec.AC_LON_TYP = "CL" Then
        'prior to 07/01/93
        If CDate(Rec.AD_PRC) < CDate("07/01/93") Then
            Borr.loans(UBound(Borr.loans)).PriorTo7_93 = "Y"
        Else
            Borr.loans(UBound(Borr.loans)).PriorTo7_93 = "N"
        End If
        Borr.loans(UBound(Borr.loans)).WtdAveInt = Rec.PR_RPD_FOR_ITR
        'undrly dsb
        If CDate(Rec.AD_PRC) < CDate("07/01/01") Then
            Borr.loans(UBound(Borr.loans)).UndrlyDisb = "B"
        Else
            Borr.loans(UBound(Borr.loans)).UndrlyDisb = "A"
        End If
    End If
    'for PLUS only
    If Rec.AC_LON_TYP = "PL" Then
        Borr.loans(UBound(Borr.loans)).SSSN = Rec.STU_SSN
    End If
    'for stafford and unsub stafford only
    If Rec.AC_LON_TYP = "SF" Or Rec.AC_LON_TYP = "SU" Then
        Borr.loans(UBound(Borr.loans)).CurrSchool = Rec.IF_OPS_SCL_RPT
        Borr.loans(UBound(Borr.loans)).SchSepDate = Rec.LD_LFT_SCL
        'sep reason
        If Rec.LC_STU_ENR_TYP = "A" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "05"
        ElseIf Rec.LC_STU_ENR_TYP = "D" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "06"
        ElseIf Rec.LC_STU_ENR_TYP = "F" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "11"
        ElseIf Rec.LC_STU_ENR_TYP = "G" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "01"
        ElseIf Rec.LC_STU_ENR_TYP = "H" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "10"
        ElseIf Rec.LC_STU_ENR_TYP = "L" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "08"
        ElseIf Rec.LC_STU_ENR_TYP = "W" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "02"
        ElseIf Rec.LC_STU_ENR_TYP = "X" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "07"
        ElseIf Rec.LC_STU_ENR_TYP = "Z" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "18"
        Else
            MsgBox "Unexpected Value For Sep Reason."
            End
        End If
        Borr.loans(UBound(Borr.loans)).SepSource = "GR"
        Borr.loans(UBound(Borr.loans)).CertDt = Rec.LD_ENR_CER
        Borr.loans(UBound(Borr.loans)).NtfRecdDt = Rec.LD_LDR_NTF
        Borr.loans(UBound(Borr.loans)).GraceMonth = "06"
        Borr.loans(UBound(Borr.loans)).GracePeriodEnd = DateAdd("m", 6, CDate(Rec.LD_LFT_SCL))
        Borr.loans(UBound(Borr.loans)).RepayStartDt = CDate(Borr.loans(UBound(Borr.loans)).GracePeriodEnd) + 1
    Else
        'for non stafford and unsub stafford only
        Borr.loans(UBound(Borr.loans)).GraceMonth = ""
        Borr.loans(UBound(Borr.loans)).GracePeriodEnd = ""
    End If
    'original int rate
    Borr.loans(UBound(Borr.loans)).OriginalIntRate = Rec.ORG_INT_RATE
    'Rehab info
    Borr.loans(UBound(Borr.loans)).LA_PAY_XPC = Rec.LA_PAY_XPC
    Borr.loans(UBound(Borr.loans)).BL_LA_PAY_XPC = Rec.BL_LA_PAY_XPC
    Borr.loans(UBound(Borr.loans)).BA_STD_PAY = Rec.BA_STD_PAY
    Borr.loans(UBound(Borr.loans)).PRI_COST = Rec.PRI_COST
    'int rate
    Borr.loans(UBound(Borr.loans)).IntRate = Rec.PR_RPD_FOR_ITR
    'int type
    If Rec.LC_INT_TYP = "A" Then
        Borr.loans(UBound(Borr.loans)).IntType = "F2"
    ElseIf Rec.LC_INT_TYP = "F" Then
        Borr.loans(UBound(Borr.loans)).IntType = "F1"
    ElseIf Rec.LC_INT_TYP = "V" Then
        If Rec.AC_LON_TYP = "CL" Or Rec.AC_LON_TYP = "PL" Or Rec.AC_LON_TYP = "SL" Then
            Borr.loans(UBound(Borr.loans)).IntType = "SV"
        ElseIf Rec.AC_LON_TYP = "SF" Or Rec.AC_LON_TYP = "SU" Then
            If CDate(Rec.AD_DISB_1) < CDate("07/01/95") Then
                Borr.loans(UBound(Borr.loans)).IntType = "SV"
            Else
                If Rec.LD_LFT_SCL > Date Then
                    Borr.loans(UBound(Borr.loans)).IntType = "C1"
                Else
                    Borr.loans(UBound(Borr.loans)).IntType = "C2"
                End If
            End If
        End If
    Else
        MsgBox "Unexpected Value For Int Type."
        End
    End If
    Borr.loans(UBound(Borr.loans)).SpecRate = "N"
    'financial info
    Borr.loans(UBound(Borr.loans)).TotIndbt = Rec.AA_TOT_EDU_DET_PNT
    Borr.loans(UBound(Borr.loans)).Principal = Rec.PRI_COST
    Borr.loans(UBound(Borr.loans)).IntLastCapped = DateAdd("d", -1, CDate(Rec.LD_TRX_EFF))
    Borr.loans(UBound(Borr.loans)).LD_TRX_EFF = Rec.LD_TRX_EFF
    Borr.loans(UBound(Borr.loans)).LD_TRX_EFF_Less9Months = DateAdd("m", -9, CDate(Rec.LD_TRX_EFF))
    Borr.loans(UBound(Borr.loans)).LD_RHB = Rec.LD_RHB
    Borr.loans(UBound(Borr.loans)).LA_PRI = Rec.LA_PRI
    Borr.loans(UBound(Borr.loans)).LA_INT = Rec.LA_INT
    'endorser info
    Borr.loans(UBound(Borr.loans)).AC_EDS_TYP = Rec.AC_EDS_TYP
    Borr.loans(UBound(Borr.loans)).EAddr1 = Rec.EDSR_DX_STR_ADR_1
    Borr.loans(UBound(Borr.loans)).EAddr2 = Rec.EDSR_DX_STR_ADR_2
    Borr.loans(UBound(Borr.loans)).EAddrInd = Rec.EDSR_DI_VLD_ADR
    Borr.loans(UBound(Borr.loans)).EAltPhone = Rec.EDSR_DN_ALT_PHN
    Borr.loans(UBound(Borr.loans)).EAltPhoneInd = Rec.EDSR_DI_ALT_PHN_VLD
    Borr.loans(UBound(Borr.loans)).ECity = Rec.EDSR_DM_CT
    Borr.loans(UBound(Borr.loans)).ECountry = Rec.EDSR_DM_FGN_CNY
    Borr.loans(UBound(Borr.loans)).EDOB = Rec.EDSR_DD_BRT
    Borr.loans(UBound(Borr.loans)).EFirstName = Rec.EDSR_DM_PRS_1
    Borr.loans(UBound(Borr.loans)).ELastName = Rec.EDSR_DM_PRS_LST
    Borr.loans(UBound(Borr.loans)).EMID = Rec.EDSR_DM_PRS_MID
    Borr.loans(UBound(Borr.loans)).EPhone = Rec.EDSR_DN_PHN
    Borr.loans(UBound(Borr.loans)).EPhoneInd = Rec.EDSR_DI_PHN_VLD
    Borr.loans(UBound(Borr.loans)).ESSN = Rec.EDSR_SSN
    Borr.loans(UBound(Borr.loans)).EState = Rec.EDSR_DC_DOM_ST
    Borr.loans(UBound(Borr.loans)).EZip = Rec.EDSR_DF_ZIP
    'student info
    Borr.loans(UBound(Borr.loans)).SAddr1 = Rec.STU_DX_STR_ADR_1
    Borr.loans(UBound(Borr.loans)).SAddr2 = Rec.STU_DX_STR_ADR_2
    Borr.loans(UBound(Borr.loans)).SAddrInd = Rec.STU_DI_VLD_ADR
    Borr.loans(UBound(Borr.loans)).SAltPhone = Rec.STU_DN_ALT_PHN
    Borr.loans(UBound(Borr.loans)).SAltPhoneInd = Rec.STU_DI_ALT_PHN_VLD
    Borr.loans(UBound(Borr.loans)).SCity = Rec.STU_DM_CT
    Borr.loans(UBound(Borr.loans)).SCountry = Rec.STU_DM_FGN_CNY
    Borr.loans(UBound(Borr.loans)).SDOB = Rec.STU_DD_BRT
    Borr.loans(UBound(Borr.loans)).SFirstName = Rec.STU_DM_PRS_1
    Borr.loans(UBound(Borr.loans)).SLastName = Rec.STU_DM_PRS_LST
    Borr.loans(UBound(Borr.loans)).SMID = Rec.STU_DM_PRS_MID
    Borr.loans(UBound(Borr.loans)).SPhone = Rec.STU_DN_PHN
    Borr.loans(UBound(Borr.loans)).SPhoneInd = Rec.STU_DI_PHN_VLD
    Borr.loans(UBound(Borr.loans)).SSSN = Rec.STU_SSN
    Borr.loans(UBound(Borr.loans)).SState = Rec.STU_DC_DOM_ST
    Borr.loans(UBound(Borr.loans)).SZip = Rec.STU_DF_ZIP
    'disbursment information
    ReDim Borr.loans(UBound(Borr.loans)).Disbursements(0)
    If Rec.AD_DISB_1 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).GTE = Rec.GTE_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_1
        If Rec.CA_DISB_1 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        If Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "PL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "SL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "CL" Then
            Borr.loans(UBound(Borr.loans)).RepayStartDt = Rec.AD_DISB_1
        End If
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_1
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    If Rec.AD_DISB_2 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).GTE = Rec.GTE_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_2
        If Rec.CA_DISB_2 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        If Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "PL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "SL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "CL" Then
            Borr.loans(UBound(Borr.loans)).RepayStartDt = Rec.AD_DISB_1
        End If
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_2
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    If Rec.AD_DISB_3 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).GTE = Rec.GTE_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_3
        If Rec.CA_DISB_3 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        If Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "PL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "SL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "CL" Then
            Borr.loans(UBound(Borr.loans)).RepayStartDt = Rec.AD_DISB_1
        End If
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_3
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    If Rec.AD_DISB_4 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).GTE = Rec.GTE_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_4
        If Rec.CA_DISB_4 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        If Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "PL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "SL" Or Borr.loans(UBound(Borr.loans)).AC_LON_TYP = "CL" Then
            Borr.loans(UBound(Borr.loans)).RepayStartDt = Rec.AD_DISB_1
        End If
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_4
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    'deferment info
    ReDim Borr.loans(UBound(Borr.loans)).Deferments(0)
    ReDim Borr.loans(UBound(Borr.loans)).Forbearances(0)
    If Rec.LC_DFR_TYP1 <> "" Then
        UseCOMPASSDeferInfo = True
    Else
        UseCOMPASSDeferInfo = False
    End If
    If UseCOMPASSDeferInfo Then
        'load COMPASS deferment info
        'Deferment #1
        If Rec.LD_DFR_BEG1 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG1
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END1
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP1
            If Rec.LC_DFR_TYP1 = "15" Or Rec.LC_DFR_TYP1 = "06" Or Rec.LC_DFR_TYP1 = "16" Or Rec.LC_DFR_TYP1 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR1
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER1
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #2
        If Rec.LD_DFR_BEG2 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG2
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END2
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP2
            If Rec.LC_DFR_TYP2 = "15" Or Rec.LC_DFR_TYP2 = "06" Or Rec.LC_DFR_TYP2 = "16" Or Rec.LC_DFR_TYP2 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR2
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER2
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #3
        If Rec.LD_DFR_BEG3 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG3
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END3
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP3
            If Rec.LC_DFR_TYP3 = "15" Or Rec.LC_DFR_TYP3 = "06" Or Rec.LC_DFR_TYP3 = "16" Or Rec.LC_DFR_TYP3 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR3
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER3
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #4
        If Rec.LD_DFR_BEG4 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG4
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END4
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP4
            If Rec.LC_DFR_TYP4 = "15" Or Rec.LC_DFR_TYP4 = "06" Or Rec.LC_DFR_TYP4 = "16" Or Rec.LC_DFR_TYP4 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR4
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER4
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #5
        If Rec.LD_DFR_BEG5 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG5
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END5
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP5
            If Rec.LC_DFR_TYP5 = "15" Or Rec.LC_DFR_TYP5 = "06" Or Rec.LC_DFR_TYP5 = "16" Or Rec.LC_DFR_TYP5 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR5
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER5
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #6
        If Rec.LD_DFR_BEG6 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG6
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END6
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP6
            If Rec.LC_DFR_TYP6 = "15" Or Rec.LC_DFR_TYP6 = "06" Or Rec.LC_DFR_TYP6 = "16" Or Rec.LC_DFR_TYP6 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR6
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER6
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #7
        If Rec.LD_DFR_BEG7 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG7
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END7
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP7
            If Rec.LC_DFR_TYP7 = "15" Or Rec.LC_DFR_TYP7 = "06" Or Rec.LC_DFR_TYP7 = "16" Or Rec.LC_DFR_TYP7 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR7
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER7
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #8
        If Rec.LD_DFR_BEG8 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG8
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END8
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP8
            If Rec.LC_DFR_TYP8 = "15" Or Rec.LC_DFR_TYP8 = "06" Or Rec.LC_DFR_TYP8 = "16" Or Rec.LC_DFR_TYP8 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR8
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER8
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #9
        If Rec.LD_DFR_BEG9 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG9
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END9
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP9
            If Rec.LC_DFR_TYP9 = "15" Or Rec.LC_DFR_TYP9 = "06" Or Rec.LC_DFR_TYP9 = "16" Or Rec.LC_DFR_TYP9 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR9
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER9
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #10
        If Rec.LD_DFR_BEG10 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG10
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END10
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP10
            If Rec.LC_DFR_TYP10 = "15" Or Rec.LC_DFR_TYP10 = "06" Or Rec.LC_DFR_TYP10 = "16" Or Rec.LC_DFR_TYP10 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR10
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER10
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #11
        If Rec.LD_DFR_BEG11 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG11
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END11
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP11
            If Rec.LC_DFR_TYP11 = "15" Or Rec.LC_DFR_TYP11 = "06" Or Rec.LC_DFR_TYP11 = "16" Or Rec.LC_DFR_TYP11 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR11
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER11
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #12
        If Rec.LD_DFR_BEG12 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG12
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END12
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP12
            If Rec.LC_DFR_TYP12 = "15" Or Rec.LC_DFR_TYP12 = "06" Or Rec.LC_DFR_TYP12 = "16" Or Rec.LC_DFR_TYP12 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR12
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER12
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #13
        If Rec.LD_DFR_BEG13 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG13
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END13
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP13
            If Rec.LC_DFR_TYP13 = "15" Or Rec.LC_DFR_TYP13 = "06" Or Rec.LC_DFR_TYP13 = "16" Or Rec.LC_DFR_TYP13 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR13
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER13
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #14
        If Rec.LD_DFR_BEG14 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG14
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END14
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP14
            If Rec.LC_DFR_TYP14 = "15" Or Rec.LC_DFR_TYP14 = "06" Or Rec.LC_DFR_TYP14 = "16" Or Rec.LC_DFR_TYP14 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR14
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER14
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #15
        If Rec.LD_DFR_BEG15 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG15
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END15
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP15
            If Rec.LC_DFR_TYP15 = "15" Or Rec.LC_DFR_TYP15 = "06" Or Rec.LC_DFR_TYP15 = "16" Or Rec.LC_DFR_TYP15 = "18" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR15
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER15
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
    Else
        'load OneLINK deferment info
        'Deferment #1
        If Rec.AD_DFR_BEG1 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG1
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END1
            If Rec.AC_LON_STA_REA1 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA1 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA1 = "FT" Or Rec.AC_LON_STA_REA1 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA1 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA1 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA1 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA1 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA1 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA1 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA1 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA1 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA1 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA1 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA1 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA1 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA1 = "UE" Or Rec.AC_LON_STA_REA1 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA1 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA1 = "FT" Or Rec.AC_LON_STA_REA1 = "GF" Or Rec.AC_LON_STA_REA1 = "HT" Or Rec.AC_LON_STA_REA1 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT1
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER1
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #2
        If Rec.AD_DFR_BEG2 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG2
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END2
            If Rec.AC_LON_STA_REA2 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA2 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA2 = "FT" Or Rec.AC_LON_STA_REA2 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA2 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA2 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA2 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA2 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA2 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA2 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA2 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA2 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA2 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA2 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA2 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA2 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA2 = "UE" Or Rec.AC_LON_STA_REA2 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA2 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA2 = "FT" Or Rec.AC_LON_STA_REA2 = "GF" Or Rec.AC_LON_STA_REA2 = "HT" Or Rec.AC_LON_STA_REA2 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT2
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER2
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #3
        If Rec.AD_DFR_BEG3 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG3
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END3
            If Rec.AC_LON_STA_REA3 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA3 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA3 = "FT" Or Rec.AC_LON_STA_REA3 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA3 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA3 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA3 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA3 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA3 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA3 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA3 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA3 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA3 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA3 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA3 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA3 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA3 = "UE" Or Rec.AC_LON_STA_REA3 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA3 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA3 = "FT" Or Rec.AC_LON_STA_REA3 = "GF" Or Rec.AC_LON_STA_REA3 = "HT" Or Rec.AC_LON_STA_REA3 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT3
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER3
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #4
        If Rec.AD_DFR_BEG4 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG4
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END4
            If Rec.AC_LON_STA_REA4 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA4 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA4 = "FT" Or Rec.AC_LON_STA_REA4 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA4 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA4 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA4 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA4 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA4 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA4 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA4 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA4 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA4 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA4 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA4 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA4 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA4 = "UE" Or Rec.AC_LON_STA_REA4 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA4 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA4 = "FT" Or Rec.AC_LON_STA_REA4 = "GF" Or Rec.AC_LON_STA_REA4 = "HT" Or Rec.AC_LON_STA_REA4 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT4
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER4
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #5
        If Rec.AD_DFR_BEG5 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG5
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END5
            If Rec.AC_LON_STA_REA5 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA5 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA5 = "FT" Or Rec.AC_LON_STA_REA5 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA5 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA5 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA5 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA5 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA5 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA5 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA5 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA5 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA5 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA5 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA5 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA5 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA5 = "UE" Or Rec.AC_LON_STA_REA5 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA5 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA5 = "FT" Or Rec.AC_LON_STA_REA5 = "GF" Or Rec.AC_LON_STA_REA5 = "HT" Or Rec.AC_LON_STA_REA5 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT5
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER5
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #6
        If Rec.AD_DFR_BEG6 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG6
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END6
            If Rec.AC_LON_STA_REA6 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA6 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA6 = "FT" Or Rec.AC_LON_STA_REA6 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA6 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA6 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA6 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA6 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA6 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA6 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA6 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA6 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA6 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA6 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA6 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA6 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA6 = "UE" Or Rec.AC_LON_STA_REA6 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA6 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA6 = "FT" Or Rec.AC_LON_STA_REA6 = "GF" Or Rec.AC_LON_STA_REA6 = "HT" Or Rec.AC_LON_STA_REA6 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT6
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER6
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #7
        If Rec.AD_DFR_BEG7 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG7
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END7
            If Rec.AC_LON_STA_REA7 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA7 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA7 = "FT" Or Rec.AC_LON_STA_REA7 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA7 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA7 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA7 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA7 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA7 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA7 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA7 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA7 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA7 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA7 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA7 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA7 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA7 = "UE" Or Rec.AC_LON_STA_REA7 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA7 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA7 = "FT" Or Rec.AC_LON_STA_REA7 = "GF" Or Rec.AC_LON_STA_REA7 = "HT" Or Rec.AC_LON_STA_REA7 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT7
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER7
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #8
        If Rec.AD_DFR_BEG8 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG8
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END8
            If Rec.AC_LON_STA_REA8 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA8 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA8 = "FT" Or Rec.AC_LON_STA_REA8 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA8 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA8 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA8 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA8 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA8 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA8 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA8 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA8 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA8 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA8 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA8 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA8 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA8 = "UE" Or Rec.AC_LON_STA_REA8 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA8 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA8 = "FT" Or Rec.AC_LON_STA_REA8 = "GF" Or Rec.AC_LON_STA_REA8 = "HT" Or Rec.AC_LON_STA_REA8 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT8
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER8
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #9
        If Rec.AD_DFR_BEG9 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG9
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END9
            If Rec.AC_LON_STA_REA9 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA9 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA9 = "FT" Or Rec.AC_LON_STA_REA9 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA9 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA9 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA9 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA9 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA9 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA9 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA9 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA9 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA9 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA9 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA9 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA9 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA9 = "UE" Or Rec.AC_LON_STA_REA9 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA9 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA9 = "FT" Or Rec.AC_LON_STA_REA9 = "GF" Or Rec.AC_LON_STA_REA9 = "HT" Or Rec.AC_LON_STA_REA9 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT9
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER9
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #10
        If Rec.AD_DFR_BEG10 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG10
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END10
            If Rec.AC_LON_STA_REA10 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA10 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA10 = "FT" Or Rec.AC_LON_STA_REA10 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA10 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA10 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA10 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA10 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA10 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA10 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA10 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA10 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA10 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA10 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA10 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA10 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA10 = "UE" Or Rec.AC_LON_STA_REA10 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA10 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA10 = "FT" Or Rec.AC_LON_STA_REA10 = "GF" Or Rec.AC_LON_STA_REA10 = "HT" Or Rec.AC_LON_STA_REA10 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT10
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER10
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #11
        If Rec.AD_DFR_BEG11 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG11
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END11
            If Rec.AC_LON_STA_REA11 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA11 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA11 = "FT" Or Rec.AC_LON_STA_REA11 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA11 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA11 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA11 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA11 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA11 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA11 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA11 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA11 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA11 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA11 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA11 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA11 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA11 = "UE" Or Rec.AC_LON_STA_REA11 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA11 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA11 = "FT" Or Rec.AC_LON_STA_REA11 = "GF" Or Rec.AC_LON_STA_REA11 = "HT" Or Rec.AC_LON_STA_REA11 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT11
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER11
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #12
        If Rec.AD_DFR_BEG12 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG12
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END12
            If Rec.AC_LON_STA_REA12 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA12 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA12 = "FT" Or Rec.AC_LON_STA_REA12 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA12 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA12 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA12 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA12 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA12 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA12 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA12 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA12 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA12 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA12 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA12 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA12 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA12 = "UE" Or Rec.AC_LON_STA_REA12 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA12 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA12 = "FT" Or Rec.AC_LON_STA_REA12 = "GF" Or Rec.AC_LON_STA_REA12 = "HT" Or Rec.AC_LON_STA_REA12 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT12
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER12
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #13
        If Rec.AD_DFR_BEG13 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG13
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END13
            If Rec.AC_LON_STA_REA13 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA13 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA13 = "FT" Or Rec.AC_LON_STA_REA13 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA13 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA13 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA13 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA13 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA13 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA13 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA13 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA13 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA13 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA13 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA13 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA13 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA13 = "UE" Or Rec.AC_LON_STA_REA13 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA13 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA13 = "FT" Or Rec.AC_LON_STA_REA13 = "GF" Or Rec.AC_LON_STA_REA13 = "HT" Or Rec.AC_LON_STA_REA13 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT13
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER13
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #14
        If Rec.AD_DFR_BEG14 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG14
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END14
            If Rec.AC_LON_STA_REA14 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA14 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA14 = "FT" Or Rec.AC_LON_STA_REA14 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA14 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA14 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA14 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA14 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA14 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA14 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA14 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA14 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA14 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA14 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA14 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA14 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA14 = "UE" Or Rec.AC_LON_STA_REA14 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA14 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA14 = "FT" Or Rec.AC_LON_STA_REA14 = "GF" Or Rec.AC_LON_STA_REA14 = "HT" Or Rec.AC_LON_STA_REA14 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT14
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER14
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #15
        If Rec.AD_DFR_BEG15 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG15
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END15
            If Rec.AC_LON_STA_REA15 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA15 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA15 = "FT" Or Rec.AC_LON_STA_REA15 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA15 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA15 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA15 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA15 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA15 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA15 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA15 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA15 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA15 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA15 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA15 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA15 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA15 = "UE" Or Rec.AC_LON_STA_REA15 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA15 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA15 = "FT" Or Rec.AC_LON_STA_REA15 = "GF" Or Rec.AC_LON_STA_REA15 = "HT" Or Rec.AC_LON_STA_REA15 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT15
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER15
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
    End If
    'create another loan
    ReDim Preserve Borr.loans(UBound(Borr.loans) + 1)
End Sub

'this function loads data from the file to the processing objects
Private Sub LoadPurchDat(ByRef Borr As Borrower, Rec As RepurchaseRec, LoadBorrLvlDat As Boolean)
    Dim UseCOMPASSDeferInfo As Boolean
    If LoadBorrLvlDat Then
        'borrower level data
        'borrower
        Borr.BAddr1 = Rec.DX_STR_ADR_1
        Borr.BAddr2 = Rec.DX_STR_ADR_2
        Borr.BAddrInd = Rec.DI_VLD_ADR
        Borr.BCity = Rec.DM_CT
        Borr.BDOB = Rec.DD_BRT
        Borr.BFirstName = Rec.DM_PRS_1
        Borr.BLastName = Rec.DM_PRS_LST
        Borr.BMID = Rec.DM_PRS_MID
        Borr.BPhone = Rec.DN_PHN
        Borr.BPhoneInd = Rec.DI_PHN_VLD
        Borr.BPhoneSource = "56"
        Borr.BSSN = Rec.BF_SSN
        Borr.BState = Rec.DC_DOM_ST
        Borr.BZip = Rec.DF_ZIP
        'ref1
        Borr.R1Addr1 = Rec.BX_RFR_STR_ADR_1_1
        Borr.R1Addr2 = Rec.BX_RFR_STR_ADR_2_1
        Borr.R1AddrInd = Rec.BI_VLD_ADR_1
        Borr.R1City = Rec.BM_RFR_CT_1
        Borr.R1FirstName = Rec.BM_RFR_1_1
        Borr.R1LastName = Rec.BM_RFR_LST_1
        Borr.R1MID = Rec.BM_RFR_MID_1
        Borr.R1Phone = Rec.BN_RFR_DOM_PHN_1
        Borr.R1PhoneInd = Rec.BI_DOM_PHN_VLD_1
        Borr.R1Relation = Rec.BC_RFR_REL_BR_1
        Borr.R1State = Rec.BC_RFR_ST_1
        Borr.R1Zip = Rec.BF_RFR_ZIP_1
        'ref2
        Borr.R2Addr1 = Rec.BX_RFR_STR_ADR_1_2
        Borr.R2Addr2 = Rec.BX_RFR_STR_ADR_2_2
        Borr.R2AddrInd = Rec.BI_VLD_ADR_2
        Borr.R2City = Rec.BM_RFR_CT_2
        Borr.R2FirstName = Rec.BM_RFR_1_2
        Borr.R2LastName = Rec.BM_RFR_LST_2
        Borr.R2MID = Rec.BM_RFR_MID_2
        Borr.R2Phone = Rec.BN_RFR_DOM_PHN_2
        Borr.R2PhoneInd = Rec.BI_DOM_PHN_VLD_2
        Borr.R2Relation = Rec.BC_RFR_REL_BR_2
        Borr.R2State = Rec.BC_RFR_ST_2
        Borr.R2Zip = Rec.BF_RFR_ZIP_2
        'IBR
        Borr.IBREligibleInd = Rec.BR_ELIG_IND
        If Rec.LD_IBR_25Y_FGV_BEG <> "" Then
            Borr.IBRForgivenessStartDate = MID(Rec.LD_IBR_25Y_FGV_BEG, 1, 3) & "01/" & MID(Rec.LD_IBR_25Y_FGV_BEG, 7, 4)
        End If
        If Rec.LD_IBR_RPD_SR <> "" Then
            Borr.IBRCreatDate = MID(Rec.LD_IBR_RPD_SR, 1, 3) & "01/" & MID(Rec.LD_IBR_RPD_SR, 7, 4)
        End If
        Borr.IBRStndStndPayAmount = Rec.LA_IBR_STD_STD_PAY
        Borr.IBRQualifyingPayments = Rec.LN_IBR_QLF_FGV_MTH
    End If
    'loan lvl data
    Borr.loans(UBound(Borr.loans)).BondID = Rec.BondID
    Borr.loans(UBound(Borr.loans)).Guarantor = "000749"
    Borr.loans(UBound(Borr.loans)).AC_LON_TYP = Rec.AC_LON_TYP
    'loan prg
    If Rec.AC_LON_TYP = "SF" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "STFFRD"
    ElseIf Rec.AC_LON_TYP = "SU" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "UNSTFD"
    ElseIf Rec.AC_LON_TYP = "PL" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "PLUS"
    ElseIf Rec.AC_LON_TYP = "SL" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "SLS"
    ElseIf Rec.AC_LON_TYP = "GB" Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "PLUSGB"
    ElseIf Rec.AC_LON_TYP = "CL" And Rec.AD_PRC <= CDate("07/01/1997") Then
        Borr.loans(UBound(Borr.loans)).LoanPrg = "CNSLDN"
    ElseIf Rec.AC_LON_TYP = "CL" Then
        If Rec.SUBSIDY = "Y" And Rec.EDSR_SSN <> "" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "SUBSPC"
        ElseIf Rec.SUBSIDY = "N" And Rec.EDSR_SSN <> "" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "UNSPC"
        ElseIf Rec.SUBSIDY = "Y" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "SUBCNS"
        ElseIf Rec.SUBSIDY = "N" Then
            Borr.loans(UBound(Borr.loans)).LoanPrg = "UNCNS"
        Else
            MsgBox "Unexpected Value For Loan Program."
            End
        End If
    Else
        MsgBox "Unexpected Value For Loan Program."
        End
    End If
    Borr.loans(UBound(Borr.loans)).OrigLender = Rec.AF_ORG_APL_OPS_LDR
    Borr.loans(UBound(Borr.loans)).RepType = "N"
    If CDate(Rec.AD_APL_RCV) > CDate(Rec.AD_DISB_1) Then
        Borr.loans(UBound(Borr.loans)).AppRcvdDt = Rec.AD_DISB_1
    Else
        Borr.loans(UBound(Borr.loans)).AppRcvdDt = Rec.AD_APL_RCV
    End If
    Borr.loans(UBound(Borr.loans)).DtNoteSigned = Rec.AD_BR_SIG
    Borr.loans(UBound(Borr.loans)).DtGuaranteed = Rec.AD_PRC
    Borr.loans(UBound(Borr.loans)).AmountGuaranteed = Rec.AA_GTE_LON_AMT
    Borr.loans(UBound(Borr.loans)).clid = Rec.AF_APL_ID & Rec.AF_APL_ID_SFX
    'if SLS loan
    If Rec.AC_LON_TYP = "SL" Then
        If Rec.AC_STU_DFR_REQ = "" Then
            Borr.loans(UBound(Borr.loans)).AC_STU_DFR_REQ = "N"
        Else
            Borr.loans(UBound(Borr.loans)).AC_STU_DFR_REQ = Rec.AC_STU_DFR_REQ
        End If
    End If
    'if not consolidation
    If Rec.AC_LON_TYP <> "CL" Then
        Borr.loans(UBound(Borr.loans)).TermBg = Rec.AD_IST_TRM_BEG
        Borr.loans(UBound(Borr.loans)).TermEd = Rec.AD_IST_TRM_END
        Borr.loans(UBound(Borr.loans)).OrigSchool = Rec.AF_APL_OPS_SCL
    End If
    'subsidy
    If Rec.AC_LON_TYP = "SF" Then
        Borr.loans(UBound(Borr.loans)).SubsidyCd = "Y"
    ElseIf Rec.AC_LON_TYP = "CL" And Rec.AD_PRC <= CDate("07/01/1997") Then
        Borr.loans(UBound(Borr.loans)).SubsidyCd = "N"
    ElseIf Rec.AC_LON_TYP = "CL" Then
        Borr.loans(UBound(Borr.loans)).SubsidyCd = Rec.SUBSIDY
    Else
        Borr.loans(UBound(Borr.loans)).SubsidyCd = "N"
    End If
    Borr.loans(UBound(Borr.loans)).SpallElig = "Y"
    'consolidation only
    If Rec.AC_LON_TYP = "CL" Then
        'prior to 07/01/93
        If CDate(Rec.AD_PRC) < CDate("07/01/93") Then
            Borr.loans(UBound(Borr.loans)).PriorTo7_93 = "Y"
        Else
            Borr.loans(UBound(Borr.loans)).PriorTo7_93 = "N"
        End If
        Borr.loans(UBound(Borr.loans)).WtdAveInt = Rec.PR_RPD_FOR_ITR
        'undrly dsb
        If CDate(Rec.AD_PRC) < CDate("07/01/01") Then
            Borr.loans(UBound(Borr.loans)).UndrlyDisb = "B"
        Else
            Borr.loans(UBound(Borr.loans)).UndrlyDisb = "A"
        End If
    End If
    'for PLUS only
    If Rec.AC_LON_TYP = "PL" Then
        Borr.loans(UBound(Borr.loans)).SSSN = Rec.STU_SSN
    End If
    'for stafford and unsub stafford only
    If Rec.AC_LON_TYP = "SF" Or Rec.AC_LON_TYP = "SU" Then
        Borr.loans(UBound(Borr.loans)).CurrSchool = Rec.IF_OPS_SCL_RPT
        Borr.loans(UBound(Borr.loans)).SchSepDate = Rec.LD_LFT_SCL
        'sep reason
        If Rec.LC_STU_ENR_TYP = "A" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "05"
        ElseIf Rec.LC_STU_ENR_TYP = "D" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "06"
        ElseIf Rec.LC_STU_ENR_TYP = "F" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "11"
        ElseIf Rec.LC_STU_ENR_TYP = "G" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "01"
        ElseIf Rec.LC_STU_ENR_TYP = "H" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "10"
        ElseIf Rec.LC_STU_ENR_TYP = "L" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "08"
        ElseIf Rec.LC_STU_ENR_TYP = "W" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "02"
        ElseIf Rec.LC_STU_ENR_TYP = "X" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "07"
        ElseIf Rec.LC_STU_ENR_TYP = "Z" Then
            Borr.loans(UBound(Borr.loans)).SepReason = "18"
        Else
            MsgBox "Unexpected Value For Sep Reason."
            End
        End If
        Borr.loans(UBound(Borr.loans)).SepSource = "GR"
        Borr.loans(UBound(Borr.loans)).CertDt = Rec.LD_ENR_CER
        Borr.loans(UBound(Borr.loans)).NtfRecdDt = Rec.LD_LDR_NTF
        Borr.loans(UBound(Borr.loans)).GraceMonth = "06"
        Borr.loans(UBound(Borr.loans)).GracePeriodEnd = DateAdd("m", 6, CDate(Rec.LD_LFT_SCL))
        Borr.loans(UBound(Borr.loans)).RepayStartDt = CDate(Borr.loans(UBound(Borr.loans)).GracePeriodEnd) + 1
    Else
        'for non stafford and unsub stafford only
        Borr.loans(UBound(Borr.loans)).GraceMonth = ""
        Borr.loans(UBound(Borr.loans)).GracePeriodEnd = ""
    End If
    'original int rate
    Borr.loans(UBound(Borr.loans)).OriginalIntRate = Rec.ORG_INT_RATE
    'int rate
    Borr.loans(UBound(Borr.loans)).IntRate = Rec.PR_RPD_FOR_ITR
    'int type
    If Rec.LC_INT_TYP = "A" Then
        Borr.loans(UBound(Borr.loans)).IntType = "F2"
    ElseIf Rec.LC_INT_TYP = "F" Then
        Borr.loans(UBound(Borr.loans)).IntType = "F1"
    ElseIf Rec.LC_INT_TYP = "V" Then
        If Rec.AC_LON_TYP = "CL" Or Rec.AC_LON_TYP = "PL" Or Rec.AC_LON_TYP = "SL" Then
            Borr.loans(UBound(Borr.loans)).IntType = "SV"
        ElseIf Rec.AC_LON_TYP = "SF" Or Rec.AC_LON_TYP = "SU" Then
            If CDate(Rec.AD_DISB_1) < CDate("07/01/95") Then
                Borr.loans(UBound(Borr.loans)).IntType = "SV"
            Else
                If Rec.LD_LFT_SCL > Date Then
                    Borr.loans(UBound(Borr.loans)).IntType = "C1"
                Else
                    Borr.loans(UBound(Borr.loans)).IntType = "C2"
                End If
            End If
        End If
    Else
        MsgBox "Unexpected Value For Int Type."
        End
    End If
    Borr.loans(UBound(Borr.loans)).SpecRate = "N"
    'financial info
    Borr.loans(UBound(Borr.loans)).TotIndbt = Rec.AA_TOT_EDU_DET_PNT
    Borr.loans(UBound(Borr.loans)).Principal = Rec.Bal
    Borr.loans(UBound(Borr.loans)).IntLastCapped = DateAdd("d", -1, CDate(Rec.DT_REPUR))
    'endorser info
    Borr.loans(UBound(Borr.loans)).AC_EDS_TYP = Rec.AC_EDS_TYP
    Borr.loans(UBound(Borr.loans)).EAddr1 = Rec.EDSR_DX_STR_ADR_1
    Borr.loans(UBound(Borr.loans)).EAddr2 = Rec.EDSR_DX_STR_ADR_2
    Borr.loans(UBound(Borr.loans)).EAddrInd = Rec.EDSR_DI_VLD_ADR
    Borr.loans(UBound(Borr.loans)).EAltPhone = Rec.EDSR_DN_ALT_PHN
    Borr.loans(UBound(Borr.loans)).EAltPhoneInd = Rec.EDSR_DI_ALT_PHN_VLD
    Borr.loans(UBound(Borr.loans)).ECity = Rec.EDSR_DM_CT
    Borr.loans(UBound(Borr.loans)).ECountry = Rec.EDSR_DM_FGN_CNY
    Borr.loans(UBound(Borr.loans)).EDOB = Rec.EDSR_DD_BRT
    Borr.loans(UBound(Borr.loans)).EFirstName = Rec.EDSR_DM_PRS_1
    Borr.loans(UBound(Borr.loans)).ELastName = Rec.EDSR_DM_PRS_LST
    Borr.loans(UBound(Borr.loans)).EMID = Rec.EDSR_DM_PRS_MID
    Borr.loans(UBound(Borr.loans)).EPhone = Rec.EDSR_DN_PHN
    Borr.loans(UBound(Borr.loans)).EPhoneInd = Rec.EDSR_DI_PHN_VLD
    Borr.loans(UBound(Borr.loans)).ESSN = Rec.EDSR_SSN
    Borr.loans(UBound(Borr.loans)).EState = Rec.EDSR_DC_DOM_ST
    Borr.loans(UBound(Borr.loans)).EZip = Rec.EDSR_DF_ZIP
    'student info
    Borr.loans(UBound(Borr.loans)).SAddr1 = Rec.STU_DX_STR_ADR_1
    Borr.loans(UBound(Borr.loans)).SAddr2 = Rec.STU_DX_STR_ADR_2
    Borr.loans(UBound(Borr.loans)).SAddrInd = Rec.STU_DI_VLD_ADR
    Borr.loans(UBound(Borr.loans)).SAltPhone = Rec.STU_DN_ALT_PHN
    Borr.loans(UBound(Borr.loans)).SAltPhoneInd = Rec.STU_DI_ALT_PHN_VLD
    Borr.loans(UBound(Borr.loans)).SCity = Rec.STU_DM_CT
    Borr.loans(UBound(Borr.loans)).SCountry = Rec.STU_DM_FGN_CNY
    Borr.loans(UBound(Borr.loans)).SDOB = Rec.STU_DD_BRT
    Borr.loans(UBound(Borr.loans)).SFirstName = Rec.STU_DM_PRS_1
    Borr.loans(UBound(Borr.loans)).SLastName = Rec.STU_DM_PRS_LST
    Borr.loans(UBound(Borr.loans)).SMID = Rec.STU_DM_PRS_MID
    Borr.loans(UBound(Borr.loans)).SPhone = Rec.STU_DN_PHN
    Borr.loans(UBound(Borr.loans)).SPhoneInd = Rec.STU_DI_PHN_VLD
    Borr.loans(UBound(Borr.loans)).SSSN = Rec.STU_SSN
    Borr.loans(UBound(Borr.loans)).SState = Rec.STU_DC_DOM_ST
    Borr.loans(UBound(Borr.loans)).SZip = Rec.STU_DF_ZIP
    'Principal & Interest
    Borr.loans(UBound(Borr.loans)).LA_PRI = Rec.LA_PRI
    Borr.loans(UBound(Borr.loans)).LA_INT = Rec.LA_INT
    'disbursment information
    ReDim Borr.loans(UBound(Borr.loans)).Disbursements(0)
    If Rec.AD_DISB_1 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_1
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_1
        If Rec.CA_DISB_1 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_1
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    If Rec.AD_DISB_2 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_2
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_2
        If Rec.CA_DISB_2 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_2
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    If Rec.AD_DISB_3 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_3
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_3
        If Rec.CA_DISB_3 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_3
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    If Rec.AD_DISB_4 <> "" Then 'only add disbursement if date is populated
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = Rec.CA_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_4
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = Rec.CD_DISB_4
        If Rec.CA_DISB_4 <> "" Then Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = "0165"
        Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_4
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    End If
    'deferment info
    ReDim Borr.loans(UBound(Borr.loans)).Deferments(0)
    If Rec.LC_DFR_TYP1 <> "" Then
        UseCOMPASSDeferInfo = True
    Else
        UseCOMPASSDeferInfo = False
    End If
    If UseCOMPASSDeferInfo Then
        'load COMPASS deferment info
        'Deferment #1
        If Rec.LD_DFR_BEG1 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG1
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END1
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP1
            If Rec.LC_DFR_TYP1 = "15" Or Rec.LC_DFR_TYP1 = "06" Or Rec.LC_DFR_TYP1 = "16" Or Rec.LC_DFR_TYP1 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR1
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER1
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #2
        If Rec.LD_DFR_BEG2 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG2
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END2
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP2
            If Rec.LC_DFR_TYP2 = "15" Or Rec.LC_DFR_TYP2 = "06" Or Rec.LC_DFR_TYP2 = "16" Or Rec.LC_DFR_TYP2 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR2
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER2
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #3
        If Rec.LD_DFR_BEG3 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG3
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END3
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP3
            If Rec.LC_DFR_TYP3 = "15" Or Rec.LC_DFR_TYP3 = "06" Or Rec.LC_DFR_TYP3 = "16" Or Rec.LC_DFR_TYP3 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR3
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER3
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #4
        If Rec.LD_DFR_BEG4 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG4
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END4
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP4
            If Rec.LC_DFR_TYP4 = "15" Or Rec.LC_DFR_TYP4 = "06" Or Rec.LC_DFR_TYP4 = "16" Or Rec.LC_DFR_TYP4 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR4
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER4
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #5
        If Rec.LD_DFR_BEG5 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG5
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END5
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP5
            If Rec.LC_DFR_TYP5 = "15" Or Rec.LC_DFR_TYP5 = "06" Or Rec.LC_DFR_TYP5 = "16" Or Rec.LC_DFR_TYP5 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR5
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER5
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #6
        If Rec.LD_DFR_BEG6 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG6
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END6
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP6
            If Rec.LC_DFR_TYP6 = "15" Or Rec.LC_DFR_TYP6 = "06" Or Rec.LC_DFR_TYP6 = "16" Or Rec.LC_DFR_TYP6 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR6
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER6
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #7
        If Rec.LD_DFR_BEG7 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG7
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END7
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP7
            If Rec.LC_DFR_TYP7 = "15" Or Rec.LC_DFR_TYP7 = "06" Or Rec.LC_DFR_TYP7 = "16" Or Rec.LC_DFR_TYP7 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR7
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER7
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #8
        If Rec.LD_DFR_BEG8 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG8
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END8
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP8
            If Rec.LC_DFR_TYP8 = "15" Or Rec.LC_DFR_TYP8 = "06" Or Rec.LC_DFR_TYP8 = "16" Or Rec.LC_DFR_TYP8 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR8
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER8
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #9
        If Rec.LD_DFR_BEG9 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG9
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END9
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP9
            If Rec.LC_DFR_TYP9 = "15" Or Rec.LC_DFR_TYP9 = "06" Or Rec.LC_DFR_TYP9 = "16" Or Rec.LC_DFR_TYP9 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR9
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER9
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #10
        If Rec.LD_DFR_BEG10 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG10
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END10
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP10
            If Rec.LC_DFR_TYP10 = "15" Or Rec.LC_DFR_TYP10 = "06" Or Rec.LC_DFR_TYP10 = "16" Or Rec.LC_DFR_TYP10 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR10
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER10
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #11
        If Rec.LD_DFR_BEG11 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG11
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END11
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP11
            If Rec.LC_DFR_TYP11 = "15" Or Rec.LC_DFR_TYP11 = "06" Or Rec.LC_DFR_TYP11 = "16" Or Rec.LC_DFR_TYP11 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR11
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER11
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #12
        If Rec.LD_DFR_BEG12 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG12
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END12
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP12
            If Rec.LC_DFR_TYP12 = "15" Or Rec.LC_DFR_TYP12 = "06" Or Rec.LC_DFR_TYP12 = "16" Or Rec.LC_DFR_TYP12 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR12
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER12
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #13
        If Rec.LD_DFR_BEG13 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG13
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END13
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP13
            If Rec.LC_DFR_TYP13 = "15" Or Rec.LC_DFR_TYP13 = "06" Or Rec.LC_DFR_TYP13 = "16" Or Rec.LC_DFR_TYP13 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR13
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER13
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #14
        If Rec.LD_DFR_BEG14 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG14
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END14
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP14
            If Rec.LC_DFR_TYP14 = "15" Or Rec.LC_DFR_TYP14 = "06" Or Rec.LC_DFR_TYP14 = "16" Or Rec.LC_DFR_TYP14 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR14
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER14
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #15
        If Rec.LD_DFR_BEG15 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.LD_DFR_BEG15
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.LD_DFR_END15
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = Rec.LC_DFR_TYP15
            If Rec.LC_DFR_TYP15 = "15" Or Rec.LC_DFR_TYP15 = "06" Or Rec.LC_DFR_TYP15 = "16" Or Rec.LC_DFR_TYP15 = "18" Or Rec.LC_DFR_TYP1 = "45" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.LF_DOE_SCL_DFR15
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_DFR_INF_CER15
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
    Else
        'load OneLINK deferment info
        'Deferment #1
        If Rec.AD_DFR_BEG1 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG1
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END1
            If Rec.AC_LON_STA_REA1 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA1 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA1 = "FT" Or Rec.AC_LON_STA_REA1 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA1 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA1 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA1 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA1 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA1 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA1 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA1 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA1 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA1 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA1 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA1 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA1 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA1 = "UE" Or Rec.AC_LON_STA_REA1 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA1 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA1 = "FT" Or Rec.AC_LON_STA_REA1 = "GF" Or Rec.AC_LON_STA_REA1 = "HT" Or Rec.AC_LON_STA_REA1 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT1
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER1
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #2
        If Rec.AD_DFR_BEG2 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG2
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END2
            If Rec.AC_LON_STA_REA2 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA2 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA2 = "FT" Or Rec.AC_LON_STA_REA2 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA2 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA2 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA2 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA2 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA2 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA2 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA2 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA2 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA2 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA2 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA2 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA2 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA2 = "UE" Or Rec.AC_LON_STA_REA2 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA2 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA2 = "FT" Or Rec.AC_LON_STA_REA2 = "GF" Or Rec.AC_LON_STA_REA2 = "HT" Or Rec.AC_LON_STA_REA2 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT2
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER2
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #3
        If Rec.AD_DFR_BEG3 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG3
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END3
            If Rec.AC_LON_STA_REA3 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA3 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA3 = "FT" Or Rec.AC_LON_STA_REA3 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA3 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA3 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA3 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA3 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA3 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA3 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA3 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA3 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA3 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA3 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA3 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA3 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA3 = "UE" Or Rec.AC_LON_STA_REA3 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA3 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA3 = "FT" Or Rec.AC_LON_STA_REA3 = "GF" Or Rec.AC_LON_STA_REA3 = "HT" Or Rec.AC_LON_STA_REA3 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT3
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER3
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #4
        If Rec.AD_DFR_BEG4 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG4
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END4
            If Rec.AC_LON_STA_REA4 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA4 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA4 = "FT" Or Rec.AC_LON_STA_REA4 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA4 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA4 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA4 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA4 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA4 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA4 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA4 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA4 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA4 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA4 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA4 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA4 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA4 = "UE" Or Rec.AC_LON_STA_REA4 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA4 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA4 = "FT" Or Rec.AC_LON_STA_REA4 = "GF" Or Rec.AC_LON_STA_REA4 = "HT" Or Rec.AC_LON_STA_REA4 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT4
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER4
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #5
        If Rec.AD_DFR_BEG5 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG5
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END5
            If Rec.AC_LON_STA_REA5 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA5 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA5 = "FT" Or Rec.AC_LON_STA_REA5 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA5 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA5 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA5 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA5 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA5 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA5 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA5 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA5 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA5 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA5 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA5 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA5 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA5 = "UE" Or Rec.AC_LON_STA_REA5 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA5 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA5 = "FT" Or Rec.AC_LON_STA_REA5 = "GF" Or Rec.AC_LON_STA_REA5 = "HT" Or Rec.AC_LON_STA_REA5 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT5
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER5
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #6
        If Rec.AD_DFR_BEG6 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG6
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END6
            If Rec.AC_LON_STA_REA6 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA6 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA6 = "FT" Or Rec.AC_LON_STA_REA6 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA6 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA6 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA6 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA6 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA6 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA6 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA6 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA6 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA6 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA6 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA6 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA6 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA6 = "UE" Or Rec.AC_LON_STA_REA6 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA6 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA6 = "FT" Or Rec.AC_LON_STA_REA6 = "GF" Or Rec.AC_LON_STA_REA6 = "HT" Or Rec.AC_LON_STA_REA6 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT6
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER6
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #7
        If Rec.AD_DFR_BEG7 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG7
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END7
            If Rec.AC_LON_STA_REA7 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA7 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA7 = "FT" Or Rec.AC_LON_STA_REA7 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA7 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA7 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA7 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA7 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA7 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA7 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA7 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA7 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA7 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA7 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA7 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA7 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA7 = "UE" Or Rec.AC_LON_STA_REA7 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA7 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA7 = "FT" Or Rec.AC_LON_STA_REA7 = "GF" Or Rec.AC_LON_STA_REA7 = "HT" Or Rec.AC_LON_STA_REA7 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT7
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER7
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #8
        If Rec.AD_DFR_BEG8 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG8
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END8
            If Rec.AC_LON_STA_REA8 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA8 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA8 = "FT" Or Rec.AC_LON_STA_REA8 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA8 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA8 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA8 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA8 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA8 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA8 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA8 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA8 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA8 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA8 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA8 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA8 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA8 = "UE" Or Rec.AC_LON_STA_REA8 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA8 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA8 = "FT" Or Rec.AC_LON_STA_REA8 = "GF" Or Rec.AC_LON_STA_REA8 = "HT" Or Rec.AC_LON_STA_REA8 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT8
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER8
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #9
        If Rec.AD_DFR_BEG9 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG9
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END9
            If Rec.AC_LON_STA_REA9 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA9 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA9 = "FT" Or Rec.AC_LON_STA_REA9 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA9 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA9 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA9 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA9 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA9 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA9 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA9 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA9 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA9 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA9 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA9 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA9 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA9 = "UE" Or Rec.AC_LON_STA_REA9 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA9 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA9 = "FT" Or Rec.AC_LON_STA_REA9 = "GF" Or Rec.AC_LON_STA_REA9 = "HT" Or Rec.AC_LON_STA_REA9 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT9
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER9
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #10
        If Rec.AD_DFR_BEG10 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG10
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END10
            If Rec.AC_LON_STA_REA10 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA10 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA10 = "FT" Or Rec.AC_LON_STA_REA10 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA10 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA10 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA10 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA10 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA10 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA10 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA10 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA10 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA10 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA10 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA10 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA10 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA10 = "UE" Or Rec.AC_LON_STA_REA10 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA10 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA10 = "FT" Or Rec.AC_LON_STA_REA10 = "GF" Or Rec.AC_LON_STA_REA10 = "HT" Or Rec.AC_LON_STA_REA10 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT10
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER10
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #11
        If Rec.AD_DFR_BEG11 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG11
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END11
            If Rec.AC_LON_STA_REA11 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA11 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA11 = "FT" Or Rec.AC_LON_STA_REA11 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA11 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA11 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA11 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA11 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA11 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA11 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA11 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA11 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA11 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA11 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA11 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA11 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA11 = "UE" Or Rec.AC_LON_STA_REA11 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA11 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA11 = "FT" Or Rec.AC_LON_STA_REA11 = "GF" Or Rec.AC_LON_STA_REA11 = "HT" Or Rec.AC_LON_STA_REA11 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT11
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER11
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #12
        If Rec.AD_DFR_BEG12 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG12
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END12
            If Rec.AC_LON_STA_REA12 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA12 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA12 = "FT" Or Rec.AC_LON_STA_REA12 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA12 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA12 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA12 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA12 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA12 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA12 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA12 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA12 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA12 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA12 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA12 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA12 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA12 = "UE" Or Rec.AC_LON_STA_REA12 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA12 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA12 = "FT" Or Rec.AC_LON_STA_REA12 = "GF" Or Rec.AC_LON_STA_REA12 = "HT" Or Rec.AC_LON_STA_REA12 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT12
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER12
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #13
        If Rec.AD_DFR_BEG13 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG13
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END13
            If Rec.AC_LON_STA_REA13 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA13 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA13 = "FT" Or Rec.AC_LON_STA_REA13 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA13 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA13 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA13 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA13 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA13 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA13 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA13 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA13 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA13 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA13 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA13 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA13 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA13 = "UE" Or Rec.AC_LON_STA_REA13 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA13 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA13 = "FT" Or Rec.AC_LON_STA_REA13 = "GF" Or Rec.AC_LON_STA_REA13 = "HT" Or Rec.AC_LON_STA_REA13 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT13
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER13
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #14
        If Rec.AD_DFR_BEG14 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG14
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END14
            If Rec.AC_LON_STA_REA14 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA14 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA14 = "FT" Or Rec.AC_LON_STA_REA14 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA14 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA14 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA14 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA14 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA14 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA14 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA14 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA14 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA14 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA14 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA14 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA14 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA14 = "UE" Or Rec.AC_LON_STA_REA14 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA14 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA14 = "FT" Or Rec.AC_LON_STA_REA14 = "GF" Or Rec.AC_LON_STA_REA14 = "HT" Or Rec.AC_LON_STA_REA14 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT14
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER14
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
        'Deferment #15
        If Rec.AD_DFR_BEG15 <> "" Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Rec.AD_DFR_BEG15
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = Rec.AD_DFR_END15
            If Rec.AC_LON_STA_REA15 = "AP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "04"
            ElseIf Rec.AC_LON_STA_REA15 = "EH" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "29"
            ElseIf Rec.AC_LON_STA_REA15 = "FT" Or Rec.AC_LON_STA_REA15 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "15"
            ElseIf Rec.AC_LON_STA_REA15 = "GF" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
            ElseIf Rec.AC_LON_STA_REA15 = "HT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "18"
            ElseIf Rec.AC_LON_STA_REA15 = "IR" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "09"
            ElseIf Rec.AC_LON_STA_REA15 = "MO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "38"
            ElseIf Rec.AC_LON_STA_REA15 = "NO" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "01"
            ElseIf Rec.AC_LON_STA_REA15 = "PC" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "07"
            ElseIf Rec.AC_LON_STA_REA15 = "PL" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "12"
            ElseIf Rec.AC_LON_STA_REA15 = "PP" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "19"
            ElseIf Rec.AC_LON_STA_REA15 = "RT" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "03"
            ElseIf Rec.AC_LON_STA_REA15 = "TD" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "02"
            ElseIf Rec.AC_LON_STA_REA15 = "TE" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "10"
            ElseIf Rec.AC_LON_STA_REA15 = "TS" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "14"
            ElseIf Rec.AC_LON_STA_REA15 = "UE" Or Rec.AC_LON_STA_REA15 = "UN" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "13"
            ElseIf Rec.AC_LON_STA_REA15 = "WM" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "11"
            Else
                MsgBox "Unexpected Value For Type."
                End
            End If
            If Rec.AC_LON_STA_REA15 = "FT" Or Rec.AC_LON_STA_REA15 = "GF" Or Rec.AC_LON_STA_REA15 = "HT" Or Rec.AC_LON_STA_REA15 = "31" Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = Rec.IF_OPS_SCL_RPT15
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = Rec.LD_ENR_CER15
            End If
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "Y"
            ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
        End If
    End If
    'Forbearance
    ReDim Borr.loans(UBound(Borr.loans)).Forbearances(0)
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG1, Rec.LD_FOR_END1, Rec.OL_FRB_BEG1, Rec.OL_FRB_END1, Rec.LC_FOR_TYP1, Rec.LI_CAP_FOR_INT_REQ1
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG2, Rec.LD_FOR_END2, Rec.OL_FRB_BEG2, Rec.OL_FRB_END2, Rec.LC_FOR_TYP2, Rec.LI_CAP_FOR_INT_REQ2
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG3, Rec.LD_FOR_END3, Rec.OL_FRB_BEG3, Rec.OL_FRB_END3, Rec.LC_FOR_TYP3, Rec.LI_CAP_FOR_INT_REQ3
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG4, Rec.LD_FOR_END4, Rec.OL_FRB_BEG4, Rec.OL_FRB_END4, Rec.LC_FOR_TYP4, Rec.LI_CAP_FOR_INT_REQ4
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG5, Rec.LD_FOR_END5, Rec.OL_FRB_BEG5, Rec.OL_FRB_END5, Rec.LC_FOR_TYP5, Rec.LI_CAP_FOR_INT_REQ5
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG6, Rec.LD_FOR_END6, Rec.OL_FRB_BEG6, Rec.OL_FRB_END6, Rec.LC_FOR_TYP6, Rec.LI_CAP_FOR_INT_REQ6
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG7, Rec.LD_FOR_END7, Rec.OL_FRB_BEG7, Rec.OL_FRB_END7, Rec.LC_FOR_TYP7, Rec.LI_CAP_FOR_INT_REQ7
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG8, Rec.LD_FOR_END8, Rec.OL_FRB_BEG8, Rec.OL_FRB_END8, Rec.LC_FOR_TYP8, Rec.LI_CAP_FOR_INT_REQ8
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG9, Rec.LD_FOR_END9, Rec.OL_FRB_BEG9, Rec.OL_FRB_END9, Rec.LC_FOR_TYP9, Rec.LI_CAP_FOR_INT_REQ9
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG10, Rec.LD_FOR_END10, Rec.OL_FRB_BEG10, Rec.OL_FRB_END10, Rec.LC_FOR_TYP10, Rec.LI_CAP_FOR_INT_REQ10
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG11, Rec.LD_FOR_END11, Rec.OL_FRB_BEG11, Rec.OL_FRB_END11, Rec.LC_FOR_TYP11, Rec.LI_CAP_FOR_INT_REQ11
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG12, Rec.LD_FOR_END12, Rec.OL_FRB_BEG12, Rec.OL_FRB_END12, Rec.LC_FOR_TYP12, Rec.LI_CAP_FOR_INT_REQ12
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG13, Rec.LD_FOR_END13, Rec.OL_FRB_BEG13, Rec.OL_FRB_END13, Rec.LC_FOR_TYP13, Rec.LI_CAP_FOR_INT_REQ13
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG14, Rec.LD_FOR_END14, Rec.OL_FRB_BEG14, Rec.OL_FRB_END14, Rec.LC_FOR_TYP14, Rec.LI_CAP_FOR_INT_REQ14
    AddForbearanceToLoan Borr, Rec.LD_FOR_BEG15, Rec.LD_FOR_END15, Rec.OL_FRB_BEG15, Rec.OL_FRB_END15, Rec.LC_FOR_TYP15, Rec.LI_CAP_FOR_INT_REQ15
    'create another loan
    ReDim Preserve Borr.loans(UBound(Borr.loans) + 1)
End Sub

Private Sub AddForbearanceToLoan(ByRef Borr As Borrower, CB As String, CE As String, OB As String, OE As String, Ftype As String, FCapInt As String)
    'add forbearance to array
    'CB = compass begin date CE = compass End date. OB = Onelink begin etc...
    Dim BeginDt As String
    Dim EndDt As String
    BeginDt = CB
    EndDt = CE
    If CB = "" Then
       BeginDt = OB
       EndDt = OE
    End If
    If BeginDt = "" Then Exit Sub
    ReDim Preserve Borr.loans(UBound(Borr.loans)).Forbearances(UBound(Borr.loans(UBound(Borr.loans)).Forbearances) + 1)
    Borr.loans(UBound(Borr.loans)).Forbearances(UBound(Borr.loans(UBound(Borr.loans)).Forbearances)).BeginDate = BeginDt
    Borr.loans(UBound(Borr.loans)).Forbearances(UBound(Borr.loans(UBound(Borr.loans)).Forbearances)).EndDate = EndDt
    Borr.loans(UBound(Borr.loans)).Forbearances(UBound(Borr.loans(UBound(Borr.loans)).Forbearances)).Type = Ftype
    Borr.loans(UBound(Borr.loans)).Forbearances(UBound(Borr.loans(UBound(Borr.loans)).Forbearances)).CapInt = FCapInt
End Sub

Private Sub UnassignQueueTsks()
    Dim UID As String
    Dim Row As Integer
    Row = 9
    UID = SP.Common.GetUserID()
    FastPath "TX3ZCTX6JAD;A1"
    If SP.Q.Check4Text(1, 72, "TXX6N") Then
    'selection screen
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        If Check4Text(Row + 1, 20, UID) Then
            puttext 21, 18, GetText(Row, 3, 1), "enter" 'select option
            puttext 8, 15, "", "End"
            Hit "Enter"
            Exit Sub
        End If
        Row = Row + 2
        'check for page forward
        If Check4Text(Row, 3, " ") Then
            Row = 9
            If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                Hit "Enter"
            Else
                Hit "F8"
            End If
        End If
    Wend
    Else
        'target screen
        If SP.Q.Check4Text(8, 15, UID) Then
            SP.Q.puttext 8, 15, "", "END"
            SP.Q.Hit "ENTER"
        End If
    End If
End Sub

Private Sub ModFastPath(str As String)
    Session.TransmitTerminalKey rcIBMHomeKey
    Session.WaitForEvent rcKbdEnabled, "60", "0", 1, 1
    Session.TransmitANSI str
    Session.TransmitTerminalKey rcIBMEraseEOFKey
    Session.WaitForEvent rcKbdEnabled, "60", "0", 1, 1
    Session.TransmitTerminalKey rcIBMEnterKey
    Session.WaitForEvent rcKbdEnabled, "60", "0", 1, 1
End Sub

Private Sub AddToLenderServicerNoDup(tSSN As String, tUID As String, tLD_TRX As String)
    'Add to lender servicer file if it is not already there.
    Dim UID As String
    Dim LD_TRX As String
    Dim ssn As String
    Dim found As Boolean
    found = False
    Open "T:\AACLenderServicer.txt" For Input As #91
        Do While Not EOF(91)
            Input #91, ssn, UID, LD_TRX
            If tSSN = ssn And tUID = UID And tLD_TRX = LD_TRX Then
                found = True
                Exit Do
            End If
        Loop
    Close #91
    If found = False Then
        Open "T:\AACLenderServicer.txt" For Append As #90
            Write #90, tSSN, tUID, tLD_TRX
        Close #90
    End If
End Sub

Private Sub LenderAndServicer(ssn As String, UID As String, LD_TRX As String)
    Dim x As Integer
    SP.Q.FastPath "LG20C" & ssn
    Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        For x = 7 To 19
            Dim testString As String
            If SP.Q.Check4Text(x, 46, UID) Or (Check4Text(x, 39, "CL") And Check4Text(x, 46, MID(UID, 1, 17))) Then
                SP.Q.puttext 20, 15, Format(SP.Q.GetText(x, 5, 2), "00"), "ENTER"
                Exit Do
            End If
        Next
        SP.Q.Hit "F8"
    Loop
    If Not Check4Text(6, 14, " ") Then
        SP.Q.puttext 6, 14, "K"
        If SP.Q.Check4Text(9, 31, "828476") = False Then
            SP.Q.puttext 9, 31, "828476"
            SP.Q.puttext 10, 31, Format(CDate(LD_TRX), "mmddyyyy")
        End If
        If SP.Q.Check4Text(12, 31, "700126") = False Then
            SP.Q.puttext 12, 31, "700126"
            SP.Q.puttext 11, 31, Format(CDate(LD_TRX), "mmddyyyy")
        End If
        SP.Q.Hit "ENTER"
    End If
End Sub

'this sub blanks out the current borrower/loan data and loads new borrower/loan data
Private Function TILPObjectsLoaded(FTIP As FileTypeInProcess, TPRec As TILPRec, FIP As String) As String
    Dim LastSSNProcessed As String
    Dim Batch As String
    Dim TrackingTemp As String
    Dim LoansInBatch As String
    
    'blank current borrower data and init loan level info arrays
    ReDim Borrs(0)
    ReDim Borrs(UBound(Borrs)).loans(0)
    If TPRec.BF_SSN = "" Then
        'get priming read
        Input #1, TPRec.BF_SSN, TPRec.DM_PRS_1, TPRec.DM_PRS_MID, TPRec.DM_PRS_LST, TPRec.DD_BRT, TPRec.DX_STR_ADR_1, TPRec.DX_STR_ADR_2, TPRec.DM_CT, TPRec.DC_DOM_ST, TPRec.DF_ZIP, TPRec.DM_FGN_CNY, TPRec.DI_VLD_ADR, TPRec.DN_PHN, TPRec.DI_PHN_VLD, TPRec.DN_ALT_PHN, TPRec.DI_ALT_PHN_VLD, _
        TPRec.AC_LON_TYP, TPRec.SUBSIDY, TPRec.AD_PRC, TPRec.AF_ORG_APL_OPS_LDR, TPRec.AF_APL_ID, TPRec.AF_APL_ID_SFX, TPRec.AD_IST_TRM_BEG, TPRec.AD_IST_TRM_END, TPRec.AA_GTE_LON_AMT, TPRec.AF_APL_OPS_SCL, TPRec.AD_BR_SIG, TPRec.LD_LFT_SCL, TPRec.PR_RPD_FOR_ITR, TPRec.LC_INT_TYP, TPRec.LD_TRX_EFF, TPRec.LA_TRX, TPRec.IF_OPS_SCL_RPT, TPRec.LC_STU_ENR_TYP, TPRec.LD_ENR_CER, TPRec.LD_LDR_NTF, TPRec.AR_CON_ITR, TPRec.AD_APL_RCV, TPRec.AC_STU_DFR_REQ, _
        TPRec.AN_DISB_1, TPRec.AC_DISB_1, TPRec.AD_DISB_1, TPRec.AA_DISB_1, TPRec.ORG_1, TPRec.CD_DISB_1, TPRec.CA_DISB_1, TPRec.GTE_1, TPRec.AN_DISB_2, TPRec.AC_DISB_2, TPRec.AD_DISB_2, TPRec.AA_DISB_2, TPRec.ORG_2, TPRec.CD_DISB_2, TPRec.CA_DISB_2, TPRec.GTE_2, TPRec.AN_DISB_3, TPRec.AC_DISB_3, TPRec.AD_DISB_3, TPRec.AA_DISB_3, TPRec.ORG_3, TPRec.CD_DISB_3, TPRec.CA_DISB_3, TPRec.GTE_3, TPRec.AN_DISB_4, TPRec.AC_DISB_4, TPRec.AD_DISB_4, TPRec.AA_DISB_4, TPRec.ORG_4, TPRec.CD_DISB_4, TPRec.CA_DISB_4, TPRec.GTE_4, TPRec.AA_TOT_EDU_DET_PNT, TPRec.LC_DFR_TYP1, TPRec.LC_DFR_TYP2, TPRec.LC_DFR_TYP3, TPRec.LC_DFR_TYP4, TPRec.LC_DFR_TYP5, TPRec.LC_DFR_TYP6, TPRec.LC_DFR_TYP7, TPRec.LC_DFR_TYP8, TPRec.LC_DFR_TYP9, TPRec.LC_DFR_TYP10, TPRec.LC_DFR_TYP11, TPRec.LC_DFR_TYP12, TPRec.LC_DFR_TYP13, TPRec.LC_DFR_TYP14, TPRec.LC_DFR_TYP15, TPRec.LD_DFR_BEG1, TPRec.LD_DFR_BEG2, TPRec.LD_DFR_BEG3, TPRec.LD_DFR_BEG4, TPRec.LD_DFR_BEG5, _
        TPRec.LD_DFR_BEG6, TPRec.LD_DFR_BEG7, TPRec.LD_DFR_BEG8, TPRec.LD_DFR_BEG9, TPRec.LD_DFR_BEG10, TPRec.LD_DFR_BEG11, TPRec.LD_DFR_BEG12, TPRec.LD_DFR_BEG13, TPRec.LD_DFR_BEG14, TPRec.LD_DFR_BEG15, TPRec.LD_DFR_END1, TPRec.LD_DFR_END2, TPRec.LD_DFR_END3, TPRec.LD_DFR_END4, TPRec.LD_DFR_END5, TPRec.LD_DFR_END6, TPRec.LD_DFR_END7, TPRec.LD_DFR_END8, TPRec.LD_DFR_END9, TPRec.LD_DFR_END10, TPRec.LD_DFR_END11, TPRec.LD_DFR_END12, TPRec.LD_DFR_END13, TPRec.LD_DFR_END14, TPRec.LD_DFR_END15, TPRec.LF_DOE_SCL_DFR1, TPRec.LF_DOE_SCL_DFR2, TPRec.LF_DOE_SCL_DFR3, TPRec.LF_DOE_SCL_DFR4, TPRec.LF_DOE_SCL_DFR5, TPRec.LF_DOE_SCL_DFR6, TPRec.LF_DOE_SCL_DFR7, TPRec.LF_DOE_SCL_DFR8, TPRec.LF_DOE_SCL_DFR9, TPRec.LF_DOE_SCL_DFR10, TPRec.LF_DOE_SCL_DFR11, TPRec.LF_DOE_SCL_DFR12, TPRec.LF_DOE_SCL_DFR13, TPRec.LF_DOE_SCL_DFR14, TPRec.LF_DOE_SCL_DFR15, TPRec.LD_DFR_INF_CER1, _
        TPRec.LD_DFR_INF_CER2, TPRec.LD_DFR_INF_CER3, TPRec.LD_DFR_INF_CER4, TPRec.LD_DFR_INF_CER5, TPRec.LD_DFR_INF_CER6, TPRec.LD_DFR_INF_CER7, TPRec.LD_DFR_INF_CER8, TPRec.LD_DFR_INF_CER9, TPRec.LD_DFR_INF_CER10, TPRec.LD_DFR_INF_CER11, TPRec.LD_DFR_INF_CER12, TPRec.LD_DFR_INF_CER13, TPRec.LD_DFR_INF_CER14, TPRec.LD_DFR_INF_CER15, TPRec.AC_LON_STA_REA1, TPRec.AC_LON_STA_REA2, TPRec.AC_LON_STA_REA3, TPRec.AC_LON_STA_REA4, TPRec.AC_LON_STA_REA5, TPRec.AC_LON_STA_REA6, TPRec.AC_LON_STA_REA7, TPRec.AC_LON_STA_REA8, TPRec.AC_LON_STA_REA9, TPRec.AC_LON_STA_REA10, TPRec.AC_LON_STA_REA11, TPRec.AC_LON_STA_REA12, TPRec.AC_LON_STA_REA13, TPRec.AC_LON_STA_REA14, _
        TPRec.AC_LON_STA_REA15, TPRec.AD_DFR_BEG1, TPRec.AD_DFR_BEG2, TPRec.AD_DFR_BEG3, TPRec.AD_DFR_BEG4, TPRec.AD_DFR_BEG5, TPRec.AD_DFR_BEG6, TPRec.AD_DFR_BEG7, TPRec.AD_DFR_BEG8, TPRec.AD_DFR_BEG9, TPRec.AD_DFR_BEG10, TPRec.AD_DFR_BEG11, TPRec.AD_DFR_BEG12, TPRec.AD_DFR_BEG13, TPRec.AD_DFR_BEG14, TPRec.AD_DFR_BEG15, TPRec.AD_DFR_END1, TPRec.AD_DFR_END2, TPRec.AD_DFR_END3, TPRec.AD_DFR_END4, TPRec.AD_DFR_END5, TPRec.AD_DFR_END6, TPRec.AD_DFR_END7, TPRec.AD_DFR_END8, TPRec.AD_DFR_END9, TPRec.AD_DFR_END10, TPRec.AD_DFR_END11, TPRec.AD_DFR_END12, TPRec.AD_DFR_END13, TPRec.AD_DFR_END14, TPRec.AD_DFR_END15, TPRec.IF_OPS_SCL_RPT1, TPRec.IF_OPS_SCL_RPT2, TPRec.IF_OPS_SCL_RPT3, _
        TPRec.IF_OPS_SCL_RPT4, TPRec.IF_OPS_SCL_RPT5, TPRec.IF_OPS_SCL_RPT6, TPRec.IF_OPS_SCL_RPT7, TPRec.IF_OPS_SCL_RPT8, TPRec.IF_OPS_SCL_RPT9, TPRec.IF_OPS_SCL_RPT10, TPRec.IF_OPS_SCL_RPT11, TPRec.IF_OPS_SCL_RPT12, TPRec.IF_OPS_SCL_RPT13, TPRec.IF_OPS_SCL_RPT14, TPRec.IF_OPS_SCL_RPT15, TPRec.LD_ENR_CER1, TPRec.LD_ENR_CER2, TPRec.LD_ENR_CER3, TPRec.LD_ENR_CER4, TPRec.LD_ENR_CER5, TPRec.LD_ENR_CER6, TPRec.LD_ENR_CER7, TPRec.LD_ENR_CER8, TPRec.LD_ENR_CER9, TPRec.LD_ENR_CER10, TPRec.LD_ENR_CER11, TPRec.LD_ENR_CER12, TPRec.LD_ENR_CER13, TPRec.LD_ENR_CER14, TPRec.LD_ENR_CER15, _
        TPRec.STU_SSN, TPRec.STU_DM_PRS_1, TPRec.STU_DM_PRS_MID, TPRec.STU_DM_PRS_LST, TPRec.STU_DD_BRT, TPRec.STU_DX_STR_ADR_1, TPRec.STU_DX_STR_ADR_2, TPRec.STU_DM_CT, TPRec.STU_DC_DOM_ST, TPRec.STU_DF_ZIP, TPRec.STU_DM_FGN_CNY, TPRec.STU_DI_VLD_ADR, TPRec.STU_DN_PHN, TPRec.STU_DI_PHN_VLD, TPRec.STU_DN_ALT_PHN, TPRec.STU_DI_ALT_PHN_VLD, _
        TPRec.EDSR_SSN, TPRec.EDSR_DM_PRS_1, TPRec.EDSR_DM_PRS_MID, TPRec.EDSR_DM_PRS_LST, TPRec.EDSR_DD_BRT, TPRec.EDSR_DX_STR_ADR_1, TPRec.EDSR_DX_STR_ADR_2, TPRec.EDSR_DM_CT, TPRec.EDSR_DC_DOM_ST, TPRec.EDSR_DF_ZIP, TPRec.EDSR_DM_FGN_CNY, TPRec.EDSR_DI_VLD_ADR, TPRec.EDSR_DN_PHN, TPRec.EDSR_DI_PHN_VLD, TPRec.EDSR_DN_ALT_PHN, TPRec.EDSR_DI_ALT_PHN_VLD, TPRec.AC_EDS_TYP, _
        TPRec.REF_IND, TPRec.BM_RFR_1_1, TPRec.BM_RFR_MID_1, TPRec.BM_RFR_LST_1, TPRec.BX_RFR_STR_ADR_1_1, TPRec.BX_RFR_STR_ADR_2_1, TPRec.BM_RFR_CT_1, TPRec.BC_RFR_ST_1, TPRec.BF_RFR_ZIP_1, TPRec.BM_RFR_FGN_CNY_1, TPRec.BI_VLD_ADR_1, TPRec.BN_RFR_DOM_PHN_1, TPRec.BI_DOM_PHN_VLD_1, TPRec.BN_RFR_ALT_PHN_1, TPRec.BI_ALT_PHN_VLD_1, TPRec.BC_RFR_REL_BR_1, _
        TPRec.BM_RFR_1_2, TPRec.BM_RFR_MID_2, TPRec.BM_RFR_LST_2, TPRec.BX_RFR_STR_ADR_1_2, TPRec.BX_RFR_STR_ADR_2_2, TPRec.BM_RFR_CT_2, TPRec.BC_RFR_ST_2, TPRec.BF_RFR_ZIP_2, TPRec.BM_RFR_FGN_CNY_2, TPRec.BI_VLD_ADR_2, TPRec.BN_RFR_DOM_PHN_2, TPRec.BI_DOM_PHN_VLD_2, TPRec.BN_RFR_ALT_PHN_2, TPRec.BI_ALT_PHN_VLD_2, TPRec.BC_RFR_REL_BR_2, _
        TPRec.BondID, TPRec.AVE_REHB_PAY_AMT, TPRec.RPY_LETTER_DT, TPRec.RPY_INIT_BAL, TPRec.RPY_INT_RATE, TPRec.RPY_PAY_AMT, TPRec.RPY_PAY_TERM, TPRec.RPY_FIRST_DUE_DT, TPRec.TOT_INT_TO_REPAY, TPRec.TOT_REPAY_AMT, TPRec.RPY_LAST_PAY_DT, TPRec.FIT_INT_AMT, TPRec.FIT_FEE_AMT, TPRec.STH_STATUS, TPRec.STH_EFF_DT, TPRec.CACTUS_ID, TPRec.RPY_NEXT_DUE_DT, _
        TPRec.RPY_OVERPAY, TPRec.INT_ACCRUE_START_DT, TPRec.BAT_ID, TPRec.BAT_BR_CT, TPRec.BAT_LN_CT, TPRec.BAT_TOT_SUM, TPRec.BAT_ITR, TPRec.BAT_FEE
    End If
    'take note of batch ID and only process the current batch ID
    If Batch = "" Then Batch = TPRec.BAT_ID
    'get associated system batch ID
    Open "T:\AACMasterTracking.txt" For Input As #2
    '0 = SAS BatchNum, 1 = Number of borrowers in file, 2 = Number of loans, 3 = sum of principle amount, 4 = Assigned Batch, 5-9 = SSNs in batch, 10 = total interest, TILP ONLY 11 = total fees
    While Not EOF(2)
        Line Input #2, TrackingTemp
        If Replace(Split(TrackingTemp, ",")(0), """", "") = Batch Then
            TILPObjectsLoaded = Replace(Split(TrackingTemp, ",")(4), """", "")
            LoansInBatch = Replace(Split(TrackingTemp, ",")(2), """", "")
        End If
    Wend
    Close #2
    
    'load new borrower data (batches of 5)
    Do While Not EOF(1)
        'check if batch is the same if not then exit function with data collected
        If Batch <> TPRec.BAT_ID Then
            Exit Function
        End If
        'create new borrower or loan for current borrower as needed
        If LastSSNProcessed = TPRec.BF_SSN Then
            'add loan to existing borrower
            LoadTILPDat Borrs(UBound(Borrs) - 1), TPRec, False, FIP
        Else
            'create new borrower
            LastSSNProcessed = TPRec.BF_SSN
            LoadTILPDat Borrs(UBound(Borrs)), TPRec, True, FIP
            'init borrower and loan level info arrays
            LineCount = LineCount + 1
            If LineCount = CInt(LoansInBatch) Then
                Exit Do
            End If
            ReDim Preserve Borrs(UBound(Borrs) + 1)
            ReDim Borrs(UBound(Borrs)).loans(0)
        End If
        'init SSN flag for switching between SSNs
        If LastSSNProcessed = "" Then LastSSNProcessed = TPRec.BF_SSN
        Input #1, TPRec.BF_SSN, TPRec.DM_PRS_1, TPRec.DM_PRS_MID, TPRec.DM_PRS_LST, TPRec.DD_BRT, TPRec.DX_STR_ADR_1, TPRec.DX_STR_ADR_2, TPRec.DM_CT, TPRec.DC_DOM_ST, TPRec.DF_ZIP, TPRec.DM_FGN_CNY, TPRec.DI_VLD_ADR, TPRec.DN_PHN, TPRec.DI_PHN_VLD, TPRec.DN_ALT_PHN, TPRec.DI_ALT_PHN_VLD, _
        TPRec.AC_LON_TYP, TPRec.SUBSIDY, TPRec.AD_PRC, TPRec.AF_ORG_APL_OPS_LDR, TPRec.AF_APL_ID, TPRec.AF_APL_ID_SFX, TPRec.AD_IST_TRM_BEG, TPRec.AD_IST_TRM_END, TPRec.AA_GTE_LON_AMT, TPRec.AF_APL_OPS_SCL, TPRec.AD_BR_SIG, TPRec.LD_LFT_SCL, TPRec.PR_RPD_FOR_ITR, TPRec.LC_INT_TYP, TPRec.LD_TRX_EFF, TPRec.LA_TRX, TPRec.IF_OPS_SCL_RPT, TPRec.LC_STU_ENR_TYP, TPRec.LD_ENR_CER, TPRec.LD_LDR_NTF, TPRec.AR_CON_ITR, TPRec.AD_APL_RCV, TPRec.AC_STU_DFR_REQ, _
        TPRec.AN_DISB_1, TPRec.AC_DISB_1, TPRec.AD_DISB_1, TPRec.AA_DISB_1, TPRec.ORG_1, TPRec.CD_DISB_1, TPRec.CA_DISB_1, TPRec.GTE_1, TPRec.AN_DISB_2, TPRec.AC_DISB_2, TPRec.AD_DISB_2, TPRec.AA_DISB_2, TPRec.ORG_2, TPRec.CD_DISB_2, TPRec.CA_DISB_2, TPRec.GTE_2, TPRec.AN_DISB_3, TPRec.AC_DISB_3, TPRec.AD_DISB_3, TPRec.AA_DISB_3, TPRec.ORG_3, TPRec.CD_DISB_3, TPRec.CA_DISB_3, TPRec.GTE_3, TPRec.AN_DISB_4, TPRec.AC_DISB_4, TPRec.AD_DISB_4, TPRec.AA_DISB_4, TPRec.ORG_4, TPRec.CD_DISB_4, TPRec.CA_DISB_4, TPRec.GTE_4, TPRec.AA_TOT_EDU_DET_PNT, TPRec.LC_DFR_TYP1, TPRec.LC_DFR_TYP2, TPRec.LC_DFR_TYP3, TPRec.LC_DFR_TYP4, TPRec.LC_DFR_TYP5, TPRec.LC_DFR_TYP6, TPRec.LC_DFR_TYP7, TPRec.LC_DFR_TYP8, TPRec.LC_DFR_TYP9, TPRec.LC_DFR_TYP10, TPRec.LC_DFR_TYP11, TPRec.LC_DFR_TYP12, TPRec.LC_DFR_TYP13, TPRec.LC_DFR_TYP14, TPRec.LC_DFR_TYP15, TPRec.LD_DFR_BEG1, TPRec.LD_DFR_BEG2, TPRec.LD_DFR_BEG3, TPRec.LD_DFR_BEG4, TPRec.LD_DFR_BEG5, _
        TPRec.LD_DFR_BEG6, TPRec.LD_DFR_BEG7, TPRec.LD_DFR_BEG8, TPRec.LD_DFR_BEG9, TPRec.LD_DFR_BEG10, TPRec.LD_DFR_BEG11, TPRec.LD_DFR_BEG12, TPRec.LD_DFR_BEG13, TPRec.LD_DFR_BEG14, TPRec.LD_DFR_BEG15, TPRec.LD_DFR_END1, TPRec.LD_DFR_END2, TPRec.LD_DFR_END3, TPRec.LD_DFR_END4, TPRec.LD_DFR_END5, TPRec.LD_DFR_END6, TPRec.LD_DFR_END7, TPRec.LD_DFR_END8, TPRec.LD_DFR_END9, TPRec.LD_DFR_END10, TPRec.LD_DFR_END11, TPRec.LD_DFR_END12, TPRec.LD_DFR_END13, TPRec.LD_DFR_END14, TPRec.LD_DFR_END15, TPRec.LF_DOE_SCL_DFR1, TPRec.LF_DOE_SCL_DFR2, TPRec.LF_DOE_SCL_DFR3, TPRec.LF_DOE_SCL_DFR4, TPRec.LF_DOE_SCL_DFR5, TPRec.LF_DOE_SCL_DFR6, TPRec.LF_DOE_SCL_DFR7, TPRec.LF_DOE_SCL_DFR8, TPRec.LF_DOE_SCL_DFR9, TPRec.LF_DOE_SCL_DFR10, TPRec.LF_DOE_SCL_DFR11, TPRec.LF_DOE_SCL_DFR12, TPRec.LF_DOE_SCL_DFR13, TPRec.LF_DOE_SCL_DFR14, TPRec.LF_DOE_SCL_DFR15, TPRec.LD_DFR_INF_CER1, _
        TPRec.LD_DFR_INF_CER2, TPRec.LD_DFR_INF_CER3, TPRec.LD_DFR_INF_CER4, TPRec.LD_DFR_INF_CER5, TPRec.LD_DFR_INF_CER6, TPRec.LD_DFR_INF_CER7, TPRec.LD_DFR_INF_CER8, TPRec.LD_DFR_INF_CER9, TPRec.LD_DFR_INF_CER10, TPRec.LD_DFR_INF_CER11, TPRec.LD_DFR_INF_CER12, TPRec.LD_DFR_INF_CER13, TPRec.LD_DFR_INF_CER14, TPRec.LD_DFR_INF_CER15, TPRec.AC_LON_STA_REA1, TPRec.AC_LON_STA_REA2, TPRec.AC_LON_STA_REA3, TPRec.AC_LON_STA_REA4, TPRec.AC_LON_STA_REA5, TPRec.AC_LON_STA_REA6, TPRec.AC_LON_STA_REA7, TPRec.AC_LON_STA_REA8, TPRec.AC_LON_STA_REA9, TPRec.AC_LON_STA_REA10, TPRec.AC_LON_STA_REA11, TPRec.AC_LON_STA_REA12, TPRec.AC_LON_STA_REA13, TPRec.AC_LON_STA_REA14, _
        TPRec.AC_LON_STA_REA15, TPRec.AD_DFR_BEG1, TPRec.AD_DFR_BEG2, TPRec.AD_DFR_BEG3, TPRec.AD_DFR_BEG4, TPRec.AD_DFR_BEG5, TPRec.AD_DFR_BEG6, TPRec.AD_DFR_BEG7, TPRec.AD_DFR_BEG8, TPRec.AD_DFR_BEG9, TPRec.AD_DFR_BEG10, TPRec.AD_DFR_BEG11, TPRec.AD_DFR_BEG12, TPRec.AD_DFR_BEG13, TPRec.AD_DFR_BEG14, TPRec.AD_DFR_BEG15, TPRec.AD_DFR_END1, TPRec.AD_DFR_END2, TPRec.AD_DFR_END3, TPRec.AD_DFR_END4, TPRec.AD_DFR_END5, TPRec.AD_DFR_END6, TPRec.AD_DFR_END7, TPRec.AD_DFR_END8, TPRec.AD_DFR_END9, TPRec.AD_DFR_END10, TPRec.AD_DFR_END11, TPRec.AD_DFR_END12, TPRec.AD_DFR_END13, TPRec.AD_DFR_END14, TPRec.AD_DFR_END15, TPRec.IF_OPS_SCL_RPT1, TPRec.IF_OPS_SCL_RPT2, TPRec.IF_OPS_SCL_RPT3, _
        TPRec.IF_OPS_SCL_RPT4, TPRec.IF_OPS_SCL_RPT5, TPRec.IF_OPS_SCL_RPT6, TPRec.IF_OPS_SCL_RPT7, TPRec.IF_OPS_SCL_RPT8, TPRec.IF_OPS_SCL_RPT9, TPRec.IF_OPS_SCL_RPT10, TPRec.IF_OPS_SCL_RPT11, TPRec.IF_OPS_SCL_RPT12, TPRec.IF_OPS_SCL_RPT13, TPRec.IF_OPS_SCL_RPT14, TPRec.IF_OPS_SCL_RPT15, TPRec.LD_ENR_CER1, TPRec.LD_ENR_CER2, TPRec.LD_ENR_CER3, TPRec.LD_ENR_CER4, TPRec.LD_ENR_CER5, TPRec.LD_ENR_CER6, TPRec.LD_ENR_CER7, TPRec.LD_ENR_CER8, TPRec.LD_ENR_CER9, TPRec.LD_ENR_CER10, TPRec.LD_ENR_CER11, TPRec.LD_ENR_CER12, TPRec.LD_ENR_CER13, TPRec.LD_ENR_CER14, TPRec.LD_ENR_CER15, _
        TPRec.STU_SSN, TPRec.STU_DM_PRS_1, TPRec.STU_DM_PRS_MID, TPRec.STU_DM_PRS_LST, TPRec.STU_DD_BRT, TPRec.STU_DX_STR_ADR_1, TPRec.STU_DX_STR_ADR_2, TPRec.STU_DM_CT, TPRec.STU_DC_DOM_ST, TPRec.STU_DF_ZIP, TPRec.STU_DM_FGN_CNY, TPRec.STU_DI_VLD_ADR, TPRec.STU_DN_PHN, TPRec.STU_DI_PHN_VLD, TPRec.STU_DN_ALT_PHN, TPRec.STU_DI_ALT_PHN_VLD, _
        TPRec.EDSR_SSN, TPRec.EDSR_DM_PRS_1, TPRec.EDSR_DM_PRS_MID, TPRec.EDSR_DM_PRS_LST, TPRec.EDSR_DD_BRT, TPRec.EDSR_DX_STR_ADR_1, TPRec.EDSR_DX_STR_ADR_2, TPRec.EDSR_DM_CT, TPRec.EDSR_DC_DOM_ST, TPRec.EDSR_DF_ZIP, TPRec.EDSR_DM_FGN_CNY, TPRec.EDSR_DI_VLD_ADR, TPRec.EDSR_DN_PHN, TPRec.EDSR_DI_PHN_VLD, TPRec.EDSR_DN_ALT_PHN, TPRec.EDSR_DI_ALT_PHN_VLD, TPRec.AC_EDS_TYP, _
        TPRec.REF_IND, TPRec.BM_RFR_1_1, TPRec.BM_RFR_MID_1, TPRec.BM_RFR_LST_1, TPRec.BX_RFR_STR_ADR_1_1, TPRec.BX_RFR_STR_ADR_2_1, TPRec.BM_RFR_CT_1, TPRec.BC_RFR_ST_1, TPRec.BF_RFR_ZIP_1, TPRec.BM_RFR_FGN_CNY_1, TPRec.BI_VLD_ADR_1, TPRec.BN_RFR_DOM_PHN_1, TPRec.BI_DOM_PHN_VLD_1, TPRec.BN_RFR_ALT_PHN_1, TPRec.BI_ALT_PHN_VLD_1, TPRec.BC_RFR_REL_BR_1, _
        TPRec.BM_RFR_1_2, TPRec.BM_RFR_MID_2, TPRec.BM_RFR_LST_2, TPRec.BX_RFR_STR_ADR_1_2, TPRec.BX_RFR_STR_ADR_2_2, TPRec.BM_RFR_CT_2, TPRec.BC_RFR_ST_2, TPRec.BF_RFR_ZIP_2, TPRec.BM_RFR_FGN_CNY_2, TPRec.BI_VLD_ADR_2, TPRec.BN_RFR_DOM_PHN_2, TPRec.BI_DOM_PHN_VLD_2, TPRec.BN_RFR_ALT_PHN_2, TPRec.BI_ALT_PHN_VLD_2, TPRec.BC_RFR_REL_BR_2, _
        TPRec.BondID, TPRec.AVE_REHB_PAY_AMT, TPRec.RPY_LETTER_DT, TPRec.RPY_INIT_BAL, TPRec.RPY_INT_RATE, TPRec.RPY_PAY_AMT, TPRec.RPY_PAY_TERM, TPRec.RPY_FIRST_DUE_DT, TPRec.TOT_INT_TO_REPAY, TPRec.TOT_REPAY_AMT, TPRec.RPY_LAST_PAY_DT, TPRec.FIT_INT_AMT, TPRec.FIT_FEE_AMT, TPRec.STH_STATUS, TPRec.STH_EFF_DT, TPRec.CACTUS_ID, TPRec.RPY_NEXT_DUE_DT, _
        TPRec.RPY_OVERPAY, TPRec.INT_ACCRUE_START_DT, TPRec.BAT_ID, TPRec.BAT_BR_CT, TPRec.BAT_LN_CT, TPRec.BAT_TOT_SUM, TPRec.BAT_ITR, TPRec.BAT_FEE

    Loop
    'create new borrower or loan for current borrower as needed
    If LastSSNProcessed <> TPRec.BF_SSN Then
        'create new borrower
        LoadTILPDat Borrs(UBound(Borrs)), TPRec, True, FIP
    End If
End Function

Private Sub LoadTILPDat(ByRef Borr As Borrower, Rec As TILPRec, LoadBorrLvlDat As Boolean, FIP As String)
    Dim UseCOMPASSDeferInfo As Boolean
    Dim DeferementBeginDt As String
    If LoadBorrLvlDat Then
        'borrower level data
        'borrower
        Borr.BAddr1 = Rec.DX_STR_ADR_1
        Borr.BAddr2 = Rec.DX_STR_ADR_2
        If UCase(Rec.STH_STATUS) = "SK" Then
            Borr.BAddrInd = "N"
            Borr.BPhoneInd = "N"
        Else
            Borr.BAddrInd = "Y"
            Borr.BPhoneInd = "Y"
        End If
        Borr.BCity = Rec.DM_CT
        Borr.BDOB = Rec.DD_BRT
        Borr.BFirstName = Rec.DM_PRS_1
        Borr.BLastName = Rec.DM_PRS_LST
        If Rec.DM_PRS_MID <> "" Then Borr.BMID = MID(Rec.DM_PRS_MID, 1, 1)
        Borr.BPhone = Rec.DN_PHN
        Borr.BPhoneSource = "04"
        Borr.BSSN = Rec.BF_SSN
        Borr.BState = Rec.DC_DOM_ST
        Borr.BZip = Rec.DF_ZIP
        'ref1
        Borr.R1Addr1 = Rec.BX_RFR_STR_ADR_1_1
        Borr.R1Addr2 = Rec.BX_RFR_STR_ADR_2_1
        Borr.R1AddrInd = Rec.BI_VLD_ADR_1
        Borr.R1City = Rec.BM_RFR_CT_1
        Borr.R1FirstName = Rec.BM_RFR_1_1
        Borr.R1LastName = Rec.BM_RFR_LST_1
        Borr.R1MID = Rec.BM_RFR_MID_1
        Borr.R1Phone = Rec.BN_RFR_DOM_PHN_1
        Borr.R1PhoneInd = Rec.BI_DOM_PHN_VLD_1
        Borr.R1Relation = Rec.BC_RFR_REL_BR_1
        Borr.R1State = Rec.BC_RFR_ST_1
        Borr.R1Zip = Rec.BF_RFR_ZIP_1
        'ref2
        Borr.R2Addr1 = Rec.BX_RFR_STR_ADR_1_2
        Borr.R2Addr2 = Rec.BX_RFR_STR_ADR_2_2
        Borr.R2AddrInd = Rec.BI_VLD_ADR_2
        Borr.R2City = Rec.BM_RFR_CT_2
        Borr.R2FirstName = Rec.BM_RFR_1_2
        Borr.R2LastName = Rec.BM_RFR_LST_2
        Borr.R2MID = Rec.BM_RFR_MID_2
        Borr.R2Phone = Rec.BN_RFR_DOM_PHN_2
        Borr.R2PhoneInd = Rec.BI_DOM_PHN_VLD_2
        Borr.R2Relation = Rec.BC_RFR_REL_BR_2
        Borr.R2State = Rec.BC_RFR_ST_2
        Borr.R2Zip = Rec.BF_RFR_ZIP_2
        Borr.TILPOnlyFees = Rec.FIT_FEE_AMT
        Borr.TILPOnlyInterest = Rec.BAT_ITR
        Borr.TILPOnlysth_end_date = Rec.LD_LFT_SCL
        If Rec.INT_ACCRUE_START_DT = "" Then
            Borr.TILPOnlyIntAccrueStartDt = Format(CDate(Rec.LD_TRX_EFF), "MM/DD/YYYY")
        ElseIf CDate(Rec.INT_ACCRUE_START_DT) > Date Then
            Borr.TILPOnlyIntAccrueStartDt = Format(CDate(Rec.LD_TRX_EFF), "MM/DD/YYYY")
        ElseIf Val(Rec.BAT_ITR) = 0 Then
            Borr.TILPOnlyIntAccrueStartDt = Format(CDate(Rec.LD_TRX_EFF), "MM/DD/YYYY")
        Else
            Borr.TILPOnlyIntAccrueStartDt = Rec.INT_ACCRUE_START_DT
        End If
        Borr.TILPOnlyRpy_init_bal = Rec.RPY_INIT_BAL
        Borr.TILPOnlyRpy_first_due_dt = Rec.RPY_FIRST_DUE_DT
        Borr.TILPOnlyRpy_pay_term = Rec.RPY_PAY_TERM
        Borr.TILPOnlyRpy_pay_amt = Rec.RPY_PAY_AMT
        Borr.TILPOnlyRpy_next_due_dt = Rec.RPY_NEXT_DUE_DT
        Borr.TILPOnlyFIT_INT_AMT = Rec.FIT_INT_AMT
        Borr.TILPOnlyFileCreateDate = Rec.LD_TRX_EFF
        Borr.TILPOnlyLD_LFT_SCL = Rec.LD_LFT_SCL
        If Rec.LD_LFT_SCL <> "" And Rec.STH_EFF_DT <> "" Then
            If Rec.STH_STATUS = "CT" And CDate(Rec.LD_LFT_SCL) > CDate(Rec.STH_EFF_DT) Then Borr.TILPOnlyLD_LFT_SCLIsGTsth_eff_dt = True
        End If
        Borr.TILPOnlyBatchLvlFees = Rec.BAT_FEE
    End If
    'loan lvl data
    Borr.loans(UBound(Borr.loans)).BondID = Rec.BondID
    Borr.loans(UBound(Borr.loans)).Guarantor = "I00059"
    Borr.loans(UBound(Borr.loans)).AC_LON_TYP = Rec.AC_LON_TYP
    'loan prg
    Borr.loans(UBound(Borr.loans)).LoanPrg = "TILP"
    Borr.loans(UBound(Borr.loans)).OrigLender = "971357"
    Borr.loans(UBound(Borr.loans)).RepType = ""
    If Rec.AD_BR_SIG <> "" Then Borr.loans(UBound(Borr.loans)).AppRcvdDt = Rec.AD_APL_RCV
    Borr.loans(UBound(Borr.loans)).DtNoteSigned = Rec.AD_APL_RCV
    Borr.loans(UBound(Borr.loans)).DtGuaranteed = ""
    Borr.loans(UBound(Borr.loans)).AmountGuaranteed = Rec.AA_GTE_LON_AMT
    Borr.loans(UBound(Borr.loans)).clid = ""
    If Rec.AD_IST_TRM_END = "" Then
        Borr.loans(UBound(Borr.loans)).TermBg = ""
        Borr.loans(UBound(Borr.loans)).TermEd = ""
    Else
        If UCase(Rec.AD_IST_TRM_BEG) = "FALL" Then
            Borr.loans(UBound(Borr.loans)).TermBg = "08/25/" & Right(Rec.AD_IST_TRM_END, 2)
            Borr.loans(UBound(Borr.loans)).TermEd = "12/15/" & Right(Rec.AD_IST_TRM_END, 2)
        ElseIf UCase(Rec.AD_IST_TRM_BEG) = "WINTER" Or UCase(Rec.AD_IST_TRM_BEG) = "SPRING" Then
            Borr.loans(UBound(Borr.loans)).TermBg = "01/03/" & Right(Rec.AD_IST_TRM_END, 2)
            Borr.loans(UBound(Borr.loans)).TermEd = "04/30/" & Right(Rec.AD_IST_TRM_END, 2)
        ElseIf UCase(Rec.AD_IST_TRM_BEG) = "SUMMER" Or UCase(Rec.AD_IST_TRM_BEG) = "2ND SUMMER" Then
            Borr.loans(UBound(Borr.loans)).TermBg = "05/15/" & Right(Rec.AD_IST_TRM_END, 2)
            Borr.loans(UBound(Borr.loans)).TermEd = "08/10/" & Right(Rec.AD_IST_TRM_END, 2)
        ElseIf isnum(MID(Rec.AD_IST_TRM_BEG, 1, 2)) And isnum(MID(Rec.AD_IST_TRM_BEG, 4, 2)) And MID(Rec.AD_IST_TRM_BEG, 3, 1) = "/" And MID(Rec.AD_IST_TRM_BEG, 6, 1) = "/" Then
            'is already a date format
            Borr.loans(UBound(Borr.loans)).TermBg = Rec.AD_IST_TRM_BEG
            Borr.loans(UBound(Borr.loans)).TermEd = Rec.AD_IST_TRM_END
        Else
            MsgBox "Unexpected value for term determining value."
            End
        End If
    End If
    'org school
    Borr.loans(UBound(Borr.loans)).OrigSchool = TILPSchoolCodeTranslation(Rec.AF_APL_OPS_SCL)
    Borr.loans(UBound(Borr.loans)).SubsidyCd = "N"
    Borr.loans(UBound(Borr.loans)).SpallElig = "N"
    'current school
    Borr.loans(UBound(Borr.loans)).CurrSchool = TILPSchoolCodeTranslation(Rec.AF_APL_OPS_SCL)
    Borr.loans(UBound(Borr.loans)).SchSepDate = Rec.LD_LFT_SCL
    'sep reason
    If UCase(Rec.STH_STATUS) = "ENR" Or UCase(Rec.LC_STU_ENR_TYP) = "F" Then
        Borr.loans(UBound(Borr.loans)).SepReason = "11"
    ElseIf UCase(Rec.LC_STU_ENR_TYP) = "H" Then
        Borr.loans(UBound(Borr.loans)).SepReason = "10"
    ElseIf UCase(Rec.STH_STATUS) = "GG" Or UCase(Rec.STH_STATUS) = "CT" Then
        Borr.loans(UBound(Borr.loans)).SepReason = "01"
    ElseIf UCase(Rec.STH_STATUS) = "COE" Or UCase(Rec.STH_STATUS) = "PAY" Or UCase(Rec.STH_STATUS) = "DEF" Or UCase(Rec.STH_STATUS) = "DEL" Or UCase(Rec.STH_STATUS) = "BNK" Or UCase(Rec.STH_STATUS) = "SK" Then
        If UCase(Rec.STH_STATUS) = "BNK" And Borr.TILPOnlyBNKLnStatusFound = False Then
            Borr.TILPOnlyBNKLnStatusFound = True
            Add2QBuilderFile Rec.BF_SSN
        End If
        Borr.loans(UBound(Borr.loans)).SepReason = "02"
    ElseIf UCase(Rec.STH_STATUS) = "LA" Then
        Borr.loans(UBound(Borr.loans)).SepReason = "05"
    ElseIf UCase(Rec.STH_STATUS) = "PEN" Then
        Borr.loans(UBound(Borr.loans)).SepReason = ""
    Else
        MsgBox "Unexpected Value For Sep Reason."
        End
    End If
    Borr.loans(UBound(Borr.loans)).SepSource = "SL"
    Borr.loans(UBound(Borr.loans)).CertDt = ""
    Borr.loans(UBound(Borr.loans)).NtfRecdDt = ""
    Borr.loans(UBound(Borr.loans)).GraceMonth = "02"
    If Rec.LD_LFT_SCL <> "" Then Borr.loans(UBound(Borr.loans)).GracePeriodEnd = DateAdd("m", 2, CDate(Rec.LD_LFT_SCL))
    Borr.loans(UBound(Borr.loans)).RepayStartDt = ""
    'int rate
    Borr.loans(UBound(Borr.loans)).IntRate = "7.000"
    'int type
    Borr.loans(UBound(Borr.loans)).IntType = "F1"
    Borr.loans(UBound(Borr.loans)).SpecRate = "N"
    'financial info
    Borr.loans(UBound(Borr.loans)).TotIndbt = ""
    Borr.loans(UBound(Borr.loans)).Principal = Rec.LA_TRX
    Borr.loans(UBound(Borr.loans)).IntLastCapped = DateAdd("d", -1, CDate(Rec.LD_TRX_EFF))
    'endorser info
    Borr.loans(UBound(Borr.loans)).AC_EDS_TYP = Rec.AC_EDS_TYP
    Borr.loans(UBound(Borr.loans)).EAddr1 = Rec.EDSR_DX_STR_ADR_1
    Borr.loans(UBound(Borr.loans)).EAddr2 = Rec.EDSR_DX_STR_ADR_2
    Borr.loans(UBound(Borr.loans)).EAddrInd = Rec.EDSR_DI_VLD_ADR
    Borr.loans(UBound(Borr.loans)).EAltPhone = Rec.EDSR_DN_ALT_PHN
    Borr.loans(UBound(Borr.loans)).EAltPhoneInd = Rec.EDSR_DI_ALT_PHN_VLD
    Borr.loans(UBound(Borr.loans)).ECity = Rec.EDSR_DM_CT
    Borr.loans(UBound(Borr.loans)).ECountry = Rec.EDSR_DM_FGN_CNY
    Borr.loans(UBound(Borr.loans)).EDOB = Rec.EDSR_DD_BRT
    Borr.loans(UBound(Borr.loans)).EFirstName = Rec.EDSR_DM_PRS_1
    Borr.loans(UBound(Borr.loans)).ELastName = Rec.EDSR_DM_PRS_LST
    Borr.loans(UBound(Borr.loans)).EMID = Rec.EDSR_DM_PRS_MID
    Borr.loans(UBound(Borr.loans)).EPhone = Rec.EDSR_DN_PHN
    Borr.loans(UBound(Borr.loans)).EPhoneInd = Rec.EDSR_DI_PHN_VLD
    Borr.loans(UBound(Borr.loans)).ESSN = Rec.EDSR_SSN
    Borr.loans(UBound(Borr.loans)).EState = Rec.EDSR_DC_DOM_ST
    Borr.loans(UBound(Borr.loans)).EZip = Rec.EDSR_DF_ZIP
    'student info
    Borr.loans(UBound(Borr.loans)).SAddr1 = Rec.STU_DX_STR_ADR_1
    Borr.loans(UBound(Borr.loans)).SAddr2 = Rec.STU_DX_STR_ADR_2
    Borr.loans(UBound(Borr.loans)).SAddrInd = Rec.STU_DI_VLD_ADR
    Borr.loans(UBound(Borr.loans)).SAltPhone = Rec.STU_DN_ALT_PHN
    Borr.loans(UBound(Borr.loans)).SAltPhoneInd = Rec.STU_DI_ALT_PHN_VLD
    Borr.loans(UBound(Borr.loans)).SCity = Rec.STU_DM_CT
    Borr.loans(UBound(Borr.loans)).SCountry = Rec.STU_DM_FGN_CNY
    Borr.loans(UBound(Borr.loans)).SDOB = Rec.STU_DD_BRT
    Borr.loans(UBound(Borr.loans)).SFirstName = Rec.STU_DM_PRS_1
    Borr.loans(UBound(Borr.loans)).SLastName = Rec.STU_DM_PRS_LST
    Borr.loans(UBound(Borr.loans)).SMID = Rec.STU_DM_PRS_MID
    Borr.loans(UBound(Borr.loans)).SPhone = Rec.STU_DN_PHN
    Borr.loans(UBound(Borr.loans)).SPhoneInd = Rec.STU_DI_PHN_VLD
    Borr.loans(UBound(Borr.loans)).SSSN = Rec.STU_SSN
    Borr.loans(UBound(Borr.loans)).SState = Rec.STU_DC_DOM_ST
    Borr.loans(UBound(Borr.loans)).SZip = Rec.STU_DF_ZIP
    'disbursment information
    ReDim Borr.loans(UBound(Borr.loans)).Disbursements(0)
    'check if out of school date is less than any disbursement date
    If Rec.LD_LFT_SCL <> "" Then
        If CDate(Rec.LD_LFT_SCL) < CDate(Rec.AD_PRC) Then
            Borr.TILPOnlyLD_LFT_SCLIsLTAnyDisbDt = True
        End If
    End If
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbDate = Rec.AD_PRC
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAmt = Rec.AA_GTE_LON_AMT
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbMedium = "8"
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelAmt = ""
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).GTE = Rec.GTE_1
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbAdjCode = Rec.AC_DISB_1
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelDt = ""
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).DisbCancelReason = ""
    Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements)).OFee = Rec.ORG_1
    ReDim Preserve Borr.loans(UBound(Borr.loans)).Disbursements(UBound(Borr.loans(UBound(Borr.loans)).Disbursements) + 1)
    
    'deferment info
    ReDim Borr.loans(UBound(Borr.loans)).Deferments(0)
    'TILP Info was placed in COMPASS spots
    'Deferments
    'do GG deferment functionality
    If Rec.STH_STATUS = "GG" Then
        TILPDefermentProc Borr, "GG", Rec.STH_EFF_DT, "GG", "GG", "GG", "GG", "GG", "GG", Borr.loans(UBound(Borr.loans)).GracePeriodEnd
        'figure out which date was used for the original deferment added for GG status
        If CDate(Rec.STH_EFF_DT) < CDate(Borr.loans(UBound(Borr.loans)).GracePeriodEnd) Then
            DeferementBeginDt = Borr.loans(UBound(Borr.loans)).GracePeriodEnd
        Else
            DeferementBeginDt = Rec.STH_EFF_DT
        End If
        'if the deferement added above doesn't cover the current date then add another deferment
        If DateAdd("m", 12, CDate(DeferementBeginDt)) < Date Then
            TILPDefermentProc Borr, "GG", CStr(DateAdd("d", 1, DateAdd("m", 12, CDate(Rec.STH_EFF_DT)))), "GG", "GG", "GG", "GG", "GG", "GG", CStr(DateAdd("d", 1, DateAdd("m", 12, CDate(Borr.loans(UBound(Borr.loans)).GracePeriodEnd))))
        End If
    End If
    'create currently teaching deferement if teacher is in a currently teaching status
    If Rec.STH_STATUS = "CT" Then TILPDefermentProc Borr, "CT", Rec.STH_EFF_DT, "CT", "CT", "CT", Rec.AF_APL_OPS_SCL, "CT", "CT", Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG1 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG1, Rec.LD_DFR_END1, Rec.LC_DFR_TYP1, Rec.LF_DOE_SCL_DFR1, Rec.LD_DFR_INF_CER1, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG2 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG2, Rec.LD_DFR_END2, Rec.LC_DFR_TYP2, Rec.LF_DOE_SCL_DFR2, Rec.LD_DFR_INF_CER2, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG3 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG3, Rec.LD_DFR_END3, Rec.LC_DFR_TYP3, Rec.LF_DOE_SCL_DFR3, Rec.LD_DFR_INF_CER3, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG4 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG4, Rec.LD_DFR_END4, Rec.LC_DFR_TYP4, Rec.LF_DOE_SCL_DFR4, Rec.LD_DFR_INF_CER4, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG5 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG5, Rec.LD_DFR_END5, Rec.LC_DFR_TYP5, Rec.LF_DOE_SCL_DFR5, Rec.LD_DFR_INF_CER5, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG6 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG6, Rec.LD_DFR_END6, Rec.LC_DFR_TYP6, Rec.LF_DOE_SCL_DFR6, Rec.LD_DFR_INF_CER6, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG7 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG7, Rec.LD_DFR_END7, Rec.LC_DFR_TYP7, Rec.LF_DOE_SCL_DFR7, Rec.LD_DFR_INF_CER7, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG8 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG8, Rec.LD_DFR_END8, Rec.LC_DFR_TYP8, Rec.LF_DOE_SCL_DFR8, Rec.LD_DFR_INF_CER8, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG9 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG9, Rec.LD_DFR_END9, Rec.LC_DFR_TYP9, Rec.LF_DOE_SCL_DFR9, Rec.LD_DFR_INF_CER9, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG10 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG10, Rec.LD_DFR_END10, Rec.LC_DFR_TYP10, Rec.LF_DOE_SCL_DFR10, Rec.LD_DFR_INF_CER10, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG11 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG11, Rec.LD_DFR_END11, Rec.LC_DFR_TYP11, Rec.LF_DOE_SCL_DFR11, Rec.LD_DFR_INF_CER11, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG12 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG12, Rec.LD_DFR_END12, Rec.LC_DFR_TYP12, Rec.LF_DOE_SCL_DFR12, Rec.LD_DFR_INF_CER12, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG13 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG13, Rec.LD_DFR_END13, Rec.LC_DFR_TYP13, Rec.LF_DOE_SCL_DFR13, Rec.LD_DFR_INF_CER13, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG14 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG14, Rec.LD_DFR_END14, Rec.LC_DFR_TYP14, Rec.LF_DOE_SCL_DFR14, Rec.LD_DFR_INF_CER14, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    If Rec.LD_DFR_BEG15 <> "" Then TILPDefermentProc Borr, "Non-CT", "Non-CT", Rec.LD_DFR_BEG15, Rec.LD_DFR_END15, Rec.LC_DFR_TYP15, Rec.LF_DOE_SCL_DFR15, Rec.LD_DFR_INF_CER15, Rec.LD_LFT_SCL, Borr.loans(UBound(Borr.loans)).GracePeriodEnd
    'create another loan
    ReDim Preserve Borr.loans(UBound(Borr.loans) + 1)
End Sub

'makes translation from TILP system deferment codes to out current system codes
Private Function TILPDefermentTypeTranslation(FT As String) As String
    If UCase(FT) = "DE" Or UCase(FT) = "RS" Or UCase(FT) = "FH" Or UCase(FT) = "OT" Or UCase(FT) = "SR" Or UCase(FT) = "SC" Then
        TILPDefermentTypeTranslation = "29"
    ElseIf UCase(FT) = "IL" Then
        TILPDefermentTypeTranslation = "02"
    ElseIf UCase(FT) = "IS" Then
        TILPDefermentTypeTranslation = "15"
    ElseIf UCase(FT) = "MI" Then
        TILPDefermentTypeTranslation = "38"
    Else
        MsgBox "Unexpected value for deferment type."
        End
    End If
End Function

'makes translation from TILP system codes to our current system codes
Private Function TILPSchoolCodeTranslation(FT As String) As String
    If isnum(FT) Then
        TILPSchoolCodeTranslation = FT
        Exit Function
    End If
    Select Case UCase(FT)
        Case "UU"
            TILPSchoolCodeTranslation = "00367500"
        Case "BYU"
            TILPSchoolCodeTranslation = "00367000"
        Case "SUU"
            TILPSchoolCodeTranslation = "00367800"
        Case "SLCC"
            TILPSchoolCodeTranslation = "00522000"
        Case "USU"
            TILPSchoolCodeTranslation = "00367700"
        Case "WSU"
            TILPSchoolCodeTranslation = "00368000"
        Case "CEU"
            TILPSchoolCodeTranslation = "00367600"
        Case "WEST"
            TILPSchoolCodeTranslation = "00368100"
        Case "DSC"
            TILPSchoolCodeTranslation = "00367100"
        Case "SNOW"
            TILPSchoolCodeTranslation = "00367900"
        Case "UVSC"
            TILPSchoolCodeTranslation = "00402700"
        Case "UOP"
            TILPSchoolCodeTranslation = "02098805"
        Case Else
            MsgBox "Unexpected value for school ID value."
            End
        End Select
End Function

'handles loading of the deferment variables
Private Sub TILPDefermentProc(Borr As Borrower, STH_STATUS As String, STH_EFF_DT As String, LD_DFR_BEG As String, LD_DFR_END As String, LC_DFR_TYP As String, LF_DOE_SCL_DFR As String, LD_DFR_INF_CER As String, LD_LFT_SCL As String, GraceEnd As String)
    If UCase(STH_STATUS) = "CT" Or UCase(STH_STATUS) = "GG" Then
        If CDate(STH_EFF_DT) < CDate(GraceEnd) Then
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = CStr(DateAdd("d", 1, CDate(GraceEnd)))
        Else
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = STH_EFF_DT
        End If
        If UCase(STH_STATUS) = "CT" Then
            'CT status
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = "06/30/" & Format(DateAdd("yyyy", 1, Date), "YYYY")
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "06"
        Else
            'GG status
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = DateAdd("m", 12, CDate(Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate))
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = "33"
        End If
    Else
        If GraceEnd <> "" Then
            If CDate(LD_DFR_BEG) < CDate(GraceEnd) Then
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = Format(DateAdd("d", 1, GraceEnd), "MM/DD/YYYY")
            Else
                Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = LD_DFR_BEG
            End If
        Else
            Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate = LD_DFR_BEG
        End If
        If LD_DFR_END = "" Then
            'if end date is blank then calculate the end date as 12 months - 1 day from the begin date
            LD_DFR_END = Format(DateAdd("m", 12, DateAdd("d", -1, CDate(LD_DFR_BEG))), "MM/DD/YYYY")
        End If
        Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate = LD_DFR_END
        Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).Type = TILPDefermentTypeTranslation(LC_DFR_TYP)
        Borr.TILPOnlyDefExists = True
        If LD_LFT_SCL <> "" Then
            If Borr.TILPOnlyLD_LFT_SCLIsGTLD_DFR_BEG = False Then Borr.TILPOnlyLD_LFT_SCLIsGTLD_DFR_BEG = (CDate(LD_LFT_SCL) > CDate(LD_DFR_BEG))
        End If
    End If
    If UCase(LC_DFR_TYP) = "IS" Then
        Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = TILPSchoolCodeTranslation(LF_DOE_SCL_DFR)
        Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CertDate = LD_DFR_INF_CER
    ElseIf UCase(LC_DFR_TYP) = "CT" Then
        Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).SchoolCode = TILPSchoolCodeTranslation(LF_DOE_SCL_DFR)
    End If
    Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).CapInt = "N"
    'check if deferement start date is greater than the deferement end date if yes then don't add the deferement
    If CDate(Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).BeginDate) < CDate(Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments)).EndDate) Then
        ReDim Preserve Borr.loans(UBound(Borr.loans)).Deferments(UBound(Borr.loans(UBound(Borr.loans)).Deferments) + 1)
    End If
End Sub

'this sub enters financial information for the TILP file
Private Sub EnterTILPLoanFinancialInformation(PC As Integer, LC As Integer)
    Dim i As Integer
    If LC = (UBound(Borrs(PC).loans) - 1) Then 'only do for last loan
        ModFastPath "ATA06" & Borrs(PC).BSSN
        puttext 10, 13, Borrs(PC).loans(LC).Principal 'principal amount
        'only enter late fees if repayment information exists
        If Borrs(PC).TILPOnlyRpy_init_bal <> "" Then
            If Val(Borrs(PC).TILPOnlyFees) > 0 Then puttext 11, 17, Borrs(PC).TILPOnlyFees 'late fees
        End If
        If Val(Borrs(PC).TILPOnlyInterest) = 0 Then
            puttext 17, 32, "0.00" 'interest
        Else
            puttext 17, 32, Borrs(PC).TILPOnlyFIT_INT_AMT 'interest
        End If
        puttext 17, 48, Format(CDate(Borrs(PC).TILPOnlyIntAccrueStartDt), "MMDDYY") 'interest accrue start date
        puttext 17, 67, Format(CDate(Borrs(PC).TILPOnlyFileCreateDate), "MMDDYY") 'accrued thru date
        Hit "Enter"
        Hit "F10"
        i = 12
        'relate all loans to financial information
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            puttext i, 7, "Y"
            i = i + 1
            If GetText(i, 7, 1) = "" Then
                i = 12
                Hit "Enter" 'commit changes
                Hit "F8"
            End If
        Wend
    End If
End Sub

'does TILP repayment schedule processing
Private Sub TILPRepaymentSchedule(PC As Integer)
    Dim i As Integer
    If Borrs(PC).TILPOnlyRpy_init_bal = "" Then
        Exit Sub
    End If
    ModFastPath "ATA08" & Borrs(PC).BSSN
    puttext 10, 43, "N"
    If DateAdd("d", -45, CDate(Borrs(PC).TILPOnlyRpy_first_due_dt)) > Date Then
        puttext 11, 13, Format(DateAdd("d", -1, Date), "MMDDYY")
    Else
        puttext 11, 13, Format(DateAdd("d", -45, CDate(Borrs(PC).TILPOnlyRpy_first_due_dt)), "MMDDYY")
    End If
    puttext 11, 71, Format(CDate(Borrs(PC).TILPOnlyRpy_first_due_dt), "MM") & Format(CDate(Borrs(PC).TILPOnlyRpy_next_due_dt), "DD") & Format(CDate(Borrs(PC).TILPOnlyRpy_first_due_dt), "YY")
    puttext 14, 13, Borrs(PC).TILPOnlyRpy_init_bal
    puttext 14, 67, CStr((Val(Borrs(PC).TILPOnlyRpy_pay_term) * Val(Borrs(PC).TILPOnlyRpy_pay_amt)) - Val(Borrs(PC).TILPOnlyRpy_init_bal))
    puttext 15, 22, CStr(Val(Borrs(PC).TILPOnlyRpy_pay_term) * Val(Borrs(PC).TILPOnlyRpy_pay_amt))
    puttext 15, 52, "7.000"
    puttext 16, 17, "L"
    puttext 19, 19, Borrs(PC).TILPOnlyRpy_pay_term
    puttext 19, 26, Borrs(PC).TILPOnlyRpy_pay_amt
    Hit "Enter"
    Hit "F10"
    i = 12
    'relate all loans to financial information
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        puttext i, 7, "Y"
        i = i + 1
        If GetText(i, 7, 1) = "" Then
            i = 12
            Hit "Enter" 'commit changes
            Hit "F8"
        End If
    Wend
    TILPBillingSchedule PC
End Sub

'does TILP billing schedule processing
Private Sub TILPBillingSchedule(PC As Integer)
    Dim i As Integer
    If Borrs(PC).TILPOnlyRpy_next_due_dt = "" Then
        Exit Sub
    End If
    ModFastPath "ATA0A" & Borrs(PC).BSSN
    puttext 11, 23, Format(CDate(Borrs(PC).TILPOnlyRpy_next_due_dt), "MMDDYY")
    puttext 12, 23, "Y"
    Hit "Enter"
    Hit "F10"
    i = 12
    'relate all loans to financial information
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        puttext i, 6, "Y"
        i = i + 1
        If GetText(i, 6, 1) = "" Then
            i = 12
            Hit "Enter" 'commit changes
            Hit "F8"
        End If
    Wend
End Sub

'this function gets the effective date and Bond ID from a repurchase file
Private Function GetTILPDat(EffDt As String, FIP As String) As String
    Dim TPRec As TILPRec
    Open FTPDir & FIP For Input As #1
    'header row
    Input #1, TPRec.BF_SSN, TPRec.DM_PRS_1, TPRec.DM_PRS_MID, TPRec.DM_PRS_LST, TPRec.DD_BRT, TPRec.DX_STR_ADR_1, TPRec.DX_STR_ADR_2, TPRec.DM_CT, TPRec.DC_DOM_ST, TPRec.DF_ZIP, TPRec.DM_FGN_CNY, TPRec.DI_VLD_ADR, TPRec.DN_PHN, TPRec.DI_PHN_VLD, TPRec.DN_ALT_PHN, TPRec.DI_ALT_PHN_VLD, _
    TPRec.AC_LON_TYP, TPRec.SUBSIDY, TPRec.AD_PRC, TPRec.AF_ORG_APL_OPS_LDR, TPRec.AF_APL_ID, TPRec.AF_APL_ID_SFX, TPRec.AD_IST_TRM_BEG, TPRec.AD_IST_TRM_END, TPRec.AA_GTE_LON_AMT, TPRec.AF_APL_OPS_SCL, TPRec.AD_BR_SIG, TPRec.LD_LFT_SCL, TPRec.PR_RPD_FOR_ITR, TPRec.LC_INT_TYP, TPRec.LD_TRX_EFF, TPRec.LA_TRX, TPRec.IF_OPS_SCL_RPT, TPRec.LC_STU_ENR_TYP, TPRec.LD_ENR_CER, TPRec.LD_LDR_NTF, TPRec.AR_CON_ITR, TPRec.AD_APL_RCV, TPRec.AC_STU_DFR_REQ, _
    TPRec.AN_DISB_1, TPRec.AC_DISB_1, TPRec.AD_DISB_1, TPRec.AA_DISB_1, TPRec.ORG_1, TPRec.CD_DISB_1, TPRec.CA_DISB_1, TPRec.GTE_1, TPRec.AN_DISB_2, TPRec.AC_DISB_2, TPRec.AD_DISB_2, TPRec.AA_DISB_2, TPRec.ORG_2, TPRec.CD_DISB_2, TPRec.CA_DISB_2, TPRec.GTE_2, TPRec.AN_DISB_3, TPRec.AC_DISB_3, TPRec.AD_DISB_3, TPRec.AA_DISB_3, TPRec.ORG_3, TPRec.CD_DISB_3, TPRec.CA_DISB_3, TPRec.GTE_3, TPRec.AN_DISB_4, TPRec.AC_DISB_4, TPRec.AD_DISB_4, TPRec.AA_DISB_4, TPRec.ORG_4, TPRec.CD_DISB_4, TPRec.CA_DISB_4, TPRec.GTE_4, TPRec.AA_TOT_EDU_DET_PNT, TPRec.LC_DFR_TYP1, TPRec.LC_DFR_TYP2, TPRec.LC_DFR_TYP3, TPRec.LC_DFR_TYP4, TPRec.LC_DFR_TYP5, TPRec.LC_DFR_TYP6, TPRec.LC_DFR_TYP7, TPRec.LC_DFR_TYP8, TPRec.LC_DFR_TYP9, TPRec.LC_DFR_TYP10, TPRec.LC_DFR_TYP11, TPRec.LC_DFR_TYP12, TPRec.LC_DFR_TYP13, TPRec.LC_DFR_TYP14, TPRec.LC_DFR_TYP15, TPRec.LD_DFR_BEG1, TPRec.LD_DFR_BEG2, TPRec.LD_DFR_BEG3, TPRec.LD_DFR_BEG4, TPRec.LD_DFR_BEG5, _
    TPRec.LD_DFR_BEG6, TPRec.LD_DFR_BEG7, TPRec.LD_DFR_BEG8, TPRec.LD_DFR_BEG9, TPRec.LD_DFR_BEG10, TPRec.LD_DFR_BEG11, TPRec.LD_DFR_BEG12, TPRec.LD_DFR_BEG13, TPRec.LD_DFR_BEG14, TPRec.LD_DFR_BEG15, TPRec.LD_DFR_END1, TPRec.LD_DFR_END2, TPRec.LD_DFR_END3, TPRec.LD_DFR_END4, TPRec.LD_DFR_END5, TPRec.LD_DFR_END6, TPRec.LD_DFR_END7, TPRec.LD_DFR_END8, TPRec.LD_DFR_END9, TPRec.LD_DFR_END10, TPRec.LD_DFR_END11, TPRec.LD_DFR_END12, TPRec.LD_DFR_END13, TPRec.LD_DFR_END14, TPRec.LD_DFR_END15, TPRec.LF_DOE_SCL_DFR1, TPRec.LF_DOE_SCL_DFR2, TPRec.LF_DOE_SCL_DFR3, TPRec.LF_DOE_SCL_DFR4, TPRec.LF_DOE_SCL_DFR5, TPRec.LF_DOE_SCL_DFR6, TPRec.LF_DOE_SCL_DFR7, TPRec.LF_DOE_SCL_DFR8, TPRec.LF_DOE_SCL_DFR9, TPRec.LF_DOE_SCL_DFR10, TPRec.LF_DOE_SCL_DFR11, TPRec.LF_DOE_SCL_DFR12, TPRec.LF_DOE_SCL_DFR13, TPRec.LF_DOE_SCL_DFR14, TPRec.LF_DOE_SCL_DFR15, TPRec.LD_DFR_INF_CER1, _
    TPRec.LD_DFR_INF_CER2, TPRec.LD_DFR_INF_CER3, TPRec.LD_DFR_INF_CER4, TPRec.LD_DFR_INF_CER5, TPRec.LD_DFR_INF_CER6, TPRec.LD_DFR_INF_CER7, TPRec.LD_DFR_INF_CER8, TPRec.LD_DFR_INF_CER9, TPRec.LD_DFR_INF_CER10, TPRec.LD_DFR_INF_CER11, TPRec.LD_DFR_INF_CER12, TPRec.LD_DFR_INF_CER13, TPRec.LD_DFR_INF_CER14, TPRec.LD_DFR_INF_CER15, TPRec.AC_LON_STA_REA1, TPRec.AC_LON_STA_REA2, TPRec.AC_LON_STA_REA3, TPRec.AC_LON_STA_REA4, TPRec.AC_LON_STA_REA5, TPRec.AC_LON_STA_REA6, TPRec.AC_LON_STA_REA7, TPRec.AC_LON_STA_REA8, TPRec.AC_LON_STA_REA9, TPRec.AC_LON_STA_REA10, TPRec.AC_LON_STA_REA11, TPRec.AC_LON_STA_REA12, TPRec.AC_LON_STA_REA13, TPRec.AC_LON_STA_REA14, _
    TPRec.AC_LON_STA_REA15, TPRec.AD_DFR_BEG1, TPRec.AD_DFR_BEG2, TPRec.AD_DFR_BEG3, TPRec.AD_DFR_BEG4, TPRec.AD_DFR_BEG5, TPRec.AD_DFR_BEG6, TPRec.AD_DFR_BEG7, TPRec.AD_DFR_BEG8, TPRec.AD_DFR_BEG9, TPRec.AD_DFR_BEG10, TPRec.AD_DFR_BEG11, TPRec.AD_DFR_BEG12, TPRec.AD_DFR_BEG13, TPRec.AD_DFR_BEG14, TPRec.AD_DFR_BEG15, TPRec.AD_DFR_END1, TPRec.AD_DFR_END2, TPRec.AD_DFR_END3, TPRec.AD_DFR_END4, TPRec.AD_DFR_END5, TPRec.AD_DFR_END6, TPRec.AD_DFR_END7, TPRec.AD_DFR_END8, TPRec.AD_DFR_END9, TPRec.AD_DFR_END10, TPRec.AD_DFR_END11, TPRec.AD_DFR_END12, TPRec.AD_DFR_END13, TPRec.AD_DFR_END14, TPRec.AD_DFR_END15, TPRec.IF_OPS_SCL_RPT1, TPRec.IF_OPS_SCL_RPT2, TPRec.IF_OPS_SCL_RPT3, _
    TPRec.IF_OPS_SCL_RPT4, TPRec.IF_OPS_SCL_RPT5, TPRec.IF_OPS_SCL_RPT6, TPRec.IF_OPS_SCL_RPT7, TPRec.IF_OPS_SCL_RPT8, TPRec.IF_OPS_SCL_RPT9, TPRec.IF_OPS_SCL_RPT10, TPRec.IF_OPS_SCL_RPT11, TPRec.IF_OPS_SCL_RPT12, TPRec.IF_OPS_SCL_RPT13, TPRec.IF_OPS_SCL_RPT14, TPRec.IF_OPS_SCL_RPT15, TPRec.LD_ENR_CER1, TPRec.LD_ENR_CER2, TPRec.LD_ENR_CER3, TPRec.LD_ENR_CER4, TPRec.LD_ENR_CER5, TPRec.LD_ENR_CER6, TPRec.LD_ENR_CER7, TPRec.LD_ENR_CER8, TPRec.LD_ENR_CER9, TPRec.LD_ENR_CER10, TPRec.LD_ENR_CER11, TPRec.LD_ENR_CER12, TPRec.LD_ENR_CER13, TPRec.LD_ENR_CER14, TPRec.LD_ENR_CER15, _
    TPRec.STU_SSN, TPRec.STU_DM_PRS_1, TPRec.STU_DM_PRS_MID, TPRec.STU_DM_PRS_LST, TPRec.STU_DD_BRT, TPRec.STU_DX_STR_ADR_1, TPRec.STU_DX_STR_ADR_2, TPRec.STU_DM_CT, TPRec.STU_DC_DOM_ST, TPRec.STU_DF_ZIP, TPRec.STU_DM_FGN_CNY, TPRec.STU_DI_VLD_ADR, TPRec.STU_DN_PHN, TPRec.STU_DI_PHN_VLD, TPRec.STU_DN_ALT_PHN, TPRec.STU_DI_ALT_PHN_VLD, _
    TPRec.EDSR_SSN, TPRec.EDSR_DM_PRS_1, TPRec.EDSR_DM_PRS_MID, TPRec.EDSR_DM_PRS_LST, TPRec.EDSR_DD_BRT, TPRec.EDSR_DX_STR_ADR_1, TPRec.EDSR_DX_STR_ADR_2, TPRec.EDSR_DM_CT, TPRec.EDSR_DC_DOM_ST, TPRec.EDSR_DF_ZIP, TPRec.EDSR_DM_FGN_CNY, TPRec.EDSR_DI_VLD_ADR, TPRec.EDSR_DN_PHN, TPRec.EDSR_DI_PHN_VLD, TPRec.EDSR_DN_ALT_PHN, TPRec.EDSR_DI_ALT_PHN_VLD, TPRec.AC_EDS_TYP, _
    TPRec.REF_IND, TPRec.BM_RFR_1_1, TPRec.BM_RFR_MID_1, TPRec.BM_RFR_LST_1, TPRec.BX_RFR_STR_ADR_1_1, TPRec.BX_RFR_STR_ADR_2_1, TPRec.BM_RFR_CT_1, TPRec.BC_RFR_ST_1, TPRec.BF_RFR_ZIP_1, TPRec.BM_RFR_FGN_CNY_1, TPRec.BI_VLD_ADR_1, TPRec.BN_RFR_DOM_PHN_1, TPRec.BI_DOM_PHN_VLD_1, TPRec.BN_RFR_ALT_PHN_1, TPRec.BI_ALT_PHN_VLD_1, TPRec.BC_RFR_REL_BR_1, _
    TPRec.BM_RFR_1_2, TPRec.BM_RFR_MID_2, TPRec.BM_RFR_LST_2, TPRec.BX_RFR_STR_ADR_1_2, TPRec.BX_RFR_STR_ADR_2_2, TPRec.BM_RFR_CT_2, TPRec.BC_RFR_ST_2, TPRec.BF_RFR_ZIP_2, TPRec.BM_RFR_FGN_CNY_2, TPRec.BI_VLD_ADR_2, TPRec.BN_RFR_DOM_PHN_2, TPRec.BI_DOM_PHN_VLD_2, TPRec.BN_RFR_ALT_PHN_2, TPRec.BI_ALT_PHN_VLD_2, TPRec.BC_RFR_REL_BR_2, _
    TPRec.BondID, TPRec.AVE_REHB_PAY_AMT, TPRec.RPY_LETTER_DT, TPRec.RPY_INIT_BAL, TPRec.RPY_INT_RATE, TPRec.RPY_PAY_AMT, TPRec.RPY_PAY_TERM, TPRec.RPY_FIRST_DUE_DT, TPRec.TOT_INT_TO_REPAY, TPRec.TOT_REPAY_AMT, TPRec.RPY_LAST_PAY_DT, TPRec.FIT_INT_AMT, TPRec.FIT_FEE_AMT, TPRec.STH_STATUS, TPRec.STH_EFF_DT, TPRec.CACTUS_ID, TPRec.RPY_NEXT_DUE_DT, _
    TPRec.RPY_OVERPAY, TPRec.INT_ACCRUE_START_DT, TPRec.BAT_ID, TPRec.BAT_BR_CT, TPRec.BAT_LN_CT, TPRec.BAT_TOT_SUM, TPRec.BAT_ITR, TPRec.BAT_FEE
    'get data
    Input #1, TPRec.BF_SSN, TPRec.DM_PRS_1, TPRec.DM_PRS_MID, TPRec.DM_PRS_LST, TPRec.DD_BRT, TPRec.DX_STR_ADR_1, TPRec.DX_STR_ADR_2, TPRec.DM_CT, TPRec.DC_DOM_ST, TPRec.DF_ZIP, TPRec.DM_FGN_CNY, TPRec.DI_VLD_ADR, TPRec.DN_PHN, TPRec.DI_PHN_VLD, TPRec.DN_ALT_PHN, TPRec.DI_ALT_PHN_VLD, _
    TPRec.AC_LON_TYP, TPRec.SUBSIDY, TPRec.AD_PRC, TPRec.AF_ORG_APL_OPS_LDR, TPRec.AF_APL_ID, TPRec.AF_APL_ID_SFX, TPRec.AD_IST_TRM_BEG, TPRec.AD_IST_TRM_END, TPRec.AA_GTE_LON_AMT, TPRec.AF_APL_OPS_SCL, TPRec.AD_BR_SIG, TPRec.LD_LFT_SCL, TPRec.PR_RPD_FOR_ITR, TPRec.LC_INT_TYP, EffDt, TPRec.LA_TRX, TPRec.IF_OPS_SCL_RPT, TPRec.LC_STU_ENR_TYP, TPRec.LD_ENR_CER, TPRec.LD_LDR_NTF, TPRec.AR_CON_ITR, TPRec.AD_APL_RCV, TPRec.AC_STU_DFR_REQ, _
    TPRec.AN_DISB_1, TPRec.AC_DISB_1, TPRec.AD_DISB_1, TPRec.AA_DISB_1, TPRec.ORG_1, TPRec.CD_DISB_1, TPRec.CA_DISB_1, TPRec.GTE_1, TPRec.AN_DISB_2, TPRec.AC_DISB_2, TPRec.AD_DISB_2, TPRec.AA_DISB_2, TPRec.ORG_2, TPRec.CD_DISB_2, TPRec.CA_DISB_2, TPRec.GTE_2, TPRec.AN_DISB_3, TPRec.AC_DISB_3, TPRec.AD_DISB_3, TPRec.AA_DISB_3, TPRec.ORG_3, TPRec.CD_DISB_3, TPRec.CA_DISB_3, TPRec.GTE_3, TPRec.AN_DISB_4, TPRec.AC_DISB_4, TPRec.AD_DISB_4, TPRec.AA_DISB_4, TPRec.ORG_4, TPRec.CD_DISB_4, TPRec.CA_DISB_4, TPRec.GTE_4, TPRec.AA_TOT_EDU_DET_PNT, TPRec.LC_DFR_TYP1, TPRec.LC_DFR_TYP2, TPRec.LC_DFR_TYP3, TPRec.LC_DFR_TYP4, TPRec.LC_DFR_TYP5, TPRec.LC_DFR_TYP6, TPRec.LC_DFR_TYP7, TPRec.LC_DFR_TYP8, TPRec.LC_DFR_TYP9, TPRec.LC_DFR_TYP10, TPRec.LC_DFR_TYP11, TPRec.LC_DFR_TYP12, TPRec.LC_DFR_TYP13, TPRec.LC_DFR_TYP14, TPRec.LC_DFR_TYP15, TPRec.LD_DFR_BEG1, TPRec.LD_DFR_BEG2, TPRec.LD_DFR_BEG3, TPRec.LD_DFR_BEG4, TPRec.LD_DFR_BEG5, _
    TPRec.LD_DFR_BEG6, TPRec.LD_DFR_BEG7, TPRec.LD_DFR_BEG8, TPRec.LD_DFR_BEG9, TPRec.LD_DFR_BEG10, TPRec.LD_DFR_BEG11, TPRec.LD_DFR_BEG12, TPRec.LD_DFR_BEG13, TPRec.LD_DFR_BEG14, TPRec.LD_DFR_BEG15, TPRec.LD_DFR_END1, TPRec.LD_DFR_END2, TPRec.LD_DFR_END3, TPRec.LD_DFR_END4, TPRec.LD_DFR_END5, TPRec.LD_DFR_END6, TPRec.LD_DFR_END7, TPRec.LD_DFR_END8, TPRec.LD_DFR_END9, TPRec.LD_DFR_END10, TPRec.LD_DFR_END11, TPRec.LD_DFR_END12, TPRec.LD_DFR_END13, TPRec.LD_DFR_END14, TPRec.LD_DFR_END15, TPRec.LF_DOE_SCL_DFR1, TPRec.LF_DOE_SCL_DFR2, TPRec.LF_DOE_SCL_DFR3, TPRec.LF_DOE_SCL_DFR4, TPRec.LF_DOE_SCL_DFR5, TPRec.LF_DOE_SCL_DFR6, TPRec.LF_DOE_SCL_DFR7, TPRec.LF_DOE_SCL_DFR8, TPRec.LF_DOE_SCL_DFR9, TPRec.LF_DOE_SCL_DFR10, TPRec.LF_DOE_SCL_DFR11, TPRec.LF_DOE_SCL_DFR12, TPRec.LF_DOE_SCL_DFR13, TPRec.LF_DOE_SCL_DFR14, TPRec.LF_DOE_SCL_DFR15, TPRec.LD_DFR_INF_CER1, _
    TPRec.LD_DFR_INF_CER2, TPRec.LD_DFR_INF_CER3, TPRec.LD_DFR_INF_CER4, TPRec.LD_DFR_INF_CER5, TPRec.LD_DFR_INF_CER6, TPRec.LD_DFR_INF_CER7, TPRec.LD_DFR_INF_CER8, TPRec.LD_DFR_INF_CER9, TPRec.LD_DFR_INF_CER10, TPRec.LD_DFR_INF_CER11, TPRec.LD_DFR_INF_CER12, TPRec.LD_DFR_INF_CER13, TPRec.LD_DFR_INF_CER14, TPRec.LD_DFR_INF_CER15, TPRec.AC_LON_STA_REA1, TPRec.AC_LON_STA_REA2, TPRec.AC_LON_STA_REA3, TPRec.AC_LON_STA_REA4, TPRec.AC_LON_STA_REA5, TPRec.AC_LON_STA_REA6, TPRec.AC_LON_STA_REA7, TPRec.AC_LON_STA_REA8, TPRec.AC_LON_STA_REA9, TPRec.AC_LON_STA_REA10, TPRec.AC_LON_STA_REA11, TPRec.AC_LON_STA_REA12, TPRec.AC_LON_STA_REA13, TPRec.AC_LON_STA_REA14, _
    TPRec.AC_LON_STA_REA15, TPRec.AD_DFR_BEG1, TPRec.AD_DFR_BEG2, TPRec.AD_DFR_BEG3, TPRec.AD_DFR_BEG4, TPRec.AD_DFR_BEG5, TPRec.AD_DFR_BEG6, TPRec.AD_DFR_BEG7, TPRec.AD_DFR_BEG8, TPRec.AD_DFR_BEG9, TPRec.AD_DFR_BEG10, TPRec.AD_DFR_BEG11, TPRec.AD_DFR_BEG12, TPRec.AD_DFR_BEG13, TPRec.AD_DFR_BEG14, TPRec.AD_DFR_BEG15, TPRec.AD_DFR_END1, TPRec.AD_DFR_END2, TPRec.AD_DFR_END3, TPRec.AD_DFR_END4, TPRec.AD_DFR_END5, TPRec.AD_DFR_END6, TPRec.AD_DFR_END7, TPRec.AD_DFR_END8, TPRec.AD_DFR_END9, TPRec.AD_DFR_END10, TPRec.AD_DFR_END11, TPRec.AD_DFR_END12, TPRec.AD_DFR_END13, TPRec.AD_DFR_END14, TPRec.AD_DFR_END15, TPRec.IF_OPS_SCL_RPT1, TPRec.IF_OPS_SCL_RPT2, TPRec.IF_OPS_SCL_RPT3, _
    TPRec.IF_OPS_SCL_RPT4, TPRec.IF_OPS_SCL_RPT5, TPRec.IF_OPS_SCL_RPT6, TPRec.IF_OPS_SCL_RPT7, TPRec.IF_OPS_SCL_RPT8, TPRec.IF_OPS_SCL_RPT9, TPRec.IF_OPS_SCL_RPT10, TPRec.IF_OPS_SCL_RPT11, TPRec.IF_OPS_SCL_RPT12, TPRec.IF_OPS_SCL_RPT13, TPRec.IF_OPS_SCL_RPT14, TPRec.IF_OPS_SCL_RPT15, TPRec.LD_ENR_CER1, TPRec.LD_ENR_CER2, TPRec.LD_ENR_CER3, TPRec.LD_ENR_CER4, TPRec.LD_ENR_CER5, TPRec.LD_ENR_CER6, TPRec.LD_ENR_CER7, TPRec.LD_ENR_CER8, TPRec.LD_ENR_CER9, TPRec.LD_ENR_CER10, TPRec.LD_ENR_CER11, TPRec.LD_ENR_CER12, TPRec.LD_ENR_CER13, TPRec.LD_ENR_CER14, TPRec.LD_ENR_CER15, _
    TPRec.STU_SSN, TPRec.STU_DM_PRS_1, TPRec.STU_DM_PRS_MID, TPRec.STU_DM_PRS_LST, TPRec.STU_DD_BRT, TPRec.STU_DX_STR_ADR_1, TPRec.STU_DX_STR_ADR_2, TPRec.STU_DM_CT, TPRec.STU_DC_DOM_ST, TPRec.STU_DF_ZIP, TPRec.STU_DM_FGN_CNY, TPRec.STU_DI_VLD_ADR, TPRec.STU_DN_PHN, TPRec.STU_DI_PHN_VLD, TPRec.STU_DN_ALT_PHN, TPRec.STU_DI_ALT_PHN_VLD, _
    TPRec.EDSR_SSN, TPRec.EDSR_DM_PRS_1, TPRec.EDSR_DM_PRS_MID, TPRec.EDSR_DM_PRS_LST, TPRec.EDSR_DD_BRT, TPRec.EDSR_DX_STR_ADR_1, TPRec.EDSR_DX_STR_ADR_2, TPRec.EDSR_DM_CT, TPRec.EDSR_DC_DOM_ST, TPRec.EDSR_DF_ZIP, TPRec.EDSR_DM_FGN_CNY, TPRec.EDSR_DI_VLD_ADR, TPRec.EDSR_DN_PHN, TPRec.EDSR_DI_PHN_VLD, TPRec.EDSR_DN_ALT_PHN, TPRec.EDSR_DI_ALT_PHN_VLD, TPRec.AC_EDS_TYP, _
    TPRec.REF_IND, TPRec.BM_RFR_1_1, TPRec.BM_RFR_MID_1, TPRec.BM_RFR_LST_1, TPRec.BX_RFR_STR_ADR_1_1, TPRec.BX_RFR_STR_ADR_2_1, TPRec.BM_RFR_CT_1, TPRec.BC_RFR_ST_1, TPRec.BF_RFR_ZIP_1, TPRec.BM_RFR_FGN_CNY_1, TPRec.BI_VLD_ADR_1, TPRec.BN_RFR_DOM_PHN_1, TPRec.BI_DOM_PHN_VLD_1, TPRec.BN_RFR_ALT_PHN_1, TPRec.BI_ALT_PHN_VLD_1, TPRec.BC_RFR_REL_BR_1, _
    TPRec.BM_RFR_1_2, TPRec.BM_RFR_MID_2, TPRec.BM_RFR_LST_2, TPRec.BX_RFR_STR_ADR_1_2, TPRec.BX_RFR_STR_ADR_2_2, TPRec.BM_RFR_CT_2, TPRec.BC_RFR_ST_2, TPRec.BF_RFR_ZIP_2, TPRec.BM_RFR_FGN_CNY_2, TPRec.BI_VLD_ADR_2, TPRec.BN_RFR_DOM_PHN_2, TPRec.BI_DOM_PHN_VLD_2, TPRec.BN_RFR_ALT_PHN_2, TPRec.BI_ALT_PHN_VLD_2, TPRec.BC_RFR_REL_BR_2, _
    TPRec.BondID, TPRec.AVE_REHB_PAY_AMT, TPRec.RPY_LETTER_DT, TPRec.RPY_INIT_BAL, TPRec.RPY_INT_RATE, TPRec.RPY_PAY_AMT, TPRec.RPY_PAY_TERM, TPRec.RPY_FIRST_DUE_DT, TPRec.TOT_INT_TO_REPAY, TPRec.TOT_REPAY_AMT, TPRec.RPY_LAST_PAY_DT, TPRec.FIT_INT_AMT, TPRec.FIT_FEE_AMT, TPRec.STH_STATUS, TPRec.STH_EFF_DT, TPRec.CACTUS_ID, TPRec.RPY_NEXT_DUE_DT, _
    TPRec.RPY_OVERPAY, TPRec.INT_ACCRUE_START_DT, TPRec.BAT_ID, TPRec.BAT_BR_CT, TPRec.BAT_LN_CT, TPRec.BAT_TOT_SUM, TPRec.BAT_ITR, TPRec.BAT_FEE
    Close #1
End Function

'add SSN to data file for queue builder
Private Sub Add2QBuilderFile(ssn As String)
    Open FTPDir & "TILPBKP.txt" For Append As #33
    Write #33, ssn, "TLP07", "", "", Format(DateAdd("d", 3, Date), "MM/DD/YYYY"), "", "", "", "ALL", "Bankruptcy Info on TILP Loans Needs to be added"
    Close #33
End Sub












