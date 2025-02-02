SELECT 
    Posts.PostID, 
    Posts.Caption, 
    COUNT(Likes.LikeID) AS TotalLikes, 
    COUNT(Comments.CommentID) AS TotalComments
FROM 
    Posts
LEFT JOIN 
    Likes ON Posts.PostID = Likes.PostID
LEFT JOIN 
    Comments ON Posts.PostID = Comments.PostID
GROUP BY 
    Posts.PostID, Posts.Caption
ORDER BY 
    TotalLikes DESC, TotalComments DESC;
