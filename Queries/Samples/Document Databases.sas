%MACRO DOC_DB(SV,DB);
PROC SQL;
CONNECT TO OLEDB as DB(init_string="provider=Microsoft OLE DB Provider for SQL Server; 
	Data Source=&SV;Initial Catalog=&DB;Integrated Security=SSPI; update_lock_type=nolock");
CREATE TABLE doc AS
SELECT *
FROM CONNECTION TO DB (
SELECT A.TABLE_NAME	as 'Table'
	,A.COLUMN_NAME AS NAME
	,A.DATA_TYPE
	,A.CHARACTER_MAXIMUM_LENGTH AS 'LENGTH'
	,A.NUMERIC_PRECISION
	,A.NUMERIC_SCALE
	,case
		when E.TABLE_CATALOG IS NULL THEN ''
		ELSE 'X'
	END AS 'PK'
		,h.NAME AS 'FK_Table'
		,g.name as 'FK_Column'
	,case 
		when A.IS_NULLABLE = 'NO' THEN 'X'
		ELSE ''
	END AS REQUIRED
	,CASE
		WHEN A.COLUMN_DEFAULT IS NULL THEN ''
		ELSE 'X'
	END AS 'WITH DEFAULT'
	,C.VALUE AS 'COMMENT'
	,a.ordinal_position
from information_schema.columns	A
INNER JOIN SYS.TABLES D
	ON A.TABLE_NAME = D.NAME
INNER JOIN SYS.COLUMNS B
	ON A.COLUMN_NAME = B.NAME
	AND D.OBJECT_ID = B.OBJECT_ID
LEFT OUTER JOIN sys.FOREIGN_KEY_COLUMNS F
	ON B.OBJECT_ID = F.PARENT_OBJECT_id
	and a.ordinal_position = F.parent_column_id
LEFT OUTER JOIN SYS.tables h
	ON F.REFERENCED_OBJECT_ID = h.OBJECT_ID
LEFT OUTER JOIN SYS.COLUMNS G
	ON F.REFERENCED_OBJECT_ID = G.OBJECT_ID
	AND F.REFERENCED_COLUMN_ID = G.column_id
LEFT OUTER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE E
	ON A.TABLE_CATALOG = E.TABLE_CATALOG
	AND A.TABLE_NAME = E.TABLE_NAME
	AND A.ORDINAL_POSITION = E.ORDINAL_POSITION
	and e.constraint_name like 'PK%'
LEFT OUTER JOIN sys.extended_properties C 
	ON C.major_id = B.object_id
	AND C.minor_id = B.column_id 
	AND C.name ='MS_Description' 
);
DISCONNECT FROM DB;
QUIT;

DATA DOC(DROP=LENGTH NUMERIC_SCALE NUMERIC_PRECISION FK_TABLE FK_COLUMN);
SET DOC;
IF LENGTH > 0 THEN DATA_TYPE = TRIM(DATA_TYPE) || "(" || TRIM(LEFT(PUT(LENGTH,10.0)))|| ")";
ELSE IF NUMERIC_SCALE > 0 
	THEN DATA_TYPE = TRIM(DATA_TYPE) || "(" || TRIM(LEFT(PUT(NUMERIC_PRECISION,10.0)))|| ',' || TRIM(LEFT(PUT(NUMERIC_SCALE,3.0)))|| ")";
IF FK_TABLE ^= '' THEN FK = TRIM(FK_TABLE) || '.' || FK_COLUMN;
RUN;
PROC SORT DATA=DOC; BY TABLE ORDINAL_POSITION; RUN;


ODS _ALL_ CLOSE;
TITLE "&DB Data Model";
title2 "Server = &SV";
ODS PDF FILE="T:\SAS\&SV - &DB Data Model.pdf" UNIFORM  PDFTOC=2 style=sansprinter;
/*ODS rtf FILE="T:\SAS\&DB Data Model.rtf" style=sansprinter;*/
PROC PRINT DATA=DOC width=UNIFORM noobs LABEL;
BY TABLE;
VAR NAME DATA_TYPE PK FK REQUIRED WITH_DEFAULT COMMENT;
LABEL DATA_TYPE = 'TYPE'
	COMMENT = 'DESCRIPTION';
RUN;
ODS PDF CLOSE;
/*ods rtf close;*/
ODS LISTING;
%MEND;

%MACRO ALL_DB(SERVER);
PROC SQL NOPRINT;
CONNECT TO OLEDB as DB(init_string="provider=Microsoft OLE DB Provider for SQL Server; 
	Data Source=&SERVER;Integrated Security=SSPI; update_lock_type=nolock");

SELECT NAME INTO: DB_LIST SEPARATED BY ' '
FROM CONNECTION TO DB (
select distinct name
from [model].[sys].[sysdatabases]
where dbid > 4
);

SELECT COUNT(*) INTO: DB_CT 
FROM CONNECTION TO DB (
select distinct name
from [model].[sys].[sysdatabases]
where dbid > 4
);
quit;
%PUT &DB_LIST;
%PUT &DB_CT;
%DO I = 1 %TO &DB_CT; 
	%DOC_DB(&SERVER,%SCAN(&DB_LIST,&I));
%END;
%MEND;

/*For an entire server (access required):*/
/*%ALL_DB(OPSDEV);*/

/*For a specific database:*/
%DOC_DB(UHEAASQLDB,UDW);
%DOC_DB(UHEAASQLDB,CDW);
%DOC_DB(UHEAASQLDB,ULS);
%DOC_DB(UHEAASQLDB,CLS);

