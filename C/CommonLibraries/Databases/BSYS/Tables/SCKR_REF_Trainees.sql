CREATE TABLE [dbo].[SCKR_REF_Trainees] (
    [Request]     INT           NOT NULL,
    [Class]       NVARCHAR (50) NOT NULL,
    [Trainee]     NVARCHAR (50) NOT NULL,
    [TrainedDate] SMALLDATETIME NULL,
    [Initialed]   BIT           CONSTRAINT [DF_SCKR_REF_Trainees_Initialed] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_refTrainees] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC, [Trainee] ASC) WITH (FILLFACTOR = 90)
);

