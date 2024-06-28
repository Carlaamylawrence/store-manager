drop proc if exists branch_product_delete
go

create proc branch_product_delete
     ( @id  int)
as

   delete
     from branchProduct
	  where id = @id

go

grant execute on branch_product_delete to public
go
 