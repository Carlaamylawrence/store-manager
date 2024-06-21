drop proc if exists branch_delete
go

create proc branch_delete
     ( @id  int)
as

   delete
     from branch
	where id = @id

go

grant execute on branch_delete to public
go
 