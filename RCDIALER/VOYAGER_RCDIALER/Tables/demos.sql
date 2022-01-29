CREATE TABLE [dbo].[demos] (
    [borrower_id]         INT           NOT NULL,
    [school_id]           VARCHAR (6)   NOT NULL,
    [dob]                 DATE          NOT NULL,
    [first_name]          NVARCHAR (50) NOT NULL,
    [middle_name]         NVARCHAR (50) NULL,
    [last_name]           NVARCHAR (50) NOT NULL,
    [demo_source]         VARCHAR (50)  NULL,
    [demo_eff_date]       DATE          NULL,
    [current_source_flag] CHAR (1)      NULL,
    [current_nslds_flag]  CHAR (1)      NULL,
    [add_source]          VARCHAR (50)  NULL,
    [add_eff_date]        DATE          NULL,
    [address_1]           VARCHAR (240) NULL,
    [address_2]           VARCHAR (240) NULL,
    [city]                VARCHAR (50)  NULL,
    [state]               VARCHAR (2)   NULL,
    [zip]                 VARCHAR (10)  NULL,
    [good_flag]           CHAR (1)      NULL,
    [table_source]        VARCHAR (4)   NOT NULL,
    [created_on]          DATETIME      CONSTRAINT [DF_demos_created_on] DEFAULT (getdate()) NOT NULL,
    [modified_on]         DATETIME      CONSTRAINT [DF_demos_modified_on] DEFAULT (getdate()) NOT NULL
);

