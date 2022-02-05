CREATE TABLE [dbo].[LST_FacilitiesPriorityUrgency] (
    [UrgencyOption] VARCHAR (100) NOT NULL,
    [Urgency]       VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_NDHP_LST_FacilitiesPriorityUrgencies] PRIMARY KEY CLUSTERED ([UrgencyOption] ASC) WITH (FILLFACTOR = 90)
);

