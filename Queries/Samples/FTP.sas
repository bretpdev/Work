
FILENAME FILE1 
FTP '/uheaa/whse/olrp_lookup_directory/utlwo2_1.txt'
HOST='205.238.227.77'
PROMPT
USER='mcowley';


DATA _NULL_;
SET SALEIDS;
FILE FILE1 DLM=',' DSD DROPOVER;
PRELIM1 = ' ';
PRELIM2 = ' ';
FORMAT ID_SEL_NXT_PLR ID_LON_SLE MMDDYY8.;
PUT IF_LON_SLE IF_SLL_OWN IF_BUY_OWN 
PRELIM1 PRELIM2
ID_SEL_NXT_PLR ID_LON_SLE;

RUN;