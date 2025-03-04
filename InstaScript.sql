USE [master]
GO
/****** Object:  Database [Instagram]    Script Date: 2025-01-25 23:06:42 ******/
CREATE DATABASE [Instagram]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Instagram', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Instagram.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Instagram_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Instagram_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Instagram] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Instagram].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Instagram] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Instagram] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Instagram] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Instagram] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Instagram] SET ARITHABORT OFF 
GO
ALTER DATABASE [Instagram] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Instagram] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Instagram] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Instagram] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Instagram] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Instagram] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Instagram] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Instagram] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Instagram] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Instagram] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Instagram] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Instagram] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Instagram] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Instagram] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Instagram] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Instagram] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Instagram] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Instagram] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Instagram] SET  MULTI_USER 
GO
ALTER DATABASE [Instagram] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Instagram] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Instagram] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Instagram] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Instagram] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Instagram] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Instagram] SET QUERY_STORE = ON
GO
ALTER DATABASE [Instagram] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Instagram]
GO
/****** Object:  User [UserReadonly]    Script Date: 2025-01-25 23:06:42 ******/
CREATE USER [UserReadonly] FOR LOGIN [UserReadonly] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [UserOwner]    Script Date: 2025-01-25 23:06:42 ******/
CREATE USER [UserOwner] FOR LOGIN [UserOwner] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [ReadonlyRole]    Script Date: 2025-01-25 23:06:42 ******/
CREATE ROLE [ReadonlyRole]
GO
/****** Object:  DatabaseRole [OwnerRole]    Script Date: 2025-01-25 23:06:42 ******/
CREATE ROLE [OwnerRole]
GO
ALTER ROLE [ReadonlyRole] ADD MEMBER [UserReadonly]
GO
ALTER ROLE [OwnerRole] ADD MEMBER [UserOwner]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 2025-01-25 23:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostID] [int] NOT NULL,
	[Caption] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 2025-01-25 23:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentID] [int] NOT NULL,
	[PostID] [int] NULL,
	[CommentContent] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewPostsWithComments]    Script Date: 2025-01-25 23:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Skapa en vy som visar Posts och deras kommentarer
CREATE VIEW [dbo].[ViewPostsWithComments] AS
SELECT 
    p.PostID, 
    p.Caption, 
    p.CreatedDate AS PostDate,
    c.CommentID, 
    c.CommentContent, 
    c.CreatedDate AS CommentDate
FROM Post p
LEFT JOIN Comment c ON p.PostID = c.PostID;
GO
/****** Object:  Table [dbo].[Like]    Script Date: 2025-01-25 23:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Like](
	[LikeID] [int] NOT NULL,
	[PostID] [int] NULL,
	[UserID] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LikeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (1, 1, N'Looks amazing! Wish I could join you!', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 2)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (2, 1, N'Such a fun day! Hope we can do it again soon.', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 3)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (3, 2, N'The lake looks so peaceful, I need to visit!', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 4)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (4, 2, N'Nature is so calming, perfect way to spend the day.', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 5)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (5, 3, N'That pasta looks delicious, can you share the recipe?', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 6)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (6, 3, N'I love pasta, you’re making me hungry!', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 1)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (7, 4, N'Great job on the workout! Keep it up!', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 2)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (8, 4, N'Incredible! I should start working out too!', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 3)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (9, 5, N'Your travel adventures are inspiring! Keep exploring!', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 4)
INSERT [dbo].[Comment] ([CommentID], [PostID], [CommentContent], [CreatedDate], [UserID]) VALUES (10, 6, N'I need to read that book too, what’s it about?', CAST(N'2025-01-25T23:00:22.220' AS DateTime), 5)
GO
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (1, 1, 2, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (2, 1, 3, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (3, 1, 4, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (4, 2, 5, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (5, 2, 6, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (6, 3, 1, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (7, 3, 2, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (8, 4, 3, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (9, 5, 4, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
INSERT [dbo].[Like] ([LikeID], [PostID], [UserID], [CreatedDate]) VALUES (10, 6, 5, CAST(N'2025-01-25T23:00:43.143' AS DateTime))
GO
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (1, N'Exploring the city with friends! #fun #citylife', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 1)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (2, N'A peaceful walk by the lake. #nature', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 2)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (3, N'Delicious homemade pasta for dinner! #foodie #yum', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 3)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (4, N'Had a great workout session today! #fitness', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 4)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (5, N'Traveling the world, one country at a time! #wanderlust', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 1)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (6, N'Cozy weekend at home with a good book. #relaxation', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 2)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (7, N'Sunset at the beach! #sunset #beachlife', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 3)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (8, N'Morning coffee with a beautiful view. #coffeetime', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 4)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (9, N'Trying out a new restaurant downtown. #foodie #delicious', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 5)
INSERT [dbo].[Post] ([PostID], [Caption], [CreatedDate], [UserID]) VALUES (10, N'Weekend hiking trip in the mountains. #adventure #hiking', CAST(N'2025-01-25T23:00:09.560' AS DateTime), 6)
GO
ALTER TABLE [dbo].[Comment] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Like] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Post] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([PostID])
GO
ALTER TABLE [dbo].[Like]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([PostID])
GO
USE [master]
GO
ALTER DATABASE [Instagram] SET  READ_WRITE 
GO
