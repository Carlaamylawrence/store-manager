drop proc if exists branch_edit
go

create proc branch_edit
     ( @id   int,
       @name varchar(100),
       @telephoneNumber varchar(15),
			 @openDate DateTime
			 )
as
   update branch
      set name     = @name,
          telephoneNumber = @telephoneNumber,
          openDate = @openDate
    where id       = @id

go
grant execute on branch_edit to public
go