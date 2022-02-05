CREATE TABLE [dbo].[phones] (
    [borrower_id]     INT          NOT NULL,
    [school_id]       VARCHAR (6)  NOT NULL,
    [phone_source]    VARCHAR (50) NULL,
    [phone_number]    VARCHAR (12) NOT NULL,
    [phone_type]      CHAR (1)     NULL,
    [phone_preferred] CHAR (1)     NULL,
    [modified_on]     DATETIME     CONSTRAINT [DF_phones_modified_on] DEFAULT (getdate()) NOT NULL,
    [created_on]      DATETIME     CONSTRAINT [DF_phones_created_on] DEFAULT (getdate()) NOT NULL
);

