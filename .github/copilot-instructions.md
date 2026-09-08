# Instructions Copilot — dépôt de démo GH-300 (édition .NET)

## Contexte du projet
Il s'agit d'un petit dépôt de démonstration servant à présenter les capacités de GitHub Copilot.
Il contient un module d'inventaire hérité adossé à SQLite et un module de panier d'achat,
tous deux volontairement imparfaits afin que Copilot ait un vrai travail à accomplir. C'est un portage
.NET 8 (C#) du kit de démo JavaScript + Python d'origine.

## Normes de codage
- C# : cibler .NET 8, activer les types référence nullables, préférer les `records` pour les objets valeur,
  lever des exceptions spécifiques plutôt que de renvoyer des valeurs sentinelles.
- Utiliser des espaces de noms à portée de fichier et `var` uniquement lorsque le type est évident.
- Chaque type et méthode public doit avoir un commentaire de documentation XML (`///`) expliquant les paramètres,
  la valeur de retour et les exceptions levées.
- La monnaie est toujours gérée en centimes entiers, jamais en `double`/`float`.

## Normes de test
- Les tests utilisent `xUnit`, se trouvent sous `tests/` et sont nommés `<Type>Tests.cs`.
- Chaque fichier de test doit couvrir : le cas nominal, au moins deux valeurs limites et un cas d'échec.
- Paramétrer les valeurs limites avec `[Theory]`/`[InlineData]` plutôt que de copier-coller les cas.
- Ne jamais écrire un test qui vérifie une méthode d'assistance privée.

## Attentes en matière de sécurité
- Ne jamais construire de commandes SQL ou shell par concaténation de chaînes ou avec `string.Format` ;
  utiliser des commandes paramétrées (`SqliteParameter`).
- Valider et normaliser toutes les entrées externes à la frontière.
- Ne jamais journaliser de secrets, de jetons ou de détails complets de paiement.

## Normes de revue
Lors d'une revue, ne commenter que la justesse, la sécurité et la clarté de l'API publique.
Ne pas commenter le formatage — il est pris en charge par le formateur / `dotnet format`.
