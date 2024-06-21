if not exists(select 1 from sys.objects where name  = 'product' and type = 'U')
begin 

create table Product (
    Id                     INT PRIMARY KEY,
    Name                   VARCHAR(100) NOT NULL,
    WeightedItem           BIT NULL,
    SuggestedSellingPrice  DECIMAL(14,2) NULL
)
end