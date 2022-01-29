/********************************************************************************/
/*                                                                              */
/*            NEVER EXECUTE THIS CODE, EVEN WHEN PROMOTING ON CYPRUS            */
/*            AS IT UPDATES THE DATES IN THE UTLWG02_RUNDT DATA SET             */
/*            USED BY VARIOUS JOBS TO DETERMINE THE LAST TIME LOANS             */
/*            WERE REPORTED.                                                    */
/*                                                                              */
/********************************************************************************/

/*UTLWG00 - UTLWG02_RUNDT UPDATE*/

RSUBMIT;
OPTIONS SYMBOLGEN;

LIBNAME OLRPLD V8 '/sas/whse/olrp_lookup_directory';	*CYPRUS LIVE;

*WRITE CURRENT RUN DATE FOR THIS JOB TO CYPRUS DATASET;
DATA OLRPLD.UTLWG02_RUNDT;
SET OLRPLD.UTLWG02_RUNDT;
PVS_RUNDT = LAST_RUNDT;
LAST_RUNDT = DATETIME();
FORMAT PVS_RUNDT LAST_RUNDT DATETIME.;
RUN;

ENDRSUBMIT;

