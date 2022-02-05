CREATE TABLE [dbo].[OLD_NDHP_LST_PreDefAssignToStaff] (
    [WindowsUserID] NVARCHAR (50) NOT NULL,
    [TicketCode]    CHAR (3)      NOT NULL,
    CONSTRAINT [PK_NDHP_LST_PredefAssignToStaff] PRIMARY KEY CLUSTERED ([WindowsUserID] ASC, [TicketCode] ASC),
    CONSTRAINT [FK_NDHP_LST_PreDefAssignToStaff_NDHP_LST_TicketTypes] FOREIGN KEY ([TicketCode]) REFERENCES [dbo].[OLD_NDHP_LST_TicketTypes] ([TicketCode]) ON UPDATE CASCADE,
    CONSTRAINT [FK_NDHP_LST_PreDefAssignToStaff_SYSA_LST_Users] FOREIGN KEY ([WindowsUserID]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

