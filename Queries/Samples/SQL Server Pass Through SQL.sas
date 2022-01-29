/*use c:\windows\sysWOW64\odbcad32.exe to set up System DSN to BSYS (Name: BSYS; Server: NOCHOUSE; default database: BSYS)*/


PROC SQL;
	CONNECT TO ODBC (DATASRC='BSYS');
	CREATE TABLE MAILBOXES AS
		SELECT * FROM CONNECTION TO ODBC (
		SELECT
			MB.Mailbox,
			MB.UserID
		FROM
			SYSA_REF_UserID_PagecenterMailBoxes MB
			LEFT JOIN SYSA_LST_UserIDInfo UI
				ON MB.UserID = UI.UserID
				AND UI.[Date Access Removed] IS NULL
		WHERE
			UI.UserID IS NOT NULL		)
	;
QUIT;

