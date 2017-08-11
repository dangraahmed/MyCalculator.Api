USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_tax_slab_delete]    Script Date: 11-08-2017 16:23:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_tax_slab_delete]
	@tax_slab_id int
AS
BEGIN
/*

DECLARE	@return_value int

EXEC	@return_value = [dbo].[proc_tax_slab_delete]
		@tax_slab_id = 27

SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;
	
	BEGIN TRANSACTION
	BEGIN TRY
		delete from dbo.tax_slab_details where tax_slab_id = @tax_slab_id
		
		delete from dbo.tax_slab where id = @tax_slab_id
		COMMIT
	
	END TRY  
	BEGIN CATCH
		ROLLBACK 	
		
		DECLARE @ErrorNumber INT = ERROR_NUMBER();
		DECLARE @ErrorLine INT = ERROR_LINE();
		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
		DECLARE @ErrorState INT = ERROR_STATE();
	 
		PRINT 'Actual error number: ' + CAST(@ErrorNumber AS VARCHAR(10));
		PRINT 'Actual line number: ' + CAST(@ErrorLine AS VARCHAR(10));
	 
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);	
	END CATCH
END




GO


