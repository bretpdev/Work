CREATE TABLE [NobleController].[CallCampaignSprocMap]
(
	[ApprovedStoredProdecuresId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Campaign] VARCHAR(1000) NOT NULL,
	[StoredProcedureName] VARCHAR(200) NOT NULL
)
