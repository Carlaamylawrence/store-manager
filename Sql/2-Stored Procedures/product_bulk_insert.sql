CREATE PROCEDURE dbo.product_bulk_insert
    @products dbo.ProductType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Product (id, name, weightedItem, suggestedSellingPrice)
    SELECT id, name, weightedItem, suggestedSellingPrice
    FROM @products;

    SELECT @@ROWCOUNT AS 'RowsInserted';
END;