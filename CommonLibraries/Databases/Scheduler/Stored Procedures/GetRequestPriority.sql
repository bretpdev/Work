CREATE PROCEDURE [dbo].[GetRequestPriority]
(
	@RequestType varchar(6),
	@RequestId int
)
AS

select RP.PriorityLevel 
from RequestPriorities_Ordered RP 
inner join RequestTypes RT on RT.RequestTypeId = RP.RequestTypeId
where RT.RequestType = @RequestType and RP.RequestId = @RequestId
OPTION (MAXRECURSION 32767)

