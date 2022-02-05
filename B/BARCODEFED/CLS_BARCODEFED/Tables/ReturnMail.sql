CREATE TABLE [barcodefed].[ReturnMail] ( 
    [ReturnMailId] INT NOT NULL IDENTITY,
    [RecipientId] VARCHAR (10) NOT NULL,
    [LetterId]    VARCHAR (10) NOT NULL,
	[CreateDate]  DATETIME NOT NULL,
    [Address1]    VARCHAR (30) NULL,
    [Address2]    VARCHAR (30) NULL,
    [City]        VARCHAR (20) NULL,
    [State]       CHAR (2)     NULL,
    [Zip]         VARCHAR (9)  NULL,
    [Country]     VARCHAR (25) NULL,
    [ProcessedAt] DATETIME NULL,
    [AddedBy]      VARCHAR (50) NOT NULL ,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [DeletedAt] DATETIME NULL, 
    [BorrowerSsn] VARCHAR(9) NULL, 
    [PersonType] CHAR NULL, 
    [ReceivedDate] DATETIME NULL, 
    CONSTRAINT [PK_ReturnMail] PRIMARY KEY ([ReturnMailId]),
	CONSTRAINT [AK_LetterLoaded] UNIQUE ([RecipientId],[LetterId],[CreateDate])
);

