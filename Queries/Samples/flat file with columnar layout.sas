/*write data to flat file with columnar layout*/
DATA _NULL_;
	SET MR_and_DB2_DATA;
	FILE 'T:\SAS\FSA\UNH 57234.txt';

	FORMAT BD_CLM_PAY_PRC YYMMDDn8.; /*	"n" in format specifies no separators (no dashes or slashes)*/
	FORMAT LD_TRX_EFF YYMMDDn8.;

	PUT /*"@" moves cursor to start position of the column*/

		@1 DM_PRS_LST /*char data automatically left aligned, use format and switch to alter alignment (e.g. $32 -c to center or $32 -r to right align)*/
		@33 DM_PRS_1 /* (char data) */
		@65 SSN /* (char data) */
		@74 CLID /* (char data) */
		@104 PRINC $15. -r /* (char data) format ($15.) to define column width with -r switch overrides default alignment for char format and right aligns data*/
		@119 ACINT $15. -r /* (char data) */
		@134 CHRGS $15. -r /* (char data) */
		@149 BD_CLM_PAY_PRC /*already formatted above to fill column8*/
		@157 LC_STA_DC10 $8. -r  /* (char data) */
		@165 LD_TRX_EFF /*already formatted above to fill column8*/  
		@173 LA_APL_PRI 15.2 /*numeric data automatically right aligned, just needs format to define column width*/
		@188 LA_APL_INT 15.2 /*numeric data, use format and switch to alter alignment (e.g. 15.2 -c to center or $15.2 -l to left align)*/
		@203 LA_APL_OTH 15.2 /*numeric data*/
	;
RUN;
