USE ServicerInventoryMetrics
GO

DECLARE @UserId INT
INSERT INTO ServicerInventoryMetrics..AllowedUsers(AllowedUser,IsAdmin)
VALUES('dvansteeter',1)

SET @UserId = (SELECT AllowedUserId FROM ServicerInventoryMetrics..AllowedUsers WHERE AllowedUser = 'dvansteeter')

INSERT INTO ServicerInventoryMetrics..UserMetricMapping(AllowedUserId, ServicerMetricId)
VALUES(@UserId,4),
(@UserId,8)