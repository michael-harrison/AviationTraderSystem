USE [aviation-trader]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetAdvertType]
	(
		@product_type int
	)
RETURNS varchar(1024)
AS
	BEGIN
		DECLARE @type varchar(100)
	
		SET @type =
			CASE 
				WHEN @product_type = 4 OR @product_type = 9 THEN
					'Enhanced Classified'
				WHEN @product_type = 11 THEN
					'Display Advert'
				ELSE
					'Classified'
			END
					
		RETURN @type
	END
