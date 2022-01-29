DECLARE @DATA TABLE (BF_SSN INT)
INSERT INTO @DATA VALUES
(	032506447	),
(	083747543	),
(	105585854	),
(	230339053	),
(	288743537	),
(	297808027	),
(	367800027	),
(	376023134	),
(	376945100	),
(	381961515	),
(	393028150	),
(	431130128	),
(	465798260	),
(	504066911	),
(	515925617	),
(	516802362	),
(	518115095	),
(	518136725	),
(	518196346	),
(	518258643	),
(	519154763	),
(	519359925	),
(	520216011	),
(	521632156	),
(	522371985	),
(	523353180	),
(	524231358	),
(	525672842	),
(	525851240	),
(	528083534	),
(	528251775	),
(	528376780	),
(	528393732	),
(	528413411	),
(	528499812	),
(	528537340	),
(	528538165	),
(	528550816	),
(	528552566	),
(	528654816	),
(	528657747	),
(	528678931	),
(	528679003	),
(	528737193	),
(	528778482	),
(	528792588	),
(	528818479	),
(	528837055	),
(	528879985	),
(	528891692	),
(	528986793	),
(	528991773	),
(	529118881	),
(	529131905	),
(	529194954	),
(	529290974	),
(	529330561	),
(	529370343	),
(	529439156	),
(	529452959	),
(	529514441	),
(	529516382	),
(	529572552	),
(	529572746	),
(	529577964	),
(	529616745	),
(	529730106	),
(	529730189	),
(	529770667	),
(	529778695	),
(	529831837	),
(	529871692	),
(	529876015	),
(	529912214	),
(	529918638	),
(	529959240	),
(	530869349	),
(	531763516	),
(	533065138	),
(	533084180	),
(	541949610	),
(	546754990	),
(	551431257	),
(	552510111	),
(	553598368	),
(	560450266	),
(	562918898	),
(	570437136	),
(	570875751	),
(	573715489	),
(	574743063	),
(	585198883	),
(	601032408	),
(	603079184	),
(	603208161	),
(	606166828	),
(	609408889	),
(	613243654	),
(	632077886	),
(	635103871	),
(	641187478	),
(	646037956	),
(	647033485	),
(	647186532	),
(	647289106	),
(	647728155	)

		






SELECT DISTINCT
	DC02.BF_SSN,
	AY01.DATE_OF_ACTION_CODE,
	COUNT(*) AS LOAN_COUNT,
	SUM(ISNULL(LA_CLM_BAL,0.00) - ISNULL(LA_CLM_PRJ_COL_CST,0.00)) AS PRINCIPAL_INTEREST
FROM
	@DATA D
	INNER JOIN ODW..DC02_BAL_INT DC02
		ON CAST(DC02.BF_SSN AS INT) = D.BF_SSN
	LEFT JOIN 
	(
		SELECT 
			DF_PRS_ID,
			MAX(BD_ATY_PRF) AS DATE_OF_ACTION_CODE
		FROM
			ODW..AY01_BR_ATY
		WHERE
			PF_ACT = 'DRWTN'
		GROUP BY
			DF_PRS_ID
	) AY01
		ON DC02.BF_SSN = AY01.DF_PRS_ID
GROUP BY
	DC02.BF_SSN,
	AY01.DATE_OF_ACTION_CODE