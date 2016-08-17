EXEC sp_helpserver			-- Also shows services etc.

-- Delete any existing linked-server attempt (optional)
EXEC sp_dropserver  @server = 'MyRemoteServer',  @droplogins = 'droplogins'		-- 'droplogins' = Drop associated logins, NULL=Do not drop logins

-- Create Linked Server (use command below for NON-Standard port
EXEC sp_addlinkedserver  @server = 'MyRemoteServer'	,	@srvproduct = 'SQL Server'	,@provider = 'SQLOLEDB'	 ,@datasrc = 'MyRemoteServer'	

-- For NON-Standard SQL Port use:
EXEC sp_addlinkedserver @server = 'MyRemoteServer', @srvproduct = 'SQL Server', @provider = 'SQLOLEDB', @datasrc = 'MyRemoteServer,1144'	-- Set the port appropriately
-- Example of Oracle connection:
EXEC sp_addlinkedserver @server = 'MyOracleServer', @srvproduct = 'Oracle', @provider = 'MSDAORA', @datasrc = 'OracleServerName'	-- , @location=NULL, @provstr=NULL, @catalog=NULL

-- Remove existing Linked Server Login (optional)
EXEC sp_droplinkedsrvlogin @rmtsrvname = 'MyRemoteServer',	@locallogin = 'MyUserID'

-- Create Linked Server Login
EXEC sp_addlinkedsrvlogin @rmtsrvname = 'MyRemoteServer' ,	@useself = 'false',	@locallogin = 'MyUserID',	@rmtuser = 'MyUserID',	@rmtpassword = 'MyPassword'	

-- Test connection - should list databases on remote machine
select top 10 name from [MyRemoteServer].master.dbo.sysdatabases

-- If you get this error message:  "Server '111.222.333.444' is not configured for DATA ACCESS"
exec sp_serveroption 'MyRemoteServer', 'data access', 'true'

-- Alternative test using OPENQUERY
SELECT	* FROM	OPENQUERY([MyRemoteServer], 'SELECT TOP 10 * FROM master.dbo.sysobjects') 
GO

