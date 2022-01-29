CREATE TABLE [dbo].[MABC_SSN_User_XRef] (
    [RangeID]         INT           IDENTITY (1, 1) NOT NULL,
    [UserID]          VARCHAR (7)   NOT NULL,
    [WindowsUserName] NVARCHAR (50) NOT NULL,
    [SSNRangeBegin]   INT           NOT NULL,
    [SSNRangeEnd]     INT           NOT NULL,
    [Dept]            NVARCHAR (50) NOT NULL,
    [UserID2]         VARCHAR (7)   NULL
);

