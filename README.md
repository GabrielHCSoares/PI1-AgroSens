# PI1-AgroSens
Repositório para a matéria de Projeto Integrado 1 - Desenvolvimento Móvel (2026/2)

## Nome do Projeto

AgroSens

## Componentes do Time

- Evaldo Lemos Servulo - Desenvolvedor
- Gabriel Henrique Costa Soares - Desenvolvedor
- João Francisco Machado Cardoso - Desenvolvedor

## Nossa Proposta de Desenvolvimento

### Objetivo do app

O AgroSens é um aplicativo de monitoramento rural que trabalha com a integração de hardware para facilitar o gerenciamento de propriedade. O objetivo principal é dar ao agricultor o controle centralizado de suas terras e infraestrutura em tempo real, reduzindo perdas operacionais, rastreando equipamentos, garantindo o bem-estar dos animais de diferentes portes e ajudando com o gerenciamento em geral.

Ele servirá para rastrear maquinários e implementos, monitorar a segurança através dos status de diferentes tipos de porteiras (como acessos principais, pastos de gado e cercados de porcos), e gerenciar os níveis de recursos hídricos e alimentares, transformando dados físicos coletados por sensores em informações logísticas para o usuário.

### Público-alvo do app

O público-alvo do AgroSens são:

- proprietários de fazenda;
- administradores rurais;
- gestores logísticos do campo.

### Principais funcionalidades

- **Rastreamento de Frota e Implementos:**

  Localização GPS em tempo real de tratores e colheitadeiras, além do monitoramento de implementos acoplados (arados, carretas) através de sensores específicos.

- **Monitoramento de Garagens:**

  Sensores de presença que indicam em tempo real se o maquinário está guardado no galpão ou operando no campo.

- **Controle de Acessos e Cercados:**

  Sensores instalados nas diversas porteiras da propriedade com categorização específica para pastos de gado, áreas de criação de porcos, acessos gerais, ou outros para indicar o status (aberta/fechada). Isso previne a fuga de animais, misturas indesejadas de rebanhos e falhas na segurança.

- **Gestão Hídrica e Alimentar:**

  Leitura em tempo real do nível de água em caixas d'água e bebedouros, além do monitoramento do volume de comida armazenada em cochos e silos para o rebanho e equinos.

- **Calendário Integrado:**

  O AgroSens contará com um calendário integrado no aplicativo que facilitará a gestão de atividades dentro e fora da fazenda, como estimativa de colheitas, visitas de fiscais e outros, que poderão ser editadas e planejadas pelo usuário da forma que quiser.

## Rascunho das telas do App

### Tela 1

**Dashboard Principal:** Tela inicial contendo um mapa interativo da fazenda com pinos indicando a posição atual dos tratores e implementos. Na parte superior, cards de alerta rápido (ex: "Porteira do Gado Aberta", "Cercado dos Porcos Destravado" ou "Nível crítico no bebedouro central").

### Tela 2

**Gestão de Maquinário:** Lista detalhada da frota. Ao selecionar um trator, exibe o status de garagem (guardado/fora) e detalha exatamente qual implemento está conectado a ele naquele momento.

### Tela 3

**Recursos e Estoque:** Painel com gráficos em barra demonstrando a volumetria atual das caixas d'água e galpões de ração, além de indicadores mostrando se os animais têm água e comida suficientes nos pastos.

### Tela 4

**Central de Sensores (IoT):** Tela técnica para cadastro e gerenciamento dos dispositivos de hardware. Permite vincular uma tag GPS a um novo trator ou calibrar os sensores de nível instalados nos cochos e monitorar o status dos sensores em cada porteira (gado, porcos etc.).

### Tela 5

**Calendário interativo:** Uma tela com um calendário em que o usuário pode editar seus eventos importantes, lembretes, dias em que fez plantio ou colheita de seus produtos ou outras informações que julgar importantes de serem colocadas. O aplicativo então ajudará o produtos com lembretes para as datas e eventos que estiverem marcados.
