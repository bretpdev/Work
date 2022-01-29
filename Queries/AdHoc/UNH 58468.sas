/*creates special message table to local permanent data set for use in SAS script UTLWS14*/

LIBNAME SPECMSG 'T:\SAS';
DATA SPECMSG.SPECIAL_MESSAGE_example;	
INPUT 
	MESSAGE_ID $ 1-2 
	MESSAGE $ 3-222; *change number to match the length of the message;
DATALINES;
1 NOW AVAILABLE: Sign up for Autopay directly through your online portal without having to submit a form! Never miss a payment again and log into your online account through https://UHEAA.org to sign up for Autopay, today!
2 NOW AVAILABLE: Sign up for Autopay directly through your online portal without having to submit a form! Never miss a payment again and log into your online account through https://UHEAA.org to sign up for Autopay, today!
;
LIBNAME SPECMSG CLEAR;
