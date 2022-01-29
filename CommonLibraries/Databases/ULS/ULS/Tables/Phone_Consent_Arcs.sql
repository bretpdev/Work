CREATE TABLE [dbo].[Phone_Consent_Arcs] (
    [phone_consent_arc_id] INT         IDENTITY (1, 1) NOT NULL,
    [arc]                  VARCHAR (5) NOT NULL,
    [compass]              BIT         NOT NULL,
    [endorser]             BIT         NOT NULL,
    CONSTRAINT [PK_Phone_Consent_Arcs] PRIMARY KEY CLUSTERED ([phone_consent_arc_id] ASC)
);

