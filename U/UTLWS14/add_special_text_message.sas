/*creates special message table to local permanent data set for use in SAS script UTLWS14*/

LIBNAME SPECMSG 'Q:\Support Services\Test Files\SAS\SASR 4106';
DATA SPECMSG.SPECIAL_MESSAGE;	
INPUT MESSAGE_ID $ MESSAGE $ 3-4; *change number to match the length of the message;
DATALINES;
1   
2   
;
