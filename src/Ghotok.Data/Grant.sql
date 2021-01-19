--Grant permission to database for IIS. followed link:
--https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis

--Grant permission for defaultappPool
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'IIS APPPOOL\DefaultAppPool')
BEGIN
    CREATE LOGIN [IIS APPPOOL\DefaultAppPool] 
      FROM WINDOWS WITH DEFAULT_DATABASE=[master], 
      DEFAULT_LANGUAGE=[us_english]
END
GO
CREATE USER [GhotokApiUser] 
  FOR LOGIN [IIS APPPOOL\DefaultAppPool]
GO
EXEC sp_addrolemember 'db_owner', 'GhotokApiUser'
GO


--Grant permission for Fhotokapi app pool

IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'IIS APPPOOL\GhotokApi AppPool')
BEGIN
    CREATE LOGIN [IIS APPPOOL\GhotokApi AppPool] 
      FROM WINDOWS WITH DEFAULT_DATABASE=[master], 
      DEFAULT_LANGUAGE=[us_english]
END
GO
CREATE USER GhotokApiIISUser 
  FOR LOGIN [IIS APPPOOL\GhotokApi AppPool]
GO
EXEC sp_addrolemember 'db_owner', 'GhotokApiIISUser'
GO