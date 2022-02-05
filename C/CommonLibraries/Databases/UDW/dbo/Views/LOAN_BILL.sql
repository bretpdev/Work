



CREATE VIEW [dbo].[LOAN_BILL]

AS
SELECT DISTINCT 
	PD10.DF_SPE_ACC_ID, 
	MAX_BILL.LN_SEQ, 
	ISNULL(FTBL.Label, '') AS BIL_MTD,
	CASE 
		WHEN BL10.LC_IND_BIL_SNT in ('N','5') AND LN80.LD_BIL_DU_LON > GETDATE() THEN 'Y'
		ELSE 'N'
	END AS PAID_AHEAD
FROM
	dbo.BL10_BR_BIL BL10
	INNER JOIN dbo.LN80_LON_BIL_CRF LN80	
		ON LN80.BF_SSN = BL10.BF_SSN
		AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
		AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
	INNER JOIN dbo.PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = BL10.BF_SSN
	INNER JOIN
    (--GETS THE BORROWERS MOST RECENT BILL
		SELECT
			LN80.BF_SSN, 
			LN80.LN_SEQ, 
			MAX(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON
		FROM
			dbo.BL10_BR_BIL BL10
			INNER JOIN dbo.LN80_LON_BIL_CRF LN80	
				ON LN80.BF_SSN = BL10.BF_SSN
				AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
				AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
		WHERE
			BL10.LC_STA_BIL10 = 'A'
			AND LN80.LC_STA_LON80 = 'A'
        GROUP BY 
			LN80.BF_SSN,
			LN80.LN_SEQ
	) AS MAX_BILL
		ON MAX_BILL.BF_SSN = LN80.BF_SSN 
		AND MAX_BILL.LN_SEQ = LN80.LN_SEQ
		AND MAX_BILL.LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
	LEFT JOIN dbo.FormatTranslation FTBL
		ON FTBL.FmtName = '$BILMTD'
		AND FTBL.Start = BL10.LC_BIL_MTD
	WHERE
		BL10.LC_STA_BIL10 = 'A'
		AND LN80.LC_STA_LON80 = 'A'
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'LOAN_BILL';


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
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "B"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 99
               Right = 423
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'LOAN_BILL';

