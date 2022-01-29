PROC EXPORT 
	DATA= WORK.DATASET
	OUTFILE= "T:\SAS\NEW XL FILE.XLSX" 
	DBMS=XLSX /*this is the correct DBMS for EXCEL 2016 */
	REPLACE /*comment out or delete this line and change the name of the sheet below if you want to add the output to a new tab in an existing spreadsheet*/
	; /*NOTE everything up to this semi-colon is actually one command, it has just been broken up on separate lines for readability*/
	SHEET="A"; 
RUN;
