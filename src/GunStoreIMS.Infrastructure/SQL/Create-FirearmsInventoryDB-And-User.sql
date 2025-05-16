-- Step 1: Create the Firearms Inventory Database
CREATE DATABASE FirearmsInventoryDB;
GO

-- Step 2: Remove existing login if it exists
IF EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = 'Trueniarobik')
BEGIN
    DROP LOGIN Trueniarobik;
END
GO

-- Step 3: Create a new login with password policy enforcement
CREATE LOGIN Trueniarobik 
    WITH PASSWORD = 'Rambo67Cosmo$#!',
         CHECK_POLICY = ON;
GO

-- Step 4: Grant access to the FirearmsInventoryDB database
USE FirearmsInventoryDB;

-- Create user mapped to the login
CREATE USER Trueniarobik FOR LOGIN Trueniarobik;
GO

-- Assign db_owner role (consider using more restrictive roles if applicable)
ALTER ROLE db_owner ADD MEMBER Trueniarobik;
GO

-- Step 5: Verify login and user creation at the server and database level
SELECT name, type_desc FROM sys.server_principals WHERE name = 'Trueniarobik';
GO

SELECT name, type_desc FROM sys.database_principals WHERE name = 'Trueniarobik';
GO

-- Step 6: List tables in the database to verify structure
SELECT * FROM FirearmsInventoryDB.INFORMATION_SCHEMA.TABLES;
GO

-- Additional: Setup for a secondary audit database (if required)
USE master;
CREATE DATABASE VendorAuditDB;
GO

-- Create a login for a user with general access
CREATE LOGIN YourUsername 
    WITH PASSWORD = 'YourSecurePassword!',
         CHECK_POLICY = ON;
GO

-- Map login to application database and define appropriate roles
USE FirearmsInventoryDB;
CREATE USER YourUsername FOR LOGIN YourUsername;
GO

-- Assign permissions based on the required level of access
EXEC sp_addrolemember 'db_owner', 'YourUsername'; -- Use cautiously in production
-- EXEC sp_addrolemember 'db_datareader', 'YourUsername';
-- EXEC sp_addrolemember 'db_datawriter', 'YourUsername';
GO