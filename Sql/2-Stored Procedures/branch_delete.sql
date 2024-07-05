drop proc if exists branch_delete
go

create proc branch_delete
     ( @id  int)
as
Begin
   delete 
      from branchProduct
     where BranchId = @id;

   delete
     from branch
	where id = @id
End
go

grant execute on branch_delete to public
go
 