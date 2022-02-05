CREATE TABLE [dbo].[Arc_Letter_Data] (
    [mapping_id]           INT           IDENTITY (1, 1) NOT NULL,
    [arc]                  VARCHAR (5)   NULL,
    [system_comment]       VARCHAR (300) NULL,
    [letter_id]            VARCHAR (10)  NULL,
    [letter_merge_comment] VARCHAR (300) NULL,
    [co_borr_arc] VARCHAR(5) NULL, 
    [co_borr_system_comment] VARCHAR(1200) NULL, 
    CONSTRAINT [PK_Arc_Letter_Mapping] PRIMARY KEY CLUSTERED ([mapping_id] ASC)
);

