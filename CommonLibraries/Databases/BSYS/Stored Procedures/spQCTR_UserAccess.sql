CREATE PROCEDURE dbo.spQCTR_UserAccess 

@WhatToDo		VARCHAR(10) = 'VIEW', --could be ADD, REMOVE or VIEW
@UserID		VARCHAR(50) = '',
@BU 			VARCHAR(50) = ''

AS

--ADD or remove if needed
IF @WhatToDo = 'ADD'
BEGIN
	INSERT INTO GENR_REF_BU_Agent_Xref (BusinessUnit, WindowsUserID, Role) VALUES (@BU, @UserID, 'Authorized QC')
END
ELSE IF @WhatToDo = 'REMOVE'
BEGIN
	DELETE FROM GENR_REF_BU_Agent_Xref WHERE BusinessUnit = @BU AND WindowsUserID = @UserID AND Role =  'Authorized QC'
END

--return results
SELECT A.BusinessUnit, 
	B.FirstName + ' ' + B.LastName as UserName,
	A.WindowsUserID
FROM GENR_REF_BU_Agent_Xref A
JOIN dbo.SYSA_LST_Users B
	ON A.WindowsUserID = B.WindowsUserName
WHERE A.Role = 'Authorized QC'