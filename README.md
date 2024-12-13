# LDI - Integração de Dispositivos (Tuya e Home Assistant)

## Descrição

Este projeto tem como objetivo integrar sistemas de dispositivos IoT, especificamente os dispositivos Tuya e o Home Assistant, utilizando a arquitetura Clean (Clean Architecture). A aplicação oferece uma interface de API RESTful que permite interagir com dispositivos de IoT, obter informações sobre o status e controlar dispositivos.

Através dessa integração, é possível:

- **Autenticação** com a API Tuya para obter tokens de acesso e interagir com os dispositivos.
- **Obtenção de Dispositivos** da API Tuya e do Home Assistant.
- **Controle de Dispositivos** no Home Assistant, como ligar/desligar luzes e outros dispositivos.
- **Consulta de Estado de Dispositivos** em ambas as plataformas.

## Arquitetura

O projeto foi desenvolvido seguindo os princípios da **Clean Architecture**, que é um estilo arquitetural que visa promover a **manutenibilidade**, **testabilidade** e **desacoplamento** entre as diferentes camadas da aplicação. A arquitetura foi estruturada da seguinte forma:

### Camadas

1. **Camada de Apresentação (API)**:
   - Esta camada é responsável pela exposição dos endpoints da aplicação.
   - Utiliza o **ASP.NET Core** para criar a API RESTful, oferecendo aos usuários a possibilidade de interagir com os dispositivos IoT.
   - A API possui controladores (Controllers) que chamam os casos de uso da aplicação (Serviços), executando a lógica de negócios.

2. **Camada de Aplicação**:
   - A camada de aplicação contém a **lógica de negócios** da aplicação. Ela é composta por casos de uso (Use Cases) que definem as ações que a aplicação pode realizar, como obter dispositivos, autenticar na API Tuya, controlar dispositivos no Home Assistant, etc.
   - Os casos de uso comunicam-se com a camada de domínio e infraestrutura para realizar suas ações, mas não sabem como os dados são armazenados ou acessados.

3. **Camada de Domínio**:
   - A camada de domínio é o coração do sistema e contém **entidades**, **interfaces** e **regras de negócios**.
   - As entidades representam os dados da aplicação, como `Dispositivo`, `Entidade` e `Resultado`.
   - As interfaces são usadas para abstrair a comunicação com a camada de infraestrutura e com os serviços externos.

4. **Camada de Infraestrutura**:
   - A camada de infraestrutura é responsável pela implementação dos detalhes de comunicação com serviços externos e bancos de dados.
   - Nessa camada estão implementados os **serviços de integração** com as APIs do **Tuya** e **Home Assistant**.
   - Além disso, a camada de infraestrutura lida com a comunicação HTTP, manipulação de tokens, controle de dispositivos, etc.

### Principais Tecnologias Utilizadas

- **.NET Core 8**: Framework principal utilizado para o desenvolvimento da aplicação.
- **ASP.NET Core**: Para criação da API RESTful.
- **Clean Architecture**: Arquitetura utilizada para organizar o código de forma desacoplada e testável.
- **Tuya API**: Integração com a API do Tuya para controle de dispositivos IoT.
- **Home Assistant API**: Integração com o Home Assistant para controle de dispositivos domésticos inteligentes.
- **Flunt**: Biblioteca utilizada para validação de entradas e regras de negócio.
- **Dependency Injection**: Usada para injeção de dependências, promovendo a flexibilidade e testabilidade do sistema.

## Estrutura do Projeto

O projeto segue a estrutura de diretórios organizada para Clean Architecture:




## Funcionalidades

### 1. **Autenticação Tuya**

- **Endpoint**: `POST /api/tuya/autenticar`
- **Descrição**: Realiza a autenticação na API Tuya e retorna um token de acesso que será usado para interagir com os dispositivos.
- **Parâmetros**: `accessId` e `accessSecret` (fornecidos pela plataforma Tuya).

### 2. **Obter Dispositivos Tuya**

- **Endpoint**: `GET /api/tuya/devices`
- **Descrição**: Retorna uma lista de dispositivos disponíveis na conta Tuya.
- **Autenticação**: Requer que o token de autenticação tenha sido obtido previamente.

### 3. **Obter Estado da Entidade no Home Assistant**

- **Endpoint**: `GET /api/homeassistant/estado/{entidadeId}`
- **Descrição**: Retorna o estado de uma entidade específica no Home Assistant.
- **Parâmetros**: `entidadeId` (ID da entidade a ser consultada).

### 4. **Controlar Dispositivo no Home Assistant**

- **Endpoint**: `POST /api/homeassistant/servico/{dominio}/{servico}`
- **Descrição**: Chama um serviço no Home Assistant (ex: ligar luz, desligar ventilador).
- **Parâmetros**: `dominio` e `servico` (ex: `light` e `turn_on`), e um `payload` opcional com parâmetros do serviço.

### 5. **Obter Entidades do Home Assistant**

- **Endpoint**: `GET /api/homeassistant/entidades`
- **Descrição**: Retorna todas as entidades disponíveis no Home Assistant.

## Como Rodar o Projeto

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/LuizBritotk/LeitorDispositivosInteligentes.git
   cd LeitorDispositivosInteligentes

2. **Instale as dependências:**:
   ```bash
   dotnet restore

3. **Execute o projeto:**:
   ```bash
   dotnet run --project LeitorDispositivosInteligentes.Interface
4. **EAcesse a API:**:
- A API estará disponível no endereço http://localhost:5000.
- Você pode testar os endpoints utilizando o Postman ou ferramentas similares.

Contribuições
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests. Certifique-se de que seus testes estão passando e que o código segue as convenções do projeto.

Licença
Este projeto está licenciado sob a Licença MIT - veja o arquivo LICENSE para mais detalhes.
