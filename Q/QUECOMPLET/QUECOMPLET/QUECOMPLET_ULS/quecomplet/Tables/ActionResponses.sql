CREATE TABLE [quecomplet].[ActionResponses] (
    [ActionResponseId] INT      IDENTITY (1, 1) NOT NULL,
    [ActionResponse]   CHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([ActionResponseId] ASC) WITH (FILLFACTOR = 95)
);

