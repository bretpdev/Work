CREATE TABLE [dbo].[DAT_AgencyDataInvolved] (
    [TicketNumber]                                  BIGINT        NOT NULL,
    [TicketType]                                    VARCHAR (50)  NOT NULL,
    [AccountingOrAdministrativeRecordsWereReleased] BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_AccountingOrAdministrativeRecordsWereReleased] DEFAULT ((0)) NOT NULL,
    [ClosedSchoolRecordsWereReleased]               BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_ClosedSchoolRecordsWereReleased] DEFAULT ((0)) NOT NULL,
    [ConfidentialCaseFilesWereReleased]             BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_ConfidentialCaseFilesWereReleased] DEFAULT ((0)) NOT NULL,
    [ContractInformationWasReleased]                BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_ContractInformationWasReleased] DEFAULT ((0)) NOT NULL,
    [OperationsReportsWereReleased]                 BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_OperationsReportsWereReleased] DEFAULT ((0)) NOT NULL,
    [ProposalAndLoanPurchaseRequestsWereReleased]   BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_ProposalAndLoanPurchaseRequestsWereReleased] DEFAULT ((0)) NOT NULL,
    [UespParticipantRecordsWereReleased]            BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_UespParticipantRecordsWereReleased] DEFAULT ((0)) NOT NULL,
    [OtherInformationWasReleased]                   BIT           CONSTRAINT [DF_DAT_AgencyDataInvolved_OtherInformationWasReleased] DEFAULT ((0)) NOT NULL,
    [OtherInformation]                              VARCHAR (100) NULL,
    CONSTRAINT [PK_DAT_AgencyDataInvolved] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_AgencyDataInvolved_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

