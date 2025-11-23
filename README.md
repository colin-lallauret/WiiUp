# WiiUp - Projet Unity

## Prérequis

- Unity Editor version **6000.2.6f2** (obligatoire)
- Git installé sur la machine

## Installation du projet

1. **Cloner le repository :**
   ```bash
   git clone [URL_DU_REPO]
   cd WiiUp
   ```

2. **Ouvrir avec Unity :**
   - Lancez Unity Hub
   - Cliquez sur "Open" ou "Add"
   - Sélectionnez le dossier racine du projet (`WiiUp/`)
   - Unity va automatiquement :
     - Re-générer le dossier `Library/`
     - Télécharger les packages manquants
     - Compiler les scripts

3. **Première ouverture :**
   - La première ouverture peut prendre plusieurs minutes
   - Unity doit importer tous les assets et télécharger les packages
   - Ne fermez pas Unity pendant ce processus

## Problèmes courants

### "Failed to resolve packages" ou erreurs de packages
1. Supprimez le dossier `Library/` (s'il existe)
2. Dans Unity, allez dans `Window > Package Manager`
3. Cliquez sur le bouton refresh ou redémarrez Unity

### Version de Unity différente
- Assurez-vous d'utiliser exactement Unity **6000.2.6f2**
- Téléchargez cette version via Unity Hub si nécessaire

### Erreurs de compilation
- Allez dans `Assets > Reimport All` pour re-importer tous les assets
- Redémarrez Unity si nécessaire

## Structure du projet

- `Assets/` - Tous les assets du jeu (scènes, scripts, prefabs, etc.)
- `ProjectSettings/` - Configuration Unity du projet
- `Packages/` - Liste des packages Unity utilisés
- `UserSettings/` - Paramètres utilisateur (ignoré par Git)
- `Library/` - Cache Unity (généré automatiquement, ignoré par Git)

## Note importante

Le dossier `Library/` est automatiquement généré par Unity et ne doit jamais être commité dans Git. Si vous voyez ce dossier, assurez-vous qu'il est bien ignoré par le `.gitignore`.
