USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_user_master_update]    Script Date: 11-08-2017 16:24:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_user_master_update]
	@id int,
	@user_name varchar(50),
	@user_id varchar(50),
	@password nvarchar(256)
AS
BEGIN

/*
DECLARE	@return_value INT

EXEC	@return_value = [dbo].[proc_user_master_update]
		@id = 1,
		@user_name varchar(50),
		@user_id varchar(50),
		@password nvarchar(256)

SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;

	BEGIN TRANSACTION
	BEGIN TRY
		
		UPDATE dbo.user_master SET
			[user_name] = @user_name,
			[user_id] = @user_id,
			[password] = @password
		WHERE id = @id
	           
		
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


