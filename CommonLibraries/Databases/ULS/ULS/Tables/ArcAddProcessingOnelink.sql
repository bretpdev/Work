CREATE TABLE [dbo].[ArcAddProcessingOnelink] (
    [ArcAddProcessingOnelinkId] INT           IDENTITY (1, 1) NOT NULL,
    [AccountIdentifer]          VARCHAR (10)  NOT NULL,
    [AssociatedPersonID]        VARCHAR (10)  NULL,
    [ActionCode]                VARCHAR (5)   NOT NULL,
    [ActivityType]              CHAR (2)      NOT NULL,
    [ActivityContactType]       CHAR (2)      NOT NULL,
    [DocumentID]                VARCHAR (4)   NULL,
    [ActivityDateTime]          DATETIME      NULL,
    [ActivityCloseDate]         DATETIME      NULL,
    [UniqueID]                  VARCHAR (17)  NULL,
    [InstitutionID]             VARCHAR (8)   NULL,
    [UserID]                    VARCHAR (8)   NULL,
    [ClaimPackageCreateDate]    DATETIME      NULL,
    [UserIDClaimPackage]        VARCHAR (8)   NULL,
    [Comment]                   VARCHAR (589) NULL,
    [ScriptID]                  VARCHAR (10)  NOT NULL,
    [ProcessOn]                 DATETIME      DEFAULT (getdate()) NOT NULL,
    [ProcessedAt]               DATETIME      NULL,
    [AddedAt]                   DATETIME      DEFAULT (getdate()) NULL,
    [AddedBy]                   VARCHAR (100) DEFAULT (suser_sname()) NULL,
    PRIMARY KEY CLUSTERED ([ArcAddProcessingOnelinkId] ASC)
);


