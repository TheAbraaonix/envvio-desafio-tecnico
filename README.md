# 🚗 Sistema de Gerenciamento de Estacionamento

> Desafio técnico para a vaga de Desenvolvedor Full Stack Júnior na **Envvio**

Sistema completo de gerenciamento de estacionamento com controle de entrada/saída de veículos, cálculo automático de valores e relatórios gerenciais.

---

## 📋 Índice

- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Arquitetura do Sistema](#-arquitetura-do-sistema)
- [Funcionalidades Implementadas](#-funcionalidades-implementadas)
- [Regras de Negócio](#-regras-de-negócio)
- [Pré-requisitos](#-pré-requisitos)
- [Como Executar](#-como-executar)
- [Estrutura de Pastas](#-estrutura-de-pastas)

---

## 🛠️ Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework Web API
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Banco de dados
- **AutoMapper** - Mapeamento objeto-objeto
- **FluentValidation** - Validação de dados

### Frontend
- **Angular 19** - Framework SPA
- **Angular Material** - Componentes UI
- **TypeScript** - Linguagem
- **RxJS** - Programação reativa
- **SCSS** - Estilização

---

## 🏗️ Arquitetura do Sistema

### Backend - Clean Architecture

O backend segue os princípios da **Clean Architecture**, garantindo separação de responsabilidades e independência de frameworks:

```
envvio-desafio-server/
├── ParkingManagement.Domain/          # Camada de Domínio
│   ├── Entities/                      # Entidades de negócio (Vehicle, ParkingSession)
│   ├── Interfaces/                    # Contratos de repositórios
│   └── Exceptions/                    # Exceções customizadas
│
├── ParkingManagement.Application/     # Camada de Aplicação
│   ├── DTOs/                          # Objetos de transferência de dados
│   ├── Services/                      # Lógica de negócio e casos de uso
│   ├── Interfaces/                    # Contratos de serviços
│   ├── Validators/                    # Validações com FluentValidation
│   └── Mappings/                      # Perfis do AutoMapper
│
├── ParkingManagement.Infrastructure/  # Camada de Infraestrutura
│   ├── Data/                          # DbContext e configurações
│   ├── Repositories/                  # Implementação de repositórios
│   └── Migrations/                    # Migrações do banco de dados
│
├── ParkingManagement.IoC/             # Injeção de Dependências
│   └── DependencyInjection.cs         # Configuração de DI
│
└── ParkingManagement.WebAPI/          # Camada de Apresentação
    ├── Controllers/                   # Endpoints da API REST
    ├── Middleware/                    # Middlewares (tratamento de erros)
    └── Program.cs                     # Configuração da aplicação
```

**Fluxo de Dependências:**
```
WebAPI → IoC → Application → Domain
              ↓
         Infrastructure → Domain
```

---

### Frontend - Feature-Based Architecture

O frontend utiliza **arquitetura baseada em features** com módulos NgModule (não standalone):

```
envvio-desafio-client/src/app/
├── core/                              # Funcionalidades singleton
│   ├── interceptors/                  # Interceptadores HTTP
│   ├── services/                      # Serviços globais
│   └── constants/                     # Constantes e configurações
│
├── shared/                            # Recursos compartilhados
│   └── utils/                         # Funções utilitárias
│       ├── date.utils.ts              # Formatação de datas/timezone
│       ├── currency.utils.ts          # Formatação de moeda (BRL)
│       └── vehicle.utils.ts           # Tradução de tipos de veículo
│
└── features/                          # Módulos de funcionalidades
    ├── vehicles/                      # Gestão de Veículos
    │   ├── components/
    │   ├── services/
    │   ├── models/
    │   └── vehicles.module.ts
    │
    ├── parking/                       # Operações de Estacionamento
    │   ├── components/
    │   ├── services/
    │   ├── models/
    │   └── parking.module.ts
    │
    └── reports/                       # Relatórios Gerenciais
        ├── components/
        ├── services/
        ├── models/
        └── reports.module.ts
```

**Características:**
- **Lazy Loading**: Módulos carregados sob demanda
- **Shared Utilities**: Funções puras para formatação sem duplicação de código
- **Error Translation**: Tradução automática de erros do backend para PT-BR
- **Timezone Handling**: Conversão automática UTC → BRT

---

## ✨ Funcionalidades Implementadas

### 🚘 Gestão de Veículos
- ✅ Cadastro de veículos (placa, modelo, cor, tipo)
- ✅ Edição de veículos (modelo, cor e tipo editáveis)
- ✅ Exclusão de veículos
- ✅ Listagem de veículos cadastrados
- ✅ Validação de placas brasileiras (formato antigo ABC1234 e Mercosul ABC1D23)
- ✅ Validação de placa única (não permite duplicatas)

### 🅿️ Operações de Estacionamento
- ✅ Registro de entrada de veículos
- ✅ Criação inline de veículo durante entrada (UX otimizada)
- ✅ Pré-visualização de saída com cálculo de valor
- ✅ Registro de saída com fechamento de sessão
- ✅ Monitoramento de veículos estacionados em tempo real
- ✅ Cálculo automático de duração (formato legível: "2h 30m")
- ✅ Validação: impede entrada duplicada do mesmo veículo
- ✅ Validação: impede saída de veículo não estacionado

### 📊 Relatórios Gerenciais
- ✅ **Receita por Dia**: Faturamento diário com totalizadores (7/15/30 dias)
- ✅ **Top Veículos**: Ranking por tempo total de estacionamento (5/10/20 veículos)
- ✅ **Ocupação por Hora**: Análise de ocupação horária (24h/3 dias/7 dias)
- ✅ Seletores de período personalizáveis
- ✅ Indicadores visuais (badges, alertas de alta ocupação)
- ✅ Formatação de moeda em Real (R$)

### 🎨 Experiência do Usuário (UX)
- ✅ Design System global (cores, espaçamento, tipografia)
- ✅ Interface 100% em Português (PT-BR)
- ✅ Material Design components
- ✅ Estados de loading, erro e vazio
- ✅ Mensagens de feedback (sucesso/erro) com snackbars
- ✅ Validações em tempo real com mensagens claras
- ✅ Acessibilidade: ARIA labels em botões
- ✅ Contraste de cores (WCAG compliance)
- ✅ Timezone automático (UTC → Horário Local)

---

## 💰 Regras de Negócio

### Cálculo de Preço de Estacionamento

O sistema implementa precificação **proporcional ao tempo** com **período de graça**:

#### Tabela de Preços
| Tempo de Permanência | Valor Cobrado |
|----------------------|---------------|
| ≤ 15 minutos         | **R$ 0,00** (Gratuito - Período de Graça) |
| > 15 min e ≤ 1 hora  | **R$ 5,00** (Taxa da primeira hora) |
| > 1 hora             | **R$ 5,00 + (horas adicionais × R$ 3,00)** |

#### Características
- ✅ **Cálculo Proporcional**: Minutos exatos são considerados
- ✅ **Período de Graça**: Permanências ≤ 15 minutos são gratuitas
- ✅ **Hora Adicional**: R$ 3,00 por hora após a primeira

#### Exemplos de Cálculo

```
┌─────────────────┬──────────────┬─────────────────────────────────┐
│ Tempo           │ Valor        │ Cálculo                         │
├─────────────────┼──────────────┼─────────────────────────────────┤
│ 10 minutos      │ R$ 0,00      │ Período de graça                │
│ 15 minutos      │ R$ 0,00      │ Período de graça (limite)       │
│ 20 minutos      │ R$ 5,00      │ Primeira hora                   │
│ 1 hora          │ R$ 5,00      │ Primeira hora                   │
│ 1h 30min        │ R$ 6,50      │ R$ 5,00 + (0,5 × R$ 3,00)      │
│ 2 horas         │ R$ 8,00      │ R$ 5,00 + (1 × R$ 3,00)        │
│ 2h 45min        │ R$ 10,25     │ R$ 5,00 + (1,75 × R$ 3,00)     │
│ 5 horas         │ R$ 17,00     │ R$ 5,00 + (4 × R$ 3,00)        │
└─────────────────┴──────────────┴─────────────────────────────────┘
```

### Validações de Placas

O sistema aceita apenas placas no **formato brasileiro válido**:

- **Formato Antigo**: `ABC1234` (3 letras + 4 números)
- **Formato Mercosul**: `ABC1D23` (3 letras + 1 número + 1 letra + 2 números)

**Exemplos Válidos**: ABC1234, XYZ9876, DEF1G23, JKL5E89  
**Exemplos Inválidos**: AAAAAAA, 1234567, AB12345, ABCD123

---

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET 8.0 SDK** ou superior ([Download](https://dotnet.microsoft.com/download))
- **Node.js 18+** e npm ([Download](https://nodejs.org/))
- **Angular CLI 19+**: `npm install -g @angular/cli`
- **SQLite** (opcional, já incluído no EF Core)

### Verificar Instalações

```bash
# Verificar .NET
dotnet --version

# Verificar Node.js
node --version

# Verificar npm
npm --version

# Verificar Angular CLI
ng version
```

---

## 🚀 Como Executar

### 1️⃣ Clonar o Repositório

```bash
git clone <url-do-repositorio>
cd envvio-desafio-tecnico
```

---

### 2️⃣ Configurar e Executar o Backend (.NET)

```bash
# Navegar para a pasta do servidor
cd envvio-desafio-server

# Restaurar dependências
dotnet restore

# Aplicar migrações do banco de dados (cria parking_management.db)
dotnet ef database update --project ParkingManagement.Infrastructure --startup-project ParkingManagement.WebAPI

# Executar a API
dotnet run --project ParkingManagement.WebAPI
```

✅ **Backend rodando em**: `http://localhost:7172`  
✅ **Swagger disponível em**: `http://localhost:7172/swagger/index.html`

---

### 3️⃣ Configurar e Executar o Frontend (Angular)

```bash
# Abrir um novo terminal
cd envvio-desafio-client

# Instalar dependências
npm install

# Executar em modo de desenvolvimento
npm start
```

✅ **Frontend rodando em**: `http://localhost:4200`

---

### 4️⃣ Acessar o Sistema

Abra seu navegador e acesse: **http://localhost:4200**

🎉 **Pronto!** O sistema está funcionando.

---

## 📂 Estrutura de Pastas

```
envvio-desafio-tecnico/
│
├── envvio-desafio-server/           # Backend (.NET 8.0)
│   ├── ParkingManagement.Domain/
│   ├── ParkingManagement.Application/
│   ├── ParkingManagement.Infrastructure/
│   ├── ParkingManagement.IoC/
│   ├── ParkingManagement.WebAPI/
│   └── parking_management.db        # Banco de dados SQLite (criado após migração)
│
├── envvio-desafio-client/           # Frontend (Angular 19)
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/
│   │   │   ├── shared/
│   │   │   └── features/
│   │   ├── styles.scss
│   │   └── index.html
│   ├── package.json
│   └── angular.json
│
├── escopo_projeto.txt               # Escopo original do desafio
└── README.md                        # Este arquivo
```

---

## 🎯 Endpoints Principais da API

### Veículos
- `GET /api/vehicles` - Listar todos os veículos
- `GET /api/vehicles/{id}` - Buscar veículo por ID
- `GET /api/vehicles/plate/{plate}` - Buscar veículo por placa
- `POST /api/vehicles` - Criar novo veículo
- `PUT /api/vehicles/{id}` - Atualizar veículo
- `DELETE /api/vehicles/{id}` - Excluir veículo

### Operações de Estacionamento
- `GET /api/parkingoperations/open-sessions` - Listar sessões abertas
- `POST /api/parkingoperations/entry` - Registrar entrada
- `GET /api/parkingoperations/exit-preview/plate/{plate}` - Pré-visualizar saída
- `POST /api/parkingoperations/exit` - Registrar saída

### Relatórios
- `GET /api/reports/revenue-by-day?days={days}` - Receita por dia
- `GET /api/reports/top-vehicles-by-parking-time?startDate={start}&endDate={end}&top={count}` - Top veículos
- `GET /api/reports/occupancy-by-hour?startDate={start}&endDate={end}` - Ocupação por hora

📚 **Documentação completa**: Acesse `/swagger` após iniciar o backend

---

## 👨‍💻 Desenvolvido por

Carlos Abraão - Candidato à vaga de Desenvolvedor Full Stack Júnior na Envvio

---

## 📝 Licença

Este projeto foi desenvolvido como parte de um processo seletivo e é destinado exclusivamente para fins de avaliação técnica.

