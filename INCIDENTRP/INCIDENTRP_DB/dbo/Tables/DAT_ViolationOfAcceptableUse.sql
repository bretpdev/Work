CREATE TABLE [dbo].[DAT_ViolationOfAcceptableUse] (
    [TicketNumber]                       BIGINT       NOT NULL,
    [TicketType]                         VARCHAR (50) NOT NULL,
    [AccessKeycardWasShared]             BIT          CONSTRAINT [DF_DAT_ViolationOfAcceptableUse_AccessKeycardWasShared] DEFAULT ((0)) NOT NULL,
    [MisuseOfSystemResourcesByValidUser] BIT          CONSTRAINT [DF_DAT_ViolationOfAcceptableUse_MisuseOfSystemResourcesByValidUser] DEFAULT ((0)) NOT NULL,
    [UserSystemCredentialsWereShared]    BIT          CONSTRAINT [DF_DAT_ViolationOfAcceptableUse_UserSystemCredentialsWereShared] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_ViolationOfAcceptableUse] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ViolationOfAcceptableUse_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

