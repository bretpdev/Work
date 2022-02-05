CREATE TABLE [dbo].[Current_Def_Forb_Options] (
    [current_def_forb_option_Id] INT           IDENTITY (1, 1) NOT NULL,
    [current_def_forb_option]    VARCHAR (300) NOT NULL,
    [file_value]                 CHAR (1)      NULL,
    PRIMARY KEY CLUSTERED ([current_def_forb_option_Id] ASC)
);

