# TODO - TickBox

- [x] Ao criar uma child, ela não e referenciada na lista de ‘ids’ da parent, resolver isso.
- [x] Iniciar a criação assíncrona de forma primitiva.
- [x] Criar JSON formatado.
- [x] separar o content da Action para um markdown externo.
- [x] WARN - Tornar todo a classe TickBox Assíncrona
- [x] Criar função de load para reconstruir o projeto a partir dos dados.

## Próximos passos - Validações
Ta, vamos pensar aqui, eu vou focar agora nas validações e na somatória automática de tempo
nas Children, assim como nas Actions.

- [x] Limitar o tempo de trabalho as actions a 4 horas.
- [x] Definir o tempo das Children calculando entre o tempo da suas Actions.
- [ ] Verificar se todo o tempo das Children cabem no tempo da Parent.
- [ ] Retorno de erro (don't panic) para tempo.