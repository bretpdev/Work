CREATE PROCEDURE [monitor].[GetRedisclosureExemptions]
	@AccountNumber char(10),
	@R0CreateDate datetime
AS

SET NOCOUNT ON
	
declare @IsInExemptLoanStatus bit = 0
select @IsInExemptLoanStatus = 
	case 
		when Count(LoanStatusCode) = Count(*) and Count(*) > 0
		then 1 else 0 
	end
from 
	dbo.DW01_Loan l
left join
	monitor.ExemptLoanStatuses e on l.WC_DW_LON_STA = e.LoanStatusCode
where
	l.DF_SPE_ACC_ID = @AccountNumber

declare @IsInExemptForbType bit = 0
select
	@IsInExemptForbType = 1
from
	dbo.FB10_Forbearance f
join 
	monitor.ExemptForbearanceTypes e on f.LC_FOR_TYP = e.ForbearanceTypeCode
where
	f.DF_SPE_ACC_ID = @AccountNumber
	and LD_FOR_BEG < GetDate()
    and LD_FOR_END > GetDate()
    and LC_STA_LON60 = 'A'
    and LC_FOR_STA = 'A'
    and LC_STA_FOR10 = 'A'

declare @IsInExemptScheduleType bit = 0
declare @AllExemptScheduleTypes bit = 0
select 
	@IsInExemptScheduleType = case when count(e.ScheduleName) > 0 then 1 else 0 end,
	@AllExemptScheduleTypes = case when count(e.ScheduleName) = count(*) then 1 else 0 end
from 
	dbo.LN65_RepaymentSched r
left join
	monitor.ExemptScheduleTypes e on r.TYP_SCH_DIS = e.ScheduleName
where
	r.DF_SPE_ACC_ID = @AccountNumber


declare @HasUndisclosedSetupArc bit = 0
select 
	@HasUndisclosedSetupArc = 1
from 
	dbo.AY10_History h
join
	monitor.ExemptSetupArcs e on h.PF_REQ_ACT = e.ARC
where
	h.DF_SPE_ACC_ID = @AccountNumber
	and h.LD_ATY_RSP = ''

declare @HasUndisclosedRepayOptionsArc bit = 0
select 
	@HasUndisclosedRepayOptionsArc = 1
from 
	dbo.AY10_History h
where
	h.DF_SPE_ACC_ID = @AccountNumber
	and h.LD_ATY_RSP = ''
	and h.PF_REQ_ACT = 'OVRPS'

declare @RedisclosedAfterR0Date bit = 0
select 
	@RedisclosedAfterR0Date = 1
from 
	dbo.BORR_Repayment b
where
	b.DF_SPE_ACC_ID = @AccountNumber
	and b.LD_CRT_LON65 > @R0CreateDate

declare @CreateDate10 datetime = null
select
	@CreateDate10 = min(LD_ATY_REQ_RCV)
from
	dbo.AY10_History
where
	DF_SPE_ACC_ID = @AccountNumber
and
	PF_REQ_ACT = 'OVRPS'
and
	LD_ATY_REQ_RCV > @R0CreateDate

SET NOCOUNT OFF

select
	@CreateDate10 as CreateDate10,
	@IsInExemptLoanStatus as IsInExemptLoanStatus,
	@IsInExemptForbType as IsInExemptForbType,
	@IsInExemptScheduleType as IsInExemptScheduleType,
	@AllExemptScheduleTypes as AllExemptScheduleTypes,
	@HasUndisclosedSetupArc as HasUndisclosedSetupArc,
	@HasUndisclosedRepayOptionsArc as HasUndisclosedRepayOptionsArc,
	@RedisclosedAfterR0Date as RedisclosedAfterR0Date

RETURN 0