#language: pt-br

# Critérios de aceitação

# É possível criar uma pizza sem personalização.
# É possível criar uma pizza com mais de uma personalização.
# Os valores e tempos adicionais devem ser somados no total do pedido.

Funcionalidade: Personalizar pizza
        Sendo um cliente​ ​do sistema da pizzaria UDS, eu quero personalizar a minha pizza. 
        O objetivo da personalização é tornar a minha pizza única.

Cenario: Personalizar pizza com adicionais
        Dado um cliente que montou sua pizza com tamanho e sabor
        E que personalizou seu pedido adicionando itens tornando única pizza
        Quando sistema salva a personalizacao do pedido
        Entao o pedido é salvo com todas as personalizações escolhidas bem como adicional de preço e tempo de preparo
        E os valores e tempos adicionais devem ser somados no total do pedido
 
Cenario: Personalizar pizza sem adicionais
        Dado um cliente que montou sua pizza com tamanho e sabor
        Quando sistema salva o pedido
        Entao o pedido é salvo sem adicionais
        E com o mesmo valor e tempo de preparo da montagem da pizza