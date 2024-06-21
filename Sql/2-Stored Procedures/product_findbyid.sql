drop procedure if exists product_findbyid
go

create procedure product_findbyid
     ( @id       int)
as

   select id,
          name,
					WeightedItem,
					SuggestedSellingPrice
    from  product
   where  id = @id
go 


grant execute on product_findbyid to public
go


