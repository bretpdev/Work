CREATE PROCEDURE [dbo].[GetLON66_DAT]
    @RepaymentDataId int
AS
--BDNBPM: ITLQSQLDF_Disclosure.number_of_payments
--BDGPAM: D.group_payment_amount
--GRHSST: ITEKSQLDF_Group.gr_pmt_schedule
--GRGMPC: G.gr_monthly_pmt_amt_curr
--GRGTPR: G.gr_num_term_period_remain
--GRGTPO: G.gr_num_term_periods_orig
--BIPSDTA: IBR.
--If GRHSST =  G or X  (use a $5 tolerance on the amount comparisons)
--a. Current payment = GRGMPC
--b. Current term = GRGTPR (if not filled use GRGTPO)
--If GRHSST = F 
--a. Current payment = GRGMPC
--b. Current term = 12
--c. 2nd payment = BIPSDTA
--d. 2nd term = GRGTPR minus 12
--If GRHSST = P
--a. Current payment = GRGMPC 
--b. Current term = GRGTPR (if not filled use GRGTPO)
--If GRHSST = blank or E 
--a. Current payment = GRGMPC
--b. Current Term = GRGTPR (if not filled use GRGTPO)
declare @temp as table(
	number_of_payments varchar(3),
	group_payment_amount varchar(9),
	gr_pmt_schedule varchar(1),
	gr_num_term_period_remain nvarchar(max),
	gr_num_term_periods_orig nvarchar(max),
	gr_monthly_pmt_amt_curr nvarchar(max),
	perm_standard_amount nvarchar(max)
)
	insert into @temp
    select distinct
		D.number_of_payments,
		D.group_payment_amount,
		G.gr_pmt_schedule,
		G.gr_num_term_period_remain,
		G.gr_num_term_periods_orig,
		G.gr_monthly_pmt_amt_curr,
		ibr.perm_standard_amount
    from
	    [dbo].[ITELSQLDF_Loan_NoZeroBalances] l
		INNER JOIN [dbo].[ITEKSQLDF_Group] G
			ON G.br_ssn = L.br_ssn
			AND G.gr_id = L.gr_id
	    left join (
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
		left join [dbo].[ITLSSQLDF_IBR] ibr
			on ibr.br_ssn = l.br_ssn
			and ibr.ln_num = l.ln_num
    where 
        l.FinancialDataId = @RepaymentDataId

	declare @numRows int
	select @numRows = count(*) from @temp

	(
	SELECT
		CASE
			WHEN number_of_payments IS NOT NULL THEN number_of_payments
			WHEN number_of_payments IS NULL AND gr_pmt_schedule in ('F') THEN '012'
			ELSE ISNULL(gr_num_term_period_remain, gr_num_term_periods_orig) 
		END AS LN_RPS_TRM, 
		cast((convert(int, 
			CASE
				WHEN number_of_payments IS NOT NULL THEN group_payment_amount
				WHEN number_of_payments IS NULL THEN gr_monthly_pmt_amt_curr
			END) / 100.0) as decimal(9, 2))
			AS LA_RPS_ISL	
	FROM @temp
	)
	UNION
	(
	SELECT 
		Replace(Str(cast(gr_num_term_period_remain as int) - 12, 3), ' ', '0') LN_RPS_TERM, 
		cast((convert(int,perm_standard_amount) / 100.0) as decimal(9, 2)) LA_RPS_ISL
	FROM @temp
	WHERE
		@numRows = 1 AND
		not exists(
			select 
				* 
			from 
				@temp 
			where 
				gr_pmt_schedule = 'F' and 
				number_of_payments is null and cast(gr_num_term_period_remain as int) > 12
		)
	)
	order by 
    LN_RPS_TRM desc

RETURN 0
