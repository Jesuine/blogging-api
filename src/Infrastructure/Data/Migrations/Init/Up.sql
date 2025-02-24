﻿USE BLOG_DATABASE;
GO

CREATE TABLE BlogPost (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,     
    Content NVARCHAR(MAX) NOT NULL,   
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE() 
);


CREATE TABLE Comment (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    BlogPostId UNIQUEIDENTIFIER NOT NULL,            
    Content NVARCHAR(MAX) NOT NULL,   
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), 
    FOREIGN KEY (BlogPostId) REFERENCES BlogPost(Id) 
);
