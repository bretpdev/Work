CREATE TABLE [scra].[ActiveDuty] (
    [ActiveDutyId] INT      IDENTITY (1, 1) NOT NULL,
    [BorrowerId]   INT      NOT NULL,
    [BeginDate]    DATETIME NOT NULL,
    [EndDate]      DATETIME NULL,
    [IsBorrower]   BIT      NOT NULL,
    [CreatedAt]    DATETIME CONSTRAINT [DF__ActiveDut__Creat__2AD6269A] DEFAULT (getdate()) NOT NULL,
    [Active]       BIT      NOT NULL,
    [BenefitSourceId] INT NULL, 
    [NotificationDate] DATETIME NULL, 
    [IsReservist] BIT NULL, 
    CONSTRAINT [PK__ActiveDu__C309A33D214CBC60] PRIMARY KEY CLUSTERED ([ActiveDutyId] ASC),
    CONSTRAINT [FK_ActiveDuty_ToBorrowers] FOREIGN KEY ([BorrowerId]) REFERENCES [scra].[Borrowers] ([BorrowerId])
);


