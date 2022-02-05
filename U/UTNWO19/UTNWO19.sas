/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO19.NWO19RZ";
FILENAME REPORT2 "&RPTLIB/UNWO19.NWO19R2";

LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CREATE TABLE INVALID_SSNS AS
		SELECT DISTINCT
			PD10.DF_PRS_ID
		FROM
			PKUB.PD10_PRS_NME PD10
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LA_CUR_PRI > 0
				AND LN10.LC_STA_LON10 = 'R'
			LEFT JOIN PKUB.LN15_DSB LN15
				ON LN10.BF_SSN = LN15.BF_SSN
				AND LN10.LN_SEQ = LN15.LN_SEQ
				AND LN15.LC_STA_LON15 = '1'
		WHERE
			PD10.DF_PRS_ID NOT LIKE('P%')
			AND 
				(
					PD10.DF_PRS_ID LIKE ('000%')
					OR PD10.DF_PRS_ID LIKE ('666%')
					OR PD10.DF_PRS_ID LIKE ('9%')
					OR SUBSTR(PD10.DF_PRS_ID,4,2) = '00'
					OR SUBSTR(PD10.DF_PRS_ID,6,4) = '0000'
					OR TRANSLATE(PD10.DF_PRS_ID,'','0123456789') IS NOT NULL
					OR /*11*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('001','002','006','007','222','318','319','320','321','322','323','324','325','326','327','328','329','330','331','332','333','334','335','336','337','338','339','340','341','342','343','344','345')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 11
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*13*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('004','005','221')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 13
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)

					OR /*08*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('003','346','347','348','349','350','351','352','353','354','355','356','357','358','359','360','361')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*94*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('008','009','010','011','012','013','014','015','016','017','018','019','020','021','596')
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('96','98','02','04','06','08')
								)
						)
					OR /*92*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('022','023','024','025','026','027','028','029','030','031','032','033','034','597','598','599')
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('94','96','98','02','04','06','08')
								)
						)
					OR /*74*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('035','036','037','038','039')
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 74 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*15*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('040','041','042','043','044','045','046','047','048','049','279','280','281','282','283','284','285','286','287','288','289','290','291','292','293','294','295','296','297','298','299','300','301','302')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 15
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1			
						)
					OR /*17*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('268','269','270','271','272','273','274','275','276','277','278')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 17
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1			
						)
					OR /*02*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND (INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 50 AND 105 OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 767 AND 772)
							AND 
								(
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('04','06','08')
								)
						)
					OR /*98*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 106 AND 134
							AND 
								(
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
								)
						)
					OR /*25*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 135 AND 138
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 25
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*23*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND (INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 139 AND 158 OR SUBSTR(PD10.DF_PRS_ID,1,3) = '646')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 23
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*86*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 159 AND 195
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 86 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*84*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 196 AND 211
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 84 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*91*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 212 AND 215
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 91
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*89*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 216 AND 220 
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '518'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 89
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*57*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 232 AND 235
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 468 AND 472
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 506 AND 508
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 57
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*55*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									SUBSTR(PD10.DF_PRS_ID,1,3) = '236'
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 473 AND 477
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 55
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*37*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 303 AND 310
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 370 AND 386
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 501 AND 502
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 37
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*35*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 311 AND 317
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 35
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*39*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 362 AND 369
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 39
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*33*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 387 AND 397
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 509 AND 512
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 33
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*31*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 398 AND 399
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 513 AND 515
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 627 AND 636
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '680'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 31
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*73*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 400 AND 407
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 73
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*67*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 416 AND 423
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '586'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 67
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*65*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '424'
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 65
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*29*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 440 AND 442
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 486 AND 500
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 637 AND 645
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '764'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 29
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*27*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 443 AND 448
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '765'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 27
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*43*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '478'
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 43
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*41*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 479 AND 485
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '580'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 41
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*45*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 503 AND 504
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 45
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*59*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '505'
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 59
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*49*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 516 AND 517
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 49
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*87*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 602 AND 619
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '519'
								)
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 87
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*61*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('520','574')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 61
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*71*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 531 AND 539
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 71
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*83*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('540','541')
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 83
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*81*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 542 AND 544
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 81
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*53*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 577 AND 579
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 53
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*85*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 620 AND 626
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 85
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*21*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '647'
							AND INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 21
							AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1
						)
					OR /*09*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) IN ('588','752')
							AND SUBSTR(DF_PRS_ID,4,2) NOT IN ('01','03','05','07','09')
						)
					OR /*58*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '648'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 58 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*56*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '649'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 56 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*62*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 650 AND 652
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 62 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*60*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '653'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 60 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*38*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '654'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 38 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*36*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 655 AND 658
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 36 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*22*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 661 AND 665
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 676 AND 678
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '690'
								)
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 22 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*48*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 667 AND 675
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 48 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*24*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 659 AND 660
									OR INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 681 AND 689
								)
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 24 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*16*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 693 AND 699
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 16 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*18*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 700 AND 721
									OR SUBSTR(PD10.DF_PRS_ID,1,3) IN ('691','692','751','722','723','725','726')
								)
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 18 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*10*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '727'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 10 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*14*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '728'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 14 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*28*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND 
								(
									INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 729 AND 732
									OR SUBSTR(PD10.DF_PRS_ID,1,3) = '724'
								)
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 28 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*26*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '733'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 26 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*12*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 756 AND 763
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('02','04','06','08')
									OR (INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 12 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 0)
								)
						)
					OR /*04*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND SUBSTR(PD10.DF_PRS_ID,1,3) = '766'
							AND (
									(INPUT(SUBSTR(DF_PRS_ID,4,2),2.) > 9 AND MOD(INPUT(SUBSTR(DF_PRS_ID,4,2),2.),2) = 1)
									OR SUBSTR(DF_PRS_ID,4,2) IN ('06','08')
								)
						)
					OR /*07*/
						(
							LN15.LD_DSB < '25JUN2011'D
							AND INPUT(SUBSTR(PD10.DF_PRS_ID,1,3),3.) BETWEEN 753 AND 755
							AND SUBSTR(DF_PRS_ID,4,2) NOT IN ('01','03','05','07')
						)
			)
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA INVALID_SSNS; SET LEGEND.INVALID_SSNS; RUN;

DATA _NULL_;
	SET INVALID_SSNS ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	FORMAT NOTDATE MMDDYY10.;

	NOTDATE = TODAY();

	IF _N_ = 1 THEN	PUT "Notification date,Correct SSN,Invalid SSN";

	PUT NOTDATE @;
	PUT ',' @;
	PUT DF_PRS_ID $ ;
RUN;
