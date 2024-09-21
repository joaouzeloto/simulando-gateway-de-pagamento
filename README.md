 Gateway de Pagamento - 1º Bimestre

## Objetivo
Criar uma aplicação ASP.NET Core API que funcione como um gateway de pagamento, semelhante a serviços como Cielo e GetNet.

## Requisitos Técnicos

### Middleware de Log - Serilog
- Implementação do Serilog para gerenciamento de logs.
- Logs armazenados em arquivo local e no banco de dados MySQL, com limite de retenção de um dia.
- Erros capturados em blocos `catch` registrados nos logs.

### Estrutura do Código
- Organização em classes de serviço, ViewModels e entidades.
- Uso de injeção de dependência sempre que possível.
- Documentação da API utilizando Swagger para todos os endpoints.

## Requisitos da API

### Endpoints Implementados

1. **GET /cartoes/{cartao}/obter-bandeira**
   - Recebe o número do cartão e retorna sua bandeira (VISA, MASTERCARD, ELO) com base em regras fictícias.
   - Retorno: 200 + bandeira ou 404.

2. **GET /cartoes/{cartao}/valido**
   - Verifica a validade do cartão.
   - Retorno: booleano indicando validade.

3. **POST /pagamentos/calcular-parcelas**
   - Calcula o valor das parcelas de um pagamento.
   - Retorno: Lista de parcelas calculadas.

4. **POST /pagamentos**
   - Inicia o processo de pagamento com os detalhes (valor, cartão, CVV, parcelas).
   - Retorno: 201 com ID gerado ou BadRequest.

5. **GET /pagamentos/{id}/situacao**
   - Consulta a situação do pagamento pelo ID.
   - Retorno: situação do pagamento.

6. **PUT /pagamentos/{id}/confirmar**
   - Confirma o pagamento e muda a situação para "confirmado".
   - Retorno: 200 ou BadRequest.

7. **PUT /pagamentos/{id}/cancelar**
   - Cancela o pagamento, se ainda não confirmado.
   - Retorno: 200 ou BadRequest.
