CREATE TYPE dbo.BranchType AS TABLE
(
    id INT NOT NULL,
    name NVARCHAR(100) NOT NULL,
    TelephoneNumber NVARCHAR(15) NULL,
    OpenDate DATETIME NULL
);