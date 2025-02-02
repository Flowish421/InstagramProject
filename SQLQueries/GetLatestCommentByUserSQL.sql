SELECT 
    Comments.CommentID, 
    Comments.CommentText, 
    Comments.Timestamp AS CommentDate, 
    Users.UserName AS CommentedBy,
    Posts.Caption AS PostCaption
FROM 
    Comments
JOIN 
    Users ON Comments.UserID = Users.UserID
JOIN 
    Posts ON Comments.PostID = Posts.PostID
ORDER BY 
    Comments.Timestamp DESC;
