CREATE TABLE [dbo].[GENR_REF_LenderAffiliation] (
    [LenderID]    VARCHAR (10) NOT NULL,
    [Affiliation] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_REF_LenderAffiliation] PRIMARY KEY CLUSTERED ([LenderID] ASC, [Affiliation] ASC)
);

