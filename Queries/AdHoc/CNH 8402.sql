 SELECT * FROM
 [ServicerInventoryMetrics].[dbo].[AllowedUserAccessGroupMapping] AAGM
 INNER JOIN [ServicerInventoryMetrics].[dbo].[AllowedUsers] AU
	ON AU.AllowedUserId = AAGM.AllowedUserId
INNER JOIN [ServicerInventoryMetrics].[dbo].[AccessGroups] AG
	ON AG.AccessGroupId = AAGM.AccessGroupId
INNER JOIN [ServicerInventoryMetrics].[dbo].[AccessGroupMetricMapping] AGMM
	ON AGMM.AccessGroupId = AG.AccessGroupId
INNER JOIN ServicerInventoryMetrics.dbo.ServicerMetrics SM
	ON SM.ServicerMetricsId = AGMM.ServicerMetricsId
INNER JOIN ServicerInventoryMetrics.dbo.ServicerCategory SC
	ON SC.ServicerCategoryId = SM.ServicerCategoryId
WHERE SM.ServicerMetric IN ('Forbearance','Deferment','IDR','Bankruptcy - Notices','Bankruptcy - Proof of Claim','Aging +XXX & Still at Servicer','Aging +XXX & Sent to DMCS to Process','Aging +XXX & Not accepted by DMCS','Death Discharge Servicer Aging','Death Discharge FSA Aging','Closed School FSA Aging','Fraud Servicer Aging','Closed School Notifications Sent','Closed School Applications Processed','Disability Discharge Servicer Aging')
ORDER BY 
	AU.AllowedUser
/*to fix: split Brad, Teri, and Jenna to a different group from Cindy*/
/*
UPDATE ServicerInventoryMetrics.dbo.AccessGroups SET AccessGroupName = 'ASSET MANAGEMENT' WHERE AccessGroupId = X
INSERT INTO ServicerInventoryMetrics.dbo.AccessGroups(AccessGroupId, AccessGroupName) VALUES(XX,'LOAN SERVICING')
DELETE FROM ServicerInventoryMetrics.dbo.AllowedUserAccessGroupMapping where AllowedUserId = (select AllowedUserId from ServicerInventoryMetrics.dbo.AllowedUsers where AllowedUser = 'mballard')
UPDATE ServicerInventoryMetrics.dbo.AllowedUsers set IsAdmin = X where AllowedUser = 'briding'
UPDATE ServicerInventoryMetrics.dbo.AllowedUserAccessGroupMapping set AccessGroupId = XX where AllowedUserId in (select AllowedUserId from ServicerInventoryMetrics.dbo.AllowedUsers where AllowedUser in('briding','tvig','jkonold')
UPDATE ServicerInventoryMetrics.dbo.AccessGroupMetricMapping set AccessGroupId = XX where servicermetricsid in (select ServicerMetricsId from ServicerInventoryMetrics.dbo.ServicerMetrics where ServicerMetric in ('Closed School Notifications Sent','Closed School Applications Processed','Disability Discharge Servicer Aging','Forbearance','Deferment','IDR'))
*/

