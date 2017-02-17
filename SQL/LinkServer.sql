EXEC sp_helpserver			-- Also shows services etc.

-- Delete any existing linked-server attempt (optional)
EXEC sp_dropserver  @server = 'MyRemoteServer',  @droplogins = 'droplogins'		-- 'droplogins' = Drop associated logins, NULL=Do not drop logins

-- Create Linked Server (use command below for NON-Standard port
EXEC master.dbo.sp_addlinkedserver @server = N'139.224.190.159', @srvproduct=N'SQL Server'
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'139.224.190.159',@useself=N'False',@locallogin=NULL,@rmtuser=N'iZhd7m11gnjt8eZ',@rmtpassword='########'

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




GO

EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'collation compatible', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'data access', @optvalue=N'true'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'dist', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'pub', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'rpc', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'rpc out', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'sub', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'connect timeout', @optvalue=N'0'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'collation name', @optvalue=null
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'lazy schema validation', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'query timeout', @optvalue=N'0'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'use remote collation', @optvalue=N'true'
GO
EXEC master.dbo.sp_serveroption @server=N'139.224.190.159', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO

