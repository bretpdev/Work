CREATE TABLE [dbo].[TimeTracking] (
    [TimeTrackingId]  INT           IDENTITY (1, 1) NOT NULL,
    [SqlUserID]       INT           NOT NULL,
    [TicketID]        INT           NULL,
    [SystemTypeId]    INT           NULL,
    [CostCenterId]    INT           NULL,
    [StartTime]       DATETIME      NULL,
    [EndTime]         DATETIME      NULL,
    [Region]          VARCHAR (11)  NOT NULL,
    [BatchProcessing] BIT           DEFAULT ((0)) NOT NULL,
    [GenericMeeting]  VARCHAR (250) NULL,
    CONSTRAINT [PK_TimeTracking] PRIMARY KEY CLUSTERED ([TimeTrackingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [CK_TimeTracking_Region] CHECK ([Region] collate latin1_general_cs_as='uheaa' OR [Region] collate latin1_general_cs_as='cornerstone'),
    CONSTRAINT [FK_TimeTracking_CostCenter] FOREIGN KEY ([CostCenterId]) REFERENCES [dbo].[CostCenter] ([CostCenterId]),
    CONSTRAINT [FK_TimeTracking_SystemType] FOREIGN KEY ([SystemTypeId]) REFERENCES [dbo].[SystemType] ([SystemTypeId])
);


GO

CREATE TRIGGER [dbo].[Trigger_TimeTrackingUpdate]
    ON [dbo].[TimeTracking]
    INSTEAD OF UPDATE
    AS
    BEGIN
        SET NoCount ON

		INSERT INTO UpdateLog(Region, TimeTrackingId, StartTimeOldValue, StartTimeNewValue, EndTimeOldValue, EndTimeNewValue, UpdateBy)
		SELECT
			tt.Region, i.TimeTrackingId, tt.StartTime, i.StartTime, tt.EndTime, i.EndTime, i.SqlUserID
		FROM
			TimeTracking tt
			JOIN inserted i
				ON tt.TimeTrackingId = i.TimeTrackingId

		UPDATE
			TimeTracking
		SET
			StartTime = i.StartTime,
			EndTime = i.EndTime,
			SystemTypeId = i.SystemTypeId,
			CostCenterId = i.CostCenterId,
			BatchProcessing = i.BatchProcessing,
			Region = i.Region,
			GenericMeeting = i.GenericMeeting
		FROM
			TimeTracking tt
			INNER JOIN inserted i
				ON tt.TimeTrackingId = i.TimeTrackingId
		WHERE
			tt.TimeTrackingId = i.TimeTrackingId
    END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enforcing case sensitivity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeTracking', @level2type = N'CONSTRAINT', @level2name = N'CK_TimeTracking_Region';

