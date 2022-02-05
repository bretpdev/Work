/*SUPPLEMENTAL FILE LOADER*/
%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA;

%MACRO dropTable(tbl);
PROC SQL noprint;
CONNECT TO ODBC AS BANA (&BANA);
SELECT *
FROM CONNECTION TO BANA (
drop table &TBL
);
DISCONNECT FROM BANA;
QUIT;
%MEND;

%MACRO loadTable(tbl);
%dropTable(&TBL);
DATA BANA.&TBL;
SET &TBL;
RUN;
%MEND;

/*PRECLAIMS LOAD
DATA PRECLAIMS;
	INFILE 'T:/PRECLAIMS.TXT' DSD MISSOVER FIRSTOBS=2;
	FORMAT 	SSN $9. PKT $1. CMLNID $19. LOANIDENT $24. PCASTATUS $1. PCADATE $10.;
	INPUT 	@2 SSN $ @14 PKT $ @21 CMLNID $ @43 LOANIDENT $ @67 PCASTATUS $ @79 PCADATE $;
RUN;
*/

/*the data script below references UNH 25729*/
DATA ACH_BillType;
	INFILE 'Q:\UHEAA Projects\BANA\Xerox Test Files\J00BX_829769_INCENTIVE_SC370R5_2015101310192678_UU.CSV.asc' DSD MISSOVER FIRSTOBS=2;
	INFORMAT /*informats determine how data values are read into a SAS data set*/
		Deal_ID $2. 
		Borrower_SSN $11. 
		Borrower_Last_Name $13. 
		Borrower_First_Name $9. 
		Commonline_Unique_ID $22. 
		Loan_Guarantee_Date $8. 
		Borrower_Benefit_Code $6. 
		Plan_Applied_Date $8. 
		On_Time_Payments $7. 
		Interest_Status $7. 
		Interest_Applied $8. 
		Rebate_Status $7. 
		Rebate_Applied $8. 
		Rebate_Amount $8. 
		Disqualification_Date $8. 
		ACH_Status $7. 
		SPC_Status $7. 
		Loan_Ident $20. 
		Subsidy_Cd $3. 
		Reduced_Interest_Rate $9. 
		Plan_Seq $3. 
		Program_End $8. 
		Incent_Plan_Opt $3.
	;
	FORMAT /*formats write values out using some particular form*/
		Deal_ID $2. 
		Borrower_SSN $11. 
		Borrower_Last_Name $13. 
		Borrower_First_Name $9. 
		Commonline_Unique_ID $22. 
		Loan_Guarantee_Date $8. 
		Borrower_Benefit_Code $6. 
		Plan_Applied_Date $8. 
		On_Time_Payments $7. 
		Interest_Status $7. 
		Interest_Applied $8. 
		Rebate_Status $7. 
		Rebate_Applied $8. 
		Rebate_Amount $8. 
		Disqualification_Date $8. 
		ACH_Status $7. 
		SPC_Status $7. 
		Loan_Ident $20. 
		Subsidy_Cd $3. 
		Reduced_Interest_Rate $9. 
		Plan_Seq $3. 
		Program_End $8. 
		Incent_Plan_Opt $3.
	;
	INPUT
		Deal_ID $
		Borrower_SSN $
		Borrower_Last_Name $
		Borrower_First_Name $
		Commonline_Unique_ID $
		Loan_Guarantee_Date $
		Borrower_Benefit_Code $
		Plan_Applied_Date $
		On_Time_Payments $
		Interest_Status $
		Interest_Applied $
		Rebate_Status $
		Rebate_Applied $
		Rebate_Amount $
		Disqualification_Date $
		ACH_Status $
		SPC_Status $
		Loan_Ident $
		Subsidy_Cd $
		Reduced_Interest_Rate $
		Plan_Seq $
		Program_End $
		Incent_Plan_Opt $
	;
RUN;
%loadTable(ACH_BillType);
