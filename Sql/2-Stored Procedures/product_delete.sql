drop proc if exists product_delete
go

create proc product_delete
     ( @id  int)
as
Begin
    delete 
      from branchProduct
     where ProductId = @id;
   delete
     from product
	  where id = @id

End
go

grant execute on product_delete to public
go

-- exec product_delete 1
 