/*NOTE:  you must open XES-XXXXXX.xlsx from the NH ticket using the password in the ticket and delete the first two (blank) rows before running this query*/

LIBNAME XL 'T:\XES-XXXXXX.xlsx';
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;

DATA FSA;
	SET XL.'t-j-all-XXXXe$'N;
RUN;

DATA LEGEND.FSA; SET FSA; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE AWD AS
		SELECT
			PDXX.DM_PRS_X AS First_Name,
			PDXX.DM_PRS_MID AS Middle_Name,
			PDXX.DM_PRS_LST AS Last_Name,
			LNXX.LN_SEQ AS Loan_Sequence,
			LNXX.IC_LON_PGM AS Loan_Type,
			PDXX.DF_SPE_ACC_ID AS Account_Number,
			CATX('',FSXX.LF_FED_AWD,PUT(FSXX.LN_FED_AWD_SEQ,ZX.)) AS Award_ID,
			FSA.TIVA
		FROM
			PKUB.PDXX_PRS_NME PDXX
			JOIN PKUB.LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			JOIN PKUB.FSXX_DL_LON FSXX
				ON LNXX.BF_SSN = FSXX.BF_SSN
				AND LNXX.LN_SEQ = FSXX.LN_SEQ
			JOIN FSA
				ON CATX('',FSXX.LF_FED_AWD,PUT(FSXX.LN_FED_AWD_SEQ,ZX.)) = FSA.AWARD
	;
QUIT;
ENDRSUBMIT;

DATA AWD; SET LEGEND.AWD; RUN;

DATA TIVAX (DROP=TIVA) TIVAX (DROP=TIVA);
	SET AWD;
	IF TIVA = X THEN OUTPUT TIVAX;
	ELSE OUTPUT TIVAX;
RUN;

PROC EXPORT
		DATA=TIVAX
		OUTFILE='T:\SAS\XE Request for FSA.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=TIVAX
		OUTFILE='T:\SAS\XE Request for FSA.XLSX'
		REPLACE;
RUN;


/*Please locate all award IDs in the Federal Region which appear on the list provided by FSA. Attributes for award ID are found in table AYFX_ATY_AWD_DTL and are LF_ATY_AWD_DTL_AWD and*/
/*LN_ATY_AWD_ID_SEQ. The values in the above described fields must be concatenated to match the award IDs on the provided list. Where award IDs exist, please provide on the output; first and last name, loan sequence number, loan type and account number as well as the above mentioned award ID.*/
/**/
/*PDXX_PRS_NME*/
/*�	DM_PRS_MID � Middle Name*/
/*�	DM_PRS_X � First Name*/
/*�	DM_PRS_LST � Last Name*/
/*�	DF_SPE_ACC_ID � Account number*/
/**/
/*LNXX_LON*/
/*�	LN_SEQ � Loan Sequence*/
/*�	IC_LON_PGM � Loan Type*/
/**/
/*AYFX_ATY_AWD_DTL*/
/*�	LF_ATY_AWD_DTL_AWD � Award ID*/
/*�	LN_ATY_AWD_ID_SEQ � Award ID suffix*/
/**/
/*As Sasha would like the output on two reports, based on values in the attached spread sheet, */
/*e.g. one list for the above mentioned criteria where the value in the column titled �tiva� = X, */
/*and one list for the above mentioned criteria where the value in the column titled �tiva� = all other values, */
/*it may be best to work out that division before running the query. Please let me know if you have any questions. */
/*In order to open the attached spread sheet, a password is required, which will be provided once the programmer has been assigned. */
