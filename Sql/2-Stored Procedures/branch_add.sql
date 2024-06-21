drop procedure if exists branch_add
go
  create procedure branch_add
      (
				@id int,
         @name varchar(100),
         @telephoneNumber varchar(15),
				 @openDate DateTime
      )  
as

 insert  branch
      ( id,
         name,
         telephoneNumber,
         openDate
      )

 values
      (  @id,
         @name,
         @telephoneNumber,
         @openDate
      )
go
grant execute on branch_add to public
go

select * from branch where id = 1