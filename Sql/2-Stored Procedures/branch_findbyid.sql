drop procedure if exists branch_findbyid
go

create procedure branch_findbyid
     ( @id       int)
as

   select id,
          name,
					TelephoneNumber,
					OpenDate
    from  branch
   where  id = @id
go 


grant execute on branch_findbyid to public
go
