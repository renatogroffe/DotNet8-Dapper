USE BaseEmpresas
GO

CREATE TABLE [dbo].[Empresas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Contato] [varchar](100) NOT NULL,
	[Cidade] [varchar](100) NOT NULL,
	[Pais] [varchar](60) NOT NULL
)
GO

ALTER TABLE [dbo].[Empresas] ADD  CONSTRAINT [PK_Chamados] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
GO

PRINT 'Tabela dbo.Empresas criada com sucesso!'