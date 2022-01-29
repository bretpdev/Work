%LET CHTRANSFILE = "S:\sasha\U of U Change Transactions\Clcs.2002";
%LET PRINTFILE = "C:\WINDOWS\TEMP\CLCHG&sysdate9..rtf";

%macro fdate(fmt);
   %global fdate;
   data _null_;
      call symput("fdate",left(put("&sysdate"d,&fmt)));
   run;
%mend fdate;
%fdate(MMDDYY8.);

OPTIONS ERRORS=MIN ls=80;

 /*@H*/                  
    data WORK.clchangeH    (where=( rec_code EQ '@H'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=186 ;
       format rec_code $2. ;                                                               
       format soft_prod $4. ;                                                              
       format soft_ver $4. ;                                                               
       format batch_id $12. ;                                                              
       format file_cr_dt MMDDYY10. ;                                                             
       format file_cr_time $6. ;                                                           
       format file_trans_dt MMDDYY10. ;                                                          
       format file_trans_time $6. ;                                                        
       format file_id_name $19. ;                                                          
       format file_id_code $5. ;                                                           
       format source_name $32. ;                                                           
       format source_id $8. ;                                                              
       format source_br $4. ;                                                              
       format source_type $1. ;                                                            
       format recip_name $32. ;                                                            
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format media_type $1. ;                                                             
       format duns_source $9. ;                                                            
       format duns_recip $9. ;                                                             
       label rec_code="Record Code";                                                       
       label soft_prod="Software Product Code";                                            
       label soft_ver="Software Version";                                                  
       label batch_id="Batch ID";                                                          
       label file_cr_dt="File Creation Date";                                              
       label file_cr_time="File Creation Time";                                            
       label file_trans_dt="File Transmission Date";                                       
       label file_trans_time="File Transmission Time";                                     
       label file_id_name="File Identifier Name";                                          
       label file_id_code="File Identifier Code";                                          
       label source_name="Source Name";                                                    
       label source_id="Source ID";                                                        
       label source_br="Source Non-ED Branch ID";                                          
       label source_type="Source Type Code";                                               
       label recip_name="Recipient Name";                                                  
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label media_type="Media Type Code";                                                 
       label duns_source="DUNS Source ID";                                                 
       label duns_recip="DUNS Recipient ID";                                               
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 soft_prod $4.                                                              
             @7 soft_ver $4.                                                               
             @11 batch_id $12.                                                             
             @23 file_cr_dt YYMMDD8.                                                            
             @31 file_cr_time $6.                                                          
             @37 file_trans_dt YYMMDD8.                                                         
             @45 file_trans_time $6.                                                       
             @51 file_id_name $19.                                                         
             @70 file_id_code $5.                                                          
             @75 source_name $32.                                                          
             @107 source_id $8.                                                            
             @117 source_br $4.                                                            
             @121 source_type $1.                                                          
             @122 recip_name $32.                                                          
             @154 recip_id $8.                                                             
             @164 recip_br $4.                                                             
             @168 media_type $1.                                                           
             @169 duns_source $9.                                                          
             @178 duns_recip $9.                                                           
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;          
 /*@102*/                  
    data WORK.clchange02    (where=( rec_code EQ '@1' and rec_type EQ '02'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=150 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format last $35. ;                                                                  
       format first $12. ;                                                                 
       format mi $1. ;                                                                     
       format dob MMDDYY10. ;                                                                    
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label last="Borrower Last Name";                                                    
       label first="Borrower First Name";                                                  
       label mi="Borrower Middle Initial";                                                 
       label dob="Borrower Date of Birth";                                                 
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @56 last $35.                                                                 
             @91 first $12.                                                                
             @103 mi $1.                                                                   
             @104 dob YYMMDD8.                                                                  
             @113 stamp $20.                                                               
             @133 duns_sch $9.                                                             
             @142 duns_recip $9.                                                           
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                           
 /*@107*/                  
    data WORK.clchange07    (where=( rec_code EQ '@1' and rec_type EQ '07'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=220 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $11. ;                                                               
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format rev_ln_beg_dt MMDDYY10. ;                                                          
       format rev_ln_end_dt MMDDYY10. ;                                                          
       format grade $1. ;                                                                  
       format chg_cert_dt MMDDYY10. ;                                                             
       format acd MMDDYY10. ;                                                                    
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch/Division Code";                         
       label plus_ssn="PLUS/Alternative SSN";                                              
       label cluid="Commonline Unique Identifier";                                         
       label rev_ln_beg_dt="Revised Loan Period Begin Date";                               
       label rev_ln_end_dt="Revised Loan Period End Date";                                 
       label grade="Grade Level Code";                                                     
       label chg_cert_dt="Change Certification Date";                                       
       label acd="Anticipated Completion Date";                                            
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.                                                             
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 rev_ln_beg_dt YYMMDD8.                                                        
             @147 rev_ln_end_dt YYMMDD8.                                                        
             @155 grade $1.                                                                
             @156 chg_cert_dt YYMMDD8.                                                           
             @165 acd YYMMDD8.                                                                  
             @174 stamp $20.                                                               
             @194 duns_sch $9.                                                             
             @203 duns_recip $9.                                                           
             @212 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                                           
 /*@108*/                  
    data WORK.clchange08    (where=( rec_code EQ '@1' and rec_type EQ '08'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=202 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $11. ;                                                               
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format canc_dt MMDDYY10. ;                                                                
       format rein_ln_amt DOLLAR10.2 ;                                                            
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch/Division Code";                         
       label plus_ssn="PLUS/Alternative SSN";                                              
       label cluid="Commonline Unique Identifier";                                         
       label canc_dt="Cancellation Date";                                                  
       label rein_ln_amt="Reinstated Loan Amount";                                         
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.                                                             
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 canc_dt YYMMDD8.                                                              
             @147 rein_ln_amt 8.2                                                          
             @156 stamp $20.                                                               
             @176 duns_sch $9.                                                             
             @185 duns_recip $9.                                                           
             @194 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                                
 /*@109*/                  
    data WORK.clchange09    (where=( rec_code EQ '@1' and rec_type EQ '09'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=230 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $11. ;                                                               
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format disb_no $1. ;                                                                
       format disb_dt MMDDYY10. ;                                                                
       format canc_dt MMDDYY10. ;                                                                
       format canc_amt DOLLAR10.2 ;                                                               
       format hold_rel_ind $1. ;                                                           
       format rev_disb_dt MMDDYY10. ;                                                            
       format rev_disb_amt DOLLAR10.2 ;                                                           
       format reinst_ind $1. ;                                                             
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch/Division Code";                         
       label plus_ssn="PLUS/Alternative SSN";                                              
       label cluid="Commonline Unique Identifier";                                         
       label disb_no="Disbursement Number";                                                
       label disb_dt="Disbursement Date";                                                  
       label canc_dt="Cancellation Date";                                                  
       label canc_amt="Cancellation Amount";                                               
       label hold_rel_ind="Disbursement Hold/Release Indicator Code";                      
       label rev_disb_dt="Revised Disbursement Date";                                      
       label rev_disb_amt="Revised Disbursement Amount";                                   
       label reinst_ind="Reinstatement Indicator Code";                                    
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.                                                             
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @140 disb_no $1.                                                              
             @141 disb_dt YYMMDD8.                                                              
             @149 canc_dt YYMMDD8.                                                              
             @157 canc_amt 8.2                                                             
             @165 hold_rel_ind $1.                                                         
             @166 rev_disb_dt YYMMDD8.                                                          
             @174 rev_disb_amt 8.2                                                         
             @182 reinst_ind $1.                                                           
             @184 stamp $20.                                                               
             @204 duns_sch $9.                                                             
             @213 duns_recip $9.                                                           
             @222 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                                             
 /*@110*/                  
    data WORK.clchange10    (where=( rec_code EQ '@1' and rec_type EQ '10'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=256 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $11. ;                                                               
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format disb_no $1. ;                                                               
       format disb_dt MMDDYY10. ;                                                                
       format gr_disb_amt DOLLAR10.2 ;                                                            
       format canc_dt MMDDYY10. ;                                                                
       format canc_amt DOLLAR10.2 ;                                                               
       format disb_con_ind $1. ;                                                           
       format act_ret_amt DOLLAR10.2 ;                                                            
       format fund_ret_meth $1. ;                                                          
       format fund_reis_ind $1. ;                                                          
       format rev_disb_dt MMDDYY10. ;                                                            
       format rev_disb_amt DOLLAR10.2 ;                                                           
       format reinst_ind $1. ;                                                               
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch/Division Code";                         
       label plus_ssn="PLUS/Alternative SSN";                                              
       label cluid="Commonline Unique Identifier";                                         
       label disb_no="Disbursement Number";                                               
       label disb_dt="Disbursement Date";                                                  
       label gr_disb_amt="Gross Disbursement Amount";                                      
       label canc_dt="Cancellation Date";                                                  
       label canc_amt="Cancellation Amount";                                               
       label disb_con_ind="Disbursement Consummation Indicator Code";                      
       label act_ret_amt="Actual Returned Amount";                                         
       label fund_ret_meth="Funds Return Method Code";                                      
       label fund_reis_ind="Funds Reissue Indicator Code";                                 
       label rev_disb_dt="Revised Disbursement Date";   
       label rev_disb_amt="Revised Disbursement Amount"; 
       label reinst_ind="Reinstatement Indicator Code";                                      
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.                                                             
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @140 disb_no $1.                                                             
             @149 disb_dt YYMMDD8.                                                              
             @157 gr_disb_amt 8.2                                                          
             @165 canc_dt YYMMDD8.                                                              
             @173 canc_amt 8.2                                                             
             @181 disb_con_ind $1.                                                         
             @182 act_ret_amt 8.2                                                          
             @190 fund_ret_meth $1.                                                        
             @191 fund_reis_ind $1.                                                        
             @192 rev_disb_dt YYMMDD8.                                                          
             @200 rev_disb_amt 8.2                                                         
             @208 reinst_ind $1.                                                             
             @210 stamp $20.                                                               
             @230 duns_sch $9.                                                             
             @239 duns_recip $9.                                                           
             @248 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                                       
 /*@111*/                  
    data WORK.clchange11    (where=( rec_code EQ '@1' and rec_type EQ '11'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=219 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $11. ;                                                               
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format sch_ref_dt MMDDYY10. ;                                                             
       format sch_ref_amt DOLLAR10.2 ;                                                            
       format wd_dt MMDDYY10. ;                                                                  
       format fund_ret_meth $1. ;                                                          
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch Code";                                  
       label plus_ssn="PLUS/Alternative Student SSN";                                      
       label cluid="CommonLine Unique Identifier";                                         
       label cluid_sfx="CommonLine Loan Sequence Number";                                  
       label sch_ref_dt="School Refund Date";                                              
       label sch_ref_amt="School Refund Amount";                                           
       label wd_dt="Withdrawal Date";                                                      
       label fund_ret_meth="Funds Return Method Code";                                     
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.                                                             
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 sch_ref_dt YYMMDD8.                                                           
             @147 sch_ref_amt 8.2                                                          
             @163 wd_dt YYMMDD8.                                                                
             @171 fund_ret_meth $1.                                                        
             @173 stamp $20.                                                               
             @193 duns_sch $9.                                                             
             @202 duns_recip $9.                                                           
             @211 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                             
 /*@112*/                  
    data WORK.clchange12    (where=( rec_code EQ '@1' and rec_type EQ '12'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=218 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $11. ;                                                               
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format lst_sch_ref_dt MMDDYY10. ;                                                         
       format cum_sch_ref_amt DOLLAR10.2 ;                                                        
       format rev_sch_ref_dt MMDDYY10. ;                                                         
       format rev_sch_ref_amt DOLLAR10.2 ;                                                        
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch Code";                                  
       label plus_ssn="PLUS/Alternative Student SSN";                                      
       label cluid="CommonLine Unique Identifier";                                         
       label cluid_sfx="CommonLine Loan Sequence Number";                                  
       label lst_sch_ref_dt="Last School Refund Date";                                     
       label cum_sch_ref_amt="Cumulative School Refund Amount";                            
	   label rev_sch_ref_dt="Revised School Refund Amount";
       label rev_sch_ref_amt="Revised School Refund Amount";                               
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.                                                             
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 lst_sch_ref_dt YYMMDD8.                                                       
             @147 cum_sch_ref_amt 8.2                                                      
             @155 rev_sch_ref_dt YYMMDD8.                                                       
             @163 rev_sch_ref_amt 8.2                                                      
             @172 stamp $20.                                                               
             @192 duns_sch $9.                                                             
             @201 duns_recip $9.                                                           
             @210 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                   
/*@113*/                  
    data WORK.clchange13    (where=( rec_code EQ '@1' and rec_type EQ '13'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=285 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format dec_ln_amt DOLLAR10.2 ;                                                             
       format rev_cert_amt DOLLAR10.2 ;                                                           
       format cost_attend DOLLAR10.2 ;                                                            
       format fam_cont_amt DOLLAR10.2 ;                                                           
       format est_aid_amt DOLLAR10.2 ;                                                            
       format disb_dt_1 MMDDYY10. ;                                                              
       format disb_amt_1 DOLLAR10.2 ;                                                             
       format disb_dt_2 MMDDYY10. ;                                                              
       format disb_amt_2 DOLLAR10.2 ;                                                             
       format disb_dt_3 MMDDYY10. ;                                                              
       format disb_amt_3 DOLLAR10.2 ;                                                             
       format disb_dt_4 MMDDYY10. ;                                                              
       format disb_amt_4 DOLLAR10.2 ;                                                             
       format chg_cert_dt MMDDYY10. ;                                                            
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch Code";                                  
       label cluid="CommonLine Unique Identifier";                                         
       label cluid_sfx="CommonLine Loan Sequence Number";                                  
       label dec_ln_amt="Decreased Loan Amount";                                           
       label rev_cert_amt="Revised Certification Amount";                                  
       label cost_attend="Cost of Attendance";                                             
       label fam_cont_amt="Expected Family Contribution Amount";                           
       label est_aid_amt="Estimated Financial Aid Amount";                                 
       label disb_dt_1="Disbursement Date 1";                                               
       label disb_amt_1="Disbursement Amount 1";                                           
       label disb_dt_2="Disbursement Date 2";                                              
       label disb_amt_2="Disbursement Amount 2";                                           
       label disb_dt_3="Disbursement Date 3";                                              
       label disb_amt_3="Disbursement Amount 3";                                           
       label disb_dt_4="Disbursement Date 4";                                              
       label chg_cert_dt="Change Certification Date";                                      
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 dec_ln_amt 6.                                                           
             @145 rev_cert_amt 6.                                                         
             @151 cost_attend 5.                                                          
             @156 fam_cont_amt 5.                                                         
             @161 est_aid_amt 5.                                                          
             @166 disb_dt_1 YYMMDD8.                                                            
             @174 disb_amt_1 8.2                                                           
             @182 disb_dt_2 YYMMDD8.                                                            
             @190 disb_amt_2 8.2                                                           
             @198 disb_dt_3 YYMMDD8.                                                            
             @206 disb_amt_3 8.2                                                           
             @214 disb_dt_4 YYMMDD8.                                                            
             @222 disb_amt_4 8.2                                                           
             @231 chg_cert_dt YYMMDD8.                                                          
             @239 stamp $20.                                                               
             @259 duns_sch $9.                                                             
             @268 duns_recip $9.                                                           
             @277 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
               
 /*@114*/                  
    data WORK.clchange14    (where=( rec_code EQ '@1' and rec_type EQ '14'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=285 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format inc_ln_amt DOLLAR10.2 ;                                                             
       format rev_cert_amt DOLLAR10.2 ;                                                           
       format cost_attend DOLLAR10.2 ;                                                            
       format fam_cont_amt DOLLAR10.2 ;                                                           
       format est_aid_amt DOLLAR10.2 ;                                                            
       format disb_dt_1 MMDDYY10. ;                                                              
       format disb_amt_1 DOLLAR10.2 ;                                                             
       format disb_dt_2 MMDDYY10. ;                                                              
       format disb_amt_2 DOLLAR10.2 ;                                                             
       format disb_dt_3 MMDDYY10. ;                                                              
       format disb_amt_3 DOLLAR10.2 ;                                                             
       format disb_dt_4 MMDDYY10. ;                                                              
       format disb_amt_4 DOLLAR10.2 ;                                                             
       format chg_cert_dt MMDDYY10. ;                                                            
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch/Division Code";                         
       label cluid="CommonLine Unique Identifier";                                         
       label cluid_sfx="CommonLine Loan Sequence Number";                                  
       label inc_ln_amt="Increased Loan Amount";                                           
       label rev_cert_amt="Revised Certification Amount";                                  
       label cost_attend="Cost of Attendance";                                             
       label fam_cont_amt="Expected Family Contribution Amount";                           
       label est_aid_amt="Estimated Financial Aid Amount";                                 
       label disb_dt_1="Disbursement Date 1";                                              
       label disb_amt_1="Disbursement Amount 1";                                           
       label disb_dt_2="Disbursement Date 2";                                              
       label disb_amt_2="Disbursement Amount 2";                                           
       label disb_dt_3="Disbursement Date 3";                                              
       label disb_amt_3="Disbursement Amount 3";                                           
       label disb_dt_4="Disbursement Date 4";                                              
       label disb_amt_4="Disbursement Amount 4";                                           
       label chg_cert_dt="Change Certification Date";                                      
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 inc_ln_amt 6.                                                           
             @145 rev_cert_amt 6.                                                         
             @151 cost_attend 5.                                                          
             @156 fam_cont_amt 5.                                                         
             @161 est_aid_amt 5.                                                          
             @166 disb_dt_1 YYMMDD8.                                                            
             @174 disb_amt_1 8.2                                                           
             @182 disb_dt_2 YYMMDD8.                                                            
             @190 disb_amt_2 8.2                                                           
             @198 disb_dt_3 YYMMDD8.                                                            
             @206 disb_amt_3 8.2                                                           
             @214 disb_dt_4 YYMMDD8.                                                            
             @222 disb_amt_4 8.2                                                           
             @231 chg_cert_dt YYMMDD8.                                                          
             @239 stamp $20.                                                               
             @259 duns_sch $9.                                                             
             @268 duns_recip $9.                                                           
             @277 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                                                                                   
                               
 /*@119*/                  
    data WORK.clchange19    (where=( rec_code EQ '@1' and rec_type EQ '19'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=347 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format email_add $256. ;                                                            
       format email_valid $1. ;                                                            
       format email_eff_dt MMDDYY10. ;                                                           
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label email_add="E-mail Address";                                                   
       label email_valid="E-mail Address Validity Indicator";                              
       label email_eff_dt="E-mail Address Effective Date";                                 
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @44 email_add $256.                                                           
             @300 email_valid $1.                                                          
             @301 email_eff_dt YYMMDD8.                                                         
             @310 stamp $20.                                                               
             @330 duns_sch $9.                                                             
             @339 duns_recip $9.                                                           
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;             
 /*@124*/                  
    data WORK.clchange24    (where=( rec_code EQ '@1' and rec_type EQ '24'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=285 ;
       format rec_code $2. ;                                                               
       format rec_type $2. ;                                                               
       format ssn SSN11. ;                                                                    
       format sch_id $8. ;                                                                 
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format gty_dt MMDDYY10. ;                                                                 
       format ln_type $2. ;                                                                
       format alt_ln_prog $3. ;                                                            
       format disb_1_dt MMDDYY10. ;                                                              
       format lender_id $6. ;                                                              
       format ln_beg_dt MMDDYY10. ;                                                              
       format ln_end_dt MMDDYY10. ;                                                              
       format sch_br_code $2. ;                                                            
       format plus_ssn $9.;
       format cluid $17. ;                                                                 
       format cluid_sfx $2. ;                                                              
       format inc_ln_amt DOLLAR10.2 ;                                                             
       format rev_cert_amt DOLLAR10.2 ;                                                           
       format cost_attend DOLLAR10.2 ;                                                            
       format fam_cont_amt DOLLAR10.2 ;                                                           
       format est_aid_amt DOLLAR10.2 ;                                                            
       format disb_dt_1 MMDDYY10. ;                                                              
       format disb_amt_1 DOLLAR10.2 ;                                                             
       format disb_dt_2 MMDDYY10. ;                                                              
       format disb_amt_2 DOLLAR10.2 ;                                                             
       format disb_dt_3 MMDDYY10. ;                                                              
       format disb_amt_3 DOLLAR10.2 ;                                                             
       format disb_dt_4 MMDDYY10. ;                                                              
       format disb_amt_4 DOLLAR10.2 ;                                                             
       format chg_cert_dt MMDDYY10. ;                                                            
       format stamp $20. ;                                                                 
       format duns_sch $9. ;                                                               
       format duns_recip $9. ;                                                             
       format duns_lend $9. ;                                                              
       label rec_code="Record Code";                                                       
       label rec_type="Record Type";                                                       
       label ssn="Borrower SSN";                                                           
       label sch_id="School ID";                                                           
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label gty_dt="Guarantee Date";                                                      
       label ln_type="Loan Type Code";                                                     
       label alt_ln_prog="Alternative Loan Program Type Code";                             
       label disb_1_dt="First Disbursement Date";                                          
       label lender_id="Lender ID";                                                        
       label ln_beg_dt="Loan Period Begin Date";                                           
       label ln_end_dt="Loan Period End Date";                                             
       label sch_br_code="School Designated Branch/Division Code";                         
       label plus_ssn="Plus/Alternative Student SSN";
       label cluid="CommonLine Unique Identifier";                                         
       label cluid_sfx="CommonLine Loan Sequence Number";                                  
       label inc_ln_amt="Increased Loan Amount";                                           
       label rev_cert_amt="Revised Certification Amount";                                  
       label cost_attend="Cost of Attendance";                                             
       label fam_cont_amt="Expected Family Contribution Amount";                           
       label est_aid_amt="Estimated Financial Aid Amount";                                 
       label disb_dt_1="Disbursement Date 1";                                              
       label disb_amt_1="Disbursement Amount 1";                                           
       label disb_dt_2="Disbursement Date 2";                                              
       label disb_amt_2="Disbursement Amount 2";                                           
       label disb_dt_3="Disbursement Date 3";                                              
       label disb_amt_3="Disbursement Amount 3";                                           
       label disb_dt_4="Disbursement Date 4";                                              
       label disb_amt_4="Disbursement Amount 4";                                           
       label chg_cert_dt="Change Certification Date";                                      
       label stamp="Date/Time Stamp";                                                      
       label duns_sch="DUNS School ID";                                                    
       label duns_recip="DUNS Recipient ID";                                               
       label duns_lend="DUNS Lender ID";                                                   
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_type $2.                                                               
             @5 ssn 9.                                                                    
             @14 sch_id $8.                                                                
             @29 recip_id $8.                                                              
             @40 recip_br $4.                                                              
             @60 gty_dt YYMMDD8.                                                                
             @68 ln_type $2.                                                               
             @70 alt_ln_prog $3.                                                           
             @73 disb_1_dt YYMMDD8.                                                             
             @81 lender_id $6.                                                             
             @93 ln_beg_dt YYMMDD8.                                                             
             @101 ln_end_dt YYMMDD8.                                                            
             @109 sch_br_code $2.                                                          
             @111 plus_ssn $9.
             @120 cluid $17.                                                               
             @137 cluid_sfx $2.                                                            
             @139 inc_ln_amt 6.                                                           
             @145 rev_cert_amt 6.                                                         
             @151 cost_attend 5.                                                          
             @156 fam_cont_amt 5.                                                         
             @161 est_aid_amt 5.                                                          
             @166 disb_dt_1 YYMMDD8.                                                            
             @174 disb_amt_1 8.2                                                           
             @182 disb_dt_2 YYMMDD8.                                                            
             @190 disb_amt_2 8.2                                                           
             @198 disb_dt_3 YYMMDD8.                                                            
             @206 disb_amt_3 8.2                                                           
             @214 disb_dt_4 YYMMDD8.                                                            
             @222 disb_amt_4 8.2                                                           
             @231 chg_cert_dt YYMMDD8.                                                          
             @239 stamp $20.                                                               
             @259 duns_sch $9.                                                             
             @268 duns_recip $9.                                                           
             @277 duns_lend $9.                                                            
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;                  
/*@T*/                  
    data WORK.clchangeT    (where=( rec_code EQ '@T'))   ;
   %let _EFIERR_ = 0; /* set the ERROR detection macro variable */                         
    infile &CHTRANSFILE   TRUNCOVER lrecl=143 ;
       format rec_code $2. ;                                                               
       format rec_count 6. ;                                                              
       format sup_rec_count 6. ;                                                          
       format file_cr_dt MMDDYY10. ;                                                             
       format file_cr_time $6. ;                                                           
       format file_id_code $5. ;                                                           
       format source_name $32. ;                                                           
       format source_id $8. ;                                                              
       format source_br $4. ;                                                              
       format recip_name $32. ;                                                            
       format recip_id $8. ;                                                               
       format recip_br $4. ;                                                               
       format duns_source $9. ;                                                            
       format duns_recip $9. ;                                                             
       label rec_code="Record Code";                                                       
       label rec_count="@1 Detail Record Count";                                           
       label sup_rec_count="Unique Supplemental (@2) Detail Record Count";                 
       label file_cr_dt="File Creation Date";                                              
       label file_cr_time="File Creation Time";                                            
       label file_id_code="File Identifier Code";                                          
       label source_name="Source Name";                                                    
       label source_id="Source ID";                                                        
       label source_br="Source Non-ED Branch ID";                                          
       label recip_name="Recipient Name";                                                  
       label recip_id="Recipient ID";                                                      
       label recip_br="Recipient Non-ED Branch ID";                                        
       label duns_source="DUNS Source ID";                                                 
       label duns_recip="DUNS Recipient ID";                                               
    input                                                                                  
             @1 rec_code $2.                                                               
             @3 rec_count 6.                                                              
             @9 sup_rec_count 6.                                                          
             @15 file_cr_dt YYMMDD8.                                                            
             @23 file_cr_time $6.                                                          
             @29 file_id_code $5.                                                          
             @34 source_name $32.                                                          
             @66 source_id $8.                                                             
             @76 source_br $4.                                                             
             @80 recip_name $32.                                                           
             @112 recip_id $8.                                                             
             @122 recip_br $4.                                                             
             @126 duns_source $9.                                                          
             @135 duns_recip $9.                                                           
    ;                                                                                      
    if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */   
    run;             

OPTIONS ERRORS=10;

PROC SORT DATA = CLCHANGE02 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE07 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE08 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE09 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE10 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE11 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE12 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE13 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE14 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE19 ; BY SSN; RUN;
PROC SORT DATA = CLCHANGE24 ; BY SSN; RUN;

DATA CLCHANGE;
SET CLCHANGE07 CLCHANGE08 CLCHANGE09 
	CLCHANGE10 CLCHANGE11 CLCHANGE12 CLCHANGE13 CLCHANGE14 
	CLCHANGE19 CLCHANGE24 ;
RUN;

PROC SORT DATA = CLCHANGE ; BY SSN; RUN;

DATA CLCHANGE  ;*(DROP = LAST FIRST MI);
MERGE CLCHANGE CLCHANGE02 (KEEP = SSN LAST FIRST MI)  ;
BY SSN;

IF PLUS_SSN = '000000000' THEN PLUS_SSN = ' ';
	ELSE IF PLUS_SSN = ' ' THEN PLUS_SSN = ' ';
	ELSE PLUS_SSN = 
	SUBSTR(PLUS_SSN,1,3)||'-'||SUBSTR(PLUS_SSN,4,2)||'-'||SUBSTR(PLUS_SSN,6);

*MID = PUT(MI, $1.);
IF TRIM(MI) NE ' ' 
	THEN NAME = TRIM(FIRST)||' '||TRIM(MI)||' '||TRIM(LAST);
ELSE IF TRIM(MI) = ' ' 
	THEN NAME = TRIM(FIRST)||' '||TRIM(LAST);
ELSE NAME = TRIM(FIRST)||' '||TRIM(MI)||' '||TRIM(LAST);

MERGEBY = '1';

IF SUBSTR(LENDER_ID,1,6) = '822373' THEN
	LENDER_NAME = SUBSTR('AMERICA FIRST CREDIT UNION',1,20);
IF SUBSTR(LENDER_ID,1,6) = '833828' THEN
	LENDER_NAME = SUBSTR('DESERET FIRST CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '819628' THEN
	LENDER_NAME = 'BANK ONE';
ELSE IF SUBSTR(LENDER_ID,1,6) = '817440' THEN
	LENDER_NAME = SUBSTR('FAMILY FIRST CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '817545' THEN
	LENDER_NAME = SUBSTR('GRANITE CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '830791' THEN
	LENDER_NAME = 'JORDAN CREDIT UNION';
ELSE IF SUBSTR(LENDER_ID,1,6) = '813760' THEN
	LENDER_NAME = 'KEY BANK';
ELSE IF SUBSTR(LENDER_ID,1,6) = '817546' THEN
	LENDER_NAME = SUBSTR('MOUNTAIN AMERICA CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '820200' THEN
	LENDER_NAME = SUBSTR('TOOELE FEDERAL CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '830132' THEN
	LENDER_NAME = SUBSTR('UNIVERSITY OF UTAH CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '829123' THEN
	LENDER_NAME = SUBSTR('UTAH COMMUNITY CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '811698' THEN
	LENDER_NAME = 'U.S. BANK';
ELSE IF SUBSTR(LENDER_ID,1,6) = '830146' THEN
	LENDER_NAME = SUBSTR('USU COMMUNITY CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '820043' THEN
	LENDER_NAME = 'WASHINGTON MUTUAL';
ELSE IF SUBSTR(LENDER_ID,1,6) = '829158' THEN
	LENDER_NAME = SUBSTR('WEBER STATE CREDIT UNION',1,20);
ELSE IF SUBSTR(LENDER_ID,1,6) = '813894' THEN
	LENDER_NAME = 'WELLS FARGO';
ELSE IF SUBSTR(LENDER_ID,1,6) = '817455' THEN
	LENDER_NAME = 'ZIONS BANK';
ELSE LENDER_NAME = ' ';
RUN;

DATA CLCHANGEH;
SET CLCHANGEH;
MERGEBY = '1';
RUN;

DATA CLCHANGE (DROP = MERGEBY);
MERGE CLCHANGE CLCHANGEH (KEEP = FILE_CR_DT MERGEBY);
BY MERGEBY;
RUN;

PROC SORT DATA = CLCHANGE ; BY REC_TYPE LENDER_ID SSN CLUID CLUID_SFX; RUN;

DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '07';
	FILE &PRINTFILE PRINT n=pagesize ;*MOD;
	TITLE;
	PUT @30 "LOAN PERIOD CHANGE" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"LOAN INFORMATION"
		/
		/"Loan Period:" @40 REV_LN_BEG_DT "- " REV_LN_END_DT
		/"Anticipated Graduation Date:" @40 ACD
		/"Grade Level:" @40 GRADE
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
		/"School Sign Date:" @20 CHG_CERT_DT
	;
	PUT _PAGE_;
	
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '08';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT @24 "LOAN CANCELLATION/REINSTATEMENT" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"REINSTATEMENT"
		/
		/"Reinstatement Loan Amt:" @40 REIN_LN_AMT
		/
		/
		/"CANCELLATION" 
		/
		/"Entire Loan as of:  " @40 CANC_DT
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
;
	PUT _PAGE_;
	
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '09';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT @23  "PRE-DISBURSEMENT CANCELLATION/CHANGE" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"REINSTATEMENT"
		/
		/"Reinstatement Indicator Code:" @40 REINST_IND
		/
		/
		/"CANCELLATION"
		/						@22 "Revised"	@34 "Revised"	@46 "Cancel/"	@58 "Cancel/"	@70 "Disb H/R"
		/"Disb" 	@10 "Disb"	@22 "Disb" 		@34 "Disb"		@46 "Reduction"	@58 "Reduction"	@70 "Indicator"
		/"No."		@10 "Date"	@22 "Date"		@34 "Amount"	@46 "Date"		@58 "Amount"	@70 "Code"
		/
		/@3 DISB_NO @10 DISB_DT	MMDDYY8. @22 REV_DISB_DT MMDDYY8. @34 REV_DISB_AMT @46 CANC_DT MMDDYY8. @58 CANC_AMT	@70 HOLD_REL_IND
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
	;
	PUT _PAGE_;
	
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '10';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT @22  "POST-DISBURSEMENT CANCELLATION/CHANGE" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"REINSTATEMENT"
		/
		/"Funds Reissue Indicator Code:" @40 FUND_REIS_IND
		/"Reinstatement Indicator Code:" @40 REINST_IND
		/
		/
		/"CANCELLATION"																		
		/						@25 "Revised"	@40 "Revised"	@55 "Cancel/"	
		/"Disb" 	@10 "Disb"	@25 "Disb" 		@40 "Disb"		@55 "Reinstate"	@70 "Cancel"	
		/"No."		@10 "Date"	@25 "Date"		@40 "Amount"	@55 "Date"		@70 "Amount"	
		/
		/@3 DISB_NO @10 DISB_DT	MMDDYY8. @25 REV_DISB_DT MMDDYY8. @40 REV_DISB_AMT @55 CANC_DT MMDDYY8. @70 CANC_AMT
		/
		/			@10 "Disb"							@40 "Funds"
		/			@10 "Consumation"	@25 "Actual"	@40 "Return"
		/			@10 "Indicator"		@25	"Returned"	@40 "Method"
		/			@10 "Code"			@25 "Amount"	@40 "Code"
		/
		/			@12 DISB_CON_IND	@25 ACT_RET_AMT	@40 FUND_RET_METH
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
	;
	PUT _PAGE_;

RUN;
DATA CLCHANGE11;
SET CLCHANGE;
WHERE REC_TYPE = '11';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT @32 "SCHOOL REFUND"  
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"REFUND"
		/
		/"Refund Date:" @40 SCH_REF_DT 
		/"Refund Amount:" @40 SCH_REF_AMT
		/"Withdrawal Date:" @40 WD_DT
		/"Funds Return Method:" @40 FUND_RET_METH
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
	;
	PUT _PAGE_;
PRINT = 1;
RUN;
DATA CLCHANGE12;
SET CLCHANGE;
WHERE REC_TYPE = '12';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT @27 "SCHOOL REFUND CORRECTION"  
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"REVISED REFUND"
		/
		/"Date:" @40 REV_SCH_REF_DT
		/"Actual Refund Amount:" @40 REV_SCH_REF_AMT
		/"Cumulative Amount:" @40 CUM_SCH_REF_AMT
		/"Last School Refund Date:" @40 LST_SCH_REF_DT
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
	;
	PUT _PAGE_;
PRINT = 1;
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '13';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT  @26 "REALLOCATION LOAN DECREASE" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"LOAN INCREASE/REALLOCATION INFORMATION"
		/
		/				@15 "Date" 		@30 "Amount"
		/"1st Disb."	@15 DISB_DT_1	@30 DISB_AMT_1
		/"2nd Disb."	@15 DISB_DT_2	@30 DISB_AMT_2
		/"3rd Disb."	@15 DISB_DT_3	@30 DISB_AMT_3
		/"4th Disb." 	@15 DISB_DT_4	@30 DISB_AMT_4
		/
		/"Revised Cert Amount:" 	@30 REV_CERT_AMT
		/"Prorated Cert Amount:"	@30 DEC_LN_AMT
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
		/"School Sign Date:" @20 CHG_CERT_DT
	;
	PUT _PAGE_;
	
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '14';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT  @26 "REALLOCATION LOAN INCREASE" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"LOAN INCREASE/REALLOCATION INFORMATION"
		/
		/				@15 "Date" 		@30 "Amount"
		/"1st Disb."	@15 DISB_DT_1	@30 DISB_AMT_1
		/"2nd Disb."	@15 DISB_DT_2	@30 DISB_AMT_2
		/"3rd Disb."	@15 DISB_DT_3	@30 DISB_AMT_3
		/"4th Disb." 	@15 DISB_DT_4	@30 DISB_AMT_4
		/
		/"Revised Cert Amount:" 		@30 REV_CERT_AMT
		/"Prorated Cert Amount:"		@30 INC_LN_AMT
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
		/"School Sign Date:" @20 CHG_CERT_DT
	;
	PUT _PAGE_;
	
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '19';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT @30 "E-MAIL INFORMATION"
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"LOAN INFORMATION"
		/
		/"New Email Address:" @40 EMAIL_ADD
		/"Valid Email Address?" @40 EMAIL_VALID
		/"Effective Date:" @40 EMAIL_EFF_DT
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
	;
	PUT _PAGE_;
	
RUN;
DATA _NULL_;
SET CLCHANGE;
WHERE REC_TYPE = '24';
	FILE &PRINTFILE PRINT n=pagesize MOD;
	TITLE;
	PUT  @32 "LOAN INCREASE" 
		/@25 "Change Transaction Data Sheet"
		/@25 "File Creation Date:  " FILE_CR_DT MMDDYY8.
		/@28 "Report Date:  &fdate"
		/
		/
		/"Data Sheet for Student:" @25 NAME @50 "Student's SSN:" @68 SSN
		/@50 "Plus SSN:" @68 PLUS_SSN
		/
		/"Loan Type:" @18 LN_TYPE @40 "Lender ID:" @60 LENDER_ID
		/"Guarantee Date:" @18 GTY_DT @40 "Lender Name:" @60 LENDER_NAME
		/"Unique Loan ID:" @18 CLUID CLUID_SFX @40 "Curr Loan Period:" @60 LN_BEG_DT MMDDYY8. " - " LN_END_DT MMDDYY8.
		/
		/
		/"LOAN INCREASE/REALLOCATION INFORMATION"
		/
		/				@15 "Date" 		@30 "Amount"
		/"1st Disb."	@15 DISB_DT_1	@30 DISB_AMT_1
		/"2nd Disb."	@15 DISB_DT_2	@30 DISB_AMT_2
		/"3rd Disb."	@15 DISB_DT_3	@30 DISB_AMT_3
		/"4th Disb." 	@15 DISB_DT_4	@30 DISB_AMT_4
		/
		/"Revised Cert Amount:" 		@30 REV_CERT_AMT
		/"Prorated Cert Amount:"		@30 INC_LN_AMT
		/
		/
		/"CERTIFICATION INFORMATION"
		/
		/"School ID:" @20 SCH_ID @40 "Recipient ID:" @60 RECIP_ID
		/"School Sign Date:" @20 CHG_CERT_DT
	;
	PUT _PAGE_;
	
RUN;

DATA CLCHANGEHT;
MERGE CLCHANGEH CLCHANGET;
BY FILE_CR_DT;
RUN;

*Find the number of each record type read from the file;
*Find the number printed of each record type;
PROC SQL;
CREATE TABLE SUMMARY AS
SELECT 	REC_TYPE||' - Borrower Detail Records' AS REC_TYPE,
		COUNT(*) 	AS COUNT
FROM CLCHANGE02
GROUP BY REC_TYPE;

ALTER TABLE SUMMARY
	MODIFY REC_TYPE CHAR(60) FORMAT = $60.;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Loan Period Change' AS REC_TYPE,
		COUNT(*)  	AS COUNT
FROM CLCHANGE07
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Loan Cancellation/Reinstatement' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE08
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Disbursement Cancellation/Change' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE09
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Disbursement Notification/Change' AS REC_TYPE,
		COUNT(*)  	AS COUNT
FROM CLCHANGE10
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - School Refund' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE11
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - School Refund Correction' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE12
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Sub/Unsub Reallocation Loan Decrease' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE13
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Sub/Unsub Reallocation Loan Increase' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE14
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - E-mail Information' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE19
GROUP BY REC_TYPE;

INSERT INTO SUMMARY 
SELECT 	REC_TYPE||' - Loan Increase' AS REC_TYPE, 
		COUNT(*)  	AS COUNT
FROM CLCHANGE24
GROUP BY REC_TYPE;
QUIT;


PROC SQL;
CREATE TABLE SUMMARY_SUM AS
SELECT 	'TOTAL CHANGE TRANSACTIONS READ'	AS REC_TYPE, 
		SUM(COUNT)							AS COUNT
FROM SUMMARY
WHERE SUBSTR(REC_TYPE,1,2) <> '02';

ALTER TABLE SUMMARY_SUM
	MODIFY REC_TYPE CHAR(60) FORMAT = $60.;
	
INSERT INTO SUMMARY_SUM
SELECT 	'TOTAL @1 RECORDS READ'		AS REC_TYPE, 
		SUM(COUNT)						AS COUNT
FROM SUMMARY;

INSERT INTO SUMMARY_SUM 
SELECT 	'TOTAL @1 RECORDS SENT' 	AS REC_TYPE, 
		REC_COUNT					AS COUNT
FROM CLCHANGEHT;

INSERT INTO SUMMARY
SELECT *
FROM SUMMARY_SUM;
QUIT;


PROC PRINTTO PRINT=&PRINTFILE ;
RUN;
OPTIONS CENTER NODATE NONUMBER LS=80 LABEL;

PROC PRINT DATA = SUMMARY NOOBS LABEL;
VAR REC_TYPE COUNT;
LABEL 	COUNT = 'Count'
		REC_TYPE = "Record Type";
TITLE "Summary Report for CommonLine Change Transaction Records";
TITLE2 "Report Date:  &fdate";
RUN;

PROC PRINTTO;
RUN;
&fdate";
RUN;

PROC PRINTTO;
RUN;
