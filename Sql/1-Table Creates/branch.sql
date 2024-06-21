if not exists(select 1 from sys.objects where name  = 'Branch' and type = 'U')
begin 

create table Branch (
    Id                INT PRIMARY KEY,
    Name              VARCHAR(100) NOT NULL,
    TelephoneNumber   VARCHAR(15) NULL,
    OpenDate          DATETIME NULL
)
end
