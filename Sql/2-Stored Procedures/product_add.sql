DROP PROCEDURE IF EXISTS product_add;
GO

CREATE PROCEDURE product_add
    @id INT,
    @name VARCHAR(100),
    @weightedItem BIT,
    @suggestedSellingPrice DECIMAL(14,2)
AS
BEGIN
    INSERT INTO product (id, name, weightedItem, suggestedSellingPrice)
    VALUES (@id, @name, @weightedItem, @suggestedSellingPrice);
END
GO

GRANT EXECUTE ON product_add TO public;
GO