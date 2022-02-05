/*PROD*/
LIBNAME CSYS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn;" ;
LIBNAME NHU ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpUheaa.dsn;" ;
LIBNAME NHCS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpCornerStone.dsn;" ;
%LET RPTLIB = Q:\Support Services\NH-Closed Tickets;

/*TEST*/
/*LIBNAME CSYS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CSYSTEST.dsn;" ;*/
/*LIBNAME NHU ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpUheaa_TEST.dsn;" ;*/
/*LIBNAME NHCS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpCornerStone_TEST.dsn;" ;*/
/*%LET RPTLIB = T:\SAS;*/

data _null_  ;
   begin=intnx('month',today(),-1,'beginning')  ;
      call symput  ( 'begin', "'"||put(begin,date9.)||"'" )   ;

   end=intnx('month',today(),-1,'end')  ;
	  call symput  ( 'end', "'"||put(end,date9.)||"'" )   ;
run;

PROC SQL;
CREATE TABLE UHEAA AS
SELECT	A.Ticket AS Ticket_Number
		,A.TicketCode AS Ticket_Type
		,A.Requested AS Submission_Date
		,A.StatusDate AS Closed_Date
		,D.Name AS Business_Unit
		,CATX(' ',C.FirstName,C.LastName) AS Assigned_To
		,A.Issue AS Issue_Text
		,A.ResolutionCause AS Cause
		,A.ResolutionFix AS Fix
		,A.ResolutionPrevention AS Prevention
FROM	NHU.DAT_Ticket A
		INNER JOIN CSYS.GENR_LST_BusinessUnits D
			ON A.Unit = D.ID
		INNER JOIN CSYS.FLOW_DAT_FlowStep E
			ON A.TicketCode = E.FlowID
			AND E.ControlDisplayText = ''
		LEFT OUTER JOIN NHU.DAT_TicketsAssociatedUserID B
			ON A.Ticket = B.Ticket
			AND B.Role = 'AssignedTo'
		LEFT OUTER JOIN CSYS.SYSA_DAT_Users C
			ON B.SqlUserId = C.SqlUserId
WHERE	A.TicketCode NOT IN
			(
				'FAC',
				'FAR',
				'FAR Fed',
				'FTRANS',
				'FTRANS Fed',
				'IR',
				'THR',
				'ACCM',
				'ACTD',
				'ARCA',
				'ARCC',
				'ARCM',
				'BBO',
				'DCR',
				'IDEM',
				'LPDS',
				'PARM',
				'SASR',
				'QASMT',
				'QCR',
				'TOC',
				'ACTD Fed',
				'ARCA Fed',
				'ARCC Fed',
				'ARCM Fed',
				'BBO Fed',
				'DCR Fed',
				'IDEM Fed',
				'LPDS Fed',
				'QASMT Fed',
				'QCR Fed',
				'TOC Fed'
			)
		AND A.Status = E.Status
		AND A.StatusDate BETWEEN "&begin"d AND "&end"d
ORDER BY A.Ticket
;
QUIT;

PROC EXPORT DATA= WORK.UHEAA
            OUTFILE= "&RPTLIB\NH - Closed Tickets - UHEAA.&sysdate..xls" 
            DBMS=EXCEL REPLACE;
     		SHEET="UHEAA"; 
RUN;

PROC SQL;
CREATE TABLE CS AS
SELECT	A.Ticket AS Ticket_Number
		,A.TicketCode AS Ticket_Type
		,A.Requested AS Submission_Date
		,A.StatusDate AS Closed_Date
		,D.Name AS Business_Unit
		,CATX(' ',C.FirstName,C.LastName) AS Assigned_To
		,A.Issue AS Issue_Text
		,A.ResolutionCause AS Cause
		,A.ResolutionFix AS Fix
		,A.ResolutionPrevention AS Prevention
FROM	NHCS.DAT_Ticket A
		INNER JOIN CSYS.GENR_LST_BusinessUnits D
			ON A.Unit = D.ID
		INNER JOIN CSYS.FLOW_DAT_FlowStep E
			ON A.TicketCode = E.FlowID
			AND E.ControlDisplayText = ''
		LEFT OUTER JOIN NHCS.DAT_TicketsAssociatedUserID B
			ON A.Ticket = B.Ticket
			AND B.Role = 'AssignedTo'
		LEFT OUTER JOIN CSYS.SYSA_DAT_Users C
			ON B.SqlUserId = C.SqlUserId
WHERE	A.TicketCode NOT IN
			(
				'FAC',
				'FAR',
				'FAR Fed',
				'FTRANS',
				'FTRANS Fed',
				'IR',
				'THR',
				'ACCM',
				'ACTD',
				'ARCA',
				'ARCC',
				'ARCM',
				'BBO',
				'DCR',
				'IDEM',
				'LPDS',
				'PARM',
				'SASR',
				'QASMT',
				'QCR',
				'TOC',
				'ACTD Fed',
				'ARCA Fed',
				'ARCC Fed',
				'ARCM Fed',
				'BBO Fed',
				'DCR Fed',
				'IDEM Fed',
				'LPDS Fed',
				'QASMT Fed',
				'QCR Fed',
				'TOC Fed'
			)
		AND A.Status = E.Status
		AND A.StatusDate BETWEEN "&begin"d AND "&end"d
ORDER BY A.Ticket
;
QUIT;

PROC EXPORT DATA= WORK.CS
            OUTFILE= "&RPTLIB\NH - Closed Tickets - CornerStone.&sysdate..xls" 
            DBMS=EXCEL REPLACE;
     		SHEET="CornerStone"; 
RUN;
