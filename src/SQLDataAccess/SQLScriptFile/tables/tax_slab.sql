USE [MyCalculator]
GO

/****** Object:  Table [dbo].[tax_slab]    Script Date: 11-08-2017 16:21:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tax_slab](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[from_year] [int] NOT NULL,
	[to_year] [int] NOT NULL,
	[category] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tax_slab_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


