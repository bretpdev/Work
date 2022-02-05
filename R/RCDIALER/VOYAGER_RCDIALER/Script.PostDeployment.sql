/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM rcdialer.SprocMapping
DBCC CHECKIDENT('voyager.rcdialer.SprocMapping', RESEED, 0)
DELETE FROM rcdialer.[DaysOfWeek]
DBCC CHECKIDENT('voyager.rcdialer.[DaysOfWeek]', RESEED, 0)
DELETE FROM rcdialer.Sprocs
DBCC CHECKIDENT('voyager.rcdialer.Sprocs', RESEED, 0)
DELETE FROM rcdialer.BucketMapping
DBCC CHECKIDENT('voyager.rcdialer.BucketMapping', RESEED, 0)

INSERT INTO rcdialer.Sprocs(SprocName, IsAlternateWeek)
VALUES('rcdialer.Load30DayCalls', 1)
,('rcdialer.Load60DayCalls', 1)
,('rcdialer.Load90DayCalls', 1)
,('rcdialer.Load270DayCalls', 1)
,('rcdialer.Load270DayCalls', 0)

INSERT INTO rcdialer.[DaysOfWeek]([DayOfWeek])
VALUES('Monday')
,('Tuesday')
,('Wednesday')
,('Friday')

INSERT INTO rcdialer.SprocMapping(DaysOfWeekId, SprocsId)
VALUES(1, 4)
,(2, 1)
,(3, 2)
,(4, 3)

INSERT INTO rcdialer.BucketMapping(Bucket, BucketBegin, BucketEnd)
VALUES(30, 30, 36)
,(30, 44, 50)
,(60, 61, 66)
,(60, 74, 80)
,(90, 90, 96)
,(90, 104, 110)
,(90, 116, 124)
,(90, 130, 138)
,(90, 144, 152)
,(90, 158, 166)
,(90, 172, 180)
,(90, 186, 194)
,(90, 200, 208)
,(90, 214, 222)
,(90, 228, 236)
,(90, 242, 250)
,(90, 256, 264)
,(270, 270, 330)