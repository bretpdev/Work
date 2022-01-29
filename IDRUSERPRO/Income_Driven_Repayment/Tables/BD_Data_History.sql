﻿CREATE TABLE [dbo].[BD_Data_History] (
    [bd_data_history_id]                     INT           IDENTITY (1, 1) NOT NULL,
    [application_id]                         INT           NOT NULL,
    [e_application_id]                       CHAR (10)     NULL,
    [award_id]                               CHAR (21)     NULL,
    [application_received_date]              DATE          NULL,
    [repayment_plan_reason_id]               INT           NULL,
    [loans_at_other_servicers]               BIT           NULL,
    [spouse_id]                              INT           NULL,
    [joint_repayment_plan_request]           BIT           NULL,
    [family_size]                            INT           NULL,
    [tax_year]                               INT           NULL,
    [filing_status_id]                       INT           NULL,
    [adjusted_gross_income]                  MONEY         NULL,
    [agi_reflects_current_income]            BIT           NULL,
    [manually_submitted_income_indicator]    BIT           NULL,
    [manually_submitted_income]              MONEY         NULL,
    [supporting_documentation_required]      BIT           NULL,
    [supporting_documentation_received_date] DATE          NULL,
    [taxes_filed_flag]                       BIT           DEFAULT ((0)) NULL,
    [borrower_selected_lowest_plan]          BIT           DEFAULT ((0)) NULL,
    [created_at]                             DATETIME      CONSTRAINT [DF_BD_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_by]                             VARCHAR (50)  NOT NULL,
    [RepaymentTypeProcessedNotSame]          BIT           NULL,
    [number_children]                        INT           NULL,
    [number_dependents]                      INT           NULL,
    [current_def_forb_id]                    INT           NULL,
    [public_service_employment]              VARCHAR (200) NULL,
    [reduced_payment_forbearance]            MONEY         NULL,
    [marital_status_id]                      INT           NULL,
    CONSTRAINT [PK_BD_Data_History] PRIMARY KEY CLUSTERED ([bd_data_history_id] ASC)
);

