/*	STATE AUDIT - POSTCLAIM LOAN STATUS : This job imports an NSLDS submittal file, queries it for certain
	loan statuses, then gathers data from the Data Warehouse for
	the selected loans.  This job was requested 6/10/02 for a
	sampling for state auditors for due diligence verification.
	Output is an Excel file named LOAN STATUS QUERY.xls in the folder
	T:\SAS.
	
	To use this program:
	1.	Change the path of the raw file on line 12.  Must be in single quotes.
*/ 

%LET SUBFILE = 'X:\Archive\NSLDS\NSLDSEXTRACT.07052020.091924'	;

    data NSTEPINA  (where=( rec_type NE 'Z'))     ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */
    infile &SUBFILE   TRUNCOVER lrecl=641 ;                          
       format ga_code $3. ;                                                             
       format ssn $9. ;                                                                 
       format dob $8. ;                                                                 
       format first $12. ;                                                              
       format ln_type $2. ;                                                             
       format guar_dt $8. ;                                                             
       format ind_sep_ln $1. ;                                                          
       format orig_sch $8. ;                                                            
       format plus_ssn $9. ;                                                            
       format rec_type $1. ;                                                            
       format act_code $1. ;                                                            
       format new_ssn $9. ;                                                             
       format new_dob $8. ;                                                             
       format new_first $12. ;                                                          
       format new_ln_type $2. ;                                                         
       format new_guar_dt $8. ;                                                         
       format new_ind_sep_ln $1. ;                                                      
       format new_orig_sch $8. ;                                                        
       format new_plus_ssn $9. ;                                                        
       format repay_dt $8. ;                                                            
       format guar_amt $6. ;                                                            
       format ln_stat_dt $8. ;                                                          
       format ln_stat $2. ;                                                             
       format canc_dt $8. ;                                                             
       format canc_amt $6. ;                                                            
       format disb_dt $8. ;                                                             
       format disb_amt $6. ;                                                            
       format pca_dt $8. ;                                                              
       format pca_flag $1. ;                                                            
       format plus_ssn_ind $1. ;                                                        
       format plus_first $12. ;                                                         
       format plus_last $35. ;                                                          
       format plus_dob $8. ;                                                            
       format ssn_ind $1. ;                                                             
       format acad_lvl $1. ;                                                            
       format last $35. ;                                                               
       format enroll_beg $8. ;                                                          
       format enroll_end $8. ;                                                          
       format mi $1. ;                                                                  
       format dl_no $30. ;                                                              
       format dl_st $2. ;                                                               
       format plus_mi $1. ;                                                             
       format plus_st $2. ;                                                             
       format servicer $6. ;                                                            
       format orig_lend $6. ;                                                           
       format guar_trans_dt $8. ;                                                       
       format defer_type $2. ;                                                          
       format defer_start $8. ;                                                         
       format defer_stop $8. ;                                                          
       format ind_lend_last $1. ;                                                       
       format ga_clm_rea $2. ;                                                          
       format clm_lend_ref_dt $8. ;                                                     
       format clm_lend_ref_amt $6. ;                                                    
       format ref_dt $8. ;                                                              
       format ref_amt $6. ;                                                             
       format lend_clm_rea $2. ;                                                        
       format clm_pd_dt $8. ;                                                           
       format clm_pd_amt $6. ;                                                          
       format oth_fee_end_bal $6. ;                                                     
       format reins_req_dt $8. ;                                                        
       format reins_pd_dt $8. ;                                                         
       format reins_req_amt $6. ;                                                       
       format sup_req_dt $8. ;                                                          
       format sup_req_amt $6. ;                                                         
       format reins_reimb_rt $1. ;                                                      
       format rep_dt $8. ;                                                              
       format rep_amt $6. ;                                                             
       format rehab_ind $1. ;                                                           
       format ga_coll_dt $8. ;                                                          
       format ga_prin_coll_amt $6. ;                                                    
       format ga_int_coll_amt $6. ;                                                     
       format irs_coll_dt $8. ;                                                         
       format irs_int_coll_amt $6. ;                                                    
       format irs_prin_coll_amt $6. ;                                                   
       format enroll_stat_dt $8. ;                                                      
       format enroll_stat $1. ;                                                         
       format agd $8. ;                                                                 
       format curr_holder $6. ;                                                         
       format outst_prin_dt $8. ;                                                       
       format outst_prin_amt $6. ;                                                      
       format outst_acc_int_dt $8. ;                                                    
       format outst_acc_int_amt $6. ;                                                   
       format sold_dt $8. ;                                                             
       format orig_fee_ind $1. ;                                                        
       format int_rt $5. ;                                                              
       format rt_type $1. ;                                                             
       format bk_ref_dt $8. ;                                                           
       format bk_ref_amt $6. ;                                                          
       format sup_pca_pmt_dt $8. ;                                                      
       format sup_pca_pmt_amt $6. ;                                                     
       format subs_ind $1. ;                                                            
       format serv_resp_dt $8. ;                                                        
       format curr_sch $8. ;                                                            
       format cluid $22. ;                                                              
    input                                                                               
             @1 ga_code $3.                                                             
             @4 ssn $9.                                                                 
             @13 dob $8.                                                                
             @21 first $12.                                                             
             @33 ln_type $2.                                                            
             @35 guar_dt $8.                                                            
             @43 ind_sep_ln $1.                                                         
             @44 orig_sch $8.                                                           
             @52 plus_ssn $9.                                                           
             @61 rec_type $1.                                                           
             @62 act_code $1.                                                           
             @63 new_ssn $9.                                                            
             @72 new_dob $8.                                                            
             @80 new_first $12.                                                         
             @92 new_ln_type $2.                                                        
             @94 new_guar_dt $8.                                                        
             @102 new_ind_sep_ln $1.                                                    
             @103 new_orig_sch $8.                                                      
             @111 new_plus_ssn $9.                                                      
             @120 repay_dt $8.                                                          
             @128 guar_amt $6.                                                          
             @134 ln_stat_dt $8.                                                        
             @142 ln_stat $2.                                                           
             @144 canc_dt $8.                                                           
             @152 canc_amt $6.                                                          
             @158 disb_dt $8.                                                           
             @166 disb_amt $6.                                                          
             @172 pca_dt $8.                                                            
             @180 pca_flag $1.                                                          
             @181 plus_ssn_ind $1.                                                      
             @182 plus_first $12.                                                       
             @194 plus_last $35.                                                        
             @229 plus_dob $8.                                                          
             @237 ssn_ind $1.                                                           
             @238 acad_lvl $1.                                                          
             @239 last $35.                                                             
             @274 enroll_beg $8.                                                        
             @282 enroll_end $8.                                                        
             @290 mi $1.                                                                
             @291 dl_no $30.                                                            
             @321 dl_st $2.                                                             
             @323 plus_mi $1.                                                           
             @324 plus_st $2.                                                           
             @326 servicer $6.                                                          
             @332 orig_lend $6.                                                         
             @338 guar_trans_dt $8.                                                     
             @346 defer_type $2.                                                        
             @348 defer_start $8.                                                       
             @356 defer_stop $8.                                                        
             @364 ind_lend_last $1.                                                     
             @365 ga_clm_rea $2.                                                        
             @367 clm_lend_ref_dt $8.                                                   
             @375 clm_lend_ref_amt $6.                                                  
             @381 ref_dt $8.                                                            
             @389 ref_amt $6.                                                           
             @395 lend_clm_rea $2.                                                      
             @397 clm_pd_dt $8.                                                         
             @405 clm_pd_amt $6.                                                        
             @411 oth_fee_end_bal $6.                                                   
             @417 reins_req_dt $8.                                                      
             @425 reins_pd_dt $8.                                                       
             @433 reins_req_amt $6.                                                     
             @439 sup_req_dt $8.                                                        
             @447 sup_req_amt $6.                                                       
             @453 reins_reimb_rt $1.                                                    
             @454 rep_dt $8.                                                            
             @462 rep_amt $6.                                                           
             @468 rehab_ind $1.                                                         
             @469 ga_coll_dt $8.                                                        
             @477 ga_prin_coll_amt $6.                                                  
             @483 ga_int_coll_amt $6.                                                   
             @489 irs_coll_dt $8.                                                       
             @497 irs_int_coll_amt $6.                                                  
             @503 irs_prin_coll_amt $6.                                                 
             @509 enroll_stat_dt $8.                                                    
             @517 enroll_stat $1.                                                       
             @518 agd $8.                                                               
             @526 curr_holder $6.                                                       
             @532 outst_prin_dt $8.                                                     
             @540 outst_prin_amt $6.                                                    
             @546 outst_acc_int_dt $8.                                                  
             @554 outst_acc_int_amt $6.                                                 
             @560 sold_dt $8.                                                           
             @568 orig_fee_ind $1.                                                      
             @569 int_rt $5.                                                            
             @574 rt_type $1.                                                           
             @575 bk_ref_dt $8.                                                         
             @583 bk_ref_amt $6.                                                        
             @589 sup_pca_pmt_dt $8.                                                    
             @597 sup_pca_pmt_amt $6.                                                   
             @603 subs_ind $1.                                                          
             @604 serv_resp_dt $8.                                                      
             @612 curr_sch $8.                                                          
             @620 cluid $22.                                                            
    ;                                                                                   
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
    run;                                                                                
      
    data WORK.NSTEPINB    (where=( rec_type EQ 'Z'))   ; 
    %let _EFIERR_ = 0; /* set the ERROR detection macro variable */
    infile &SUBFILE   TRUNCOVER lrecl=640 ;
       format ga_code $3. ;                                                             
       format ssn $9. ;                                                                 
       format dob $8. ;                                                                 
       format first $12. ;                                                              
       format ln_type $2. ;                                                             
       format guar_dt $8. ;                                                             
       format ind_sep_ln $1. ;                                                          
       format orig_sch $8. ;                                                            
       format plus_ssn $9. ;                                                            
       format rec_type $1. ;                                                            
       format act_code $1. ;                                                            
       format old_ln_stat_dt $8. ;                                                      
       format new_ln_stat_dt $8. ;                                                      
       format new_ln_stat $2. ;                                                         
       format old_defer_start $8. ;                                                     
       format new_defer_stop $8. ;                                                      
       format new_defer_type $2. ;                                                      
       format old_curr_sch $8. ;                                                        
       format new_curr_sch $8. ;                                                        
       format old_enroll_stat_dt $8. ;                                                  
       format new_enroll_stat_dt $8. ;                                                  
       format new_enroll_stat $1. ;                                                     
       format old_reins_req_dt $8. ;                                                    
       format new_reins_req_dt $8. ;                                                    
       format new_reins_pd_dt $8. ;                                                     
       format new_ga_clm_rea $2. ;                                                      
       format old_curr_holder $6. ;                                                     
       format old_sold_dt $8. ;                                                         
       format new_curr_holder $6. ;                                                     
       format new_sold_dt $8. ;                                                         
       format old_clm_pd_dt $8. ;                                                       
       format new_clm_pd_dt $8. ;                                                       
       format new_lend_clm_rea $2. ;                                                    
       format old_guar_trans_dt $8. ;                                                   
       format new_guar_trans_dt $8. ;     
       format old_serv_resp_dt $8. ;                                                    
       format old_servicer $6. ;                                                        
       format new_serv_resp_dt $8. ;                                                    
       format new_servicer $6. ;                                                        
       format old_rep_dt $8. ;                                                          
       format new_rehab_ind $1. ;                                                       
       format new_rep_dt $8. ;                                                          
       format new_rep_amt $6. ;                                                         
       format old_ga_prin_int_coll $8. ;                                                
       format new_ga_prin_int_coll $8. ;                                                
       format old_com_lend_ref_dt $8. ;                                                 
       format new_clm_lend_ref_dt $8. ;                                                 
       format old_irs_coll_dt $8. ;                                                     
       format new_irs_coll_dt $8. ;                                                     
       format old_canc_dt $8. ;                                                         
       format new_canc_dt $8. ;                                                         
       format old_disb_dt $8. ;                                                         
       format new_disb_dt $8. ;                                                         
       format old_ref_dt $8. ;                                                          
       format new_ref_dt $8. ;                                                          
       format old_bk_ref_dt $8. ;                                                       
       format new_bk_ref_dt $8. ;                                                       
       format old_sup_pca_pmt_dt $8. ;                                                  
       format new_sup_pca_pmt_dt $8. ;                                                  
       format old_sup_req_dt $8. ;                                                      
       format new_sup_req_dt $8. ;                                                      
       format old_pca_flag_dt $8. ;                                                     
       format cluid $22. ;                                                              
    input                                                                               
             @1 ga_code $3.                                                             
             @4 ssn $9.                                                                 
             @13 dob $8.                                                                
             @21 first $12.                                                             
             @33 ln_type $2.                                                            
             @35 guar_dt $8.                                                            
             @43 ind_sep_ln $1.                                                         
             @44 orig_sch $8.                                                           
             @52 plus_ssn $9.                                                           
             @61 rec_type $1.                                                           
             @62 act_code $1.                                                           
             @63 old_ln_stat_dt $8.                                                     
             @71 new_ln_stat_dt $8.                                                     
             @79 new_ln_stat $2.                                                        
             @81 old_defer_start $8.                   
             @97 new_defer_stop $8.                                                     
             @105 new_defer_type $2.                                                    
             @107 old_curr_sch $8.                                                      
             @115 new_curr_sch $8.                                                      
             @123 old_enroll_stat_dt $8.                                                
             @131 new_enroll_stat_dt $8.                                                
             @139 new_enroll_stat $1.                                                   
             @140 old_reins_req_dt $8.                                                  
             @148 new_reins_req_dt $8.                                                  
             @156 new_reins_pd_dt $8.                                                   
             @164 new_ga_clm_rea $2.                                                    
             @166 old_curr_holder $6.                                                   
             @172 old_sold_dt $8.                                                       
             @180 new_curr_holder $6.                                                   
             @186 new_sold_dt $8.                                                       
             @194 old_clm_pd_dt $8.                                                     
             @202 new_clm_pd_dt $8.                                                     
             @210 new_lend_clm_rea $2.                                                  
             @212 old_guar_trans_dt $8.                                                 
             @220 new_guar_trans_dt $8.                                                 
             @228 old_serv_resp_dt $8.                                                  
             @236 old_servicer $6.                                                      
             @242 new_serv_resp_dt $8.                                                  
             @250 new_servicer $6.                                                      
             @256 old_rep_dt $8.                                                        
             @264 new_rehab_ind $1.                                                     
             @265 new_rep_dt $8.                                                        
             @273 new_rep_amt $6.                                                       
             @279 old_ga_prin_int_coll $8.                                              
             @287 new_ga_prin_int_coll $8.                                              
             @295 old_com_lend_ref_dt $8.                                               
             @303 new_clm_lend_ref_dt $8.                                               
             @311 old_irs_coll_dt $8.                                                   
             @319 new_irs_coll_dt $8.                                                   
             @327 old_canc_dt $8.                                                       
             @335 new_canc_dt $8.                                                       
             @343 old_disb_dt $8.                                                       
             @351 new_disb_dt $8.                                                       
             @359 old_ref_dt $8.                                                        
             @367 new_ref_dt $8.                                                        
             @375 old_bk_ref_dt $8.                                                     
             @383 new_bk_ref_dt $8.                                                     
             @391 old_sup_pca_pmt_dt $8.                                                
             @399 new_sup_pca_pmt_dt $8.           
             @407 old_sup_req_dt $8.                                                    
             @415 new_sup_req_dt $8.                                                    
             @423 old_pca_flag_dt $8.                                                   
             @620 cluid $22.                                                            
    ;                                                                                   
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
    run;                                                                                
                                                                                        
DATA NSTEP;
SET NSTEPINA NSTEPINB;
RUN;

PROC SORT DATA = NSTEP;
BY SSN GUAR_DT;
RUN;

PROC SQL;
CREATE INDEX LN_STAT
ON NSTEP(LN_STAT);
QUIT;

PROC SQL;
CREATE TABLE NSTEPEXT AS
SELECT 
CASE WHEN PLUS_SSN <> ' ' THEN PLUS_SSN
	 ELSE SSN
END									AS SSN,
CLUID,
CASE WHEN LN_STAT = 'AE' THEN 'ASSIGNED TO ED'
	 WHEN LN_STAT IN ('DF','DU','DX','DZ','XD') THEN 'DEFAULT UNRESOLVED'
	 WHEN LN_STAT IN ('DB','DO') THEN 'DEFAULT IN BANKRUPTCY'
	 WHEN LN_STAT = 'DL' THEN 'DEFAULT IN LITIGATION'
END									AS LN_STAT
FROM NSTEP
WHERE ln_stat IN ('AE','DB','DF','DL','DO','DU','DX','DZ','XD')
ORDER BY SSN, CLUID;
QUIT;

libname  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=work  ;
DATA WORKLOCL.NSTEPEXT;
SET NSTEPEXT;
RUN;

RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	A.bf_ssn						as SSN,
		A.af_apl_id||A.af_apl_id_sfx	as CLUID,
RTRIM(D.DM_PRS_LST)||', '||RTRIM(D.DM_PRS_1) AS NAME,
		A.LF_CLM_ID						AS LONID,
		A.LD_LDR_POF					AS CLMPDT,
CASE WHEN A.LC_STA_DC10 = '03' THEN
		B.LA_CLM_BAL -
		B.LA_CLM_PRJ_COL_CST			
	 ELSE (A.lA_clm_pri + A.lA_clm_int)
		- (A.lA_pri_col)
		+ (A.lA_int_Acr) 
		- (A.lA_int_col)
		+(A.lA_leg_cst_Acr)
		-(A.lA_leg_cst_col)
		+(A.lA_oth_chr_Acr)
		-(A.lA_oth_chr_col)
		+(A.lA_col_cst_Acr)
		-(A.lA_col_cst_col)
END										AS CURBAL
	,D.DM_PRS_LST
	,D.DD_BRT


FROM  OLWHRM1.DC01_LON_CLM_INF A INNER JOIN OLWHRM1.PD01_PDM_INF D
		on A.bf_ssn = D.DF_PRS_ID
	LEFT OUTER JOIN OLWHRM1.DC02_BAL_INT B
		ON A.af_apl_id||A.af_apl_id_sfx = B.af_apl_id||B.af_apl_id_sfx
WHERE A.LD_LDR_POF = (SELECT MAX(X.LD_LDR_POF)
						FROM OLWHRM1.DC01_LON_CLM_INF X
						WHERE X.AF_APL_ID = A.AF_APL_ID
						AND X.AF_APL_ID_SFX = A.AF_APL_ID_SFX)
ORDER BY A.bf_ssn, A.af_apl_id||A.af_apl_id_sfx
);
DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE DEMO2 AS
SELECT A.SSN, 
	A.NAME, 
	A.LONID 	AS LOAN_ID, 
	A.CLMPDT 	AS CLAIM_PAID_DT,
	B.LN_STAT	AS LOAN_STATUS,
	A.CURBAL	AS CURRENT_BALANCE
FROM DEMO A INNER JOIN NSTEPEXT B
	ON A.SSN = B.SSN
	AND A.CLUID = B.CLUID
ORDER BY LOAN_STATUS, SSN, LOAN_ID DESC;
QUIT;
endrsubmit  ;

DATA LOAN_STATUS_QUERY;
SET WORKLOCL.DEMO2;
RUN;

PROC EXPORT DATA= Work.LOAN_STATUS_QUERY
            OUTFILE= "T:\SAS\LOAN STATUS QUERY.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
