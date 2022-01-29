CREATE TABLE [dbo].[ReturnMail] (
    [UserId]      VARCHAR (50) NOT NULL,
    [RecipientId] VARCHAR (10) NOT NULL,
    [LetterId]    VARCHAR (10) NOT NULL,
    [CreateDate]  DATETIME     NOT NULL,
    [PersonType]  CHAR (2)     NOT NULL,
    [Address1]    VARCHAR (30) NULL,
    [Address2]    VARCHAR (30) NULL,
    [City]        VARCHAR (20) NULL,
    [State]       CHAR (2)     NULL,
    [Zip]         VARCHAR (9)  NULL,
    [Country]     VARCHAR (25) NULL,
    CONSTRAINT [PK_ReturnMail] PRIMARY KEY CLUSTERED ([UserId] ASC, [RecipientId] ASC, [LetterId] ASC, [CreateDate] ASC)
);

