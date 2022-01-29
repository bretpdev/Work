CREATE TABLE [dbo].[RTML_DAT_BarcodeData] (
    [RecipientId]                  VARCHAR (10) NOT NULL,
    [LetterId]                     VARCHAR (10) NOT NULL,
    [CreateDate]                   DATETIME     NOT NULL,
    [AddressInvalidatedDate]       DATETIME     NULL,
    [ForwardingAddressUpdatedDate] DATETIME     NULL,
    CONSTRAINT [PK_RTML_DAT_BarcodeData] PRIMARY KEY CLUSTERED ([RecipientId] ASC, [LetterId] ASC, [CreateDate] ASC)
);

