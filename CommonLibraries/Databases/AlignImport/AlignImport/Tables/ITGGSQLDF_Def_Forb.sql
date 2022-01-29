﻿CREATE TABLE [dbo].[ITGGSQLDF_Def_Forb] (
    [def_forb_id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [control_num]               INT           NULL,
    [br_ssn]                    VARCHAR (9)   NULL,
    [gr_id]                     VARCHAR (1)   NULL,
    [f1_loan_number]            VARCHAR (2)   NULL,
    [f1_seq_num]                VARCHAR (5)   NULL,
    [f1_type_of_defer_forb]     VARCHAR (3)   NULL,
    [f1_start_date]             VARCHAR (7)   NULL,
    [f1_end_date]               VARCHAR (7)   NULL,
    [f1_active_inactive_sts]    VARCHAR (1)   NULL,
    [f1_post_date]              VARCHAR (7)   NULL,
    [f1_user_name]              VARCHAR (10)  NULL,
    [f1_canceled_flag]          VARCHAR (1)   NULL,
    [f1_cancel_date]            VARCHAR (7)   NULL,
    [f1_cancel_user_name]       VARCHAR (10)  NULL,
    [f1_group_status_before]    VARCHAR (6)   NULL,
    [f1_gov_int_allowed]        VARCHAR (1)   NULL,
    [f1_post_defer_grace_sw]    VARCHAR (1)   NULL,
    [f1_gov_current_int_amt_st] VARCHAR (11)  NULL,
    [f1_cap_at_end]             VARCHAR (1)   NULL,
    [f1_admin_cap]              VARCHAR (1)   NULL,
    [f1_low_payment_amount]     VARCHAR (7)   NULL,
    [f1_number_of_low_payments] VARCHAR (3)   NULL,
    [f1_number_remaining]       VARCHAR (3)   NULL,
    [f1_from_receive_date]      VARCHAR (7)   NULL,
    [de_svc_geographic_area]    VARCHAR (4)   NULL,
    [de_svc_subject_area]       VARCHAR (2)   NULL,
    [si_school_code]            VARCHAR (6)   NULL,
    [si_branch]                 VARCHAR (2)   NULL,
    [f1_dependent_student_ssn]  VARCHAR (9)   NULL,
    [f1_discretion_mandatory]   VARCHAR (1)   NULL,
    [f1_grad_sep_notif_date]    VARCHAR (7)   NULL,
    [f1_grad_sep_certif_date]   VARCHAR (7)   NULL,
    [N]                         VARCHAR (132) NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_identity_index]
    ON [dbo].[ITGGSQLDF_Def_Forb]([f1_canceled_flag] ASC)
    INCLUDE([def_forb_id], [br_ssn], [f1_loan_number], [f1_type_of_defer_forb], [f1_start_date], [f1_end_date]);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20140429-111030]
    ON [dbo].[ITGGSQLDF_Def_Forb]([br_ssn] ASC, [f1_loan_number] ASC, [gr_id] ASC);

