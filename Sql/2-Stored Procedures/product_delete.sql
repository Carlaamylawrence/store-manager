drop proc if exists product_delete
go

create proc product_delete
     ( @id  int)
as

   delete
     from product
	  where id = @id

go

grant execute on product_delete to public
go
 