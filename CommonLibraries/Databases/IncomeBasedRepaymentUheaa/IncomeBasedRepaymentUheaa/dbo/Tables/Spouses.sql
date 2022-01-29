﻿CREATE TABLE [dbo].[Spouses] (
    [spouse_id]                         INT         IDENTITY (1, 1) NOT NULL,
    [person_role_id]                    INT         NULL,
    [SSN]                               CHAR (9)    NULL,
    [birth_date]                        DATE        NULL,
    [first_name]                        CHAR (35)   NULL,
    [last_name]                         CHAR (35)   NULL,
    [middle_name]                       CHAR (35)   NULL,
    [pseudo_ssn]                        BIT         NULL,
    [driver_license_or_state_id]        CHAR (30)   NULL,
    [state_code]                        CHAR (2)    NULL,
    [separated_from_spouse]             BIT         NULL,
    [access_spouse_income_info]         BIT         NULL,
    [spouse_taxes_filed_flag]           BIT         NULL,
    [spouse_tax_year]                   VARCHAR (4) NULL,
    [spouse_filing_status_id]           INT         NULL,
    [spouse_AGI]                        MONEY       NULL,
    [spouse_AGI_relects_current_income] BIT         NULL,
    [spouse_support_docs_required]      BIT         NULL,
    [spouse_support_docs_recvd_date]    DATETIME    NULL,
    [spouse_alt_submitted_income]       MONEY       NULL,
    [spouse_Loans_Same_Region]          BIT         NULL,
    [created_at]                        DATETIME    CONSTRAINT [DF__Spouses__created__25DB9BFC] DEFAULT (getdate()) NULL,
    [updated_at]                        DATETIME    CONSTRAINT [DF__Spouses__created__25DB9BFC1] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Spouses] PRIMARY KEY CLUSTERED ([spouse_id] ASC),
    CONSTRAINT [FK_Spouses_Person_Roles] FOREIGN KEY ([person_role_id]) REFERENCES [dbo].[Person_Roles] ([person_role_id])
);



