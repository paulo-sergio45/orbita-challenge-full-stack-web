# Comentários sobre a Arquitetura e Bibliotecas

## Decisão da Arquitetura Utilizada

### Visão Geral
Este projeto implementa uma arquitetura **Full-Stack** com separação clara entre frontend e backend, seguindo os princípios de **API REST** e **Single Page Application (SPA)**.

### Backend (.NET 9.0)
**Arquitetura:** ASP.NET Core Web API com padrão **Repository/Service**

#### Estrutura da Arquitetura:
- **Controllers** (`Controllers/v1/`): Camada de apresentação que expõe endpoints REST
- **Services** (`Services/`): Camada de lógica de negócio implementando interfaces
- **Interfaces** (`Interfaces/`): Contratos para injeção de dependência
- **Entities** (`Entities/`): Modelos de domínio (entidades do banco de dados)
- **Models** (`Models/`): DTOs (Data Transfer Objects) para entrada/saída de dados
- **DataContext** (`DataContext/`): Contexto do Entity Framework Core

#### Padrões Utilizados:
- **Dependency Injection**: Serviços registrados no container DI
- **Repository Pattern**: Abstração do acesso a dados através de interfaces
- **DTO Pattern**: Separação entre modelos de domínio e modelos de transferência
- **RESTful API**: Endpoints seguindo convenções REST
- **CORS**: Configurado para permitir comunicação com frontend

### Frontend (Vue.js 3)
**Arquitetura:** Single Page Application com Vue 3 Composition API

#### Estrutura da Arquitetura:
- **Components** (`components/`): Componentes reutilizáveis
- **Services** (`services/`): Camada de comunicação com API
- **Router** (`router/`): Configuração de rotas da aplicação
- **Assets** (`assets/`): Recursos estáticos

#### Padrões Utilizados:
- **Component-Based Architecture**: Componentes Vue reutilizáveis
- **Service Layer**: Abstração da comunicação com backend
- **Client-Side Routing**: Navegação sem recarregamento da página
- **State Management**: Gerenciamento de estado com Pinia

### Comunicação
- **HTTP/REST**: Comunicação entre frontend e backend via API REST
- **JSON**: Formato de dados para troca de informações
- **CORS**: Configurado para permitir comunicação cross-origin

## Lista de Bibliotecas de Terceiros Utilizadas

### Backend (.NET)

- **Microsoft.AspNetCore.OpenApi** (9.0.7): Geração automática de documentação OpenAPI
- **Microsoft.EntityFrameworkCore** (9.0.7): ORM para acesso a dados
- **Microsoft.EntityFrameworkCore.Design** (9.0.7): Ferramentas de design-time para EF Core
- **Microsoft.EntityFrameworkCore.InMemory** (9.0.7): Provider de banco em memória para testes

- **Npgsql.EntityFrameworkCore.PostgreSQL** (9.0.4): Provider para PostgreSQL

- **Scalar.AspNetCore** (2.6.3): Geração de documentação de API
- **xunit.extensibility.core** (2.9.3): Framework de testes unitários

### Frontend (JavaScript)

- **Vue** (^3.5.17): Framework JavaScript progressivo para construção de interfaces

- **vue-router** (^4.5.1): Roteamento oficial do Vue.js

- **pinia** (^3.0.3): Store de estado para Vue 3 (substitui Vuex)

- **vuetify** (3.9.0): Framework de componentes Material Design para Vue
- **@mdi/font** (5.9.55): Ícones Material Design
- **roboto-fontface** (*): Fonte Roboto do Google Fonts
- **webfontloader** (^1.0.0): Carregamento de fontes web

- **axios** (^1.10.0): Cliente HTTP para requisições à API

- **vite** (^7.0.0): Build tool e dev server moderno
- **@vitejs/plugin-vue** (^6.0.0): Plugin Vue para Vite

- **vitest** (^3.2.4): Framework de testes unitários
- **@vue/test-utils** (^2.4.6): Utilitários para testes de componentes Vue
- **jsdom** (^26.1.0): Implementação DOM para Node.js (usado em testes)

- **eslint** (^9.29.0): Linter para JavaScript/TypeScript
- **eslint-plugin-vue** (~10.2.0): Plugin ESLint para Vue
- **prettier** (3.5.3): Formatador de código
- **@vue/eslint-config-prettier** (^10.2.0): Configuração ESLint compatível com Prettier

- **vite-plugin-vue-devtools** (^7.7.7): Plugin para Vue DevTools
- **resize-observer-polyfill** (^1.5.1): Polyfill para ResizeObserver
- **npm-run-all2** (^8.0.4): Execução de múltiplos scripts npm


## O que você melhoraria se tivesse mais tempo

** Validações e Segurança:**
- **Validação de CPF**: Implementar validação real de CPF com algoritmo oficial
- **Validação de Email**: Verificação de domínio e formato mais robusta
- **Sanitização de dados**: Remover caracteres especiais e validar entrada
- **Rate Limiting**: Proteção contra ataques de força bruta
- **Autenticação e Autorização**: Implementar JWT ou OAuth2




### ** Todos os requisitos obrigatórios foram entregues:**
