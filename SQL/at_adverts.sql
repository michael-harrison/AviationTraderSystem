USE [aviation-trader]
GO

/****** Object:  View [dbo].[at_adverts]    Script Date: 05/02/2012 00:17:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[at_adverts]
AS
SELECT     TOP (100) PERCENT dbo.AdInstance.ID, dbo.AdInstance.CreateTime, dbo.AdInstance.ModifyTime, dbo.AdInstance.WordPressCategoryID, dbo.Ad.ClassificationID, 
                      dbo.Ad.KeyWords AS titile, dbo.Ad.Text AS [content], dbo.Ad.ItemPrice AS price, dbo.fn_GetAdvertImages(dbo.AdInstance.AdID, dbo.Product.Type) 
                      AS additional_images, dbo.fn_GetAdvertFeaturedImage(dbo.AdInstance.AdID) AS featured_image, dbo.fn_IsHeadlineAdvert(dbo.AdInstance.AdID) AS headline_advert, 
                      dbo.fn_GetAdvertType(dbo.Product.Type) AS advert_type, dbo.fn_GetAdvertPDF(dbo.Ad.ID) AS display_advert_pdf, dbo.Ad.LatestListing AS latest, 
                      dbo.Edition.Visibility
FROM         dbo.AdInstance INNER JOIN
                      dbo.Ad ON dbo.Ad.ID = dbo.AdInstance.AdID INNER JOIN
                      dbo.Product ON dbo.Product.ID = dbo.AdInstance.ProductID INNER JOIN
                      dbo.Edition ON dbo.Edition.ID = dbo.AdInstance.EditionID AND dbo.AdInstance.EditionID = dbo.Edition.ID
WHERE     (dbo.Edition.PublicationID = 2) AND (dbo.Ad.ProdnStatus = 6) AND (dbo.Edition.Visibility IN (2, 3)) AND (dbo.Product.Type <> 12)
ORDER BY latest DESC, dbo.AdInstance.ModifyTime DESC

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[26] 2[9] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
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
      ActivePaneConfig = 1
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "AdInstance"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 346
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Product"
            Begin Extent = 
               Top = 133
               Left = 609
               Bottom = 252
               Right = 777
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "Ad"
            Begin Extent = 
               Top = 9
               Left = 876
               Bottom = 324
               Right = 1049
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Edition"
            Begin Extent = 
               Top = 186
               Left = 407
               Bottom = 305
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 41
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 4095
         Width = 1500
         Width = 3630
         Width = 1500
         Width = 3990
         Width = 3975
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'at_adverts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
         Column = 10005
         Alias = 2745
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'at_adverts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'at_adverts'
GO

