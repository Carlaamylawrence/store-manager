CREATE PROCEDURE [dbo].branch_file_insert
    @Branches BranchType READONLY
AS
BEGIN
    INSERT INTO Branch(Id, Name, TelephoneNumber, OpenDate)
    SELECT 
        Id, 
        Name, 
        TelephoneNumber, 
        OpenDate
    FROM @Branches;
END