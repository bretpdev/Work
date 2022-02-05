CREATE TABLE [dbo].[BORG_DAT_OneLINKCheckByPhoneData] (
    [TimeOfEntry]   DATETIME        CONSTRAINT [DF_BORG_DAT_OneLINKCheckByPhoneData_TimeOfEntry] DEFAULT (getdate()) NOT NULL,
    [UserID]        VARCHAR (50)    NOT NULL,
    [ContactType]   VARCHAR (2)     NOT NULL,
    [PaymentAmount] NUMERIC (18, 2) NOT NULL,
    CONSTRAINT [PK_BORG_DAT_OneLINKCheckByPhoneData] PRIMARY KEY CLUSTERED ([TimeOfEntry] ASC, [UserID] ASC)
);

