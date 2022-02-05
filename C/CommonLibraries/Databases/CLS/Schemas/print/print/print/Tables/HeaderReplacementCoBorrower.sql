CREATE TABLE [print].[HeaderReplacementCoBorrower] (
    [ReplacementSetId] INT           IDENTITY (1, 1) NOT NULL,
    [FileHeader]       VARCHAR (MAX) NOT NULL,
    [InternalName]     VARCHAR (20)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ReplacementSetId] ASC) WITH (FILLFACTOR = 95)
);

