CREATE PROCEDURE [dbo].[GetLON66_DAT]
    @RepaymentDataId int = 0
AS
    select distinct
	    ISNULL(d.number_of_payments,000) AS LN_RPS_TRM, 
	    ISNULL(convert(decimal(10,2),round(((cast(ISNULL(l.ln_curr_principal, 00) as decimal(10,2)) / 100) / bal.balance) * cast(ISNULL(d.group_payment_amount,00) as decimal(10,2)), 0) / 100),00) AS LA_RPS_ISL
    from
	    [dbo].[ITELSQLDF_Loan] l
	    inner join [dbo].[ITLQSQLDF_Disclosure] d
		    on d.br_ssn = l.br_ssn
		    and d.group_id = l.gr_id
	    inner join 
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
	    inner join
		    (
			    select
				    br_ssn,
				    max(dbo.convert_date(disclosure_date)) as disc_date
			    from
				    [dbo].[ITLQSQLDF_Disclosure]
			    group by br_ssn
		    ) d_date
			    on d_date.br_ssn = d.br_ssn
			    and d_date.disc_date = dbo.convert_date(d.disclosure_date)
    where 
        l.FinancialDataId = @RepaymentDataId
    order by 
        LN_RPS_TRM desc

RETURN 0
