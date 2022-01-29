select *  
	from openquery(duster, '
		select *
		from 
			olwhrm1.DC01_LON_CLM_INF DC01
			INNER JOIN olwhrm1.PD01_PDM_INF PD01
				ON BF_SSN = PD01.DF_PRS_ID
			WHERE
				PD01.DF_SPE_ACC_ID = ''4972573432''
	') --UHEAA Live

