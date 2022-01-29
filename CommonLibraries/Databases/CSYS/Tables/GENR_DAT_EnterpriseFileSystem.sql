CREATE TABLE [dbo].[GENR_DAT_EnterpriseFileSystem] (
    [Key]      VARCHAR (50)  NOT NULL,
    [TestMode] BIT           NOT NULL,
    [Region]   VARCHAR (50)  NOT NULL,
    [Path]     VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_GENR_DAT_EnterpriseFileSystem_1] PRIMARY KEY CLUSTERED ([Key] ASC, [TestMode] ASC, [Region] ASC)
);

