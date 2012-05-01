USE [aviation-trader]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[fn_GetAdvertImages]
	(
		@advert_id int,
		@product_type int
	)
RETURNS varchar(1024)
AS
	BEGIN
		DECLARE @images varchar(8000) 
		SET @images = ''

		SELECT
			@images = @images + UPPER(SUBSTRING(master.dbo.fn_varbintohexstr(dbo.Image.ID), 3, 8)) + '-' + CONVERT(varchar(2), dbo.Image.PreviewSequence) + '.jpg, '
		FROM
			[Image]
		WHERE 
			AdID = @advert_id
			AND (Status & 256 = 256)
			AND IsMainImage <> 1
			AND	Type IN (1,6)
			AND @product_type <> 11 -- DisplayAd

		IF (@images <> '') BEGIN
			SET @images = LEFT(@images, LEN(@images)-1)
		END

		RETURN @images
	END
