CREATE TABLE [dbo].[Repayment_Schedule] (
    [br_ssn]                        VARCHAR (9)     NULL,
    [nn_ln_seq]                     INT             NOT NULL,
    [ln_seq]                        INT             NULL,
    [ln_curr_principal]             MONEY           NULL,
    [ln_interest_rate]              DECIMAL (10, 3) NULL,
    [ln_inactive_active_sts]        VARCHAR (1)     NULL,
    [ln_orig_loan_term]             VARCHAR (3)     NULL,
    [gr_num_term_periods_orig]      VARCHAR (3)     NULL,
    [group_id]                      VARCHAR (1)     NULL,
    [ln_int_rate_at_repay]          MONEY           NULL,
    [disbursement_date]             DATE            NULL,
    [graduated_payment_flag]        VARCHAR (1)     NULL,
    [installment_amount]            MONEY           NULL,
    [db_prin_amount]                MONEY           NULL,
    [number_of_payments]            VARCHAR (3)     NULL,
    [disclosure_date]               DATE            NULL,
    [due_date]                      DATE            NULL,
    [initial_due_date_by_gradation] DATE            NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20130522-182949]
    ON [dbo].[Repayment_Schedule]([br_ssn] ASC, [ln_seq] ASC, [nn_ln_seq] ASC);

