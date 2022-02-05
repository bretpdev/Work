CREATE TABLE [dbo].[QuestionTB] (
    [QuestionID] BIGINT          NOT NULL,
    [GroupID]    NVARCHAR (4)    NOT NULL,
    [Question]   NVARCHAR (4000) NOT NULL,
    [Answer]     NVARCHAR (4000) NOT NULL,
    [Revised]    CHAR (10)       NOT NULL,
    [myim]       IMAGE           NULL
);

