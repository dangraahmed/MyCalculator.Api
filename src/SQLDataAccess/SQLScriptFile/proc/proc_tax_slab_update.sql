USE [MyCalculator]
GO

/****** Object:  StoredProcedure [dbo].[proc_tax_slab_update]    Script Date: 11-08-2017 16:24:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_tax_slab_update]
	@id int,
	@from_year int,
	@to_year int,
	@category varchar(50),
	@tax_slab_details varchar(max)
AS
BEGIN

/*
DECLARE	@return_value int

EXEC	@return_value = [dbo].[proc_tax_slab_update]
		@id = 1,
		@from_year = '2002',
		@to_year = '2003',
		@category = N'A',
		@tax_slab_details = N'<rows><row><DetailsId>-1</DetailsId><from_amount></from_amount><to_amount>10</to_amount><percentage>103</percentage></row><row><DetailsId>-1</DetailsId><from_amount>10</from_amount><to_amount></to_amount><percentage>102</percentage></row></rows>'
		--@tax_slab_details = N'<rows><row><DetailsId>1</DetailsId><from_amount></from_amount><to_amount>10</to_amount><percentage>102</percentage></row><row><DetailsId>2</DetailsId><from_amount>10</from_amount><to_amount>20</to_amount><percentage>102</percentage></row><row><DetailsId>3</DetailsId><from_amount>20</from_amount><to_amount></to_amount><percentage>102</percentage></row></rows>'

SELECT	'Return Value' = @return_value

*/

	SET NOCOUNT ON;

	BEGIN TRANSACTION
	BEGIN TRY
		
		update dbo.tax_slab set
			[from_year] = @from_year,
			[to_year] = @to_year,
			[category] = @category
		where id = @id
	           
		declare @hxml int

		EXEC sp_xml_preparedocument @hxml OUTPUT, @tax_slab_details

		delete from dbo.tax_slab_details
		where tax_slab_id = @id
			and id not in (SELECT DetailsId as id
							FROM OPENXML(@hXML,'/rows/TaxSlabDetail',3)
							WITH (DetailsId INT 'Id'))

		INSERT INTO dbo.tax_slab_details
			(tax_slab_id, from_amount, to_amount, percentage)
		SELECT @id tax_slab_id,
			CONVERT(INT,NULLIF(from_amount,'')) AS from_amount,
			CONVERT(INT,NULLIF(to_amount,'')) AS to_amount,
			percentage as percentage
		FROM OPENXML(@hXML,'/rows/TaxSlabDetail',3)
		WITH (DetailsId INT 'Id',
			from_amount INT 'SlabFromAmount', 
			to_amount INT 'SlabToAmount',
			percentage INT 'Percentage')
		WHERE DetailsId = -1

		update dbo.tax_slab_details set
			from_amount = b.from_amount, 
			to_amount = b.to_amount, 
			percentage = b.percentage
		from dbo.tax_slab_details a
			inner join (SELECT DetailsId as id,
							CONVERT(INT,NULLIF(from_amount,'')) AS from_amount,
							CONVERT(INT,NULLIF(to_amount,'')) AS to_amount,
							percentage as percentage
						FROM OPENXML(@hXML,'/rows/TaxSlabDetail',3)
						WITH (DetailsId INT 'Id',
							from_amount INT 'SlabFromAmount', 
							to_amount INT 'SlabToAmount',
							percentage INT 'Percentage'))b
				on a.id = b.id
		
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


