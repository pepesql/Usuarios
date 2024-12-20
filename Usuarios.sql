USE [Prueba]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 24/11/2024 09:39:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fullname] [nvarchar](255) NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[createdAt] [datetime2](7) NOT NULL,
	[updatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_16d4f7d636df336db11d87413e3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [fullname], [username], [password], [email], [createdAt], [updatedAt]) VALUES (6, N'prueba', N'prueba', N'ieUeaCv3KxIQt8zcBiuteWmGKX/S6qZ0JqeYyNOvXPr6y7eELoSrr1rHKA8aPHAV', N'prueba@gmail.com', CAST(N'2024-11-25T02:20:02.6700000' AS DateTime2), CAST(N'2024-11-25T02:20:02.6700000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_4d6dcaa56443847d1fdb3f589cd]  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_61ffd183b0382276c2d733ff018]  DEFAULT (getdate()) FOR [updatedAt]
GO
