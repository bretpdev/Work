select DN_DOM_PHN_ARA + DN_DOM_PHN_XCH + DN_DOM_PHN_LCL from UDW..PD42_PRS_PHN where DF_PRS_ID  in ('029520925','621300352','611361955','506822768')
--('3258808586','4279202618','5442420207','4426191217')

select * from UDW..PD42_PRS_PHN where DN_DOM_PHN_ARA + DN_DOM_PHN_XCH + DN_DOM_PHN_LCL in (select DN_DOM_PHN_ARA + DN_DOM_PHN_XCH + DN_DOM_PHN_LCL from UDW..PD42_PRS_PHN where DF_PRS_ID  in ('029520925','621300352','611361955','506822768')) order by DN_DOM_PHN_ARA, DN_DOM_PHN_XCH, DN_DOM_PHN_LCL

SELECT DISTINCT 
			PD42.DF_PRS_ID AS ID,
			PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL AS PHONE
		FROM
			 udw..PD42_PRS_PHN PD42
		INNER JOIN udw..LN10_LON LN10
			ON PD42.DF_PRS_ID = LN10.BF_SSN
		LEFT JOIN udw..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
				(
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						udw..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'DIDDA'
					GROUP BY
						BF_SSN
				)DID 
					ON DID.BF_SSN = LN10.BF_SSN
				LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
				(
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						udw..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'PAUTO'
					GROUP BY
						BF_SSN
				)AUTO 
					ON AUTO.BF_SSN = LN10.BF_SSN
				LEFT JOIN odw..CT30_CALL_QUE CT30
					ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
					AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
					AND CT30.IF_WRK_GRP IN ('A', 'W')
		WHERE PD42.DI_PHN_VLD = 'Y'
			AND LN10.LA_CUR_PRI > 0
			AND PD42.DC_ALW_ADL_PHN IN ('N','Q', 'U', ' ') /*Phone types with no consent*/
			AND LN10.LC_STA_LON10 = 'R'
			AND PD42.DN_DOM_PHN_ARA != '' /*NOTE in the table a blank values is stored as '' and not a null*/
			AND WQ20.BF_SSN IS NULL 
			AND CT30.DF_PRS_ID_BR IS NULL
			AND (DID.BF_SSN IS NULL OR (DID.BF_SSN IS NOT NULL AND AUTO.BF_SSN IS NULL) OR (DID.LD_ATY_REQ_RCV > AUTO.LD_ATY_REQ_RCV))
			and PD42.DF_PRS_ID in ('029520925','621300352','611361955','506822768')