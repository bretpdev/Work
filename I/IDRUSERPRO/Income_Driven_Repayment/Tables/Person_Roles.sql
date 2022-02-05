CREATE TABLE [dbo].[Person_Roles] (
    [person_role_id]          INT           IDENTITY (1, 1) NOT NULL,
    [person_role]             CHAR (1)      NOT NULL,
    [person_role_description] VARCHAR (255) NOT NULL,
    [created_at]              DATETIME      CONSTRAINT [DF__Person_Ro__creat__1BFD2C07] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Person_Roles] PRIMARY KEY CLUSTERED ([person_role_id] ASC)
);

