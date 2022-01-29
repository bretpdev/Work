data WORK.SAUCE2    ;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
infile 'T:\J00BX_829769_INCENTIVE_SC370R5_2016020301130493_UO.CSV' delimiter = ',' MISSOVER DSD lrecl=32767 firstobs=2 ;
   informat Deal_ID $2. ;
   informat Borrower_SSN $11. ;
   informat Borrower_Last_Name $13. ;
   informat Borrower_First_Name $9. ;
   informat Commonline_Unique_ID $22. ;
   informat Loan_Guarantee_Date $8. ;
   informat Borrower_Benefit_Code $6. ;
   informat Plan_Applied_Date $8. ;
   informat On_Time_Payments $7. ;
   informat Interest_Status $7. ;
   informat Interest_Applied $8. ;
   informat Rebate_Status $7. ;
   informat Rebate_Applied $8. ;
   informat Rebate_Amount $8. ;
   informat Disqualification_Date $8. ;
   informat ACH_Status $7. ;
   informat SPC_Status $7. ;
   informat Loan_Ident $20. ;
   informat Subsidy_Cd $3. ;
   informat Reduced_Interest_Rate $9. ;
   informat Plan_Seq $3. ;
   informat Program_End $8. ;
   informat Incent_Plan_Opt $3. ;
   format Deal_ID $2. ;
   format Borrower_SSN $11. ;
   format Borrower_Last_Name $13. ;
   format Borrower_First_Name $9. ;
   format Commonline_Unique_ID $22. ;
   format Loan_Guarantee_Date $8. ;
   format Borrower_Benefit_Code $6. ;
   format Plan_Applied_Date $8. ;
   format On_Time_Payments $7. ;
   format Interest_Status $7. ;
   format Interest_Applied $8. ;
   format Rebate_Status $7. ;
   format Rebate_Applied $8. ;
   format Rebate_Amount $8. ;
   format Disqualification_Date $8. ;
   format ACH_Status $7. ;
   format SPC_Status $7. ;
   format Loan_Ident $20. ;
   format Subsidy_Cd $3. ;
   format Reduced_Interest_Rate $9. ;
   format Plan_Seq $3. ;
   format Program_End $8. ;
   format Incent_Plan_Opt $3. ;
input
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
if _ERROR_ then call symputx('_EFIERR_',1);  /* set ERROR detection macro variable */
run;

PROC SQL;
	UPDATE SAUCE
		SET DEAL_ID = 'UU'
		;
QUIT;

PROC SQL;
	UPDATE SAUCE2
		SET DEAL_ID = 'UO'
		;
QUIT;

DATA COMBO;
	SET SAUCE SAUCE2;
	COMMONLINE_UNIQUE_ID = COMPRESS(COMMONLINE_UNIQUE_ID,''''); 
	LOAN_IDENT = COMPRESS(LOAN_IDENT,''''); 
RUN;

LIBNAME EA27 ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no;");

PROC SQL noprint;
CONNECT TO ODBC AS EA27 (%STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no;"));
SELECT *
FROM CONNECTION TO EA27
(
      drop table CRITERIA_LOAN_SALE;
);
DISCONNECT FROM EA27;
QUIT;

DATA EA27.CRITERIA_LOAN_SALE;
SET COMBO;
RUN;
