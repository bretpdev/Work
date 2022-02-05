CREATE TABLE [UheInvMet].[Monthly_LoanLevel] (
    [BF_SSN]                  CHAR (9)        NULL,
    [LN_SEQ]                  SMALLINT        NULL,
    [LC_STA_LON10]            CHAR (1)        NULL,
    [LD_STA_LON10]            DATE            NULL,
    [LD_LON_EFF_ADD]          DATE            NULL,
    [LD_LON_ACL_ADD]          DATE            NULL,
    [LD_PIF_RPT]              DATE            NULL,
    [LC_CAM_LON_STA]          VARCHAR (2)     NULL,
    [DX_ADR_EML]              VARCHAR (254)   NULL,
    [IC_LON_PGM]              VARCHAR (2)     NULL,
    [LA_OTS_PRI_ELG]          NUMERIC (12, 2) NULL,
    [WA_TOT_BRI_OTS]          NUMERIC (12, 2) NULL,
    [LN_DLQ_MAX]              NUMERIC (11)    NULL,
    [SPEC_FORB_IND]           VARCHAR (1)     NULL,
    [WC_DW_LON_STA]           CHAR (2)        NULL,
    [ORD]                     INT             NULL,
    [BILL_SATISFIED]          INT             NULL,
    [Segment]                 INT             NULL,
    [BorrSegment]             INT             NULL,
    [LF_LON_CUR_OWN]          VARCHAR (8)     NULL,
    [DefermentIndicator]      INT             NULL,
    [BorrDefermentIndicator]  INT             NULL,
    [PIF_TRN_DT]              DATE            NULL,
    [PerformanceCategory]     VARCHAR (6)     NULL,
    [ActiveMilitaryIndicator] INT             NULL,
    [LoanStatusPriority]      BIGINT          NULL,
    [LoanSegmentPriority]     BIGINT          NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_LoanStatusPriority_LoanSegmentPriority]
    ON [UheInvMet].[Monthly_LoanLevel]([LoanStatusPriority] ASC, [LoanSegmentPriority] ASC)
    INCLUDE([BF_SSN], [LN_SEQ], [LD_STA_LON10], [LC_CAM_LON_STA], [SPEC_FORB_IND], [WC_DW_LON_STA], [BILL_SATISFIED], [BorrSegment], [BorrDefermentIndicator], [PerformanceCategory], [ActiveMilitaryIndicator]);


GO
CREATE CLUSTERED INDEX [CIX_BFSSN_LNSEQ]
    ON [UheInvMet].[Monthly_LoanLevel]([BF_SSN] ASC, [LN_SEQ] ASC);

