CREATE TABLE [dbo].[ITLQSQLDF_Disclosure] (
    [br_ssn]                  VARCHAR (9)  NULL,
    [disclosure_date]         VARCHAR (7)  NULL,
    [group_id]                VARCHAR (1)  NULL,
    [seq_numbr]               VARCHAR (6)  NULL,
    [user]                    VARCHAR (10) NULL,
    [group_payment_amount]    VARCHAR (9)  NULL,
    [graduated_payment_flag]  VARCHAR (1)  NULL,
    [number_of_payments]      VARCHAR (3)  NULL,
    [due_date]                VARCHAR (7)  NULL,
    [post_date]               VARCHAR (7)  NULL,
    [earliest_sched_due_date] VARCHAR (7)  NULL,
    [N]                       VARCHAR (2)  NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20140310-155705]
    ON [dbo].[ITLQSQLDF_Disclosure]([br_ssn] ASC, [group_id] ASC, [seq_numbr] ASC, [due_date] ASC, [disclosure_date] ASC);

