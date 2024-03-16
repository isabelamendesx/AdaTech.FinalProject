# Motor de Aprovação - DiverseDev

(add logo ou Banner)

## Sobre

Este projeto foi desenvolvido como trabalho de conclusão do curso de back-end em c# DiverseDev da empresa Mecado Eletrônico em parceria com a Edtech ADA.

### Contexto do Desafio
Dentro do contexto de reembolso, parte crucial do controle de pagamento de reembolsos é o seu processo de aprovação. Essa etapa muitas vezes se revela trabalhosa, pois demanda que o departamento financeiro envie manualmente esse documento para o gestor, que em muitos casos essa informação não está clara e pode impactar negativamente na eficiência operacional.

Nossa missão foi criar uma API(**Motor de aprovação**) para automatizar o proceso de reembolso levando em conta os seguintes critérios:

1. Funcionalidade e efetividade da solução
2. Modelagem e estrutura de dados
3. Desenvolvimento e boas práticas
4. Segurança
5. Usabilidade e experiência do Usuário(UX)
6. Escalabilidade e Peformance
7. Manutenibilidade e Extensibilidade


## Tabela de Conteúdo

- [Sobre](#sobre)
- [Tabela de Conteúdo](#tabela-de-conteúdo)
- [Descrição do Projeto](#descrição-do-projeto)
- [Funcionalidades](#funcionalidades)
- [Pré-requisitos](#pré-requisitos)
- [Como Iniciar](#como-iniciar)
- [Segurança](#segurança)
- [Filtros](#filtros)
- [Endpoints](#endpoints)
- [Documentação](#documentação)
- [Autoras](#autoras)
- [Licença](#licença)

## Descrição do projeto
Para solucionar o desafio do controle de pagamento de reembolsos, desenvolvemos um motor de aprovação que automatiza o processo de decisão sobre a aprovação ou recusa de documentos de reembolso. Este motor é ativado automaticamente ao criar um novo documento de reembolso e segue uma tabela de decisão com critérios que podem ser pré-definidos e alterados de acordo com a necessidade da gestão financeira responsável, como por exemplo:

* Para valores de reembolso até R$100, todas as categorias são automaticamente aprovadas.
* Para valores entre R$101 e R$500, as categorias de alimentação e transporte são automaticamente aprovadas.
* Para valores acima de R$1.000, todas as categorias são automaticamente recusadas.
* Caso nenhuma dessas regras seja atendida, o status do documento é definido como "EM APROVAÇÃO", exigindo uma intervenção manual do departamento financeiro para decidir sobre a aprovação ou recusa.

**IMPORTANTE:** As regras de aprovação são facilmente alteradas por uma pessoa **autorizada**.

Para armazenar os dados relacionados aos documentos de reembolso e às regras de aprovação, utilizamos um banco de dados PostgreSQL hospedado no ElephantSQL, que oferece uma solução de banco de dados na nuvem confiável e escalável. Essa escolha permite o armazenamento seguro e eficiente dos dados, garantindo disponibilidade e performance para a nossa aplicação.

## Funcionalidades
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
  

## Pré-requisitos

## Como Iniciar

## Segurança

## Filtros

## Endpoints

## Documentação

## Autoras

Este projeto foi desenvolvido por:

[![author](https://img.shields.io/badge/author-AmandaaBastos-red.svg)](https://github.com/AmandaaBastos)
[![author](https://img.shields.io/badge/author-angelafrocha-green.svg)](https://github.com/angelafrocha)
[![author](https://img.shields.io/badge/author-isabelamendesx-pink.svg)](https://github.com/isabelamendesx)
[![author](https://img.shields.io/badge/author-vitorialira92-purple.svg)](https://github.com/vitorialira92)
[![author](https://img.shields.io/badge/author-suellensr-cyan.svg)](https://github.com/suellensr)

## Licença

