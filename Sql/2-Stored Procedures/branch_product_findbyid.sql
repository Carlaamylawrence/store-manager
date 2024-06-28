drop procedure if exists branch_product_findbyid
go

create procedure branch_product_findbyid
     ( @id       int)
as

   select id,
          branchId,
					productId
    from  branchProduct
   where  id = @id
go 


grant execute on branch_product_findbyid to public
go

--exec branch_product_findbyid '1'