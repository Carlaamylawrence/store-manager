CREATE TYPE dbo.ProductType AS TABLE
(
    id INT,
    name NVARCHAR(100),
    weightedItem BIT,
    suggestedSellingPrice DECIMAL(18, 2)
);