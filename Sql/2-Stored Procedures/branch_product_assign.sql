DROP PROCEDURE IF EXISTS branch_product_assign;
GO

CREATE PROCEDURE branch_product_assign
(
    @BranchID INT,
    @ProductID INT
)
AS
BEGIN
    INSERT INTO BranchProduct (BranchID, ProductID)
    VALUES (@BranchID, @ProductID);
END
GO

grant execute on branch_product_assign to public
go