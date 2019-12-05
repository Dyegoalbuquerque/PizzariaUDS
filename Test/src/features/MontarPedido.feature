#language: pt-br

# Critérios de aceitação

# Todos os valores devem ser discriminados no resumo do pedido.
# Somente exibir o tempo total de preparo no resumo do pedido.

Funcionalidade: Montar pedido
        Sendo um cliente​ ​do sistema da pizzaria UDS, eu quero visualizar os detalhes do meu pedido e 
        saber o preço final e o tempo de preparo. 
        O objetivo é saber o quanto irei gastar e em quanto tempo a pizza ficará pronta.
 
Cenario: Montar pedido para visualizar os detalhes do pedido
        Dado um cliente que deseja visualizar os detalhes do pedido montado anteriormente
        Quando sistema consulta o pedido
        Entao o sistema apresenta o resumo do pedido listando sabor da pizza escolhida juntamente com o preço
        E a lista de personalização que foi selecionado e seu valor
        E o valor total do pedido e seu tempo de preparo