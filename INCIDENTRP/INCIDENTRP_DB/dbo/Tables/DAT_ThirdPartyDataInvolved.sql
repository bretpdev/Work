CREATE TABLE [dbo].[DAT_ThirdPartyDataInvolved] (
    [TicketNumber]                              BIGINT        NOT NULL,
    [TicketType]                                VARCHAR (50)  NOT NULL,
    [Name]                                      VARCHAR (100) NULL,
    [AccountNumber]                             VARCHAR (10)  NULL,
    [State]                                     CHAR (2)      NULL,
    [DataRegion]                                VARCHAR (15)  NOT NULL,
    [NotifierKnowsPiiOwner]                     BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_NotifierKnowsPiiOwner] DEFAULT ((0)) NOT NULL,
    [NotifierRelationshipToPiiOwner]            VARCHAR (50)  NULL,
    [SocialSecurityNumbersWereReleased]         BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_SocialSecurityNumbersWereReleased] DEFAULT ((0)) NOT NULL,
    [LoanIdsOrNumbersWereReleased]              BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_LoanIdsOrNumbersWereReleased] DEFAULT ((0)) NOT NULL,
    [LoanAmountsOrBalancesWereReleased]         BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_LoanAmountsOrBalancesWereReleased] DEFAULT ((0)) NOT NULL,
    [LoanPaymentHistoriesWereReleased]          BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_LoanPaymentHistoriesWereReleased] DEFAULT ((0)) NOT NULL,
    [PayoffAmountsWereReleased]                 BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_PayoffAmountsWereReleased] DEFAULT ((0)) NOT NULL,
    [BankAccountNumbersWereReleased]            BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_BankAccountNumbersWereReleased] DEFAULT ((0)) NOT NULL,
    [DateOfBirthWasReleased]                    BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_DateOfBirthWasReleased] DEFAULT ((0)) NOT NULL,
    [MedicalOrConditionalDisabilityWasReleased] BIT           CONSTRAINT [DF_DAT_ThirdPartyDataInvolved_MedicalOrConditionalDisabilityWasReleased] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_ThirdPartyDataInvolved] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThirdPartyDataInvolved_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_ThirdPartyDataInvolved_LST_Region] FOREIGN KEY ([DataRegion]) REFERENCES [dbo].[LST_Region] ([Region]) ON UPDATE CASCADE
);

