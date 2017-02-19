CREATE TABLE [dbo].[Mst_Profile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[NickName] [varchar](10) NULL,
	[Position] [int] NOT NULL,
	[Devision] [int] NOT NULL,
	[Email] [varchar](50) NULL,
	[Telephone] [varchar](50) NULL,
	[Address] [varchar](max) NULL,
	[Photo] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]