CREATE PROCEDURE [alerts].[GetManualBorrowerAlerts]
	@Ssn char(9)
AS

	select
		m.Message as Alert,
		m.AbortAfterMessageDisplay as AbortAfterMessageDisplay
	from 
		ManualBorrowerAlerts a
	join
		ManualBorrowerAlertMessages m on a.ManualBorrowerAlertMessageId = m.ManualBorrowerAlertMessageId
	where
		a.Ssn = @Ssn
	and
		(m.StartDateInclusive IS NULL or m.StartDateInclusive <= convert(date, getdate()) )
	and
		(m.EndDateInclusive IS NULL or m.EndDateInclusive >= convert(date, getdate()))

RETURN 0