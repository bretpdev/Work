
/*IF THE DATASET ISNT THERE, CREATE IT*/
%MACRO BLAH;
%let dsname=OLRPLD.UTLWO02A;
%if %sysfunc(exist(&dsname))=0 %then %DO;
   DATA &DSNAME;
		LENGTH BF_SSN $ 9 LN_SEQ $ 4;
   RUN;
   %END;
%MEND;
%BLAH;