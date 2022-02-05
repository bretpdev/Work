CREATE TABLE [dbo].[AllowedUserAccessGroupMapping]
(
	[AllowedUserAccessGroupMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccessGroupId] INT NOT NULL, 
    [AllowedUserId] INT NOT NULL, 
    CONSTRAINT [FK_AllowedUserAccessGroupMapping_ToAccessGroups] FOREIGN KEY (AccessGroupId) REFERENCES AccessGroups(AccessGroupId), 
    CONSTRAINT [FK_AllowedUserAccessGroupMapping_ToAllowedUsers] FOREIGN KEY (AllowedUserId) REFERENCES AllowedUsers(AllowedUserId)
)
