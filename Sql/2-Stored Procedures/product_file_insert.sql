CREATE PROCEDURE [dbo].product_file_insert
    @Products ProductType READONLY
AS
BEGIN
    INSERT INTO Product (Id, Name, WeightedItem, SuggestedSellingPrice)
    SELECT 
        Id, 
        Name, 
        WeightedItem, 
        SuggestedSellingPrice
    FROM @Products;
END