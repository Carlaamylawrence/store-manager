drop procedure if exists branch_product_list
go

create procedure branch_product_list
as
  SELECT 
		bp.id,
    bra.Name AS BranchName,
    pro.Name AS ProductName
FROM 
    BranchProduct bp
JOIN 
    Branch bra ON bp.BranchID = bra.Id
JOIN 
    Product pro ON bp.ProductID = pro.ID

go

grant execute on branch_product_list to public
go

--exec branch_product_list