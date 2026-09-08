---
mode: agent
description: G\u00e9n\u00e9rer une suite xUnit compl\u00e8te pour le type s\u00e9lectionn\u00e9.
---
G\u00e9n\u00e9rez une suite de tests `xUnit` pour le type que j'ai ouvert.

Respectez les normes du d\u00e9p\u00f4t d\u00e9finies dans `.github/copilot-instructions.md`.

Structurez la sortie ainsi :
1. Une courte liste des comportements que vous avez identifi\u00e9s, avant tout code.
2. Le fichier de test lui-m\u00eame, pr\u00eat \u00e0 \u00eatre enregistr\u00e9 sous `tests/`.
3. Un tableau des cas limites que vous avez d\u00e9lib\u00e9r\u00e9ment couverts et de ceux que vous avez jug\u00e9s hors p\u00e9rim\u00e8tre.

Exigences :
- Un `[Fact]` par comportement, nomm\u00e9 `<Comportement>`.
- Param\u00e9trer les valeurs limites avec `[Theory]`/`[InlineData]` plut\u00f4t que de copier-coller les cas.
- Inclure au moins un test qui v\u00e9rifie le type et le message de l'exception (`Assert.Throws`).
- Ne pas modifier le type test\u00e9.
