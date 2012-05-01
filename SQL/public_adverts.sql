USE [aviation-trader]
GO
DECLARE @PublicationID AS Int
DECLARE @ClassificationID AS Int
DECLARE @ProdnStatus AS Int
DECLARE @EditionVisibility AS Int


/*
Image.Type
	Unknown = 0
	JPG = 1
	TIF = 2
	EPS = 3
	BMP = 4
	PNG = 5
	PDF = 6
	GIF = 7
	SWF = 8
*/



SET @PublicationID = 2
/*
ClassficationID from Classification.ID
*/
SET @ClassificationID = 1
/*
Production.Status
Unspecified = 0
Initial = 1
Cancelled = 2
Saved = 3
Submitted = 4
Proofed = 5
Approved = 6
Archived = 7
*/
SET @ProdnStatus = 6

/*
Edition.Visibility
Unspecified = 0
Past = 1
Active = 2
Future = 3
*/
SET @EditionVisibility = 2

/*
Product Types (Ad.ProductID)
Use			ID	Type	Name
Print		1	3		Photo Classie - Mono text with a full colour image
Print		2	1		Standard Classie - Mono text only
Web			5	9		Premium Web Ad
Web			6	8		Standard Web Ad
Obsolete	7	1		EMagazine
Obsolete	8	1		Subscriber push email
???			9	4		Standard Display - Text only (No Image)
???			10	4		Set Display - Finished art in PDF
Web			15	11		PDF
Web			16	12		Headline Ad
Web			18	4		Photo Display - Text and up to 3 images
Web			26	4		Featured Aircraft Display - Text with a single image
*/



SELECT 
	AdInstance.*,
	Ad.*,
	Product.Type as ProductType, 
	Ad.Adnumber as Adnumber 
FROM 
	dbo.AdInstance 
	INNER JOIN dbo.Product 
	on Product.ID=AdInstance.ProductID 
	INNER JOIN dbo.Ad 
	on Ad.ID=AdInstance.AdID 
	INNER JOIN dbo.Edition 
	on Edition.ID=AdInstance.EditionID 
WHERE 
	AdInstance.EditionID=Edition.ID 
	AND Edition.PublicationID = 2 
	AND Ad.ProdnStatus = 6 
	AND Product.Type <> 12 
	AND Edition.Visibility IN (2,3)
ORDER BY 
	Ad.LatestListing DESC,
	Ad.SortKey


-- RIGHT(UPPER(master.dbo.fn_varbintohexstr(Image.ID)), 8) + CONVERT (varchar(2), dbo.Image.PreviewSequence)

/*
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[at_adverts]
AS
SELECT     TOP (100) PERCENT dbo.AdInstance.ID, dbo.AdInstance.CreateTime, dbo.AdInstance.ModifyTime, dbo.AdInstance.WordPressCategoryID, dbo.Ad.ClassificationID, 
                      dbo.Ad.SortKey, dbo.Ad.KeyWords, dbo.Ad.Text, dbo.Ad.ItemPrice, dbo.Ad.LatestListing, dbo.Product.Type AS ProductType, dbo.Ad.AdNumber, 
                      UPPER(SUBSTRING(master.dbo.fn_varbintohexstr(dbo.Image.ID), 3, 8)) + '-' + CONVERT(varchar(2), dbo.Image.PreviewSequence) AS Expr1, dbo.Image.Type
FROM         dbo.AdInstance INNER JOIN
                      dbo.Product ON dbo.Product.ID = dbo.AdInstance.ProductID INNER JOIN
                      dbo.Ad ON dbo.Ad.ID = dbo.AdInstance.AdID INNER JOIN
                      dbo.Edition ON dbo.Edition.ID = dbo.AdInstance.EditionID AND dbo.AdInstance.EditionID = dbo.Edition.ID INNER JOIN
                      dbo.Image ON dbo.Ad.ID = dbo.Image.AdID
WHERE     (dbo.Edition.PublicationID = 2) AND (dbo.Ad.ProdnStatus = 6) AND (dbo.Edition.Visibility IN (2, 3))
ORDER BY dbo.Ad.LatestListing DESC, dbo.AdInstance.ModifyTime DESC

GO

*/