PUT(<name of attribute or variable containing a number>,<format to use to format numeric data>)

PUT(LN10.LN_SEQ,Z3.) /*converts number to character with leading zeros (i.e. '003')*/
PUT(LN10.LN_SEQ,3.)  /*converts number to character with leading spaces (i.e. '  3')*/

CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(TODAY(),DATE10.)))|| "'D"); /*'17JAN2014'D*/
CALL SYMPUT('LAST_RUNPASS',"'"|| PUT(TODAY(),MMDDYY10.) || "'"); /*'01/17/2014'*/
