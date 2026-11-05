# AgroSens

App mobile de monitoramento rural (disciplina PI1). Dashboard principal com mapa interativo da fazenda (tratores, implementos e porteiras).

## Mapa (Mapbox)

O mapa foi migrado do **Google Maps** para o **Mapbox** com estilo de satélite (`mapbox://styles/mapbox/satellite-streets-v12`, configurável pela constante `EstiloSatelite` em `MainActivity.cs`).

### 1. Obter os tokens

Acesse https://account.mapbox.com e crie:

- **Access token (público)** — "Access tokens" → "Create a token". Usado para carregar os estilos/tiles do mapa.
- **Downloads token (secreto)** — "Downloads tokens". Usado somente na compilação para baixar o SDK nativo do Mapbox do repositório Maven privado.

### 2. Configurar o ambiente

Copie `.env.example` para `.env` e preencha com os seus valores (o `.env` é ignorado pelo git):

```bash
cp .env.example .env
```

O token de acesso ao mapa é lido da variável de ambiente `MAPBOX_ACCESS_TOKEN` no `MainActivity.cs`:

```csharp
MapboxOptions.AccessToken = Environment.GetEnvironmentVariable("MAPBOX_ACCESS_TOKEN") ?? string.Empty;
```

Defina `MAPBOX_ACCESS_TOKEN` no ambiente antes de executar (ou adicione ao `.env` e carregue com o seu gerenciador de variáveis de ambiente).

### 3. Compilar o Android (requer Downloads token)

O binding `Com.Mapbox.Maps.Android` baixa o SDK nativo do Mapbox de um repositório Maven privado. É necessário autenticar o Gradle com o `MAPBOX_DOWNLOADS_TOKEN`.

Adicione ao `~/.gradle/gradle.properties` (ou exporte como variável de ambiente):

```properties
MAPBOX_DOWNLOADS_TOKEN=sk.seu_token_de_download_do_mapbox_aqui
```

### 4. Compilar e rodar

```bash
dotnet build Agrosens/Agrosens.csproj
dotnet run --project Agrosens/Agrosens.csproj
```

### Arquivos alterados na migração

- `Agrosens.csproj` — remove `Xamarin.GooglePlayServices.Maps`, adiciona `Com.Mapbox.Maps.Android`.
- `AndroidManifest.xml` — remove `meta-data com.google.android.geo.API_KEY`; mantém permissões de localização.
- `Resources/layout/fragment_map.xml` — `com.mapbox.maps.MapView` no lugar do `MapView` do Google.
- `MainActivity.cs` — `MapboxMap` no lugar de `GoogleMap`; estilo satélite; ciclo de vida do `MapView`; token lido de `MAPBOX_ACCESS_TOKEN`; marcadores (tratores/implementos/porteiras), câmera inicial e métodos de atualização de posições.
- `.gitignore` — ignora `bin/`, `obj/`, `.env`, `local.properties`.
- `.env.example` — exemplo de variáveis (valores fictícios).
- `README.md` — instruções de token e compilação.

### Passos manuais pendentes

1. Criar os tokens em https://account.mapbox.com (access token + downloads token).
2. Preencher `.env` e definir `MAPBOX_ACCESS_TOKEN` no ambiente.
3. Configurar `MAPBOX_DOWNLOADS_TOKEN` em `~/.gradle/gradle.properties` para compilar o Android.
4. Compilar com `dotnet build` e ajustar, se necessário, assinaturas exatas do binding `Com.Mapbox.Maps.Android` (construtor de `CameraOptions` e criação do gerenciador de anotações).
