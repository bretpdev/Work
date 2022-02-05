CREATE TABLE [dbo].[DDRR_LST_InstitutionNotificationInfo] (
    [ID]               INT           NOT NULL,
    [InstitutionID]    VARCHAR (10)  NOT NULL,
    [AlwaysContact]    CHAR (1)      NOT NULL,
    [MethodOfContact]  VARCHAR (10)  NOT NULL,
    [FaxContactNames]  VARCHAR (300) NOT NULL,
    [FaxDisplayNumber] VARCHAR (20)  NOT NULL,
    [FaxDialNumber]    VARCHAR (20)  NOT NULL
);

