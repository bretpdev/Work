--use this as a template for doing data-driven SSRS reports
--data from UHEAASSRS:
USE Subscriptions
GO

SELECT * FROM [dbo].[ReportFormats]
SELECT * FROM [dbo].[Reports]
SELECT * FROM [dbo].[Subscribers]
SELECT * FROM [dbo].[Subscriptions]
where ReportId =16--in (7,8,9,10)
order by ReportId,SubscriberId
--42	Alexander
--43	Shannon
--44	Nicholas
--45	Jesse
--50	Jay
--51	Jacob
--52	Jeremy
--53	John

--7	Need Help Time Tracking - Long Running
--8	Need Help Time Tracking - Still Running
--9	Need Help Time Tracking - Overlapping
--10 Need Help Time Tracking - Long Running_10hrs

--INSERT INTO [dbo].[Reports] (ReportName)
--VALUES ('Timesheet Processing')

--INSERT INTO [dbo].[Subscribers] 
--(
--	[FirstName]
--	,[LastName]
--	,[FullName]
--	,[EmailAddress]
--	,[ManagerEmail]
--)
--VALUES
--	('Jacob','Kramer','Jacob Kramer','jkramer@utahsbr.edu','jryan@utahsbr.edu'),
--	('Jeremy','Blair','Jeremy Blair','jblair@utahsbr.edu','whack@utahsbr.edu'),
--	('John','Hyde','John Hyde','jhyde@utahsbr.edu','whack@utahsbr.edu')

----Long Running:
--INSERT INTO [Subscriptions].[dbo].[Subscriptions] (ReportId,SubscriberId,ReportFormatId)
--VALUES
-- (7,42,2)--Alexander
--,(7,43,2)--Shannon
--,(7,44,2)--Nicholas
--,(7,45,2)--Jesse
--,(7,50,2)--Jay
--,(7,51,2)--Jacob
--,(7,52,2)--Jeremy
--,(7,53,2)--John

----Still Running:
--INSERT INTO [Subscriptions].[dbo].[Subscriptions] (ReportId,SubscriberId,ReportFormatId)
--VALUES
-- (8,42,2)--Alexander
--,(8,43,2)--Shannon
--,(8,44,2)--Nicholas
--,(8,45,2)--Jesse
--,(8,50,2)--Jay
--,(8,51,2)--Jacob
--,(8,52,2)--Jeremy
--,(8,53,2)--John

----Overlapping:
--INSERT INTO [Subscriptions].[dbo].[Subscriptions] (ReportId,SubscriberId,ReportFormatId)
--VALUES
-- (9,42,2)--Alexander
--,(9,43,2)--Shannon
--,(9,44,2)--Nicholas
--,(9,45,2)--Jesse
--,(9,50,2)--Jay
--,(9,51,2)--Jacob
--,(9,52,2)--Jeremy
--,(9,53,2)--John

----Long Running 10+ hrs:
--INSERT INTO [Subscriptions].[dbo].[Subscriptions] (ReportId,SubscriberId,ReportFormatId)
--VALUES
-- (10,45,2)--Jesse
--,(10,50,2)--Jay
--,(10,51,2)--Jacob
--,(10,52,2)--Jeremy
--,(10,53,2)--John

----Timesheet Processing:
--INSERT INTO [Subscriptions].[dbo].[Subscriptions] (ReportId,SubscriberId,ReportFormatId)
--VALUES
-- (16,3,2)	--Evan Walker
--,(16,5,2)	--Bret Pehrson
--,(16,6,2)	--Jarom	Ryan
--,(16,7,2)	--Debbie Phillips
--,(16,8,2)	--Colton McComb
--,(16,10,2)	--Riley	Bigelow
--,(16,11,2)	--Melanie Garfield
--,(16,23,2)	--Steven Ostler
--,(16,24,2)	--Joshua Wright
--,(16,25,2)	--Wendy	Hack
--,(16,26,2)	--J.R. Nolasco
--,(16,27,2)	--Conor	MacDonald
--,(16,29,2)	--Jessica Hanson
--,(16,33,2)	--Aaron	Villont
--,(16,34,2)	--Jared	Kieschnick
--,(16,35,2)	--Savanna Gregory
--,(16,36,2)	--Ryan Allred
--,(16,37,2)	--Candice Cole
--,(16,38,2)	--Adam Isom
--,(16,40,2)	--David	Halladay
--,(16,42,2)	--Alexander	Larson
--,(16,43,2)	--Shannon Legge
--,(16,44,2)	--Nicholas Burnham
--,(16,45,2)	--Jesse	Gutierrez
--,(16,50,2)	--Jay Davis
--,(16,51,2)	--Jacob	Kramer
--,(16,52,2)	--Jeremy Blair
--,(16,53,2)	--John Hyde

----used in data-driven SSRS report:
--SELECT
--	*
--FROM
--	Reports R
--	INNER JOIN Subscriptions S 
--		ON S.ReportId = R.ReportId
--	INNER JOIN Subscribers Sb 
--		ON Sb.SubscriberId = S.SubscriberId
--	INNER JOIN ReportFormats RF 
--		ON RF.ReportFormatId = S.ReportFormatId
--WHERE
--	R.ReportName = 'Need Help Time Tracking - Overlapping'

