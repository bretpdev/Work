CREATE TABLE [dbo].[servicers] (
    [id]            AS            (CONVERT([varchar],[servicer_code])),
    [servicer_code] VARCHAR (3)   NOT NULL,
    [servicer_name] VARCHAR (50)  NOT NULL,
    [servicer_logo] VARCHAR (255) NULL,
    [email]         VARCHAR (255) NULL,
    [phone]         VARCHAR (14)  NULL,
    [website]       VARCHAR (255) NULL,
    [contact]       VARCHAR (255) NULL,
    [delay]         VARCHAR (255) NULL,
    [repay]         VARCHAR (255) NULL,
    [missed]        VARCHAR (255) NULL,
    [created_on]    DATETIME      CONSTRAINT [DF_servicers_created_on] DEFAULT (getdate()) NOT NULL,
    [modified_on]   DATETIME      CONSTRAINT [DF_servicers_modified_on] DEFAULT (getdate()) NOT NULL
);



