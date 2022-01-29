CREATE PROCEDURE [textcoord].[InsertApp1]
AS

DELETE FROM textcoord.UheaaApp1

INSERT INTO	textcoord.UheaaApp1(lm_filler2, areacode, phone)
SELECT
	REPLACE(lm_filler2, ' ', ''),
	areacode,
	phone
FROM
	OPENQUERY(UHEAAAPP1, 
		'
			SELECT  
				CH.lm_filler2, 
				CH.areacode, 
				CH.phone
			FROM 
				call_history CH 
			WHERE 
				CH.call_type != 5 
				AND CH.lm_filler2 != ''''
				AND CH.status IS NOT NULL
				AND CH.act_date::date = NOW()::date
				AND CH.areacode IS NOT NULL
				AND CH.phone IS NOT NULL

			UNION 

			SELECT 
				IB.filler2 AS lm_filler2, 
				IB.ani_acode AS areacode, 
				IB.ani_phone AS phone
			FROM 
				inboundlog IB 
			WHERE
				IB.call_date::date = NOW()::date
				AND IB.status IS NOT NULL
				AND IB.ani_acode IS NOT NULL
				AND IB.ani_phone IS NOT NULL
		'
	)