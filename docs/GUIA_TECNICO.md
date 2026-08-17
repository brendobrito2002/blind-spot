# Blind Spot — Guia técnico resumido

**Formato:** jogo 2D isométrico com visão de cima  
**Plataforma inicial:** Windows, teclado e mouse  
**Tecnologia:** Unity 6, C#, URP 2D, Input System e Physics 2D

## 1. Conceito principal

O protagonista é cego e participa de um gameshow clandestino. A exploração depende de uma habilidade que altera temporariamente sua área de visão.

### Funcionamento da visão

1. O jogador normalmente enxerga apenas uma área pequena ao redor do personagem.
2. Ao usar a habilidade, essa área se expande como um pulso.
3. A visão permanece ampliada por poucos instantes.
4. Depois, ela diminui automaticamente até voltar ao tamanho normal.
5. A habilidade entra em cooldown antes de poder ser usada novamente.

```text
Visão pequena → ativa habilidade → visão expande
       ↑                              ↓
       └──── visão volta ao normal ← espera curta
```

A habilidade não deixa o mapa revelado permanentemente. Seu objetivo é permitir que o jogador observe, memorize e continue se movimentando com informação limitada.

Parâmetros que devem ser ajustáveis no Inspector:

- raio da visão normal;
- raio máximo da habilidade;
- tempo de expansão;
- tempo com visão ampliada;
- tempo para voltar ao normal;
- cooldown;
- intensidade/cor da luz;
- efeito sonoro.

## 2. Referência da pasta `Assets`

A pasta enviada combina com a direção do projeto e pode ser usada como referência para:

- movimento 2D e animações direcionais;
- câmera com visão de cima;
- cenário construído com Tilemaps;
- Light2D para representar a visão;
- pulso com raio, duração e cooldown;
- transição entre áreas.

O conceito de `PulseAbility` separado de `PulseVisualEffect` deve ser mantido. Porém, a implementação precisa considerar que existe uma **visão pequena sempre ativa**; o pulso apenas amplia essa visão temporariamente.

Não copiar toda a pasta diretamente. Trazer apenas os arquivos úteis e revisar nomes, referências e configurações.

## 3. Organização do projeto

Estrutura inicial:

```text
Assets/_Project/
├── Art/
│   ├── Animations/
│   ├── Sprites/
│   ├── Tilesets/
│   └── VFX/
├── Audio/
├── Prefabs/
├── Scenes/
│   ├── Levels/
│   └── Tests/
├── Scripts/
│   ├── Player/
│   ├── Radar/
│   ├── Puzzles/
│   ├── Hazards/
│   └── Level/
└── Settings/
```

Tudo criado pela equipe fica em `_Project`. Cenas de teste devem ficar separadas das fases.

## 4. Nomes

Usar inglês no código e nos arquivos.

| Item | Padrão | Exemplo |
| --- | --- | --- |
| Classe e método | `PascalCase` | `PlayerVision`, `ExpandVision()` |
| Campo privado | `camelCase` | `normalVisionRadius` |
| Interface | prefixo `I` | `IRevealable` |
| Cena | `LVL_` ou `TST_` | `LVL_Prototype` |
| Prefab | `PF_` | `PF_Player` |
| Áudio | `SFX_`, `AMB_`, `VO_` | `SFX_VisionPulse` |

Evitar espaços, acentos e nomes como `Final2` ou `NewScript`.

## 5. Cena e Tilemaps

Hierarquia sugerida:

```text
LVL_Prototype
├── _Grid
│   ├── Ground
│   ├── Walls
│   ├── Collision
│   ├── DecorBehind
│   └── DecorFront
├── _Gameplay
├── _Lighting
├── _Audio
├── _PlayerSpawn
└── _Debug
```

Sorting Layers, do fundo para a frente:

1. `Background`
2. `Ground`
3. `DecorBehind`
4. `Actors`
5. `DecorFront`
6. `VFX`
7. `UI`

O ponto de ordenação dos personagens deve ficar nos pés para funcionar corretamente no cenário isométrico.

Physics Layers iniciais:

- `Player`;
- `Environment`;
- `Interactable`;
- `Revealable`;
- `Hazard`;
- `Trigger`;
- `UI`.

## 6. Sistemas principais

```text
Player
├── PlayerMovement
├── PlayerAnimation
├── PlayerVision
└── PlayerInteraction

Gameplay
├── Puzzles
├── Hazards
├── Doors
└── LevelFlow
```

### Visão e habilidade

Separar em três partes:

- `PlayerVision`: controla o raio atual, os tempos e o cooldown;
- efeito visual: altera a Light2D e apresenta a expansão;
- objetos reveláveis: reagem quando alcançados pela visão ampliada.

Regras:

- a visão normal nunca deve apagar completamente;
- o raio sempre retorna ao valor normal depois da habilidade;
- usar uma única rotina de expansão por vez;
- uma nova ativação não pode deixar a visão presa no raio máximo;
- efeito visual e som devem acompanhar o mesmo tempo;
- tiles afetados precisam usar material compatível com Light2D;
- objetos especiais podem ser encontrados com `Physics2D.OverlapCircle` e uma LayerMask.

### Comunicação

- chamada direta entre componentes do mesmo prefab;
- interface para objetos que podem ser revelados ou receber dano;
- evento para avisar que um puzzle foi resolvido e abrir uma porta.

Evitar buscas por nome e um `GameManager` responsável por tudo.

## 7. Primeiro protótipo

Deve conter:

- personagem com movimento e animação em quatro direções;
- câmera seguindo o jogador;
- visão normal pequena;
- habilidade que amplia e depois reduz a visão;
- cooldown e efeito sonoro;
- uma sala isométrica simples;
- um objeto revelável;
- um puzzle simples;
- uma porta;
- uma armadilha;
- uma saída.

Não deve conter:

- arte final completa;
- várias fases;
- combate complexo;
- inventário ou save;
- multiplayer;
- cutscenes longas;
- sistemas que não sejam necessários para testar o loop principal.

O protótipo está validado quando outra pessoa consegue explorar e terminar a sala, entendendo que a habilidade amplia a visão somente por um curto período.

## 8. Fluxo de trabalho

```text
Planejar → criar blockout → implementar gameplay
→ testar → aplicar arte → testar novamente
```

Uma tarefa está concluída quando:

- funciona conforme o critério definido;
- foi testada em Play Mode;
- não cria erros no Console;
- não possui referência quebrada;
- segue os nomes e pastas do projeto;
- foi revisada por outra pessoa quando altera cena ou prefab compartilhado.

Para registrar bugs, informar: cena, passos para reproduzir, resultado atual, resultado esperado e imagem ou vídeo quando possível.

## 9. Git

- `main` deve permanecer jogável;
- criar branches curtas, como `feat/vision-pulse` e `fix/door-trigger`;
- versionar arquivos `.meta` junto com os assets;
- não versionar `Library`, `Temp`, `Logs`, `Obj` ou builds;
- evitar duas pessoas editando a mesma cena ao mesmo tempo;
- mover ou renomear assets dentro do Unity.

## 10. Próximos passos

- [ ] Definir os valores iniciais dos raios e tempos da visão.
- [ ] Testar a profundidade isométrica e as Sorting Layers.
- [ ] Organizar os arquivos úteis da pasta de referência.
- [ ] Criar `TST_Player`, `TST_Vision` e `LVL_Prototype`.
- [ ] Montar a sala do protótipo em blockout.
- [ ] Testar se a visão ampliada fornece tempo suficiente para observar e memorizar o ambiente.
