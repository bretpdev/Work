SELECT
	BUCC.BusinessUnitId,
	BU.[Name],
	BUCC.CostCenterId,
	CC.CostCenter
FROM
	CSYS..COST_DAT_BusinessUnitCostCenters BUCC
	INNER JOIN CSYS..COST_DAT_CostCenters CC
		ON BUCC.CostCenterId = CC.CostCenterId
	INNER JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BUCC.BusinessUnitId = BU.ID
WHERE
	BUCC.EffectiveEnd IS NULL