/*To get that information I believe we need a query of the PD55_PRS_PND_DMO table.*/
/*Here is the information needed in the query:*/
/*- Borrowers ID (DF_PRS_ID)*/
/*- Where Queue Identifier (WF_QUE) = R0*/
/*- Where Sub-queue Identifier (WF_SUB_QUE) = 01*/

FILENAME REPORT2 'T:\SAS\NH 17425.TXT';
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
/*first requested query for the ticket*/
/*					SELECT DISTINCT*/
/*						WF_QUE*/
/*						DF_PRS_ID*/
/*					FROM	*/
/*						OLWHRM1.PD55_PRS_PND_DMO*/
/*					WHERE	*/
/*						WF_QUE = 'R0'*/
/*						AND WF_SUB_QUE = '01'*/

					SELECT 
						*
					FROM	
/*						OLWHRM1.WQ30_TSK_STA*/
						OLWHRM1.WQ20_TSK_QUE
					WHERE	
						WF_QUE = 'R0'
						AND WF_SUB_QUE = '01'
/*						AND WC_STA_WQUE20_HST = 'U'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC EXPORT
		DATA = DEMO
		FILE = 'T:\SAS\NH 17425 - WQ20.XLS'
		REPLACE;
QUIT;

