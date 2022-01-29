CREATE TABLE [dbo].[DAT_ThreatBackgroundNoise] (
    [TicketNumber]        BIGINT       NOT NULL,
    [TicketType]          VARCHAR (50) NOT NULL,
    [Airplanes]           BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Airplanes] DEFAULT ((0)) NOT NULL,
    [Animals]             BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Animals] DEFAULT ((0)) NOT NULL,
    [Conversation]        BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Conversation] DEFAULT ((0)) NOT NULL,
    [Crowd]               BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Crowd] DEFAULT ((0)) NOT NULL,
    [FactoryMachines]     BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_FactoryMachines] DEFAULT ((0)) NOT NULL,
    [Music]               BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Music] DEFAULT ((0)) NOT NULL,
    [OfficeMachines]      BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_OfficeMachines] DEFAULT ((0)) NOT NULL,
    [Party]               BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Party] DEFAULT ((0)) NOT NULL,
    [PublicAddressSystem] BIT          CONSTRAINT [DF_Table_1_PublicAccessSystem] DEFAULT ((0)) NOT NULL,
    [SchoolBell]          BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_SchoolBell] DEFAULT ((0)) NOT NULL,
    [StreetTraffic]       BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_StreetTraffic] DEFAULT ((0)) NOT NULL,
    [Trains]              BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Trains] DEFAULT ((0)) NOT NULL,
    [Other]               BIT          CONSTRAINT [DF_DAT_ThreatBackgroundNoise_Other] DEFAULT ((0)) NOT NULL,
    [OtherDescription]    VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_ThreatBackgroundNoise] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatBackgroundNoise_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

