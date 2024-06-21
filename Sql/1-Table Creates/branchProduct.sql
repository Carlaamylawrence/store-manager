if not exists(select 1 from sys.objects where name  = 'BranchProduct' and type = 'U')
begin 

create table BranchProduct 
(
   BranchID INT,
   ProductID INT,
   CONSTRAINT PK_BranchProduct PRIMARY KEY (BranchID, ProductID),
   CONSTRAINT FK_BranchProduct_Branch FOREIGN KEY (BranchID) REFERENCES Branch(ID),
   CONSTRAINT FK_BranchProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ID)
);

end