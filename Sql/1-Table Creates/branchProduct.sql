if not exists(select 1 from sys.objects where name  = 'BranchProduct' and type = 'U')
begin 

create table BranchProduct 
(
	 Id INT IDENTITY(1,1) PRIMARY KEY,
   BranchID INT,
   ProductID INT,
   CONSTRAINT FK_BranchProduct_Branch FOREIGN KEY (BranchID) REFERENCES Branch(ID),
   CONSTRAINT FK_BranchProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ID)
);

end