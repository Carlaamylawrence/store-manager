drop procedure if exists branch_list
go

create procedure branch_list
as
  select Id,
         Name,
         TelephoneNumber,
				 OpenDate
  from branch
go

grant execute on branch_list to public
go

--exec branch_list