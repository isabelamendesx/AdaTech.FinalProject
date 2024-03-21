
![Frame 2539](https://github.com/isabelamendesx/AdaTech.FinalProject/assets/129897959/706e3e26-022b-42b6-87f7-81f25fe4c65a)

# ⚙️ Motor de Aprovação - DiverseDev

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)
![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)

Esse projeto é uma API criada utilizando C#, .NET, PostgreSQL como banco de dados, Swagger para documentação e JWT para autorização e autenticação.

## 💡 Sobre

Este projeto foi desenvolvido como trabalho de conclusão do curso de back-end em c# DiverseDev da empresa Mecado Eletrônico em parceria com a Edtech ADA.

### 🌟 Contexto do Desafio
Dentro do contexto de reembolso, parte crucial do controle de pagamento de reembolsos é o seu processo de aprovação. Essa etapa muitas vezes se revela trabalhosa, pois demanda que o departamento financeiro envie manualmente esse documento para o gestor, que em muitos casos essa informação não está clara e pode impactar negativamente na eficiência operacional.

Nossa missão foi criar uma API(**Motor de aprovação**) para automatizar o proceso de reembolso levando em conta os seguintes critérios:

1. Funcionalidade e efetividade da solução
2. Modelagem e estrutura de dados
3. Desenvolvimento e boas práticas
4. Segurança
5. Usabilidade e experiência do Usuário(UX)
6. Escalabilidade e Peformance
7. Manutenibilidade e Extensibilidade


## 📋 Tabela de Conteúdo

- [Sobre](#sobre)
- [Tabela de Conteúdo](#tabela-de-conteúdo)
- [Descrição do Projeto](#-descrição-do-projeto)
- [Funcionalidades](#funcionalidades)
- [Pré-requisitos](#pré-requisitos)
- [Como Executar](#como-executar)
- [API Endpoints](#api-endpoints)
- [Segurança](#segurança)
- [Documentação](#documentação)
- [Autoras](#autoras)
- [Licença](#licença)

## 📝 Descrição do projeto
Para solucionar o desafio do controle de pagamento de reembolsos, desenvolvemos um motor de aprovação que automatiza o processo de decisão sobre a aprovação ou recusa de documentos de reembolso. Este motor é ativado automaticamente ao criar um novo documento de reembolso e segue uma tabela de decisão com critérios que podem ser pré-definidos e alterados de acordo com a necessidade da gestão financeira responsável, como por exemplo:

* Para valores de reembolso até R$100, todas as categorias são automaticamente aprovadas.
* Para valores entre R$101 e R$500, as categorias de alimentação e transporte são automaticamente aprovadas.
* Para valores acima de R$1.000, todas as categorias são automaticamente recusadas.
* Caso nenhuma dessas regras seja atendida, o status do documento é definido como "EM APROVAÇÃO", exigindo uma intervenção manual do departamento financeiro para decidir sobre a aprovação ou recusa.

**IMPORTANTE:** As regras de aprovação são facilmente alteradas por uma pessoa **autorizada**.

Para armazenar os dados relacionados aos documentos de reembolso e às regras de aprovação, utilizamos um banco de dados PostgreSQL hospedado no ElephantSQL, que oferece uma solução de banco de dados na nuvem confiável e escalável. Essa escolha permite o armazenamento seguro e eficiente dos dados, garantindo disponibilidade e performance para a nossa aplicação.

## 🔍 Funcionalidades
* **Refund**:
  * Criar solicitação de reembolso
  * Ver todas solicitações por status
  * Aprovar solicitação
  * Reprovar solicitação
* **Category**:
  * Criar nova categoria
  * Ver todas as categorias
  * Procurar categoria por Id
* **Rule**:
  * Criar nova regra
  * Ver todas as regras
  * Procurar regra por Id
  * Desativar regra
  * Desativar regra de uma categoria
* **User**:
  * Registrar novo usuário
  * Fazer Login de Usuário
  * Atualizar Login
  

## 📋 Pré-requisitos

Antes de iniciar, certifique-se de ter o seguinte instalado em sua máquina:

1. Visual Studio (VS): É necessário para compilar e executar o projeto.
2. .NET SDK: Verifique se você tem o .NET SDK instalado. Caso contrário, baixe-o [aqui](https://dotnet.microsoft.com/pt-br/download/visual-studio-sdks).
3. Postico/PgADmin ou outro administrador para desenvolvimento PostegreSQL.

## ▶️ Como Executar

1. Conecte-se ao banco de dados PostgreSQL.
   
   Obs: Passo-a-passo [aqui](https://medium.com/@noogetz/how-to-setup-a-database-with-elephantsql-7d87ea9953d0#03d1).
   
    **----- DADOS DO SERVIDOR -----**
   
     - Host: kesavan.db.elephantsql.com   
     - User: zpikunvm   
     - Database: zpikunvm   
     - Password: MhwN5nJkB6MwaMoWAGUeRVpp89cGTGyu
   
2. Abra o projeto no Visual Studio.
3. Selecione o modo Release.
4. Compile e execute o projeto.
5. Ao executar o Swagger irá abrir nessa página e será necessário fazer login com um dos usuários abaixo:

   Obs: Atente-se que cada um dos usuários possui permissões diferentes dentro da aplicação.
   
   **----LISTA DE USUARIOS DISPONÍVEIS----**

    **STANDARD**
    (Apenas envia requisições)
    - email: standard@me.com.br
    - senha: Standard123*
    
    **MANAGER**
   (Envia requisições e pode reprovar manualmente)
    - email: manager@me.com.br
    - senha: Manager123*
    
    **SUPERVISOR**
   (Envia requisições e pode reprovar ou aprovar manualmente)
    - email: supervisor@me.com.br
    - senha: Supervisor123*
   
     ![Imagem do campo User/Login no Swagger](https://github.com/isabelamendesx/AdaTech.FinalProject/assets/142990899/02daa3ef-a080-4731-b102-73dd06fb2ca5)

   
6. Após o login bem sucedido, você deverá autorizar o seu acesso inserindo "Baerer +'acessToken'" no campo Authorize.
   Obs: O acessToken será retornado após um login bem-sucedido no cmapo **Response body**.
   
![Imagem do acesso com acessToken](https://github.com/isabelamendesx/AdaTech.FinalProject/assets/142990899/e4c752d0-875d-4f2c-9af4-1b30b5872b85)

7. Após esse processo, a API estará pronta para ser testada e as requisições ficarão salvas no banco de dados.

## 🚀 API Endpoints
A API fornece os seguintes endpoints:

```markdown
--------- CATEGORY -----------

POST /Category - Criar uma nova categoria

GET /Category - Consultar todas as categorias

GET /Category/{id} - Consultar a categoria correspondente ao Id fornecido

--------- REFUND -----------

POST /Refund - Criar um novo documento de reembolso

GET /Refund/{id} - Consultar o reembolso correspondente ao Id fornecido

GET /Refund/status/{status} - Consultar todos os reembolsos com o status fornecido

POST /Refund/approve/{id} - Aprovar o reembolso correspondente ao Id fornecido que está com o status **Em Aprovação**

POST /reject/{id} - Reprovar o reembolso correspondente ao Id fornecido que está com o status **Em Aprovação**

POST /modify-refund/{id}/{status} - Alterar o status do reembolso correspondente ao Id fornecido já aprovado ou reprovado

---------RULE-----------

GET /Rule/{id} - Consultar a regra correspondente ao Id fornecido

GET /Rule - Consultar todas as regras

POST /Rule - Criar uma nova regra

POST /Rule/deactivate/{ruleId}- Desativar a regra correspondente ao Id fornecido

POST /Rule/deactivate/category/{categoryId} - Desativar todas as regras da categoria correspondente ao Id fornecido

---------USER-----------

POST /User/register - Criar um novo usuário

POST /User/login - Fazer login

POST /User/refresh-login - Dar refresh no login

````
## Motor de aprovação

O cerne da aplicação é o motor de aprovação. 

Ele consiste em um método `GetRulesThatApplyToCategory` que, através do [Chain Of Responsibility Pattern](https://mohamed-hendawy.medium.com/chain-of-responsibility-design-pattern-in-c-with-examples-d87da6e5ead), lista as regras aplicadas ao id de categoria requisitado e as organiza por ordem de ação. 

Ou seja, inicialmente o programa buscará as **regras que rejeitam o pedido para todas as classes**, depois a **regra que rejeita o pedido para a categoria específica**. Caso nesse ponto a requisição ainda não tenha sido recusada, o programa continua buscando, agora nas **regras que aprovam todas as categorias** e em seguida, as **regras que aprovam da categoria específica**, abrangendo todas as possibilidades. Caso nenhuma regra aplicável seja encontrada no bando de dados, a aplicação irá configurar o status da requisição como “Em aprovação” e só poderá ser modificado manualmente por um usuário responsável.

![Group 2558](https://github.com/isabelamendesx/AdaTech.FinalProject/assets/48605624/465ef666-33d5-4af0-ae7b-c0cfa5213373)


## 🔒 Segurança
**JWT (*JSON Web Token*)**

Para a segurança no processo de autenticação e autorização, utilizamos o [JWT](https://jwt.io).

Tal ferramenta proporciona maior confiabilidade nas informações trafegadas pois, após o login correto de um usuário, mais uma etapa de segurança é adicionada ao processo conforme um token único é retornado e solicitado em todas as requisições de recurso da aplicação. Dessa forma, o servidor sempre poderá identificar o usuário.

Por ser um serviço `stateless`, diversas vantagens podem ser apontadas em seu uso:
* **Segurança:** Ele usa criptografia para garantir que apenas as partes autorizadas possam acessar as informações transmitidas no token.
* **Escalabilidade:** Não é relevante para qual servidor da aplicação chegará a requisição, ela será atendida! Isso ocorre porque todas as informações necessárias são armazenadas no próprio token e são enviadas ao servidor para autenticação. Isso reduz a carga no servidor e torna o JWT uma solução muito escalável.
* **Proteção a ataques Cross-site Request Forgery [(CSRF)](https://security.stackexchange.com/questions/166724/should-i-use-csrf-protection-on-rest-api-endpoints/166798#166798):** Não é necessário se preocupar com esse tipo de ataques, pois não existe uma sessão para ser falsificada. O JWT usa um sistema de assinatura digital para verificar a autenticidade dos dados. Isso significa que, se alguém tentar alterar os dados dentro do token, a assinatura se tornará inválida e o servidor saberá que os dados foram adulterados.
* **Alta performance:** Devido a não existência de sessão, o servidor necessita apenas calcular o [hash](https://www.voitto.com.br/blog/artigo/o-que-e-hash-e-como-funciona#:~:text=A%20criptografia%20hash%20é%20utilizada,ser%20um%20registro%20alfanumérico%20complexo.), evitando fazer qualquer tipo de busca em bases ou tabelas.
* **Multiservidores:** É possível ter vários servidores rodando em domínios diferentes utilizando o mesmo token.


## 📄 Documentação

Para acessar a documentação completa, incluindo diagramas UML, diagrama de overview e outros recursos, por favor, visite nosso [Google Drive](https://drive.google.com/drive/folders/1D9TqMPc0iEOQ7T5Ity96dsPvmBXfknKT?usp=sharing).

## 👩‍💻 Autoras

Este projeto foi desenvolvido por:

[![author](https://img.shields.io/badge/author-AmandaaBastos-red.svg)](https://github.com/AmandaaBastos)
[![author](https://img.shields.io/badge/author-angelafrocha-green.svg)](https://github.com/angelafrocha)
[![author](https://img.shields.io/badge/author-isabelamendesx-pink.svg)](https://github.com/isabelamendesx)
[![author](https://img.shields.io/badge/author-vitorialira92-purple.svg)](https://github.com/vitorialira92)
[![author](https://img.shields.io/badge/author-suellensr-cyan.svg)](https://github.com/suellensr)

## ⚖️ Licença

MIT - deve ser add na main http://escolhaumalicenca.com.br/licencas/mit/
"Permite as pessoas baixarem o projeto e modificar e autor não será responsabilizado por nada."

