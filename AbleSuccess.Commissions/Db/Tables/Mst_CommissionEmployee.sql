CREATE TABLE [dbo].[Txn_CommissionProfile](
	[CommissionProfileId] [int] IDENTITY(1,1) NOT NULL,
	[CommissionId] [int] NOT NULL,
	[ProfileId] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[Rate] [decimal](18, 2) NOT NULL,
) ON [PRIMARY]
