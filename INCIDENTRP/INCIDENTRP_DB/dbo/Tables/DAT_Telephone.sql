CREATE TABLE [dbo].[DAT_Telephone] (
    [TicketNumber]                                BIGINT       NOT NULL,
    [TicketType]                                  VARCHAR (50) NOT NULL,
    [RevealedInformationOnVoicemail]              BIT          CONSTRAINT [DF_DAT_Telephone_RevealedInformationOnVoicemail] DEFAULT ((0)) NOT NULL,
    [RevealedInformationToUnauthorizedIndividual] BIT          CONSTRAINT [DF_DAT_Telephone_RevelaedInformationToUnauthorizedIndividual] DEFAULT ((0)) NOT NULL,
    [UnauthorizedIndividual]                      VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_Telephone] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_Telephone_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

