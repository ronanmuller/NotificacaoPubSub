# NotificacaoPubSub

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-7.0-blue)](https://dotnet.microsoft.com/)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()

---

## Descrição

O **NotificacaoPubSub** é uma API construída em **.NET Core 7+**, responsável por enviar notificações via **padrão Pub/Sub**.  

A API segue boas práticas de desenvolvimento:

- Injeção de dependências
- Middleware de **Correlation ID**
- Tratamento global de exceções
- Logging centralizado
- Documentação automática via **Swagger**
- Suporte a múltiplas versões de API

---

## Funcionalidades Principais

- Envio e listagem de notificações
- Integração com sistemas via Pub/Sub
- Health Checks (`/health`)
- Logging estruturado e rastreável
- Documentação Swagger UI (`/swagger`)

---

## Estrutura do Projeto

NotificacaoPubSub/
├── NotificacaoPubSub.Api/ # API principal
├── NotificacaoPubSub.Service/ # Lógica de negócio
├── NotificacaoPubSub.Domain/ # Entidades e modelos de domínio
├── NotificacaoPubSub.Tests/ # Testes unitários e integração
├── README.md
└── .gitignore


---

## Tecnologias Utilizadas

- **.NET Core 7+ / C#**
- **Swagger / OpenAPI**
- **Newtonsoft.Json** e **System.Text.Json**
- **Middleware customizado**
- **ILogger para logging**
- **HealthChecks**

---

## Pré-requisitos

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- Visual Studio 2022 (ou superior) / VS Code
- Git

---

## Como Rodar Localmente

1. **Clonar o repositório**

```bash
git clone https://github.com/ronanmuller/NotificacaoPubSub.git
cd NotificacaoPubSub
