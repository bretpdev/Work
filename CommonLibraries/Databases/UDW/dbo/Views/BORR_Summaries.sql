CREATE VIEW dbo.BORR_Summaries
AS
SELECT DISTINCT 
                      TOP (100) PERCENT A.DF_SPE_ACC_ID, SUM(A.LA_CUR_PRI) AS LA_CUR_PRI, SUM(B.WA_TOT_BRI_OTS) AS WA_TOT_BRI_OTS, 
                      CAST(SUM(CAST(CAST(((CASE WHEN B.WC_DW_LON_STA IN ('01', '02', '04') AND A.IC_LON_PGM IN ('SUBSPC', 'DSCON', 'DSS', 'SUBCNS', 'STFFRD') 
                      THEN 0 ELSE A.LA_CUR_PRI END)) * COALESCE (C.LR_ITR, 0) / 365 AS INTEGER) AS NUMERIC(8, 2)) / 100) AS NUMERIC(8, 2)) AS LR_ITR_DLY, 
                      CAST(SUM(CAST(CAST(((CASE WHEN B.WC_DW_LON_STA IN ('01', '02', '04') AND A.IC_LON_PGM IN ('SUBSPC', 'DSCON', 'DSS', 'SUBCNS', 'STFFRD') 
                      THEN 0 ELSE A.LA_CUR_PRI END)) * COALESCE (C.LR_ITR, 0) / 365 AS INTEGER) AS NUMERIC(8, 2)) / 100) AS NUMERIC(8, 2)) * 31 AS LR_ITR_MONTH,
                       CAST(CASE WHEN SUM(a.la_cur_pri) > 0 THEN SUM(CAST(CAST(((CASE WHEN B.WC_DW_LON_STA IN ('01', '02', '04') AND 
                      A.IC_LON_PGM IN ('SUBSPC', 'DSCON', 'DSS', 'SUBCNS', 'STFFRD') THEN 0 ELSE A.LA_CUR_PRI END)) * COALESCE (C.LR_ITR, 0) / 365 AS INTEGER) 
                      AS NUMERIC(8, 2)) / 100) * 31 + 5 ELSE 0 END AS numeric(8, 2)) AS LR_ITR_MONTH_5, CONVERT(varchar(4), YEAR(DATEADD(MONTH, 3, 
                      MAX(CAST(B.WD_LON_RPD_SR AS DATETIME))))) AS COHORT, CASE WHEN YEAR(DATEADD(MONTH, 3, 
                      MAX(CAST(WD_LON_RPD_SR AS DATETIME)))) >= year(DATEADD(YEAR, - 2, (DATEADD(MONTH, 3, GETDATE())))) 
                      THEN 'Y' ELSE 'N' END AS COHORT_IND
FROM         dbo.LN10_Loan AS A INNER JOIN
                      dbo.DW01_Loan AS B ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID AND A.LN_SEQ = B.LN_SEQ LEFT OUTER JOIN
                      dbo.LN72_InterestRate AS C ON A.DF_SPE_ACC_ID = C.DF_SPE_ACC_ID AND A.LN_SEQ = C.LN_SEQ
GROUP BY A.DF_SPE_ACC_ID
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'BORR_Summaries';


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
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 225
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "B"
            Begin Extent = 
               Top = 6
               Left = 263
               Bottom = 114
               Right = 451
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 489
               Bottom = 114
               Right = 702
            End
            DisplayFlags = 280
            TopColumn = 0
         End
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
      Begin ColumnWidths = 12
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'BORR_Summaries';

