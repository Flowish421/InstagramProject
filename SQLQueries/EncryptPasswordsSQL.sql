OPEN SYMMETRIC KEY UserPasswordKey  
DECRYPTION BY PASSWORD = 'SuperStrongKey123!';

-- Fungerar inte nu f�r att Users password �r VARCHAR(50) och inte VARBINARY(MAX)
UPDATE Users
SET Password = EncryptByKey(Key_GUID('UserPasswordKey'), Password);

CLOSE SYMMETRIC KEY UserPasswordKey;

