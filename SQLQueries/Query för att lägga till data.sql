-- Infoga 10 användare
INSERT INTO Users (UserName, Password, Email) VALUES
('Alice', 'pass123', 'alice@example.com'),
('Bob', 'secure456', 'bob@example.com'),
('Charlie', 'mypassword', 'charlie@example.com'),
('David', 'davidpass', 'david@example.com'),
('Emma', 'emma1234', 'emma@example.com'),
('Frank', 'frankpass', 'frank@example.com'),
('Grace', 'gracepass', 'grace@example.com'),
('Henry', 'henrypass', 'henry@example.com'),
('Ivy', 'ivypass', 'ivy@example.com'),
('Jack', 'jackpass', 'jack@example.com');

-- Infoga inlägg (5-15 per användare)
DECLARE @i INT = 1;
DECLARE @postCount INT;
DECLARE @userID INT;
WHILE @i <= 10
BEGIN
    SET @postCount = FLOOR(RAND() * 11) + 5; -- Slumpmässigt mellan 5 och 15 inlägg
    DECLARE @j INT = 1;
    WHILE @j <= @postCount
    BEGIN
        INSERT INTO Posts (Image, Caption, UserID) 
        VALUES ('image' + CAST(@i AS VARCHAR) + '_' + CAST(@j AS VARCHAR) + '.jpg', 'Caption for post ' + CAST(@j AS VARCHAR), @i);
        SET @j = @j + 1;
    END
    SET @i = @i + 1;
END

-- Infoga likes, kommentarer och shares
DECLARE @postID INT;
SET @i = 1;
WHILE @i <= (SELECT COUNT(*) FROM Posts)
BEGIN
    -- Likes (1-10 per inlägg)
    DECLARE @likeCount INT = FLOOR(RAND() * 10) + 1;
    DECLARE @k INT = 1;
    WHILE @k <= @likeCount
    BEGIN
        INSERT INTO Likes (PostID, UserID) VALUES (@i, FLOOR(RAND() * 10) + 1);
        SET @k = @k + 1;
    END
    
    -- Comments (1-5 per inlägg)
    DECLARE @commentCount INT = FLOOR(RAND() * 5) + 1;
    SET @k = 1;
    WHILE @k <= @commentCount
    BEGIN
        INSERT INTO Comments (CommentText, PostID, UserID) 
        VALUES ('Comment ' + CAST(@k AS VARCHAR) + ' on post ' + CAST(@i AS VARCHAR), @i, FLOOR(RAND() * 10) + 1);
        SET @k = @k + 1;
    END
    
    -- Shares (1-3 per inlägg)
    DECLARE @shareCount INT = FLOOR(RAND() * 3) + 1;
    SET @k = 1;
    WHILE @k <= @shareCount
    BEGIN
        INSERT INTO Shares (PostID, UserID) VALUES (@i, FLOOR(RAND() * 10) + 1);
        SET @k = @k + 1;
    END
    
    SET @i = @i + 1;
END
