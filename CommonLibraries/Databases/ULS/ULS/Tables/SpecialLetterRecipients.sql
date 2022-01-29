CREATE TABLE [dbo].[SpecialLetterRecipients] (
    [SpecialLetterRecipientId] INT           IDENTITY (1, 1) NOT NULL,
    [LetterId]                 VARCHAR (10)  NOT NULL,
    [Recipient]                VARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([SpecialLetterRecipientId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [CK_SpecialLetterRecipients_Recipient] CHECK ([Recipient] collate latin1_general_cs_as='Other' OR [Recipient] collate latin1_general_cs_as='Reference')
);


