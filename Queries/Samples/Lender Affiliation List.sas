/*******************************************************************
* USE THIS CODE TO CREATE A MACRO VARIABLE THAT CAN BE USED IN YOUR 
* SQL QUERY, USE AFTER THE RSUBMIT BUT BEFORE THE MAIN QUERY.
*********************************************************************/
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;

