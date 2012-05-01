USE [aviation-trader]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[fn_GetAdvertPDF]
	(
		@advert_id int
	)
RETURNS varchar(1024)
AS
	BEGIN
		DECLARE @file varchar(8000) 

		SELECT
			@file = UPPER(SUBSTRING(master.dbo.fn_varbintohexstr(dbo.AdInstance.ID), 3, 8)) + '-' + CONVERT(varchar(2), dbo.AdInstance.PreviewSequence) + '.pdf'
		FROM
			AdInstance
			INNER JOIN Product
			ON AdInstance.ProductID = Product.ID
		WHERE 
			AdID = @advert_id
			AND Product.Type = 7 -- DisplayFinishedArt

		IF @file IS NULL BEGIN
			SET @file = ''
		END

		RETURN @file
	END
