CREATE TABLE [dbo].[DEMS_DAT_Recovery] (
    [AccountNumber]            VARCHAR (50)  NOT NULL,
    [PageNumber]               INT           NOT NULL,
    [PdemVerificationDate]     DATETIME      NULL,
    [OriginalDemographicsText] VARCHAR (200) NULL,
    [Address1]                 VARCHAR (50)  NULL,
    [Address2]                 VARCHAR (50)  NULL,
    [City]                     VARCHAR (50)  NULL,
    [DemographicsSource]       VARCHAR (50)  NOT NULL,
    [Email]                    VARCHAR (50)  NULL,
    [PdemSource]               VARCHAR (50)  NULL,
    [Phone]                    VARCHAR (20)  NULL,
    [State]                    VARCHAR (2)   NULL,
    [SystemSource]             VARCHAR (50)  NOT NULL,
    [Zip]                      VARCHAR (50)  NULL,
    [AdditionalInfo]           VARCHAR (50)  NULL,
    CONSTRAINT [PK_DEMS_DAT_Recovery] PRIMARY KEY CLUSTERED ([AccountNumber] ASC, [PageNumber] ASC)
);

