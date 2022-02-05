CREATE TABLE [dbo].[schools] (
    [id]             AS            (CONVERT([varchar],[school_code])),
    [school_code]    VARCHAR (6)   NOT NULL,
    [school_name]    VARCHAR (255) NOT NULL,
    [text_account]   VARCHAR (50)  NULL,
    [text_token]     VARCHAR (50)  NULL,
    [msg_service_id] VARCHAR (50)  NULL,
    [short_name]     VARCHAR (50)  NOT NULL,
    [school_email]   VARCHAR (255) NULL,
    [school_phone]   VARCHAR (12)  NULL,
    [rc_school_logo] TEXT          NULL,
    [school_logo]    TEXT          NULL,
    [website]        TEXT          NULL,
    [active]         BIT           CONSTRAINT [DF_schools_active] DEFAULT ((0)) NOT NULL,
    [service_30]     BIT           CONSTRAINT [DF_schools_service_30] DEFAULT ((0)) NOT NULL,
    [service_60]     BIT           CONSTRAINT [DF_schools_service_60] DEFAULT ((0)) NOT NULL,
    [service_90]     BIT           CONSTRAINT [DF_schools_service_90] DEFAULT ((0)) NOT NULL,
    [service_270]    BIT           CONSTRAINT [DF_schools_service_270] DEFAULT ((0)) NOT NULL,
    [service_grace]  BIT           CONSTRAINT [DF_schools_service_grace] DEFAULT ((0)) NOT NULL,
    [created_on]     DATETIME      CONSTRAINT [DF_schools_created_on] DEFAULT (getdate()) NOT NULL,
    [updated_at]     DATETIME      CONSTRAINT [DF_schools_updated_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_schools] PRIMARY KEY CLUSTERED ([school_code] ASC)
);

