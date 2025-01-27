USE Instagram;  --Lägg till databas
GO

-- Create the Owner user
CREATE LOGIN owner_user WITH PASSWORD = '123';  -- Välj lösen ord
GO
--skapa Rollen till ägare
CREATE USER owner_user FOR LOGIN owner_user;
GO

-- Ge roll till ägaren
ALTER ROLE db_owner ADD MEMBER owner_user;
GO
