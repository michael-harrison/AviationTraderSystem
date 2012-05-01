USE [aviation-trader]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_IsHeadlineAdvert]
	(
		@advert_id int
	)
RETURNS varchar(1024)
AS
	BEGIN
		DECLARE @id int
		DECLARE @is_headline int
		
		SET @is_headline = 1
		
		SELECT
			@id = AdInstance.ID 
		FROM 
			AdInstance 
			INNER JOIN Product 
			ON AdInstance.ProductID = Product.ID 
		WHERE 
			Product.Type = 12 
			AND AdID = @advert_id

		IF @id IS NULL BEGIN
			SET @is_headline = 0
		END

		RETURN @is_headline
	END
