USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_tax_slab_create]    Script Date: 11-08-2017 16:23:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_tax_slab_create]
	@from_year int,
	@to_year int,
	@category varchar(50),
	@tax_slab_details varchar(max)
AS
BEGIN

/*
DECLARE	@return_value int

EXEC	@return_value = [dbo].[proc_tax_slab_create]
		@from_year = '2001',
		@to_year = '2002',
		@category = N'A',
		@tax_slab_details = N'<rows><row><DetailsId>4</DetailsId><from_amount></from_amount><to_amount>10</to_amount><percentage>300</percentage></row><row><DetailsId>5</DetailsId><from_amount>10</from_amount><to_amount>20</to_amount><percentage>300</percentage></row><row><DetailsId>6</DetailsId><from_amount>20</from_amount><to_amount></to_amount><percentage>300</percentage></row></rows>'

SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;
	declare @TaxSlabId int

	BEGIN TRANSACTION
	BEGIN TRY
		
		INSERT INTO dbo.tax_slab
			   ([from_year], [to_year], [category])
		 VALUES
			   (@from_year, @to_year, @category)
	           
		SET @TaxSlabId =  IDENT_CURRENT('dbo.tax_slab')

		DECLARE @hxml int

		EXEC sp_xml_preparedocument @hxml OUTPUT, @tax_slab_details

		INSERT INTO dbo.tax_slab_details
			(tax_slab_id, from_amount, to_amount, percentage)
		SELECT @TaxSlabId tax_slab_id,
			CONVERT(INT,NULLIF(from_amount,'')) AS from_amount,
			CONVERT(INT,NULLIF(to_amount,'')) AS to_amount,
			percentage as percentage
		FROM OPENXML(@hXML,'/rows/TaxSlabDetail',3)
		WITH (from_amount INT 'SlabFromAmount', 
			to_amount INT 'SlabToAmount',
			percentage INT 'Percentage')
		
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


