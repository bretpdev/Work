LIBNAME SOURCE 'T:\Bond ID List.xlsx' MIXED=YES ;
DATA WORK.XLSOURCE;
	SET SOURCE.'Bond IDs$'N;
RUN;
LIBNAME SOURCE CLEAR;

DATA BOND_IDS_NEW
	(RENAME = 
		(
			F3 = IF_BND_ISS_OLD 
			DESTINATION_BOND = IF_BND_ISS_NEW
		)
	);
	SET XLSOURCE 
		(DROP = 
			F1 
			F4 
			F6 
			F7 
			F8
		);
	LABEL
		F3 = ' '
		DESTINATION_BOND = ' '
	;		
	WHERE
		BONDS ^= 'IF_OWN'
		AND F3 ^= 'IF_BND_ISS'
		AND DESTINATION_BOND ^= 'IF_BND_ISS'
	;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/

*sends new data to duster;
DATA DUSTER.BOND_IDS_NEW;
	SET BOND_IDS_NEW;
RUN;

RSUBMIT;

*bond_id table location;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';

*creates backup of original data set;
DATA BOND_IDS_OLD;
	SET SAS_TAB.BOND_IDS;
RUN;

*updates data;
DATA 
/*	SAS_TAB.BOND_IDS_NEW; *TEST;*/
	SAS_TAB.BOND_IDS; *LIVE;
	SET BOND_IDS_NEW;
RUN;

ENDRSUBMIT;

DATA BOND_IDS_OLD;
	SET DUSTER.BOND_IDS_OLD;
RUN;

*sends backup to T: drive;
DATA 'T:\BOND_IDS_ORIGINAL';
	SET BOND_IDS_OLD;
RUN;
