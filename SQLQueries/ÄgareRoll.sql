USE Instagram;  --L�gg till databas
GO

-- Create the Owner user
CREATE LOGIN owner_user WITH PASSWORD = '123';  -- V�lj l�sen ord
GO
--skapa Rollen till �gare
CREATE USER owner_user FOR LOGIN owner_user;
GO

-- Ge roll till �garen
ALTER ROLE db_owner ADD MEMBER owner_user;
GO
