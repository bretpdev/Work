CREATE TABLE [dbo].[BankoReceiveResponseOutput] (
    [RecordNumber]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [CaseNumber]          VARCHAR (12) NULL,
    [StateField]          CHAR (2)     NULL,
    [Chapter]             VARCHAR (2)  NULL,
    [FileDate]            VARCHAR (10) NULL,
    [StatusDate]          VARCHAR (10) NULL,
    [DispositionCode]     VARCHAR (2)  NULL,
    [AttorneyName]        VARCHAR (35) NULL,
    [AttorneyAddress]     VARCHAR (32) NULL,
    [AttorneyCity]        VARCHAR (25) NULL,
    [AttorneyState]       VARCHAR (2)  NULL,
    [AttorneyZip]         VARCHAR (10) NULL,
    [AttorneyPhone]       VARCHAR (10) NULL,
    [CourtDistrict]       VARCHAR (30) NULL,
    [CourtAddress1]       VARCHAR (35) NULL,
    [CourtAddress2]       VARCHAR (35) NULL,
    [CourtMailingCity]    VARCHAR (20) NULL,
    [CourtZip]            VARCHAR (5)  NULL,
    [CourtPhone]          VARCHAR (10) NULL,
    [ProofOfClaimBarDate] VARCHAR (10) NULL
);

