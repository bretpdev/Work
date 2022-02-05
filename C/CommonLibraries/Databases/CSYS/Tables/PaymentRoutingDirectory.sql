CREATE TABLE [dbo].[PaymentRoutingDirectory] (
    [routing_number]          VARCHAR (9)  NULL,
    [office_code]             VARCHAR (1)  NULL,
    [servicing_FRB_number]    VARCHAR (9)  NULL,
    [record_type_code]        VARCHAR (1)  NULL,
    [change_date]             VARCHAR (6)  NULL,
    [new_routing_number]      VARCHAR (9)  NULL,
    [customer_name]           VARCHAR (36) NULL,
    [address]                 VARCHAR (36) NULL,
    [city]                    VARCHAR (20) NULL,
    [state_code]              VARCHAR (2)  NULL,
    [zipcode]                 VARCHAR (5)  NULL,
    [zipcode_extension]       VARCHAR (4)  NULL,
    [telephone_area_code]     VARCHAR (3)  NULL,
    [telephone_prefix_number] VARCHAR (3)  NULL,
    [telephone_suffix_number] VARCHAR (4)  NULL,
    [institution_status_code] VARCHAR (1)  NULL,
    [date_view_code]          VARCHAR (1)  NULL,
    [filler]                  VARCHAR (7)  NULL
);

