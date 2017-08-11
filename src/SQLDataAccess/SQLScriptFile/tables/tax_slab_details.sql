USE [MyCalculator]
GO

/****** Object:  Table [dbo].[tax_slab_details]    Script Date: 11-08-2017 16:21:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tax_slab_details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tax_slab_id] [int] NOT NULL,
	[from_amount] [int] NULL,
	[to_amount] [int] NULL,
	[percentage] [int] NOT NULL,
 CONSTRAINT [PK_tax_slab] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tax_slab_details]  WITH CHECK ADD  CONSTRAINT [FK_tax_slab_details_tax_slab] FOREIGN KEY([tax_slab_id])
REFERENCES [dbo].[tax_slab] ([id])
GO

ALTER TABLE [dbo].[tax_slab_details] CHECK CONSTRAINT [FK_tax_slab_details_tax_slab]
GO


