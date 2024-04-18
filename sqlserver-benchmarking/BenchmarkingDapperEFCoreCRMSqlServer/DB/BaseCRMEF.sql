USE master
GO

CREATE DATABASE BaseCRMEF
GO

USE BaseCRMEF
GO

CREATE TABLE dbo.Empresas(
	IdEmpresa int IDENTITY(1,1) NOT NULL,
	CNPJ char(14) NOT NULL,
	Nome varchar(100) NOT NULL,
	Cidade varchar(50) NOT NULL,
	CONSTRAINT PK_Empresas PRIMARY KEY (IdEmpresa)
)
GO

CREATE TABLE dbo.Contatos(
	IdContato int IDENTITY(1,1) NOT NULL,
	Nome varchar(100) NOT NULL,
	Telefone varchar(20) NOT NULL,
	IdEmpresa int NOT NULL,
	CONSTRAINT PK_Contatos PRIMARY KEY (IdContato),
	CONSTRAINT FK_Contato_Empresa FOREIGN KEY (IdEmpresa) REFERENCES dbo.Empresas(IdEmpresa)
)
GO

USE master
GO