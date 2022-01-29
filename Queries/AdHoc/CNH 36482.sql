DECLARE @cols AS NVARCHAR(MAX),
@colsnonnull AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX),
	@month varchar(XX) = XX,
	@Year varchar(XX) = XXXX

select @cols = STUFF(
						(SELECT distinct 
							',' + 'isnull(' + QUOTENAME(c.created_by) + ',X) ' +  QUOTENAME(c.created_by) 
						FROM 
							(
								SELECT DISTINCT
									  his.created_by
								FROM 
									[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] sub
									inner join [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] his
										on his.repayment_plan_type_status_mapping_id = sub.repayment_plan_type_substatus_id
								WHERE 
									repayment_plan_type_status_id = X
									AND MONTH(HIS.CREATED_AT) = @month
									AND YEAR(HIS.CREATED_AT) = @Year
								GROUP BY
									 [repayment_plan_type_substatus],
									  MONTH(his.created_at),
									  year(his.created_at),
									  his.created_by
							) c
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,X,X,'')

select @colsnonnull = STUFF(
						(SELECT distinct 
							',' +  QUOTENAME(c.created_by) 
						FROM 
							(
								SELECT DISTINCT
									  his.created_by
								FROM 
									[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] sub
									inner join [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] his
										on his.repayment_plan_type_status_mapping_id = sub.repayment_plan_type_substatus_id
								WHERE 
									repayment_plan_type_status_id = X
									AND MONTH(HIS.CREATED_AT) = @month
									AND YEAR(HIS.CREATED_AT) = @Year
								GROUP BY
									 [repayment_plan_type_substatus],
									  MONTH(his.created_at),
									  year(his.created_at),
									  his.created_by
							) c
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,X,X,'')

set @query = 'SELECT 
				[repayment_plan_type_substatus] as [Denial Reason Selected], 
				' + @cols + ' 
			   FROM 
			   (
                SELECT DISTINCT
				  [repayment_plan_type_substatus],
				  his.created_by,
				  count(*) AS OCC_COUNT
				FROM 
					[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] sub
					inner join [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] his
						on his.repayment_plan_type_status_mapping_id = sub.repayment_plan_type_substatus_id
				WHERE 
					repayment_plan_type_status_id = X
					AND MONTH(HIS.CREATED_AT) = '+@month+'
					AND YEAR(HIS.CREATED_AT) = '+@Year+'
				GROUP BY
					 [repayment_plan_type_substatus],
					  MONTH(his.created_at),
					  year(his.created_at),
					  his.created_by
           ) x
            PIVOT
			(
				SUM(OCC_COUNT)
				FOR created_by IN (' + @colsnonnull + ')
			) p '

execute(@query)

go 

USE [Income_Driven_Repayment]
GO
/****** Object:  StoredProcedure [dbo].[spGetArcAndComment]    Script Date: XX/XX/XXXX X:XX:XX PM ******/


	SELECT DISTINCT
		substa.repayment_plan_type_substatus,
		Rpt.repayment_plan,
		ALD.arc As Arc,
		ALD.system_comment As Comment
	FROM 
		Repayment_Plan_Selected RPS
	INNER JOIN Repayment_Type RT
		ON RT.repayment_type_id = RPS.repayment_type_id
	inner join [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type] rpt
		on rpt.repayment_plan_type_id = rt.repayment_plan_type_id
	INNER JOIN Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	INNER JOIN Repayment_Plan_Type_Substatus SUBSTA
		ON SUBSTA.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN Repayment_Type_Status RTS
		ON RTS.repayment_type_status_id = SUBSTA.repayment_type_status_id
	LEFT JOIN Arc_Letter_Mapping ALM
		ON ALM.repayment_type_id = RT.repayment_type_id
		AND ALM.repayment_type_status_id = RTS.repayment_type_status_id
	LEFT JOIN Arc_Letter_Data ALD
		ON ALD.mapping_id = ALM.mapping_id
WHERE
	ARC IS NOT NULL
	AND SUBSTA.repayment_plan_type_status_id = X
ORDER BY
	substa.repayment_plan_type_substatus
