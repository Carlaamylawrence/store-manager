CREATE PROCEDURE product_bulk_insert
    @Id INT,
    @Name NVARCHAR(100),
    @WeightedItem BIT,
    @SuggestedSellingPrice DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO Product (Id, Name, WeightedItem, SuggestedSellingPrice)
    VALUES (@Id, @Name, @WeightedItem, @SuggestedSellingPrice);
END
