# AgroSens

App mobile de monitoramento rural (disciplina PI1). Abas: Geral, Calendário, Mapa, Perfil.

## Mapa (WebView + Leaflet + OpenStreetMap)

A aba "Mapa" exibe um mapa interativo da propriedade usando WebView com a biblioteca Leaflet e tiles do OpenStreetMap. Não há dependência do Mapbox nem necessidade de access token.

### Funcionalidades do mapa

- Mapa arrastável com controles de zoom do Leaflet e atribuição do OpenStreetMap.
- Abre na posição padrão (-15.7801, -47.9292, zoom 13) quando não há localização salva.
- Abre na localização salva (`farm_lat`, `farm_lng`, `farm_zoom`) quando existir, persistindo entre reinicializações do app (incluindo desligar o celular).
- Card "Resumo da propriedade" exibe a linha "Localização salva: <lat>, <lng>" com 4 casas decimais (formato invariante) ou "não definida".

### Fluxo de salvar localização (dois modos)

**Modo Normal** (padrão ao abrir a aba):
- Mapa limpo: apenas controles de zoom e atribuição do Leaflet.
- Cabeçalho com título "Mapa da propriedade" (linha 1, largura total) e botão verde arredondado "Salvar localização" (linha 2, ícone de pin 18dp, texto 13sp, maxLines 1).
- Se houver localização salva, aparece também o botão "Ir para minha fazenda" (estilo contorno verde, ícone de alvo 18dp) ao lado, que recentra o mapa no ponto salvo.

**Modo de Seleção** (ao tocar em "Salvar localização"):
- Aparece a mira central no centro exato do WebView (não bloqueia arrasto) e a dica "Arraste o mapa até a sua fazenda e toque em Confirmar" em faixa sobre o topo do mapa.
- O cabeçalho mostra os botões "Confirmar" (verde) e "Cancelar" (contorno cinza) na mesma linha.
- Cancelar: esconde mira e dica, volta ao modo normal sem salvar.
- Confirmar: obtém o centro via `EvaluateJavascript` (`getCenter()`/`getZoom()`), parse com `CultureInfo.InvariantCulture`, valida lat [-90, 90] e lng [-180, 180], pede confirmação para substituir se já existir, grava `farm_lat`/`farm_lng`/`farm_zoom` em `ISharedPreferences` (arquivo `agrosens_map`) com precisão completa (strings invariantes), exibe toast "Localização da fazenda salva" e atualiza o card. Se a leitura falhar, exibe toast "Não foi possível salvar a localização" e registra `Log.Error("AgroSensMap", ...)`.

### Persistência

A localização da fazenda é salva em `ISharedPreferences` (chaves `farm_lat`, `farm_lng`, `farm_zoom`, arquivo `agrosens_map`) e nunca é limpa por reset de sensores ou de dados.

## Compilar

```bash
dotnet build Agrosens/Agrosens.csproj
dotnet run --project Agrosens/Agrosens.csproj
```

Requer SDK do Android (ANDROID_HOME) e o workload do .NET para Android instalados.

## Arquivos principais

- `Agrosens/MainActivity.cs` — navegação por abas, WebView/Leaflet, fluxo de salvar localização, persistência da fazenda.
- `Agrosens/Resources/layout/fragment_map.xml` — layout da aba Mapa (título em duas linhas, linha de ações, WebView, card Resumo da propriedade).
- `Agrosens/Resources/drawable/bg_btn_green_outline.xml` — fundo contorno verde para "Ir para minha fazenda" e "Cancelar".
- `Agrosens/Services/DatabaseService.cs` — banco SQLite (tarefas, eventos, sensores).
- `Agrosens/Services/PreferenceService.cs` — sessão de usuário (login/nome/e-mail).

## Layout da aba Mapa

O layout é fixo, sem rolagem de tela:

1. Título "Mapa da propriedade" (linha 1, 20sp, negrito, verde escuro, maxLines 1).
2. Linha de ações (botões "Salvar localização" / "Confirmar" e opcionalmente "Ir para minha fazenda" / "Cancelar"), width 0 + weight 1, ellipsize + maxLines 1.
3. Mapa (WebView) com `layout_weight="1"`, ocupando o espaço restante.
4. Card "Resumo da propriedade" ancorado embaixo.
