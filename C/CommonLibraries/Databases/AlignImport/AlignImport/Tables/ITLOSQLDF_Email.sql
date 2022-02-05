CREATE TABLE [dbo].[ITLOSQLDF_Email] (
    [br_ssn]             VARCHAR (9)  NULL,
    [email_indicator]    VARCHAR (1)  NULL,
    [email_address]      VARCHAR (60) NULL,
    [br_cell_ph_consent] VARCHAR (1)  NULL,
    [N]                  VARCHAR (3)  NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_Email]
    ON [dbo].[ITLOSQLDF_Email]([br_ssn] ASC, [email_indicator] ASC);

