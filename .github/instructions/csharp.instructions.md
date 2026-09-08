---
applyTo: "**/*.cs"
---
# Instructions sp\u00e9cifiques \u00e0 C#

- Utiliser `record` ou `record struct` pour les objets valeur plut\u00f4t que des types anonymes ou des classes nues.
- Pr\u00e9f\u00e9rer des `ArgumentException` / `KeyNotFoundException` explicites plut\u00f4t que de renvoyer `null` ou des sentinelles.
- La monnaie est en centimes `int`. Jamais `double`/`float`.
- Journaliser avec `Microsoft.Extensions.Logging`, jamais `Console.WriteLine`.
- Toute m\u00e9thode de plus de 30 lignes doit \u00eatre d\u00e9compos\u00e9e.
- Activer et respecter les types r\u00e9f\u00e9rence nullables ; annoter explicitement la nullabilit\u00e9.
