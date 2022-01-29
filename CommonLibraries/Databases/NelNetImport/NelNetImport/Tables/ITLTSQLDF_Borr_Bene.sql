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
    [inteligible_date]        VARCHAR (7)  NULL,
    [N]                       VARCHAR (2)  NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20130525-091618]
    ON [dbo].[ITLTSQLDF_Borr_Bene]([br_ssn] ASC, [ln_num] ASC, [group_id] ASC);

