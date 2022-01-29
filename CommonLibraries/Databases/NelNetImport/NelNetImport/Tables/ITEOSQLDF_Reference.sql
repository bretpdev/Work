﻿CREATE TABLE [dbo].[ITEOSQLDF_Reference] (
    [br_ssn]                    VARCHAR (9)   NULL,
    [rf_number]                 VARCHAR (2)   NULL,
    [rf_name_last]              VARCHAR (25)  NULL,
    [rf_name_first]             VARCHAR (14)  NULL,
    [rf_mi]                     VARCHAR (1)   NULL,
    [rf_ssn]                    VARCHAR (9)   NULL,
    [rf_source]                 VARCHAR (1)   NULL,
    [rf_address_source]         VARCHAR (1)   NULL,
    [rf_address_1]              VARCHAR (30)  NULL,
    [rf_address_2]              VARCHAR (30)  NULL,
    [rf_city]                   VARCHAR (18)  NULL,
    [cr_country_code]           VARCHAR (4)   NULL,
    [st_state_code]             VARCHAR (2)   NULL,
    [rf_zip_code]               VARCHAR (15)  NULL,
    [rf_foreign_country]        VARCHAR (1)   NULL,
    [rf_home_call_time]         VARCHAR (1)   NULL,
    [rf_time_zone]              VARCHAR (3)   NULL,
    [rf_home_intnatl_ph_exchg]  VARCHAR (4)   NULL,
    [rf_home_area_code]         VARCHAR (5)   NULL,
    [rf_home_phone_number]      VARCHAR (11)  NULL,
    [rf_home_phone_status]      VARCHAR (1)   NULL,
    [rf_home_phone_foreign_ind] VARCHAR (1)   NULL,
    [rf_contact_sw]             VARCHAR (1)   NULL,
    [rf_add_change_date]        VARCHAR (7)   NULL,
    [rf_other_call_time]        VARCHAR (1)   NULL,
    [rf_other_intnatl_ph_exchg] VARCHAR (4)   NULL,
    [rf_other_area_code]        VARCHAR (5)   NULL,
    [rf_other_phone_number]     VARCHAR (11)  NULL,
    [rf_other_phone_type]       VARCHAR (1)   NULL,
    [rf_other_phone_status]     VARCHAR (1)   NULL,
    [rf_other_ph_foreign_ind]   VARCHAR (1)   NULL,
    [rf_changed_by_user]        VARCHAR (10)  NULL,
    [rf_relationship]           VARCHAR (1)   NULL,
    [rf_address_status]         VARCHAR (1)   NULL,
    [rf_date_last_called]       VARCHAR (7)   NULL,
    [rf_skip_trace_status]      VARCHAR (1)   NULL,
    [N]                         VARCHAR (154) NULL
);

