CREATE TABLE [calls].[Reasons] (
    [ReasonId]    INT            IDENTITY (1, 1) NOT NULL,
    [CategoryId]  INT            NOT NULL,
    [ReasonText]  NVARCHAR (200) NOT NULL,
    [Uheaa]       BIT            NOT NULL,
    [Cornerstone] BIT            NOT NULL,
    [Inbound]     BIT            NOT NULL,
    [Outbound]    BIT            NOT NULL,
    [Enabled]     BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ReasonId] ASC),
    CONSTRAINT [FK_Reasons_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [calls].[Categories] ([CategoryId]),
    CONSTRAINT [UK_Reasons_ReasonText] UNIQUE NONCLUSTERED ([ReasonText] ASC, [CategoryId] ASC)
);

