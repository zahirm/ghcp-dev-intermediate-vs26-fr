# Kit de démo Copilot GH-300 — édition .NET 8

Un kit compact et autonome pour démontrer les capacités de GitHub Copilot pendant
la préparation au GH-300.

- Porté vers **.NET 8 (C#)** et adapté pour **Visual Studio 2026**.
- Fonctionne aussi dans **VS Code** avec le C# Dev Kit.
- Les deux modules sources sont **volontairement imparfaits** — un bogue de décalage d'un cran, un
  numéro de carte journalisé, une injection SQL, aucune documentation et (presque) aucun test — afin que Copilot ait un vrai
  travail à accomplir lors des démos `/fix`, `/tests`, revue, refactorisation et modernisation.

---

## 📁 Disposition de la solution

```
CopilotDemoKit.sln
├─ src/
│  ├─ ShoppingCart/        ShoppingCart\Cart.cs            (portage de cart.js)
│  ├─ InventoryLegacy/     InventoryLegacy\InventoryLegacy.cs (portage de inventory_legacy.py)
│  └─ DemoApp/             Program.cs — pilote qui exerce les deux modules
├─ tests/
│  └─ ShoppingCart.Tests/  volontairement minimal — le manque « aucun test » pour la démo /tests
├─ .github/
│  ├─ copilot-instructions.md
│  ├─ instructions/csharp.instructions.md
│  └─ prompts/{security-review,unit-tests}.prompt.md
├─ .vscode/{settings.json,mcp.json}
├─ admin/content-exclusion-example.yml
└─ global.json  (épingle le SDK .NET 8)
```

---

## ✅ Prérequis

- SDK .NET 8 (`dotnet --list-sdks` doit afficher un `8.0.x` ; `global.json` l'épingle).
- Visual Studio 2026 avec les composants GitHub Copilot et Copilot Chat, **ou** VS Code + C# Dev Kit.

---

## 🛠️ Compiler et exécuter

```powershell
dotnet build
dotnet run --project src/DemoApp
dotnet test
```

> L'application de démo enveloppe `Checkout` dans un try/catch afin que le bogue de décalage d'un cran apparaîsse comme une
> exception capturée au lieu de faire planter la présentation.

---

## 📦 Fichiers de ce kit

| Chemin | Ce qu'il démontre |
| --- | --- |
| `src/ShoppingCart/Cart.cs` | Bogue de décalage d'un cran, numéro de carte journalisé, monnaie en `double`, aucun test — suggestions en ligne, `/fix`, revue, tests |
| `src/InventoryLegacy/InventoryLegacy.cs` | Injection SQL, aucun type/documentation — modernisation, refactorisation, sécurité, documentation |
| `.github/copilot-instructions.md` | Instructions personnalisées à l'échelle du dépôt |
| `.github/instructions/csharp.instructions.md` | Instructions ciblées par chemin via `applyTo` |
| `.github/prompts/*.prompt.md` | Fichiers de prompt réutilisables pour des réponses cohérentes |
| `.vscode/settings.json` | Activation de Copilot par langage dans VS Code (dans Visual Studio, utilisez **Outils > Options > GitHub > Copilot**) |
| `.vscode/mcp.json` | Serveurs MCP pour le mode agent (Visual Studio découvre aussi `.mcp.json` / `.vs\mcp.json`) |
| `admin/content-exclusion-example.yml` | Syntaxe d'exclusion de contenu |

---

## 🅰️ Partie A — Utiliser GitHub Copilot dans l'IDE

### 1️⃣ Démo 1 — Activer et cadrer Copilot (2 min)

- Ouvrez **Outils > Options > GitHub > Copilot** et parcourez les paramètres Copilot (activation globale, complétions, Next Edit Suggestions).
- Utilisez le **badge Copilot** en haut à droite de l'IDE pour afficher/basculer l'état de Copilot pour la session en cours.
- Ouvrez un type de fichier où vous ne voulez pas de complétions et montrez que l'activation est contrôlée ici, pas fichier par fichier.

> **Point de discussion :** dans Visual Studio, l'activation se gère via **Outils > Options > GitHub > Copilot** et le badge Copilot — pas via un fichier `.vscode/settings.json` (ce mappage est un mécanisme de VS Code).

---

### 2️⃣ Démo 2 — Suggestions en ligne et Next Edit Suggestions (3 min)

- Dans `src/ShoppingCart/Cart.cs`, placez le curseur à la fin de la classe `ShoppingCartModule` (après `Cart.cs:58`, avant l'accolade fermante `}` sur `Cart.cs:59`) et tapez :

  ```csharp
  // Returns the cart total in integer cents, including tax
  public static int TotalWithTax(Cart cart, double taxRate)
  ```

- Acceptez avec `Tab`, faites défiler les alternatives avec `Alt+.` / `Alt+,`, et acceptez mot par mot avec `Ctrl+->` (ligne par ligne avec `Ctrl+Down`).
- Assurez-vous que Next Edit Suggestions est activé (**Outils > Options > GitHub > Copilot > Copilot Completions > Enable Next Edit Suggestions**), puis renommez la variable `total` dans `Subtotal` (déclarée sur `Cart.cs:40`, utilisée sur `Cart.cs:43` et `Cart.cs:45`) en `runningTotal`, et appuyez sur `Tab` pour naviguer/accepter les modifications suivantes afin que les trois références soient mises à jour ensemble.

> **Point de discussion :** les complétions en texte fantôme, l'acceptation partielle et le NES conscient des modifications sont des fonctionnalités distinctes ; NES utilise `Tab` pour sauter vers la prochaine modification prédite et l'accepter.

---

### 3️⃣ Démo 3 — Chat en ligne et panneau de chat (4 min)

- Sélectionnez la méthode `Subtotal` (`Cart.cs:38-46`), clic droit > **Ask Copilot** (ou utilisez le chat en ligne), et demandez :

  > Pourquoi cela lève-t-il une erreur à la dernière itération ? Corrige-le en conservant la même signature.

  (Le décalage d'un cran est la borne de boucle `i <= cart.Items.Count` sur `Cart.cs:41`.)

- Ouvrez ensuite la fenêtre de chat (**View > GitHub Copilot Chat**) et exécutez, dans l'ordre :
  - `/explain` sur la sélection
  - `/fix` sur `Checkout` (`Cart.cs:53-58`)
  - `/tests` pour `Cart.cs`
  - `@workspace où le pourcentage de remise est-il appliqué ?` (`ApplyDiscount` sur `Cart.cs:48-50`, appelé depuis `Checkout` sur `Cart.cs:55`)
  - `#Cart.cs résume l'API publique dans un tableau`

> **Point de discussion :** les briques du chat dans Visual Studio sont les commandes slash (`/explain`, `/fix`, `/tests`, `/doc`, `/optimize`), le participant `@workspace` pour le contexte de la solution (plus `@github` en Enterprise), et les références `#` pour les fichiers, méthodes et classes (par exemple `#Cart.cs`, `#Subtotal`). Soulignez les limites : le chat a une fenêtre de contexte bornée et des limites de débit par plan, et vous pouvez compacter la conversation pour libérer de l'espace.

---

### 4️⃣ Démo 4 — Modifications multi-fichiers en mode Agent (4 min)

- Ouvrez la fenêtre de chat, basculez le menu déroulant de mode sur **Agent**, et ajoutez `src/ShoppingCart/Cart.cs` et `src/InventoryLegacy/InventoryLegacy.cs` comme contexte (le bouton `+`).
- Demandez :

  > La monnaie doit être gérée en centimes entiers dans les deux fichiers. Mets à jour le code et tous les appelants,
  > et conserve inchangés les noms des méthodes publiques.

  La monnaie utilise actuellement `double` à : `Cart.cs:9` (`CartItem.Price`), `Cart.cs:16` (`Cart.Discount`), `Cart.cs:26-27` (`CheckoutResult.OrderId`/`Total`), et `InventoryLegacy.cs:52-54` (`price_with_tax`).

- Passez en revue les diffs proposés par fichier, acceptez-en certains, rejetez-en d'autres, puis annulez.

> **Point de discussion :** le mode agent de Visual Studio pilote des modifications multi-fichiers, axées diff et réversibles — il planifie le changement, modifie à travers les fichiers, et demande avant d'exécuter des commandes.

---

### 5️⃣ Démo 5 — Mode agent (5 min)

- Basculez le chat sur Agent, puis demandez :

  > Ajoute une suite de tests xUnit pour src/InventoryLegacy, crée la structure du projet de tests si nécessaire,
  > exécute `dotnet test`, et corrige tout ce qui échoue.

  Bonnes méthodes à cibler : `get_stock` (`InventoryLegacy.cs:13-21`), `adjust` (`InventoryLegacy.cs:23-33`), `reorder_report` (`InventoryLegacy.cs:35-50`), et `price_with_tax` (`InventoryLegacy.cs:52-54`).

- Laissez-le planifier, modifier les fichiers, et demander l'approbation du terminal. Montrez l'invite d'approbation d'outil.

> **Point de discussion :** le mode agent choisit ses propres fichiers et outils, itère sur les échecs, et
> demande avant d'exécuter des commandes.

---

### 6️⃣ Démo 6 — MCP en mode agent (3 min)

- Ouvrez la configuration MCP de ce kit dans `.vscode\mcp.json` (Visual Studio découvre aussi `%USERPROFILE%\.mcp.json`, `<SolutionDir>\.mcp.json`, et `<SolutionDir>\.vs\mcp.json`). Dans l'Explorateur de solutions, activez **Afficher tous les fichiers** pour révéler le dossier `.vscode` préfixé par un point, ou utilisez **Fichier > Ouvrir > Fichier…** (`Ctrl+O`) et naviguez jusqu'à lui.
- Dans la fenêtre de chat, basculez sur **Agent**, ouvrez l'icône **outils** (clé), et activez les outils fournis par MCP (les outils sont désactivés par défaut par serveur).
- Demandez :

  > À l'aide des outils GitHub, liste les problèmes ouverts de ce dépôt et rédige un plan de correction pour
  > le plus ancien.

- Montrez le sélecteur d'outils listant les outils fournis par MCP.

> **Point de discussion :** MCP étend le mode agent avec des systèmes externes via un protocole ouvert ; les serveurs sont configurés par solution ou par utilisateur, chaque outil est activé explicitement, et chaque appel d'outil est soumis à approbation.

---

### 7️⃣ Démo 7 — Sessions d'agent et délégation (3 min)

- Sur github.com, ouvrez le panneau Agents (ou assignez un problème à Copilot) et déléguez :

  > Corrige le bogue de décalage d'un cran dans Subtotal() et ouvre une pull request.

- Montrez le journal de session, la branche qu'il crée, et la PR brouillon.
- Mentionnez les sous-agents : la session délègue des unités de travail ciblées afin que la fenêtre de contexte principale ne soit pas consommée par des tâches annexes.

> **Point de discussion :** les sessions déléguées s'exécutent sur des ressources hébergées par GitHub ; vous examinez la PR comme
> tout autre contributeur.

---

### 8️⃣ Démo 8 — Copilot CLI (5 min)

- Dans un terminal à la racine du dépôt, démarrez une session interactive :

  ```
  copilot                       # session interactive
  ```

- Dans la session, explorez les commandes :
  - `/help`, `/model`, `/clear`, `/session`, `/exit`
- Puis essayez quelques demandes :
  - `explique ce que fait src/InventoryLegacy/InventoryLegacy.cs et liste ses risques`
  - `écris un script PowerShell qui sauvegarde chaque fichier .cs dans backups/ avec un horodatage, puis exécute-le`
  - `renomme src/ShoppingCart/Cart.cs en src/ShoppingCart/ShoppingCart.cs et mets à jour les références d'espace de noms`
- Ou exécutez de manière non interactive :

  ```
  copilot -p "génère un .gitignore pour un projet .NET et Node"
  ```

> **Point de discussion :** la CLI apporte Copilot au terminal pour ceux qui ne vivent pas dans un
> IDE — elle peut expliquer des commandes, générer des scripts et agir sur des fichiers, et elle demande l'approbation
> avant de modifier quoi que ce soit.

---

## 🅱️ Partie B — Revue de code, collaboration et paramètres d'organisation

### 9️⃣ Démo 9 — Revue de code et normes de revue personnalisées (4 min)

- Dans `src/ShoppingCart/Cart.cs`, sélectionnez `Checkout` (`Cart.cs:53-58`) > clic droit > **Copilot Actions > Review Selection** pour obtenir des commentaires de revue en ligne (ou utilisez le bouton étincelle dans la fenêtre **Git Changes** pour examiner les modifications locales). Attendez-vous à un signalement sur le numéro de carte journalisé (`Cart.cs:56`).
- Ouvrez `.github/copilot-instructions.md`, montrez la section Normes de revue, et relancez la revue pour démontrer que la consigne modifie les commentaires.
- Sur github.com, demandez une revue à Copilot sur une pull request.

> **Point de discussion :** les normes de revue sont pilotées par un fichier d'instructions, de sorte que la sortie de revue est
> cohérente dans toute l'équipe.

---

### 🔟 Démo 10 — Résumés de PR, Spaces et Spark (3 min)

- Sur une PR ouverte, utilisez Copilot > Générer un résumé et montrez la présentation qu'il rédige.
- Ouvrez un Copilot Space, attachez ce dépôt plus le lien du guide d'étude, et posez une question qui nécessite les deux.
- Dans Spark, décrivez un petit tableau de bord d'inventaire et laissez-le construire et déployer l'application.

> **Point de discussion :** les Spaces sont un contexte organisé et partageable ; Spark va du prompt à l'application en cours d'exécution.
