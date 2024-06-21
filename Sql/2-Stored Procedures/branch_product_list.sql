drop procedure if exists branch_product_list
go

create procedure branch_product_list
as
  SELECT 
    b.Name AS BranchName,
    p.Name AS ProductName
FROM 
    BranchProduct bp
JOIN 
    Branch b ON bp.BranchID = b.Id
JOIN 
    Product p ON bp.ProductID = p.ID

go

grant execute on branch_product_list to public
go

exec branch_product_list