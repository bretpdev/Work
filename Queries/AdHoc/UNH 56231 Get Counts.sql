--1. INSERT new systems (6)
--2. INSERT systems replacing old systems (6)
--3. UPDATE REF_Systems with new values (4793)
--4. DELETE deleted systems from UPDATE REF_Systems (170)
--5. DELETE deleted and renamed systems from LST_System

USE [NeedHelpUheaa]

--3. count of FEF_System records to update - system name changes (4793)
SELECT COUNT(*) AS AUTOD FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Autodialer'
SELECT COUNT(*) AS COMP FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Compass'
SELECT COUNT(*) AS EML FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Email Tracking'
SELECT COUNT(*) AS IVR FROM [dbo].[REF_System] REFS WHERE REFS.System = 'IVR'
SELECT COUNT(*) AS MD FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Maui DUDE'	

--4. count of REF_System records to delete - deleted systems (170)
SELECT
	COUNT(*) AS CNT_4
FROM
	[dbo].[REF_System] REFS
WHERE
	REFS.System IN ('LCO','Letterwriter','New Century')

--5. count of LST_System records to delete - deleted and renamed ()
SELECT
	COUNT(*) AS CNT_5
FROM
	[dbo].[LST_System] LST
WHERE
	LST.System IN (
			'Autodialer'
			,'Compass'
			,'Email Tracking'
			,'IVR'
			,'LCO'
			,'Letterwriter'
			,'Maui DUDE'
			,'New Century'
			)