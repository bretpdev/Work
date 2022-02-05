CREATE PROCEDURE [dbo].[GetLON66_DAT]
    @RepaymentDataId int
AS
    select distinct
	CASE
		WHEN D.number_of_payments IS NOT NULL THEN D.number_of_payments
		WHEN D.number_of_payments IS NULL AND G.gr_pmt_schedule IN ('P','F') THEN '012'
		WHEN D.number_of_payments IS NULL AND G.gr_pmt_schedule IN ('G', 'X') THEN '024'
		WHEN D.number_of_payments IS NULL AND G.gr_pmt_schedule IN ('', 'E') THEN G.gr_num_term_periods_orig    
	END AS LN_RPS_TRM, 
	CASE
		WHEN D.number_of_payments IS NULL THEN (round(((cast(ISNULL(l.ln_curr_principal, 00) as money) / 100) / bal.balance) * cast(ISNULL(g.gr_monthly_pmt_amt_curr,00) as money), 0) / 100) 
		ELSE (round(((cast(ISNULL(l.ln_curr_principal, 00) as money) / 100) / bal.balance) * cast(ISNULL(d.group_payment_amount,00) as money), 0) / 100)
	END AS LA_RPS_ISL
    from
	    [dbo].[ITELSQLDF_Loan] l
		INNER JOIN [dbo].[ITEKSQLDF_Group] G
			ON G.br_ssn = L.br_ssn
			AND G.gr_id = L.gr_id
	    left join 
		    (
			    select
				    br_ssn,
                    gr_id,
				    sum(cast(ln_curr_principal as money) / 100) as balance
			    from
				    [dbo].[ITELSQLDF_Loan]
			    group by 
				    br_ssn,
                    gr_id
		    ) bal
			    on bal.br_ssn = l.br_ssn
                and bal.gr_id  = l.gr_id
	    left join
		    (
			    select
				    br_ssn,
					group_id,
				    max(dbo.CONVERT_DATE(disclosure_date)) as disc_date
			    from
				    [dbo].[ITLQSQLDF_Disclosure]
			    group by 
					br_ssn,
					group_id
		    ) d_date
			    on d_date.br_ssn = l.br_ssn
			    and d_date.group_id = l.gr_id
			left join [dbo].[ITLQSQLDF_Disclosure] d
				on d.br_ssn = l.br_ssn
				and d.group_id = l.gr_id
				and dbo.convert_date(d.post_date) is not null
				and dbo.CONVERT_DATE(d.disclosure_date) = d_date.disc_date
    where 
        l.FinancialDataId = @RepaymentDataId
    order by 
        LN_RPS_TRM desc

RETURN 0
