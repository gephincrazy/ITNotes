--PIVOT DATA
SELECT QueueName,[Open],[Accepted],[Pending Customer],[Pending Other],[Pending Change],[Work In Progress]
FROM
(
SELECT QueueName, CurrentStatus FROM DBO.CASEDETAILS WHERE CURRENTSTATUS <> 'CLOSED' 
) AS SOURCETABLE
PIVOT
(
COUNT(CurrentStatus)
FOR CurrentStatus IN ([Open],[Accepted],[Pending Customer],[Pending Other],[Pending Change],[Work In Progress])
)AS PIVOTTABLE;


-- Create user for SQL Authentication
CREATE LOGIN CASES WITH PASSWORD = 'Infosys(123'
,DEFAULT_DATABASE = [SyngentaCases]
GO
-- Now add user to database
USE [SyngentaCases];
CREATE USER CASES FOR LOGIN CaseUser;
GO

EXEC sp_addrolemember N'db_datareader', N'CASES'

--DROP USER
SELECT s.name
FROM sys.schemas s
WHERE s.principal_id = USER_ID('CaseUser');

ALTER AUTHORIZATION ON SCHEMA::db_owner TO dbo;
ALTER AUTHORIZATION ON SCHEMA::db_ddladmin TO dbo;
ALTER AUTHORIZATION ON SCHEMA::db_datawriter TO dbo;

USE [SyngentaCases];
DROP USER CaseUser
GO


