CREATE TABLE [dbo].[ITGMSQLBF_Unistar_Pay_Hist] (
    [ssn]                      VARCHAR (9) NULL,
    [database_id]              VARCHAR (2) NULL,
    [effective_date]           VARCHAR (7) NULL,
    [sequence_number]          VARCHAR (3) NULL,
    [transaction_code]         VARCHAR (3) NULL,
    [transaction_date]         VARCHAR (7) NULL,
    [alternate_ssn_indicator]  VARCHAR (1) NULL,
    [loan_number]              VARCHAR (1) NULL,
    [borrower_status]          VARCHAR (1) NULL,
    [number_months]            VARCHAR (2) NULL,
    [note_id]                  VARCHAR (2) NULL,
    [transaction_type]         VARCHAR (1) NULL,
    [activity_code]            VARCHAR (1) NULL,
    [non_cash]                 VARCHAR (1) NULL,
    [transaction_amount]       VARCHAR (8) NULL,
    [prior_principal_balance]  VARCHAR (8) NULL,
    [amount_principal_applied] VARCHAR (8) NULL,
    [amount_interest_accrued]  VARCHAR (8) NULL,
    [amount_interest_income]   VARCHAR (8) NULL,
    [interest_cap_080_flag]    VARCHAR (1) NULL,
    [other_amount]             VARCHAR (8) NULL,
    [trans_160_cust_id]        VARCHAR (4) NULL,
    [other_date]               VARCHAR (7) NULL,
    [trans_160_guar_id]        VARCHAR (4) NULL,
    [double_posted]            VARCHAR (1) NULL,
    [mc_update_flag]           VARCHAR (1) NULL,
    [loan_type]                VARCHAR (1) NULL,
    [other_flag]               VARCHAR (1) NULL,
    [record_type]              VARCHAR (1) NULL,
    [adv_due_date_flag]        VARCHAR (1) NULL,
    [new_interest_rate]        VARCHAR (5) NULL,
    [batch_number]             VARCHAR (6) NULL,
    [old_note_int_rate_111]    VARCHAR (5) NULL,
    [epd_status_111]           VARCHAR (1) NULL,
    [bilp_elig_flag_111]       VARCHAR (1) NULL,
    [rtsv_status_111]          VARCHAR (2) NULL,
    [supr_status_111]          VARCHAR (1) NULL,
    [240_loan_2]               VARCHAR (1) NULL,
    [240_loan_1]               VARCHAR (1) NULL,
    [N]                        VARCHAR (2) NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20130806-105519]
    ON [dbo].[ITGMSQLBF_Unistar_Pay_Hist]([ssn] ASC, [loan_number] ASC, [effective_date] ASC, [sequence_number] ASC) WITH (ALLOW_PAGE_LOCKS = OFF, ALLOW_ROW_LOCKS = OFF);

