CREATE TABLE [dbo].[Co_Maker_Borr_Hist] (
    [Br SSN]          NVARCHAR (255) NULL,
    [BHBTDT]          FLOAT (53)     NULL,
    [BHBTIM]          FLOAT (53)     NULL,
    [BHBSQ]           FLOAT (53)     NULL,
    [Bc Hist Code]    NVARCHAR (255) NULL,
    [Bh Group]        NVARCHAR (255) NULL,
    [Bh Type]         NVARCHAR (255) NULL,
    [Bh Text]         NVARCHAR (255) NULL,
    [Bh User Id]      NVARCHAR (255) NULL,
    [Bh Group Agings] NVARCHAR (255) NULL,
    [Bh Sts1a]        NVARCHAR (255) NULL,
    [Bh Sts1b]        NVARCHAR (255) NULL,
    [Bh Sts2a]        NVARCHAR (255) NULL,
    [Bh Sts3a]        NVARCHAR (255) NULL,
    [Bh Cde4a]        NVARCHAR (255) NULL,
    [Bh Program Id]   NVARCHAR (255) NULL,
    [BHI4ND]          FLOAT (53)     NULL,
    [BHI5ND]          FLOAT (53)     NULL,
    [BHQKVA]          FLOAT (53)     NULL,
    [BHQMVA]          FLOAT (53)     NULL,
    [BHEDDT]          FLOAT (53)     NULL,
    [BHEEDT]          FLOAT (53)     NULL,
    [Bh Cust Sts1a]   NVARCHAR (255) NULL,
    [Bh Cust Sts1b]   NVARCHAR (255) NULL,
    [Bh Cust Sts1c]   NVARCHAR (255) NULL,
    [Bh Cust Sts2a]   NVARCHAR (255) NULL,
    [BHQLVA]          FLOAT (53)     NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20131018-133224]
    ON [dbo].[Co_Maker_Borr_Hist]([Br SSN] ASC);

