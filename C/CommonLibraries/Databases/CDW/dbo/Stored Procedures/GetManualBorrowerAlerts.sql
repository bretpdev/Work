CREATE PROCEDURE [dbo].[GetManualBorrowerAlerts]
	@Ssn char(9)
AS

	select
		m.Message as Alert
	from 
		ManualBorrowerAlerts a
	join
		ManualBorrowerAlertMessages m on a.ManualBorrowerAlertMessageId = m.ManualBorrowerAlertMessageId
	where
		a.Ssn = @Ssn
	and
		a.StartDateInclusive <= convert(date, getdate()) 
	and
		a.EndDateInclusive >= convert(date, getdate())

RETURN 0