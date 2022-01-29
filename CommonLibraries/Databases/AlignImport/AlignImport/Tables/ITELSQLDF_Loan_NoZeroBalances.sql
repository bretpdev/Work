CREATE VIEW [dbo].[ITELSQLDF_Loan_NoZeroBalances]
	AS 
	SELECT 
		FinancialDataId, br_ssn, ln_num, ln_loan_type, ln_status, ln_name_last, ln_name_first, ln_name_mi, br_benefiting_student_ssn, br_comaker_ssn, ln_second_comaker, ld_lender_id, gr_id, 
		co_number, secondary_co_number, ga_id, si_school_code, si_branch, cr_country_code, st_state_code, cy_county, ln_interest_rate, ln_post_deferment_grace, ln_grad_sep_date, ln_1st_disb_date, 
		ln_amt_disb, ln_capped_interest_ltd, ln_conversion_date, ln_curr_fees, ln_curr_interest, ln_curr_principal, ln_daily_interest_amt, ln_fees_due_orig, ln_fees_paid, ln_govt_subsidy_sts, 
		ln_gr_due_date, ln_grade_level, ln_guarantee_date, ln_inactive_active_sts, ln_insured_sts, ln_interest_rate_govt, ln_change_int_date, ln_change_int_rate, 
		ln_intertest_rate_var_sw, ln_length_of_grace, ln_orig_loaned_amt, ln_orig_loan_term, ln_orig_prin_purchased, ln_accr_govt_int_prev_qtr, ln_accr_govt_int_curr_qtd, 
		ln_bond_number, ln_buy_date, ln_interest_start_date, ln_paid_fee_ytd, ln_paid_fee_borr_prev_ytd, ln_paid_int_borr_ltd, ln_paid_int_borr_ytd, ln_paid_int_borr_prev_ytd, 
		ln_paid_int_total_ltd, ln_paid_int_total_ytd, ln_paid_prin_borr_ltd, ln_paid_prin_borr_ytd, ln_paid_prin_borr_ytd_priv, ln_paid_prin_total_ltd, ln_paid_prin_total_ytd, 
		ln_period_ending_date, ln_period_starting_date, ln_prev_system_id, ln_prev_lender_id, ln_refund_amount, ln_refund_date, ln_late_charges, ln_late_charges_paid_ltd, 
		ln_late_charges_paid_ytd, ln_late_chargs_pd_rev_ytd, ln_sell_date, ln_new_lender, ln_new_bond_number, ln_special_allowance_code, ln_special_allow_chg_date, ln_disclose_required_sw, 
		ln_defer_eligibility, ln_pdg_end_date, ln_backing_date_limit, ln_lh_work_disb_amt, ln_current_loaned_amt, ln_num_sa_days, ln_remaining_term_length, ln_defer_int_accrual_sw, ln_rebate, 
		ln_promissory_note, ln_promissory_note_date, ln_promissory_returned_dt, ln_pif_date, ln_payment_allocation, ln_incentive_election_ind, ln_pif_switch, ln_1st_loan, ln_cons_unsub_loans, 
		ln_region_code, ln_cde8a, ln_interest_rate_code, ln_ltr_starting_number, ln_judgement_rate, ln_max_int_rate, ln_school_ref_wo_cancel, ln_int_rate_reduction, ln_suspended_sa_flag, 
		ln_annual_incentive_sw, ln_new_responsible_lender, ln_procedure_status, ln_suspended_int_flag, ln_tracking_value, ln_check_eft_date, ln_application_date, ln_rehab_date, ln_ltd_other_ir_amts, 
		ln_ltd_bank_draft_ir_amts, ln_guarantee_loan_id, ln_academic_year, ln_nslds_loan_ld, ln_orig_mu_key, ln_cml_unique_loan_id, ln_consumer_info_ind, ln_cmp_cnd_code, ln_fiscal_year, 
		ln_cml_seq_nbr, N
	FROM 
		[dbo].[ITELSQLDF_Loan] loan
	WHERE replace(loan.ln_curr_principal, '0', '') <> ''
		
