USE [aviation-trader]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[fn_GetAdvertFeaturedImage]
	(
		@advert_id int
	)
RETURNS varchar(1024)
AS
	BEGIN
		DECLARE @image varchar(8000) 

		SELECT
			@image = UPPER(SUBSTRING(master.dbo.fn_varbintohexstr(dbo.Image.ID), 3, 8)) + '-' + CONVERT(varchar(2), dbo.Image.PreviewSequence) + '.jpg'
		FROM
			[Image]
		WHERE 
			AdID = @advert_id
			AND IsMainImage = 1

		IF @image IS NULL BEGIN
			SET @image = ''
		END

		RETURN @image
	END
