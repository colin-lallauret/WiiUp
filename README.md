# WiiUp - Projet Unity

## 🎯 Installation rapide

**Version Unity requise : 6000.2.6f2**

1. `git clone [URL_REPO] && cd WiiUp`
2. Ouvrir avec Unity Hub → "Add project from disk"
3. Laisser Unity importer (5-10 minutes la première fois)

## ✅ Vérification avant commit

Exécutez ce script pour vérifier votre projet :
```bash
./verify-project.sh
```

## 📁 Fichiers essentiels (DOIVENT être dans Git)

- ✅ `Assets/` + tous les `.meta`
- ✅ `ProjectSettings/`
- ✅ `Packages/manifest.json`
- ✅ `Packages/packages-lock.json`

## 🚫 Fichiers ignorés (générés automatiquement)

- ❌ `Library/` 
- ❌ `Temp/`
- ❌ `Logs/`
- ❌ `UserSettings/`
- ❌ `*.csproj`, `*.sln`

## 🔧 Résolution de problèmes

### Erreur "Missing references" sur l'autre machine
1. Vérifiez que tous les `.meta` sont présents
2. `Assets > Reimport All` dans Unity
3. Redémarrez Unity

### Erreur "Failed to resolve packages"  
1. Supprimez `Library/` s'il existe
2. `Window > Package Manager > Refresh`
3. Redémarrez Unity

### Scripts ne compilent pas
- Vérifiez la version Unity (doit être exactement 6000.2.6f2)
- `Assets > Reimport All`

## 🔍 Structure du dépôt

```
WiiUp/
├── Assets/          # Tous vos assets + .meta
├── ProjectSettings/ # Config Unity
├── Packages/        # manifest.json + packages-lock.json
├── .gitignore       # Ignore Library/, Temp/, etc.
└── README.md        # Ce fichier
```

## ⚠️ IMPORTANT

- Les fichiers `.meta` sont **CRITIQUES** - ne jamais les supprimer
- Le dossier `Library/` ne doit **JAMAIS** être commité
- Toujours utiliser la même version Unity sur toutes les machines
