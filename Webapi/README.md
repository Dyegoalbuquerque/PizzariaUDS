# Webapi

Este projeto foi configurado com as tecnologias .net core version 2.2 onde expõe endpoints como integração na parte do back-end.

## Build

Executar `dotnet build` para buildar o projeto.

## Running

Executar `dotnet run` para executar o projeto.

## Configure

Para configurar o ambiente de produção antes de executar o projeto, no arquivo Webapi/Data/Dao.cs, defina o ambiente em que o projeto irá rodar como "producao ou teste" assim ele irá saber de onde irá buscar a fonte de dados.

## Swagger 

Como ambiente de documentação da api, está configurado com o framework swagger na versão 4.0.1, ao executar a aplicação no passo 'Running' basta ir no endereço https://localhost:porta/swagger que irá visualizar a documentação da api.
