CREATE TABLE [dbo].[AccessGroupMetricMapping]
(
	[AccessGroupMetricMapping] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccessGroupId] INT NOT NULL,  
    [ServicerMetricsId] INT NOT NULL, 
    CONSTRAINT [FK_AccessGroupsMetricMapping_ToAccessGroups] FOREIGN KEY (AccessGroupId) REFERENCES AccessGroups(AccessGroupId), 
    CONSTRAINT [FK_AccessGroupsMetricMapping_ToServicerMetrics] FOREIGN KEY (ServicerMetricsId) REFERENCES ServicerMetrics(ServicerMetricsId)
)
