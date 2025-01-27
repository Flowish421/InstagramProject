USE [YourDatabaseName];  -- Replace with your actual database name
GO

-- Create the Read-Only user
CREATE LOGIN readonly_user WITH PASSWORD = 'AnotherSecurePassword';  -- Replace with a secure password
GO

CREATE USER readonly_user FOR LOGIN readonly_user;
GO

-- Grant the db_datareader role to the Read-Only user
ALTER ROLE db_datareader ADD MEMBER readonly_user;
GO
