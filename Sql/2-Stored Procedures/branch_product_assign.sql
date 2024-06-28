DROP PROCEDURE IF EXISTS branch_product_assign;
GO

CREATE PROCEDURE branch_product_assign
(
    @BranchName VARCHAR(100),
    @ProductName VARCHAR(100)
)
AS
BEGIN
    DECLARE @BranchID INT, @ProductID INT;

    SELECT @BranchID = b.Id
    FROM Branch b
    WHERE b.Name = @BranchName;
   
    SELECT @ProductID = p.Id
    FROM Product p
    WHERE p.Name = @ProductName;

   
    INSERT INTO BranchProduct (BranchID, ProductID)
    VALUES (@BranchID, @ProductID);
END
GO

GRANT EXECUTE ON branch_product_assign TO public;
GO
