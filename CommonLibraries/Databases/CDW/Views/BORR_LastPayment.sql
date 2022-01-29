CREATE VIEW dbo.BORR_LastPayment
AS
SELECT     TOP (100) PERCENT a.DF_SPE_ACC_ID, a.LST_PMT_RCVD, CONVERT(DECIMAL(9, 2), a.TRAN_AMT) AS TRAN_AMT, COALESCE (C.LST_PMT_IVR, '') AS LST_PMT_IVR, 
                      CONVERT(DECIMAL(9, 2), COALESCE (ABS(SUM(D.TRAN_AMT)), 0)) AS LST_AMT_IVR, CONVERT(varchar, CONVERT(datetime, a.LST_PMT_RCVD), 103) 
                      AS LST_PMT_RCVD_IVR
FROM         (SELECT     A_1.DF_SPE_ACC_ID, A_1.LST_PMT_RCVD, ABS(SUM(B.TRAN_AMT)) AS TRAN_AMT
                       FROM          (SELECT     DF_SPE_ACC_ID, CONVERT(VARCHAR, MAX(CAST(LD_FAT_EFF AS DATETIME)), 101) AS LST_PMT_RCVD
                                               FROM          dbo.LN90_FinancialTran
                                               WHERE      (PC_FAT_TYP = '10') AND (LC_FAT_REV_REA = '')
                                               GROUP BY DF_SPE_ACC_ID) AS A_1 LEFT OUTER JOIN
                                              dbo.LN90_FinancialTran AS B ON A_1.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID AND A_1.LST_PMT_RCVD = B.LD_FAT_EFF AND B.PC_FAT_TYP = '10' AND 
                                              B.LC_FAT_REV_REA = ''
                       GROUP BY A_1.DF_SPE_ACC_ID, A_1.LST_PMT_RCVD) AS a LEFT OUTER JOIN
                          (SELECT     DF_SPE_ACC_ID, CONVERT(VARCHAR(8), MAX(CAST(LD_FAT_EFF AS DATETIME)), 112) AS LST_PMT_IVR
                            FROM          dbo.LN90_FinancialTran AS LN90_FinancialTran_1
                            WHERE      (PC_FAT_TYP = '10') AND (PC_FAT_SUB_TYP = '10') AND (LC_FAT_REV_REA = '')
                            GROUP BY DF_SPE_ACC_ID) AS C ON a.DF_SPE_ACC_ID = C.DF_SPE_ACC_ID LEFT OUTER JOIN
                      dbo.LN90_FinancialTran AS D ON C.DF_SPE_ACC_ID = D.DF_SPE_ACC_ID AND C.LST_PMT_IVR = CONVERT(varchar, CAST(D.LD_FAT_EFF AS datetime), 112) AND 
                      D.PC_FAT_TYP = '10' AND D.PC_FAT_SUB_TYP = '10' AND D.LC_FAT_REV_REA = ''
GROUP BY a.DF_SPE_ACC_ID, a.LST_PMT_RCVD, a.TRAN_AMT, C.LST_PMT_IVR
ORDER BY a.DF_SPE_ACC_ID

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[15] 4[30] 2[36] 3) )"
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
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 99
               Right = 215
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 253
               Bottom = 84
               Right = 430
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "D"
            Begin Extent = 
               Top = 6
               Left = 468
               Bottom = 114
               Right = 652
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'BORR_LastPayment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'BORR_LastPayment';

