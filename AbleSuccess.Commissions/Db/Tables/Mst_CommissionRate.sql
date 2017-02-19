CREATE TABLE [dbo].[Mst_CommissionRate](
	[RateId] [int] IDENTITY(1,1) NOT NULL,
	[PositionId] [int] NOT NULL,
	[Percentage] [decimal](5, 2) NOT NULL,
	[PercentageOf] [int] NOT NULL
) ON [PRIMARY]
