CREATE TABLE [dbo].[GENR_LST_Relationships] (
    [relationship_id] INT          IDENTITY (1, 1) NOT NULL,
    [code]            CHAR (2)     NOT NULL,
    [description]     VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_LST_Relationships] PRIMARY KEY CLUSTERED ([relationship_id] ASC)
);

