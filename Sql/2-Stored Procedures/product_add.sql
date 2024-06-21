drop procedure if exists product_add
go
  create procedure product_add
      (	 @id int,
         @name varchar(100),
         @weightedItem bit,
				 @suggestedSellingPrice decimal(14,2)
      )  
as

 insert  product
      (  id,
         name,
         weightedItem,
         suggestedSellingPrice
      )

 values
      (  @id,
         @name,
         @weightedItem,
         @suggestedSellingPrice
      )
go
grant execute on product_add to public
go