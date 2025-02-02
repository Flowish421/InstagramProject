OPEN SYMMETRIC KEY UserPasswordKey  
DECRYPTION BY PASSWORD = 'SuperStrongKey123!';

SELECT UserID, UserName, 
       CONVERT(VARCHAR, DecryptByKey(Password)) AS DecryptedPassword 
FROM Users;

CLOSE SYMMETRIC KEY UserPasswordKey;
