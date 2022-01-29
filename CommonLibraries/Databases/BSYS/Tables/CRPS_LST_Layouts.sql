CREATE TABLE [dbo].[CRPS_LST_Layouts] (
    [School] VARCHAR (50) NOT NULL,
    [Code]   CHAR (8)     NOT NULL,
    [Layout] TEXT         NOT NULL,
    CONSTRAINT [PK_CRPS_LST_Layouts] PRIMARY KEY CLUSTERED ([School] ASC)
);

