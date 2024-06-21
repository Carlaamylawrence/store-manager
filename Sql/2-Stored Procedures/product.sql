drop procedure if exists product_add
go
  create procedure product_add
      (
         @name varchar(100),
         @weightedItem bit,
				 @suggestedSellingPrice decimal(14,2)
      )  
as

 insert  product
      ( 
         name,
         weightedItem,
         suggestedSellingPrice
      )

 values
      (
         @name,
         @weightedItem,
         @suggestedSellingPrice
      )

 select scope_identity()
go
grant execute on product_add to public
go