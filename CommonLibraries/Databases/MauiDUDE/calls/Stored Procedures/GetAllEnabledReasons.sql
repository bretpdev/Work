CREATE PROCEDURE [calls].[GetAllEnabledReasons]
AS
	select 
	       r.ReasonId,
		   c.Title as Category,
		   r.ReasonText,
		   r.Cornerstone,
		   r.Uheaa,
		   r.Outbound,
		   r.Inbound
	  from calls.Reasons r
	  join calls.Categories c on c.CategoryId = r.CategoryId
RETURN 0
