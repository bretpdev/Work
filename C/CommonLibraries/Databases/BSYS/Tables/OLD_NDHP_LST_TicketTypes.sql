CREATE TABLE [dbo].[OLD_NDHP_LST_TicketTypes] (
    [TicketType]     VARCHAR (50) NOT NULL,
    [TicketCode]     CHAR (3)     NOT NULL,
    [ResolutionDays] INT          CONSTRAINT [DF_NDHP_LST_TicketTypes_ResolutionDays] DEFAULT (0) NOT NULL,
    [UserChangable]  BIT          CONSTRAINT [DF_NDHP_LST_TicketTypes_UserChangable] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_NDHP_LST_TicketTypes] PRIMARY KEY CLUSTERED ([TicketCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Standard number of days to resolve issues of ticket type.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_TicketTypes', @level2type = N'COLUMN', @level2name = N'ResolutionDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Three-character ticket type code.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_TicketTypes', @level2type = N'COLUMN', @level2name = N'TicketCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of ticket type.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_TicketTypes', @level2type = N'COLUMN', @level2name = N'TicketType';

