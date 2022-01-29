CREATE TABLE [dbo].[Deferment_Forbearance_Mapping] (
    [deferment_forbearance_mapping_id] INT           IDENTITY (1, 1) NOT NULL,
    [nelnet_code]                      CHAR (3)      NULL,
    [compass_code]                     CHAR (2)      NULL,
    [description]                      VARCHAR (100) NULL,
    [cap_indicator]                    CHAR (1)      NULL
);

