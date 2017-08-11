USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_user_master_delete]    Script Date: 11-08-2017 16:24:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_user_master_delete]
	@user_id int
AS
BEGIN
/*

DECLARE	@return_value int

EXEC	@return_value = [dbo].[proc_user_master_delete]
		@@user_id = 1

SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;
	
	BEGIN TRANSACTION
	BEGIN TRY
		DELETE FROM dbo.user_master WHERE [USER_ID] = @user_id
		
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


