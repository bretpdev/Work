--08 Insert NeedHelp Tickets for Manual Allocation

INSERT INTO CSYS..[COST_DAT_NhManualAllocation] (TicketID,CostCenterDeterminant,CostCenterId,CostCenterWeight,EffectiveBegin)
VALUES
	(59742,'LPP to CS PS',13,100,'2019-01-01'),
	(36587,'CS to LPP PS',8,100,'2019-01-01'),
	(59750,'outside range stay overhead',13,100,'2019-04-01')


SELECT * FROM CSYS..[COST_DAT_NhManualAllocation]
