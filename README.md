# Projeto Portal Gerencial de Dados --version 1.0

Sistema web desenvolvido para AGERGS em ASP.NET MVC para gerenciamento de usuários, autenticação e controle de acesso por perfil.

## Sobre o Projeto

O Projeto Portal Gerencial de Dados foi desenvolvido com o objetivo de centralizar o gerenciamento de usuários e controlar o acesso ao sistema através de autenticação, perfis de usuário e sessões.

A aplicação utiliza ASP.NET MVC, Entity Framework Core e SQL Server, seguindo uma arquitetura baseada em Controllers, Models Views e Repositories.

## Funcionalidades Implementadas

### Cadastro de Usuários

- Cadastro de novos usuários
- Armazenamento em banco de dados SQL Server
- Associação automática de perfil padrão ao usuário

### Autenticação

- Login por e-mail e senha
- Validação de credenciais
- Proteção contra armazenamento de senha em texto puro

### Segurança

- Implementação de hash de senha utilizando PasswordHasher do ASP.NET Identity
- Verificação segura de credenciais
- Controle de sessão após autenticação

### Controle de Sessão

- Criação de sessão após login
- Persistência de usuário autenticado
- Logout com encerramento da sessão

### Informações do Usuário

Após autenticação, o sistema exibe:

- Nome do usuário
- E-mail
- Órgão
- Perfil

## Arquitetura Utilizada

O projeto segue uma estrutura baseada em:

- Controllers
- Models
- Repository Pattern
- Entity Framework Core
- SQL Server

Fluxo principal:

Cadastro
↓
Banco de Dados
↓
Login
↓
Validação de Hash
↓
Sessão
↓
Área Autenticada

## Tecnologias Utilizadas

- C#
- ASP.NET MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity (PasswordHasher)
- Bootstrap
- Git
- GitHub

## Estrutura do Projeto

```text
Controllers/
Data/
Models/
Repository/
Views/
wwwroot/
