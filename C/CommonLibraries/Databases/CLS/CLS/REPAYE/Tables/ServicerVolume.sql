CREATE TABLE [REPAYE].[ServicerVolume] (
    [ServicerVolumeId]       INT  IDENTITY (1, 1) NOT NULL,
    [DueDate]                DATE NOT NULL,
    [BeginDate]              DATE NOT NULL,
    [EndDate]                DATE NOT NULL,
    [PhoneInquiries]         INT  NULL,
    [WebInquiries]           INT  NULL,
    [EmailInquiries]         INT  NULL,
    [OtherInquiries]         INT  NULL,
    [ApplicationsReceived]   INT  NULL,
    [ApplicationsApproved]   INT  NULL,
    [ApplicationsDenied]     INT  NULL,
    [ApplicationsInProgress] INT  NULL,
    [OldestProgressDate]     DATE NULL,
    [ApplicationTypeOnline]  INT  NULL,
    [ApplicationTypePaper]   INT  NULL
);

