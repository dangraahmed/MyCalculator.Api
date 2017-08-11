USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_tax_slab_details_read]    Script Date: 11-08-2017 16:23:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_tax_slab_details_read]
@tax_slab_id int
AS
BEGIN

/*
DECLARE	@return_value int
EXEC	@return_value = [dbo].[proc_tax_slab_details_read] 1
SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;
	SELECT * 
	FROM dbo.tax_slab_details
	WHERE tax_slab_id = @tax_slab_id
END




GO


