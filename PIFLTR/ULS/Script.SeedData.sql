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

INSERT INTO [Uls].[pifltr].[ProcessingQueue](Queue,SubQueue,TaskControlNumber,RequestArc, TaskRequestedDate, Ssn,AccountNumber,AddedAt,AddedBy)
select  
	WF_QUE AS Queue,
	WF_SUB_QUE AS SubQueue,
	WN_CTL_TSK AS TaskControlNumber,
	PF_REQ_ACT as RequestArc,
	WD_INI_TSK AS TaskRequestedDate,
	BF_SSN As Ssn,
	(SELECT DF_SPE_ACC_ID FROM [Udw].[dbo].[PD10_PRS_NME]
	WHERE BF_SSN = DF_PRS_ID) AS AccountNumber,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
FROM
	OPENQUERY 
		(QADBD004,'
			SELECT 
				*
			FROM OLWHRM1.WQ20_TSK_QUE
				WHERE 
				WF_QUE = ''R9''
				AND WF_SUB_QUE = ''01''
				OR (WF_SUB_QUE = ''02'' AND WF_QUE = ''R9'')
				--AND WD_ACT_REQ between sysdate - 14 and sysdate
				')

				--TODO: LEFT JOIN ONTO PIFLTR.PROCESSINGQUEUE TABLE AND LOOK AT TASKCONTROLNUMBER; ONLY ADD RECORDS IF TASKCONTROLNUMBER IS NULL
				--TODO: DOES THIS NEED TO BE A SPROC ON ULS?  OR ARE WE CALLING THIS DIRECTLY FROM WITHIN THE CODE?

