CREATE VIEW dbo.[BORR_REPAYMENT_V]
AS
SELECT distinct coalesce(A.DF_SPE_ACC_ID,c.df_spe_acc_id) as DF_SPE_ACC_iD
	,COALESCE(A.LD_CRT_LON65,' ') AS LD_CRT_LON65
	,DUE_DAY = COALESCE(C.DUE_DAY,substring((SELECT     (', ' + D.DUE_DAY)
				FROM (SELECT DISTINCT A.DF_SPE_ACC_ID, A.DUE_DAY
					FROM dbo.LN65_REPAYMENTSCHED AS A
					GROUP BY A.DF_SPE_ACC_ID, A.DUE_DAY) D
				WHERE     A.df_spe_acc_id = D.df_spe_acc_id
				ORDER BY CAST(D.DUE_DAY AS INT) FOR XML PATH('')), 3, 50))
	,MONTH_AMT = COALESCE(C.MONTH_AMT,substring((SELECT     (', $' + CONVERT(VARCHAR, LA_RPS_ISL,1))
				FROM (SELECT A.DF_SPE_ACC_ID
						, A.DUE_DAY
						,SUM(LA_RPS_ISL) AS LA_RPS_ISL
					FROM dbo.LN65_REPAYMENTSCHED AS A
					GROUP BY A.DF_SPE_ACC_ID, A.DUE_DAY) D
				WHERE     A.df_spe_acc_id = D.df_spe_acc_id
				ORDER BY CAST(D.DUE_DAY AS INT) FOR XML PATH('')), 3, 100))
	,COALESCE(C.MULT_DUE_DT,A.MULT_DUE_DT) AS MULT_DUE_DT
FROM (SELECT DF_SPE_ACC_ID
		,CASE WHEN COUNT(DISTINCT DUE_DAY) > 1 THEN 'Y' ELSE 'N' END AS MULT_DUE_DT
		,CONVERT(VARCHAR, MAX(CAST(LD_CRT_LON65 AS DATETIME)), 101) AS LD_CRT_LON65

	FROM dbo.LN65_REPAYMENTSCHED
	GROUP BY DF_SPE_ACC_ID
) A
full OUTER JOIN (SELECT DISTINCT DF_SPE_ACC_ID
		,'RPF=$' + CONVERT(VARCHAR, LA_REQ_RDC_PAY,1) AS MONTH_AMT
		,CAST(DAY(DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0))) AS VARCHAR) as DUE_DAY
		,'N' AS MULT_DUE_DT
	FROM dbo.FB10_FORBEARANCE
	WHERE CAST(LD_FOR_BEG AS DATETIME) <= GETDATE()
		AND CAST(LD_FOR_END AS DATETIME) >= GETDATE()
		AND LA_REQ_RDC_PAY > 0
) C
	ON A.DF_SPE_ACC_ID = C.DF_SPE_ACC_ID
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'BORR_REPAYMENT_V';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'BORR_REPAYMENT_V';

