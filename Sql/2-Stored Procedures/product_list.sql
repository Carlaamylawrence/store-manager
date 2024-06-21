drop procedure if exists product_list
go

create procedure product_list
as
  select Id,
         Name,
         WeightedItem,
				 SuggestedSellingPrice
  from product
go

grant execute on product_list to public
go

--exec product_list