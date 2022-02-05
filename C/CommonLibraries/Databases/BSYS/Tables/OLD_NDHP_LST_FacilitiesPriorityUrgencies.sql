CREATE TABLE [dbo].[OLD_NDHP_LST_FacilitiesPriorityUrgencies] (
    [UrgencyOption] VARCHAR (100) NOT NULL,
    [Urgency]       VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_NDHP_LST_FacilitiesPriorityUrgencies] PRIMARY KEY CLUSTERED ([UrgencyOption] ASC)
);

