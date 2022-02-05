CREATE TABLE [dbo].[Loans] (
    [loan_id]       INT       IDENTITY (1, 1) NOT NULL,
    [br_ssn]        CHAR (9)  NOT NULL,
    [nn_ln_seq]     INT       NOT NULL,
    [comp_ln_seq]   INT       NULL,
    [AF_APL_ID]     CHAR (17) NULL,
    [AF_APL_ID_SEQ] SMALLINT  NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20130525-123241]
    ON [dbo].[Loans]([br_ssn] ASC, [nn_ln_seq] ASC, [comp_ln_seq] ASC);

