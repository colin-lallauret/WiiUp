#!/bin/bash

# Script de vérification pour le projet Unity WiiUp
# Utilisation: ./verify-project.sh

echo "🔍 Vérification du projet Unity WiiUp..."
echo ""

# Vérifier la présence des fichiers essentiels
echo "📁 Vérification des fichiers essentiels:"

essential_files=(
    "Assets"
    "ProjectSettings/ProjectSettings.asset" 
    "ProjectSettings/ProjectVersion.txt"
    "Packages/manifest.json"
    "Packages/packages-lock.json"
)

missing_files=()

for file in "${essential_files[@]}"; do
    if [[ -e "$file" ]]; then
        echo "  ✅ $file"
    else
        echo "  ❌ $file - MANQUANT!"
        missing_files+=("$file")
    fi
done

echo ""

# Vérifier la version Unity
echo "🎮 Version Unity requise:"
if [[ -f "ProjectSettings/ProjectVersion.txt" ]]; then
    unity_version=$(grep "m_EditorVersion:" ProjectSettings/ProjectVersion.txt | cut -d' ' -f2)
    echo "  📋 Version détectée: $unity_version"
    echo "  📋 Version requise: 6000.2.6f2"
    
    if [[ "$unity_version" == "6000.2.6f2" ]]; then
        echo "  ✅ Version Unity correcte"
    else
        echo "  ⚠️  Version Unity différente - peut causer des problèmes"
    fi
else
    echo "  ❌ Impossible de détecter la version Unity"
fi

echo ""

# Vérifier gitignore
echo "🚫 Vérification du .gitignore:"
if [[ -f ".gitignore" ]]; then
    if grep -q "/\[Ll\]ibrary/" .gitignore; then
        echo "  ✅ Library/ est ignoré"
    else
        echo "  ❌ Library/ n'est pas ignoré correctement"
    fi
    
    if grep -q "/\[Tt\]emp/" .gitignore; then
        echo "  ✅ Temp/ est ignoré" 
    else
        echo "  ❌ Temp/ n'est pas ignoré"
    fi
else
    echo "  ❌ Fichier .gitignore manquant"
fi

echo ""

# Résultat final
if [[ ${#missing_files[@]} -eq 0 ]]; then
    echo "🎉 Le projet semble correctement configuré!"
    echo ""
    echo "📝 Instructions pour l'autre machine:"
    echo "   1. Installer Unity Hub et Unity $unity_version"
    echo "   2. Cloner ce repository avec Git"
    echo "   3. Ouvrir le projet avec Unity (cela peut prendre du temps)"
    echo "   4. Laisser Unity re-générer Library/ et télécharger les packages"
else
    echo "❌ Problèmes détectés! Fichiers manquants:"
    for file in "${missing_files[@]}"; do
        echo "   - $file"
    done
    echo ""
    echo "⚠️  Le projet ne pourra pas être ouvert correctement sur une autre machine."
fi
