CREATE TABLE [dbo].[Mst_Credential](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserCode] [varchar](20) NULL,
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Role] [int] NOT NULL,
	[Status] [int] NOT NULL
) ON [PRIMARY]

