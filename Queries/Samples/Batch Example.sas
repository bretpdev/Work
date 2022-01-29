/********************************************************************
* This is an example of using BY variable processing to create batch
* IDs within a SAS file.
*********************************************************************/
DATA BATCH_A;
SET REPUR;
RUN;
/********************************************************************
* The data set that you're working with must be sorted by the variables
* you have as keys.
*********************************************************************/
PROC SORT DATA=BATCH_A;
BY SSN CLUID;
RUN;
/********************************************************************
* Once the data is sorted use the BY variables in a data step to 
* determine how many (in this case) borrowers you want in a batch. Note the 
* BY variables are the same as in the PROC SORT above. SAS will distinguish
* between (in this case) the first SSN of an occurance of SSNs and the 
* rest of the SSNs. Then it will execure the DO statements for the first
* SSN.
*********************************************************************/
DATA BATCH_A;
SET BATCH_A;
BY SSN CLUID;
RETAIN SUB_BAT_ID;
IF FIRST.SSN THEN DO;
	IF SUB_BAT_ID = 5 THEN SUB_BAT_ID = 1;
		ELSE SUB_BAT_ID+1;
	END;
RUN;
