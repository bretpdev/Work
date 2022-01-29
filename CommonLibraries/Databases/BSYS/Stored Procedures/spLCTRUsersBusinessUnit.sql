CREATE PROCEDURE dbo.spLCTRUsersBusinessUnit

@BU			varchar(200)

AS

select distinct B.FirstName + ' ' + B.LastName as eName
from GENR_REF_BU_Agent_Xref A
inner join SYSA_LST_Users B
on A.WindowsUserID = B.WindowsUserName
where A.BusinessUnit = @BU
and A.Role = 'Member Of'

FOR XML AUTO, ELEMENTS