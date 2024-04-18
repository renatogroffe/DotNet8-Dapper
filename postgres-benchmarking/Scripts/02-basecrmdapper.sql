-- Criar banco de dados
CREATE DATABASE "basecrmdapper";

-- Usar o banco de dados
\c "basecrmdapper";

-- Criar tabela Empresas
CREATE TABLE "Empresas" (
    "IdEmpresa" serial PRIMARY KEY,
    "CNPJ" char(14) NOT NULL,
    "Nome" varchar(100) NOT NULL,
    "Cidade" varchar(50) NOT NULL
);

-- Criar tabela Contatos
CREATE TABLE "Contatos" (
    "IdContato" serial PRIMARY KEY,
    "Nome" varchar(100) NOT NULL,
    "Telefone" varchar(20) NOT NULL,
    "IdEmpresa" int NOT NULL,
    CONSTRAINT "FK_Contato_Empresa" FOREIGN KEY ("IdEmpresa") REFERENCES "Empresas"("IdEmpresa")
);
