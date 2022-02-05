CREATE TABLE [complaints].[Complaints] (
    [ComplaintId]                  INT             IDENTITY (1, 1) NOT NULL,
    [AccountNumber]                CHAR (10)       NOT NULL,
    [BorrowerName]                 NVARCHAR (50)   NOT NULL,
    [ComplaintTypeId]              INT             NOT NULL,
    [ComplaintPartyId]             INT             NOT NULL,
    [ComplaintGroupId]             INT             NOT NULL,
    [ComplaintDate]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [ControlMailNumber]            NVARCHAR (100)  NULL,
    [DaysToRespond]                INT             NULL,
    [NeedHelpTicketNumber]         NVARCHAR (8)    NULL,
    [ResolutionComplaintHistoryId] INT             NULL,
    [ComplaintDescription]         NVARCHAR (4000) NOT NULL,
    [AddedOn]                      DATETIME        DEFAULT (getdate()) NOT NULL,
    [AddedBy]                      NVARCHAR (50)   DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ComplaintId] ASC),
    CONSTRAINT [FK_Complaints_ComplaintGroups] FOREIGN KEY ([ComplaintGroupId]) REFERENCES [complaints].[ComplaintGroups] ([ComplaintGroupId]),
    CONSTRAINT [FK_Complaints_ComplaintHistory] FOREIGN KEY ([ResolutionComplaintHistoryId]) REFERENCES [complaints].[ComplaintHistory] ([ComplaintHistoryId]),
    CONSTRAINT [FK_Complaints_ComplaintParties] FOREIGN KEY ([ComplaintPartyId]) REFERENCES [complaints].[ComplaintParties] ([ComplaintPartyId]),
    CONSTRAINT [FK_Complaints_ComplaintTypes] FOREIGN KEY ([ComplaintTypeId]) REFERENCES [complaints].[ComplaintTypes] ([ComplaintTypeId])
);

