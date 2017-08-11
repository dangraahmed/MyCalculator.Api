USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_tax_slab_read]    Script Date: 11-08-2017 16:24:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_tax_slab_read]
AS
BEGIN

/*
DECLARE	@return_value int
EXEC	@return_value = [dbo].[proc_tax_slab_read]
SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;
	SELECT * 
	FROM dbo.tax_slab
END




GO


