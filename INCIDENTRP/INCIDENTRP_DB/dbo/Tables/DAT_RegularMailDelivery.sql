CREATE TABLE [dbo].[DAT_RegularMailDelivery] (
    [TicketNumber]   BIGINT       NOT NULL,
    [TicketType]     VARCHAR (50) NOT NULL,
    [Problem]        VARCHAR (20) NOT NULL,
    [Address1]       VARCHAR (50) NOT NULL,
    [Address2]       VARCHAR (50) NULL,
    [City]           VARCHAR (50) NOT NULL,
    [State]          CHAR (2)     NOT NULL,
    [Zip]            VARCHAR (10) NOT NULL,
    [TrackingNumber] VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_RegularMailDelivery] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC, [Problem] ASC),
    CONSTRAINT [FK_DAT_RegularMailDelivery_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_RegularMailDelivery_LST_RegularMailDeliveryProblem] FOREIGN KEY ([Problem]) REFERENCES [dbo].[LST_RegularMailDeliveryProblem] ([Problem]) ON UPDATE CASCADE
);

