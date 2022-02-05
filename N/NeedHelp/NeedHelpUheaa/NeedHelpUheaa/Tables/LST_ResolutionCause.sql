CREATE TABLE [dbo].[LST_ResolutionCause] (
    [Cause] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NDHP_LST_ResolutionCauses] PRIMARY KEY CLUSTERED ([Cause] ASC) WITH (FILLFACTOR = 90)
);

