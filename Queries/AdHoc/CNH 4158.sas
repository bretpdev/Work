LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

DATA ARCS (KEEP=PF_REQ_ACT PX_ACT_DSC_REQ PX_ACT_COL_AUT_REQ);
	SET PKUB.ACXX_ACT_REQ;
RUN;

ENDRSUBMIT;

DATA ARCS; SET LEGEND.ARCS; RUN;

PROC EXPORT
		DATA=LEGEND.ARCS
		OUTFILE='T:\SAS\CS ARCS.CSV'
		REPLACE;
RUN;

/*write to comma delimited file because some ARCS look like dates (DECXX - DECXX)*/
DATA _NULL_;
	SET		WORK.ARCS;
	FILE	'T:\SAS\CS ARCS.txt' delimiter=',' DSD DROPOVER lrecl=XXXXX;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	'PF_REQ_ACT,PX_ACT_DSC_REQ,PX_ACT_COL_AUT_REQ';
		END;

	/* write data*/	
	DO;
		PUT PF_REQ_ACT $ @;
		PUT PX_ACT_DSC_REQ $ @;
		PUT PX_ACT_COL_AUT_REQ $;
	END;
RUN;

