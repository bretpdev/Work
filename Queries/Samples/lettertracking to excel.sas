PROC SQL;
CONNECT TO ODBC AS BSYS (REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn;");
CREATE TABLE bsys AS
SELECT *
FROM CONNECTION TO BSYS (
SELECT unit.Unit
	,FUNCT.BUSFUNCTION
    ,doc.DocName
	,doc.ID
    ,doc.Description
    ,doc.DocTyp
FROM LTDB_DAT_DocDetail doc
INNER JOIN LTDB_REF_Unit unit
      ON doc.DocName = unit.DocName
LEFT OUTER JOIN LTDB_REF_BUSINESSFUNCTION FUNCT
	  ON DOC.DOCNAME = FUNCT.DOCNAME
WHERE DOC.STATUS = 'Active'

);
DISCONNECT FROM BSYS;
QUIT;

PROC EXPORT DATA= WORK.BSYS 
            OUTFILE= "t:\sas\lettertracking.xls" 
            DBMS=EXCEL REPLACE;
     RANGE="bydocid"; 
     NEWFILE=YES;
RUN;
