--X. INSERT new systems
--X. INSERT systems replacing old systems
--X. UPDATE REF_Systems with new values
--X. DELETE deleted systems from UPDATE REF_Systems
--X. DELETE deleted and renamed systems from LST_System

USE [NeedHelpCornerStone]

--X. count of REF_System records to update - system name changes
SELECT COUNT(*) AS AUTOD FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Autodialer'
SELECT COUNT(*) AS COMP FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Compass'
SELECT COUNT(*) AS EML FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Email Tracking'
SELECT COUNT(*) AS IVR FROM [dbo].[REF_System] REFS WHERE REFS.System = 'IVR'
SELECT COUNT(*) AS MD FROM [dbo].[REF_System] REFS WHERE REFS.System = 'Maui DUDE'	

--X. count of REF_System records to delete - deleted systems
SELECT
	COUNT(*) AS CNT_X
FROM
	[dbo].[REF_System] REFS
WHERE
	REFS.System IN ('LCO','Letterwriter','New Century','OneLINK')

--X. count of LST_System records to delete - deleted and renamed ()
SELECT
	COUNT(*) AS CNT_X
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
			,'OneLINK'
			)