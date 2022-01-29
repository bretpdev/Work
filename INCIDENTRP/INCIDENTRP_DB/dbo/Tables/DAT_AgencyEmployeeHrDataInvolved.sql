CREATE TABLE [dbo].[DAT_AgencyEmployeeHrDataInvolved] (
    [TicketNumber]                      BIGINT        NOT NULL,
    [TicketType]                        VARCHAR (50)  NOT NULL,
    [Name]                              VARCHAR (100) NULL,
    [State]                             CHAR (2)      NULL,
    [NotifierKnowsEmployee]             BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_NotifierKnowsEmployee] DEFAULT ((0)) NOT NULL,
    [NotifierRelationshipToEmployee]    VARCHAR (50)  NULL,
    [DateOfBirthWasReleased]            BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_DateOfBirth] DEFAULT ((0)) NOT NULL,
    [EmployeeIdNumberWasReleased]       BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_EmployeeIdNumber] DEFAULT ((0)) NOT NULL,
    [HomeAddressWasReleased]            BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_HomeAddressWasReleased] DEFAULT ((0)) NOT NULL,
    [HealthInformationWasReleased]      BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_HealthInformationWasReleased] DEFAULT ((0)) NOT NULL,
    [PerformanceInformationWasReleased] BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_PerformanceInformationWasReleased] DEFAULT ((0)) NOT NULL,
    [PersonnelFilesWereReleased]        BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_PersonnelFilesWereReleased] DEFAULT ((0)) NOT NULL,
    [UnauthorizedReferenceWasReleased]  BIT           CONSTRAINT [DF_DAT_AgencyEmployeeHrDataInvolved_UnauthorizedReferenceWasReleased] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_AgencyEmployeeHrDataInvolved] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_AgencyEmployeeHrDataInvolved_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

