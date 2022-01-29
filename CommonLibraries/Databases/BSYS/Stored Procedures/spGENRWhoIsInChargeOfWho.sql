CREATE PROCEDURE dbo.spGENRWhoIsInChargeOfWho 
@Agent			nvarchar(50),
@GetUTID		char(1) = 'N',
@UnitsOnly		char(1) = 'N'

AS

Declare @RecCount	int
 /* get BU that agent is over */
SELECT A.BusinessUnit AS Unit, 
	'N' as ChildrenFound 
INTO #SubUnits
FROM dbo.GENR_REF_BU_Agent_Xref A 
JOIN (
	SELECT BusinessUnit 
	FROM dbo.GENR_LST_BusinessUnits 
	WHERE PseudoBU = 'N') B ON A.BusinessUnit = B.BusinessUnit 
	WHERE Role = 'Manager' AND WindowsUserID = @Agent

SELECT @RecCount = (0)

/* get all units under the main unit found above */
WHILE (@RecCount <> (SELECT Count(*) FROM #SubUnits))
BEGIN 
	SELECT @RecCount = (SELECT Count(*) FROM #SubUnits)
	INSERT INTO #SubUnits (Unit, ChildrenFound) 
		SELECT A.BusinessUnit AS Unit, 
			'?' as ChildrenFound 
		FROM dbo.GENR_LST_BusinessUnits A 
		JOIN (
			SELECT UNIT 
			FROM #SubUnits 
			WHERE ChildrenFound = 'N') B ON A.Parent = B.Unit 
		WHERE PseudoBU = 'N'
	UPDATE #SubUnits SET ChildrenFound = 'Y' WHERE ChildrenFound = 'N'
	UPDATE #SubUnits SET ChildrenFound = 'N' WHERE ChildrenFound = '?'
END

/*SELECT * FROM #SubUnits*/


/* get users under each child unit */
if (@GetUTID = 'Y')  /*UT IDs*/
Begin
SELECT DISTINCT C.UserID
FROM dbo.GENR_REF_BU_Agent_Xref A
JOIN (
	SELECT Unit
	FROM #SubUnits
	) B ON A.BusinessUnit = B.Unit
inner join SYSA_LST_UserIDInfo C
	on  A.WindowsUserID = C.WindowsUserName
WHERE Role IN ('Manager', 'Member Of')
End
ELSE IF @UnitsOnly = 'Y' /*BUs*/
BEGIN
	SELECT Unit
	FROM #SubUnits
END
Else /*Windows user ids*/
Begin
SELECT DISTINCT A.WindowsUserID 
FROM dbo.GENR_REF_BU_Agent_Xref A
JOIN (
	SELECT Unit
	FROM #SubUnits
	) B ON A.BusinessUnit = B.Unit
WHERE Role IN ('Manager', 'Member Of')
End