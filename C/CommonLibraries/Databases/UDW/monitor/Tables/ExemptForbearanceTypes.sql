CREATE TABLE [monitor].[ExemptForbearanceTypes] (
    [ExemptForbearanceTypeId]    INT          IDENTITY (1, 1) NOT NULL,
    [ForbearanceTypeCode]        CHAR (2)     NOT NULL,
    [ForbearanceTypeDescription] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([ExemptForbearanceTypeId] ASC)
);

