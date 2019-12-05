#language: pt-br

# Critérios de aceitação

# Só deve ser possível a escolha de uma pizza para o pedido.
# Não deve ser possível a escolha de meia pizza.

Funcionalidade: Montar pizza
        Sendo um cliente​ ​do sistema da pizzaria UDS, 
        eu quero montar uma pizza ideal de acordo com o meu gosto, 
        com objetivo de saciar a minha fome em um bom tempo de preparo e que caiba no meu bolso
 
Cenario: Montar pizza escolhendo tamanho e sabor
        Dado um cliente que escolheu tamanho e sabor da sua pizza
        Quando sistema monta a pizza
        Entao o sistema deve armazenar o pedido com o tempo de preparo bem como o valor final do pedido e os detalhes do produto
        E com a quantidade de uma pizza por pedido
        E sem a possibilidade de meia pizza