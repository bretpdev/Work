CREATE TABLE [dbo].[OLD_NDHP_REF_StatusesConversionTemp] (
    [Sequence]  BIGINT       NOT NULL,
    [Ticket]    BIGINT       NOT NULL,
    [Status]    VARCHAR (50) NOT NULL,
    [BeginDate] DATETIME     NOT NULL,
    [EndDate]   DATETIME     NOT NULL,
    [Court]     VARCHAR (50) NOT NULL
);

