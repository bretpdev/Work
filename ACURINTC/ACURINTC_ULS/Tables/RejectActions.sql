CREATE TABLE [acurintc].[RejectActions]
(
	[RejectActionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionCodeAddress] VARCHAR(50) NULL, 
    [ActionCodePhone] VARCHAR(50) NULL, 
    [ActionCodeEmail] VARCHAR(50) NULL, 
    [DemographicsSourceId] INT NOT NULL,
	[RejectReasonId] INT NOT NULL, 
    CONSTRAINT [FK_RejectActions_DemographicsSources] FOREIGN KEY (DemographicsSourceId) REFERENCES acurintc.DemographicsSources(DemographicsSourceId),
	CONSTRAINT [FK_RejectActions_RejectReasons] FOREIGN KEY (RejectReasonId) REFERENCES acurintc.RejectReasons(RejectReasonId)
)
