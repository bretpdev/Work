﻿CREATE TABLE [dbo].[ITGLSQLDF_Borr_Hist] (
    [br_ssn]          VARCHAR (9)  NULL,
    [bh_date]         VARCHAR (7)  NULL,
    [bh_time]         VARCHAR (6)  NULL,
    [bh_seq_num]      VARCHAR (2)  NULL,
    [bc_hist_code]    VARCHAR (3)  NULL,
    [bh_group]        VARCHAR (1)  NULL,
    [bh_type]         VARCHAR (1)  NULL,
    [bh_text]         VARCHAR (60) NULL,
    [bh_user_id]      VARCHAR (10) NULL,
    [bh_group_agings] VARCHAR (23) NULL,
    [BHHBST]          VARCHAR (1)  NULL,
    [BHHCST]          VARCHAR (1)  NULL,
    [BHHEST]          VARCHAR (2)  NULL,
    [BHHFST]          VARCHAR (3)  NULL,
    [BHGRCD]          VARCHAR (4)  NULL,
    [bh_program_id]   VARCHAR (10) NULL,
    [BHI4ND]          VARCHAR (3)  NULL,
    [BHI5ND]          VARCHAR (3)  NULL,
    [BHQKVA]          VARCHAR (5)  NULL,
    [BHQMVA]          VARCHAR (9)  NULL,
    [BHEDDT]          VARCHAR (7)  NULL,
    [BHEEDT]          VARCHAR (7)  NULL,
    [BHMYST]          VARCHAR (1)  NULL,
    [BHMZST]          VARCHAR (1)  NULL,
    [BHM0ST]          VARCHAR (1)  NULL,
    [BHM1ST]          VARCHAR (2)  NULL,
    [BHQLVA]          VARCHAR (11) NULL
);


GO
CREATE CLUSTERED INDEX [ClusteredIndex-20130628-161400]
    ON [dbo].[ITGLSQLDF_Borr_Hist]([br_ssn] ASC, [bh_seq_num] ASC, [bh_date] ASC, [bh_time] ASC);

