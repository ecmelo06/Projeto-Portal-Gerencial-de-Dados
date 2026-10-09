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
```

## Como utilizar

Siga as instruções abaixo para baixar, configurar e executar o projeto localmente em seu ambiente de desenvolvimento.

### 1. Clonar o Projeto
Abra o seu terminal ou prompt de comando na pasta onde costuma armazenar seus projetos e faça o clone deste repositório:
```bash
git clone https://github.com/ecmelo06/Projeto-Portal-Gerencial-de-Dados
```

Entre no diretório raiz do sistema:
```bash
cd Projeto-Portal-Gerencial-de-Dados
```

### 2. Configurar a String de Conexão
Abra o projeto na sua IDE (Visual Studio ou VS Code) e localize o arquivo **`appsettings.json`**. Atualize a conexão para apontar para o seu servidor SQL Server local:
```json
"ConnectionStrings": {
    "BancoContext": "Server=SEU_SERVIDOR_SQL;Database=PortalGerencialDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Restaurar as Dependências
Execute o comando abaixo para baixar e restaurar todas as bibliotecas do .NET Core e do Entity Framework gerenciadas pelo NuGet:
```bash
dotnet restore
```

### 4. Executar as Migrations do Banco de Dados
O projeto já conta com o mapeamento e com a carga automática (Seed) dos perfis de acesso. Para criar o banco físico local e estruturar todas as tabelas com os dados iniciais preenchidos, execute:

```bash
# Instala a ferramenta global do EF Core (caso seu ambiente não possua):
dotnet tool install --global dotnet-ef

# Executa o histórico de migrations e popula os perfis automaticamente:
dotnet ef database update
```
> *Nota: Após a conclusão deste comando, as tabelas estarão prontas e a tabela `Perfil` já conterá os registros de **Admin**, **AGERGS**, **Comum**, **Externo** e **SuperAdmin** vinculados de forma automática.*

### 5. Rodar a Aplicação
Inicie o servidor embutido do ASP.NET Core:
```bash
dotnet run
```
Abra o seu navegador e acesse a URL local indicada no terminal:
**`http://localhost:5202`**
