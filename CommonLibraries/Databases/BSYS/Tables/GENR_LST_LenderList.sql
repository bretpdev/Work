CREATE TABLE [dbo].[GENR_LST_LenderList] (
    [RecNum]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [LenderID]   VARCHAR (10)  NOT NULL,
    [LenderName] VARCHAR (100) NOT NULL,
    [AppKey]     VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_GENR_LST_LenderList] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

