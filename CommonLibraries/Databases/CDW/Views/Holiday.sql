CREATE VIEW dbo.Holiday
AS
SELECT     Holiday, CASE WHEN datepart(weekday, a.Holiday) = 1 THEN dateadd(day, 1, a.Holiday) WHEN datepart(weekday, a.Holiday) = 7 THEN dateadd(day, 
                      - 1, a.Holiday) ELSE a.Holiday END AS BusinessHoliday
FROM         (SELECT     CAST(CAST(DATEPART(year, GETDATE()) AS varchar(4)) + '0101' AS datetime) AS Holiday
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0101') AS datetime)) <= 2 THEN dateadd(day, 
                                             16 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0101') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '0101') AS datetime)) ELSE dateadd(day, 23 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0101') 
                                             AS datetime)), CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0101') AS datetime)) END AS Holiday
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0201') AS datetime)) <= 2 THEN dateadd(day, 
                                             16 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0201') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '0201') AS datetime)) ELSE dateadd(day, 23 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0201') 
                                             AS datetime)), CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0201') AS datetime)) END AS Holiday
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0501') AS datetime)) <= 6 THEN dateadd(day, 
                                             30 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0501') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '0501') AS datetime)) ELSE CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0531') AS datetime) END AS Holiday
                       UNION
                       SELECT     CAST(CAST(DATEPART(year, GETDATE()) AS varchar(4)) + '0704' AS datetime) AS Expr1
                       UNION
                       SELECT     CAST(CAST(DATEPART(year, GETDATE()) AS varchar(4)) + '0724' AS datetime) AS Expr1
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0901') AS datetime)) <= 2 THEN dateadd(day, 
                                             2 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0901') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '0901') AS datetime)) ELSE dateadd(day, 9 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0901') 
                                             AS datetime)), CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '0901') AS datetime)) END AS Holiday
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1001') AS datetime)) <= 2 THEN dateadd(day, 
                                             9 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1001') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '1001') AS datetime)) ELSE dateadd(day, 16 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1001') 
                                             AS datetime)), CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1001') AS datetime)) END AS Holiday
                       UNION
                       SELECT     CAST(CAST(DATEPART(year, GETDATE()) AS varchar(4)) + '1111' AS datetime) AS Expr1
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') AS datetime)) <= 5 THEN dateadd(day, 
                                             26 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '1101') AS datetime)) ELSE dateadd(day, 33 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') 
                                             AS datetime)), CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') AS datetime)) END AS Holiday
                       UNION
                       SELECT     CASE WHEN datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') AS datetime)) <= 5 THEN dateadd(day, 
                                             27 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') AS datetime)), CAST((CAST(datepart(year, getdate()) 
                                             AS varchar(4)) + '1101') AS datetime)) ELSE dateadd(day, 34 - datepart(weekday, CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') 
                                             AS datetime)), CAST((CAST(datepart(year, getdate()) AS varchar(4)) + '1101') AS datetime)) END AS Holiday
                       UNION
                       SELECT     CAST(CAST(DATEPART(year, GETDATE()) AS varchar(4)) + '1225' AS datetime) AS Expr1) AS a

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
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 69
               Right = 189
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Holiday';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Holiday';

