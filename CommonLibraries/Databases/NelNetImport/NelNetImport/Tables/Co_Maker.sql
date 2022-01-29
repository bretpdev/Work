CREATE TABLE [dbo].[Co_Maker] (
    [br_ssn]                    VARCHAR (9)   NULL,
    [br_name_first]             VARCHAR (14)  NULL,
    [br_name_last]              VARCHAR (25)  NULL,
    [br_name_mi]                VARCHAR (1)   NULL,
    [br_address_status]         VARCHAR (1)   NULL,
    [br_use_perm_temp_sw]       VARCHAR (1)   NULL,
    [br_perm_time_zone]         VARCHAR (3)   NULL,
    [br_perm_addresss_source]   VARCHAR (1)   NULL,
    [br_perm_foreign_country]   VARCHAR (1)   NULL,
    [br_perm_address_1]         VARCHAR (30)  NULL,
    [br_perm_address_2]         VARCHAR (30)  NULL,
    [br_perm_city]              VARCHAR (18)  NULL,
    [br_perm_zip_code]          VARCHAR (15)  NULL,
    [cr_country_code]           VARCHAR (4)   NULL,
    [st_state_code]             VARCHAR (2)   NULL,
    [cy_county]                 VARCHAR (3)   NULL,
    [br_perm_intnatl_ph_exchg]  VARCHAR (4)   NULL,
    [br_perm_area_code]         VARCHAR (5)   NULL,
    [br_perm_phone_number]      VARCHAR (11)  NULL,
    [br_perm_phone_ext]         VARCHAR (6)   NULL,
    [br_perm_call_time_ind]     VARCHAR (1)   NULL,
    [br_perm_foreign_phone_ind] VARCHAR (1)   NULL,
    [br_perm_phone_status]      VARCHAR (1)   NULL,
    [filler_1]                  VARCHAR (168) NULL,
    [br_date_of_birth]          VARCHAR (7)   NULL,
    [filler_2]                  VARCHAR (22)  NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20131018-180726]
    ON [dbo].[Co_Maker]([br_ssn] ASC);

