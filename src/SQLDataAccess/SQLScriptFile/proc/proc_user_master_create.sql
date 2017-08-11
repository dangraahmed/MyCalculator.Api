USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_user_master_create]    Script Date: 11-08-2017 16:24:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




create PROCEDURE [dbo].[proc_user_master_create]
	@user_name varchar(50),
	@user_id varchar(50),
	@password nvarchar(256)
AS
BEGIN

/*
DECLARE	@return_value int

EXEC	@return_value = [dbo].[proc_user_master_create]
		@from_year = '2001',
		@to_year = '2002',
		@category = N'A',
		@tax_slab_details = N'<rows><row><DetailsId>4</DetailsId><from_amount></from_amount><to_amount>10</to_amount><percentage>300</percentage></row><row><DetailsId>5</DetailsId><from_amount>10</from_amount><to_amount>20</to_amount><percentage>300</percentage></row><row><DetailsId>6</DetailsId><from_amount>20</from_amount><to_amount></to_amount><percentage>300</percentage></row></rows>'

SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;

	BEGIN TRANSACTION
	BEGIN TRY
		
		INSERT INTO dbo.user_master
			   ([user_name], [user_id], [password])
		 VALUES
			   (@user_name, @user_id, @password)
		
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


