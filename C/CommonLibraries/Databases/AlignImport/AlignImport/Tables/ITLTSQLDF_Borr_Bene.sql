CREATE TABLE [dbo].[ITLTSQLDF_Borr_Bene] (
    [br_ssn]                  VARCHAR (9)  NULL,
    [ln_num]                  VARCHAR (2)  NULL,
    [group_id]                VARCHAR (1)  NULL,
    [seq]                     VARCHAR (5)  NULL,
    [lender_id]               VARCHAR (8)  NULL,
    [action_code]             VARCHAR (2)  NULL,
    [incentive_code]          VARCHAR (20) NULL,
    [status]                  VARCHAR (3)  NULL,
    [percentage_or_points]    VARCHAR (6)  NULL,
    [payment_counter]         VARCHAR (4)  NULL,
    [awarded_pct]             VARCHAR (5)  NULL,
    [awarded_amt]             VARCHAR (11) NULL,
    [description]             VARCHAR (50) NULL,
    [principal_reduction_amt] VARCHAR (9)  NULL,
    [ineligible_date]         VARCHAR (7)  NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_bor_benefits]
    ON [dbo].[ITLTSQLDF_Borr_Bene]([br_ssn] ASC, [status] ASC, [incentive_code] ASC);

