# Kinaru - Application Immobilière Mobile

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0-512BD4?style=flat-square&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4?style=flat-square&logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=flat-square&logo=sqlite)

Kinaru est une application multiplateforme complète de gestion et de location de biens immobiliers développée avec .NET MAUI. Elle permet aux utilisateurs de parcourir des propriétés, de gérer des réservations, de communiquer avec des agents et de gérer leurs propres annonces immobilières.

## 📋 Table des Matières

- [Fonctionnalités](#-fonctionnalités)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Prérequis](#-prérequis)
- [Installation](#-installation)
- [Démarrage](#-démarrage)
- [Structure du Projet](#-structure-du-projet)
- [Configuration](#-configuration)
- [Fonctionnalités Détaillées](#-fonctionnalités-détaillées)

## ✨ Fonctionnalités

### Pour les Utilisateurs
- 🏠 **Recherche de propriétés** avec filtres avancés (type, prix, localisation)
- 📱 **Navigation intuitive** avec onglets (Accueil, Favoris, Messages, Profil)
- ❤️ **Gestion des favoris** pour sauvegarder les propriétés intéressantes
- 📅 **Réservation de visites** avec sélection de date et message personnalisé
- 💬 **Messagerie intégrée** pour communiquer avec les agents
- 👤 **Gestion de profil** avec modification des informations personnelles

### Pour les Propriétaires
- ➕ **Ajout de propriétés** avec formulaire complet
- 📸 **Gestion d'images** (galerie et caméra)
- 🔄 **Changement de statut** rapide (Disponible, Vendu, Loué)
- ✏️ **Modification et suppression** de propriétés
- 📊 **Tableau de bord** avec liste des propriétés

### Pour les Agents
- 👔 **Profil agent** avec biographie, spécialités et réseaux sociaux
- 📅 **Gestion de disponibilités** avec calendrier
- 📋 **Gestion des réservations** (accepter/refuser)
- 📊 **Tableau de bord agent** avec statistiques
- 🏢 **Gestion des propriétés** assignées

## 🏗️ Architecture

La solution est organisée en 4 projets principaux :

```
Kinaru/
├── Kinaru/                    # Application cliente .NET MAUI
│   ├── Views/                 # Pages XAML
│   ├── ViewModels/            # ViewModels MVVM
│   ├── Services/              # Services API (Refit)
│   ├── Converters/            # Convertisseurs XAML
│   └── Resources/             # Images, styles, couleurs
├── Kinaru.Api/                # Backend API ASP.NET Core
│   ├── Endpoints/             # Endpoints API (Minimal API)
│   ├── Services/              # Services métier
│   └── Middleware/            # Middleware personnalisés
├── Kinaru.Database/           # Gestion de la base de données
│   ├── KinaruDbContext.cs     # DbContext EF Core
│   └── Migrations/            # Migrations EF Core
└── Kinaru.Shared/             # Code partagé
    ├── Entities/              # Entités du domaine
    ├── DTOs/                  # Data Transfer Objects
    └── Enums/                 # Énumérations
```

## 🛠️ Technologies

### Frontend (Client Mobile)
- **.NET MAUI 9.0** - Framework multiplateforme
- **CommunityToolkit.Mvvm** - Pattern MVVM
- **Refit** - Client HTTP typé
- **XAML** - Interface utilisateur

### Backend (API)
- **ASP.NET Core 9.0** - Framework web
- **Entity Framework Core 9.0** - ORM
- **SQLite** - Base de données
- **JWT Authentication** - Authentification sécurisée
- **Minimal APIs** - Endpoints légers

### Plateformes Supportées
- ✅ Android (API 21+)
- ✅ iOS (iOS 11+)
- ✅ Windows (Windows 10.0.19041.0+)
- ✅ macOS (macOS 10.15+)

## 📦 Prérequis

Avant de commencer, assurez-vous d'avoir installé :

### Obligatoire
- **[.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)** - Version minimale requise
- **[Visual Studio 2022 (17.8+)](https://visualstudio.microsoft.com/)** avec les charges de travail :
  - Développement d'applications mobiles avec .NET
  - Développement ASP.NET et web

### Optionnel
- **[Visual Studio Code](https://code.visualstudio.com/)** avec extensions :
  - C# Dev Kit
  - .NET MAUI
- **[Android Studio](https://developer.android.com/studio)** - Pour l'émulateur Android
- **[Xcode](https://developer.apple.com/xcode/)** - Pour iOS/macOS (Mac uniquement)

## 📥 Installation

### Étape 1 : Cloner le Dépôt

```bash
git clone https://github.com/yalnguembu/Kinaru.git
cd Kinaru
```

### Étape 2 : Restaurer les Dépendances

```bash
dotnet restore
```

### Étape 3 : Configuration de la Base de Données

Le projet inclut déjà une base de données SQLite pré-configurée (`kinaru.db`). Si vous devez la recréer :

```bash
# Installer l'outil EF Core (si pas déjà fait)
dotnet tool install --global dotnet-ef

# Appliquer les migrations
dotnet ef database update --project Kinaru.Database --startup-project Kinaru.Api
```

## 🚀 Démarrage

### Méthode 1 : Avec Visual Studio (Recommandé)

#### 1. Démarrer l'API Backend

1. Ouvrez `Kinaru.sln` dans Visual Studio 2022
2. Dans l'Explorateur de solutions, clic droit sur **Kinaru.Api** → **Définir comme projet de démarrage**
3. Appuyez sur **F5** ou cliquez sur ▶️ **Exécuter**
4. L'API démarre sur `http://localhost:5117`
5. Vérifiez que la console affiche : `Now listening on: http://localhost:5117`

#### 2. Démarrer l'Application Mobile

1. Dans l'Explorateur de solutions, clic droit sur **Kinaru** → **Définir comme projet de démarrage**
2. Sélectionnez la plateforme cible dans la barre d'outils :
   - **Windows Machine** - Pour Windows
   - **Android Emulator** - Pour Android
   - **iOS Simulator** - Pour iOS (Mac uniquement)
3. Appuyez sur **F5** ou cliquez sur ▶️ **Exécuter**
4. L'application se lance sur la plateforme sélectionnée

### Méthode 2 : Avec la Ligne de Commande

#### 1. Démarrer l'API Backend

```bash
# Terminal 1 - API Backend
cd Kinaru.Api
dotnet run
```

Attendez le message : `Now listening on: http://localhost:5117`

#### 2. Démarrer l'Application Mobile

```bash
# Terminal 2 - Application Mobile (nouveau terminal)
cd Kinaru

# Pour Windows
dotnet build -t:Run -f net9.0-windows10.0.19041.0

# Pour Android
dotnet build -t:Run -f net9.0-android

# Pour iOS (Mac uniquement)
dotnet build -t:Run -f net9.0-ios

# Pour macOS (Mac uniquement)
dotnet build -t:Run -f net9.0-maccatalyst
```

### Méthode 3 : Démarrage Rapide (Développement)

Pour démarrer rapidement les deux projets :

```bash
# Terminal 1 - API
dotnet watch run --project Kinaru.Api

# Terminal 2 - Application (dans un nouveau terminal)
dotnet watch run --project Kinaru -f net9.0-windows10.0.19041.0
```

## 📁 Structure du Projet

### Kinaru (Application Mobile)

```
Kinaru/
├── Views/                      # Pages XAML
│   ├── HomePage.xaml          # Page d'accueil
│   ├── PropertyDetailsPage.xaml
│   ├── MyPropertiesPage.xaml
│   ├── AgentDashboardPage.xaml
│   └── ...
├── ViewModels/                # ViewModels MVVM
│   ├── HomeViewModel.cs
│   ├── PropertyDetailsViewModel.cs
│   └── ...
├── Services/                  # Interfaces API (Refit)
│   ├── IPropertyService.cs
│   ├── IUserService.cs
│   ├── IAgentService.cs
│   └── ...
├── Converters/               # Convertisseurs XAML
│   ├── BoolToColorConverter.cs
│   ├── PropertyStatusToColorConverter.cs
│   └── ...
├── Resources/                # Ressources
│   ├── Styles/              # Styles XAML
│   ├── Images/              # Images
│   └── Fonts/               # Polices
├── MauiProgram.cs           # Configuration DI
└── AppShell.xaml            # Navigation Shell
```

### Kinaru.Api (Backend)

```
Kinaru.Api/
├── Endpoints/               # Endpoints API
│   ├── AuthEndpoints.cs    # Authentification
│   ├── PropertyEndpoints.cs
│   ├── AgentEndpoints.cs
│   └── ...
├── Services/               # Services métier
│   ├── Interfaces/
│   └── Implementations/
├── Middleware/             # Middleware
│   └── ErrorHandlingMiddleware.cs
├── appsettings.json       # Configuration
└── Program.cs             # Point d'entrée
```

### Kinaru.Shared (Partagé)

```
Kinaru.Shared/
├── Entities/              # Entités du domaine
│   ├── User.cs
│   ├── Property.cs
│   ├── Agent.cs
│   └── ...
├── DTOs/                  # Data Transfer Objects
│   ├── Properties/
│   ├── Users/
│   ├── Agents/
│   └── ...
└── Enums/                 # Énumérations
    ├── PropertyType.cs
    ├── PropertyStatus.cs
    └── ...
```

## ⚙️ Configuration

### Configuration de l'API (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=kinaru.db"
  },
  "Jwt": {
    "Key": "votre-clé-secrète-jwt-très-longue-et-sécurisée",
    "Issuer": "KinaruApi",
    "Audience": "KinaruApp",
    "ExpiryInDays": 7
  }
}
```

### Configuration du Client Mobile

L'URL de l'API est configurée automatiquement dans `MauiProgram.cs` :

```csharp
// Android Emulator
var baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
    ? "http://10.0.2.2:5117" 
    : "http://localhost:5117";
```

Pour changer l'URL de l'API (production) :

```csharp
var baseUrl = "https://votre-api.com";
```

## 🎯 Fonctionnalités Détaillées

### Authentification
- Inscription avec email, nom, téléphone
- Connexion avec email/mot de passe
- JWT tokens avec expiration
- Stockage sécurisé des tokens

### Gestion des Propriétés
- **Ajout** : Formulaire complet avec images
- **Modification** : Édition de toutes les informations
- **Suppression** : Avec confirmation
- **Statuts** : Disponible, Vendu, Loué
- **Filtres** : Type, prix, localisation, statut

### Gestion des Images
- Sélection depuis la galerie
- Capture avec la caméra
- Prévisualisation des images
- Suppression d'images

### Réservations
- Création de réservations de visite
- Gestion des réservations (agent)
- Acceptation/Refus de réservations
- Historique des réservations

### Messagerie
- Conversations avec agents
- Messages en temps réel
- Historique des conversations

### Profil Agent
- Biographie et spécialités
- Liens réseaux sociaux
- Gestion de disponibilités
- Calendrier de disponibilités

## 🔧 Dépannage

### L'API ne démarre pas

```bash
# Vérifier que le port 5117 est libre
netstat -ano | findstr :5117

# Ou changer le port dans launchSettings.json
```

### Erreur de connexion à l'API

1. Vérifiez que l'API est démarrée
2. Vérifiez l'URL dans `MauiProgram.cs`
3. Pour Android Emulator, utilisez `10.0.2.2` au lieu de `localhost`

### Erreur de migration de base de données

```bash
# Supprimer la base de données
rm Kinaru.Api/kinaru.db

# Recréer la base de données
dotnet ef database update --project Kinaru.Database --startup-project Kinaru.Api
```

### Problèmes de build MAUI

```bash
# Nettoyer et rebuilder
dotnet clean
dotnet build
```

## 📝 Comptes de Test

Après le premier démarrage, vous pouvez créer un compte ou utiliser les données de test si configurées.

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à ouvrir une issue ou une pull request.

## 📄 Licence

Ce projet est sous licence MIT.

## 👥 Auteurs

- **Yalnguembu** - Développeur principal

## 🙏 Remerciements

- .NET MAUI Team
- ASP.NET Core Team
- Communauté .NET
