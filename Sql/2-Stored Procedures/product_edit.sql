drop proc if exists product_edit
go

create proc product_edit
     ( @id                    int,
       @name                  varchar(100),
       @weightedItem          bit,
			 @suggestedSellingPrice decimal(14,2))
as
   update product
      set name                  = @name,
          weightedItem          = @weightedItem,
          suggestedSellingPrice = @suggestedSellingPrice
    where id = @id

go
grant execute on product_edit to public
go