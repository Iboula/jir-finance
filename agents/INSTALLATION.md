# 📦 Installation des Agents MCP

## Pour ChatGPT / ChatGPT Workspace

### Méthode 1 : Import Manuel
1. Ouvrez ChatGPT
2. Cliquez sur votre profil > **Custom instructions** ou **Agents**
3. Créez un nouvel agent
4. Nom : "Architecte.NET"
5. Copiez TOUT le contenu de `1-architecte-dotnet.json`
6. Collez dans la configuration
7. Sauvegardez
8. Répétez pour les 5 autres agents

### Méthode 2 : Prompt Direct
Collez ce prompt dans ChatGPT :

```
Je veux créer 6 agents spécialisés pour mon projet. Voici les configurations :

Agent 1 - Architecte.NET :
[Coller le contenu de 1-architecte-dotnet.json]

Agent 2 - DB/PostgreSQL Engineer :
[Coller le contenu de 2-db-postgresql-engineer.json]

... etc
```

---

## Pour Cursor

### Installation
1. Ouvrez votre projet dans Cursor
2. Créez le dossier `.cursor/agents/` à la racine
3. Copiez tous les fichiers `*.json` dans ce dossier
4. Redémarrez Cursor
5. Les agents apparaîtront dans le panneau latéral

### Utilisation
```
@architecte Initialise le projet backend complet
```

---

## Pour Windsurf

### Installation
1. Créez le dossier `.windsurf/agents/`
2. Copiez les fichiers JSON
3. Redémarrez Windsurf

### Utilisation
```
/agent architecte Crée la solution .NET
```

---

## Pour VS Code (GitHub Copilot)

### Prérequis
- Extension GitHub Copilot installée
- Support MCP (vérifier la version de l'extension)

### Installation
1. Ouvrez VS Code
2. `Ctrl+Shift+P` > "Copilot: Manage Agents"
3. "Import Agent from File"
4. Sélectionnez chaque fichier JSON

### Utilisation
```
@architecte [votre requête]
```

---

## Pour Autres IDE compatibles MCP

Les fichiers JSON sont au format standard MCP. Consultez la documentation de votre IDE pour importer des agents personnalisés.

---

## Vérification de l'Installation

Pour vérifier que les agents sont bien installés, essayez :

```
@architecte Bonjour, es-tu opérationnel ?
```

Réponse attendue : L'agent doit répondre qu'il est l'agent Architecte.NET et lister ses compétences.

---

## Troubleshooting

### Les agents ne sont pas reconnus
- Vérifiez que votre IDE supporte les agents MCP
- Vérifiez que les fichiers JSON sont bien formatés
- Redémarrez l'IDE

### L'agent ne comprend pas les prompts
- Soyez plus explicite dans vos instructions
- Mentionnez le contexte du projet JIR
- Référencez le cahier des charges

### Conflit entre agents
- N'utilisez qu'un agent à la fois pour une tâche donnée
- Suivez l'ordre recommandé dans QUICKSTART.md
