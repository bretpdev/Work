CREATE PROCEDURE [skpemlbrw].[GetReassignmentId]
AS
	SELECT 
		UI.UserID
	FROM
		QSTA_LST_QueueDetail QD
		INNER JOIN GENR_REF_BU_Agent_Xref A
			ON QD.BusinessUnit = A.BusinessUnit
		INNER JOIN SYSA_LST_UserIDInfo UI
			ON A.WindowsUserID = UI.WindowsUserName
	WHERE
		QD.QueueName = 'KSKEMAIL'
		AND A.[Role] = 'Manager'
