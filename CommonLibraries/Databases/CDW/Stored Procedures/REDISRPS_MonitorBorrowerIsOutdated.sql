
CREATE Procedure REDISRPS_MonitorBorrowerIsOutdated
	(
		@AccountIdentifier nvarchar(10)
	)
AS

declare @AccountNumber char(10)
if (len(@AccountIdentifier) = 9) select @AccountNumber = DF_SPE_ACC_ID from dbo.PD10_Borrower where BF_SSN = @AccountIdentifier
else set @AccountNumber = @AccountIdentifier

declare @Exists bit = 0
set nocount on
select @Exists = 1 from LN65_RepaymentSched sched
  join AY10_History hist on hist.DF_SPE_ACC_ID = sched.DF_SPE_ACC_ID
where hist.PF_REQ_ACT in ('FNRVW', 'LS020', 'LS021', 'LS070', 'LS071', 'LS072', 'LS073', 'PS455', 'PS456', 'PS457', 'PS458', 'PS459', 'PS460', 'PS461', 'PS462', 'PS465', 'P040A', 'RPMIS', 'SOAIA', 'SOAUA')
  and sched.DF_SPE_ACC_ID = @AccountNumber
group by sched.DF_SPE_ACC_ID
having max(sched.LD_CRT_LON65) > max(hist.LD_ATY_REQ_RCV)
set nocount off
select @Exists



