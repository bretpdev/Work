CREATE TABLE [complaints].[ComplaintParties] (
    [ComplaintPartyId] INT            IDENTITY (1, 1) NOT NULL,
    [PartyName]        NVARCHAR (100) NOT NULL,
    [AddedOn]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [AddedBy]          NVARCHAR (50)  DEFAULT (suser_sname()) NOT NULL,
    [DeletedOn]        DATETIME       NULL,
    [DeletedBy]        NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([ComplaintPartyId] ASC)
);

