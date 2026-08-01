# taram_group-test-dev-web
# Test technique Développeur Web Full-Stack

## Import ERP Clients - API REST - VueJS 2

## 1. Présentation du projet

Ce projet simule une mission de développement d'une plateforme SaaS B2B permettant d'importer des données clients provenant d'un ERP externe, de les exposer via une API REST et de les consulter/modifier depuis une interface web.

Le projet est composé de :

* Un backend ASP.NET Core 8 Web API en C#
* Une interface frontend VueJS 2
* Un repository en mémoire simulant la persistance des données

---

# 2. Architecture du projet

```
.
├── backend
│   ├── Controllers
│   ├── DTOs
│   ├── Models
│   ├── Repositories
│   ├── Services
│   ├── Validators
│   └── Data
│       └── export_erp_clients.json
│
├── frontend
│   ├── .env
│   ├── components
│   ├── views
│   ├── router
│   └── api
│
└── README.md
```

---

# 3. Technologies utilisées

## Backend

* C#
* ASP.NET Core 8 Web API
* Repository Pattern
* JSON System.Text.Json

## Frontend

* VueJS 2
* Vue Router
* Axios

## Base de données

MongoDB était prévu dans le cahier des charges.

Cependant, une implémentation en mémoire a été utilisée derrière une interface de repository afin de respecter l'architecture demandée tout en gardant la possibilité de remplacer facilement la persistance.

---

# 4. Installation et lancement

## Prérequis

* .NET SDK 8
* Node.js
* npm

---

# Backend

Se placer dans le dossier :

```bash
cd backend
```

Restaurer les dépendances :

```bash
dotnet restore
```

Lancer l'API :

```bash
dotnet run
```

L'API sera disponible sur l'URL indiquée par ASP.NET Core.

Swagger est disponible sur :

```
/swagger
```

---

# Frontend

Se placer dans le dossier :

Ajouter la variable suivante :

```env
VUE_APP_API_URL=http://localhost:5055/api
```

Copier `.env.example` vers `.env` puis adapter les valeurs selon l'environnement.
Cette variable correspond à l'adresse de l'API backend utilisée par l'application VueJS.

Elle permet de changer l'URL du backend sans modifier le code source.

```bash
cd frontend
```

Installer les dépendances :

```bash
npm install
```

Lancer VueJS :

```bash
npm run serve
```

L'application sera accessible sur :

```
http://localhost:8080
```

---

# 5. Persistance des données

## Choix actuel

Le projet utilise :

```
IContactRepository
        |
        |
InMemoryContactRepository
```

Les données sont stockées temporairement en mémoire pendant l'exécution de l'application.

---

## Justification

Le sujet autorise une implémentation en mémoire lorsque MongoDB n'est pas utilisé.

L'utilisation d'une interface permet de remplacer facilement cette implémentation par une version MongoDB sans modifier les services métiers.

Une implémentation MongoDB pourrait utiliser :

* un document MongoDB par Contact
* un index unique sur `ExternalId`
* des recherches textuelles sur :

  * société
  * nom du contact
  * email

---

# 6. Fonctionnalités disponibles

## Import ERP

Endpoint :

```
POST /api/import
```

Fonctionnement :

* Lecture du fichier `export_erp_clients.json`
* Validation des données
* Transformation vers le modèle interne Contact
* Création ou mise à jour selon l'identifiant ERP
* Génération d'un rapport d'import

---

## Liste des contacts

Endpoint :

```
GET /api/contacts
```

Fonctionnalités :

* Pagination
* Recherche par société, nom ou email
* Tri disponible selon les paramètres envoyés

---

## Détail d'un contact

Endpoint :

```
GET /api/contacts/{id}
```

Retourne :

* 200 si le contact existe
* 404 sinon

---

## Modification d'un contact

Endpoint :

```
PUT /api/contacts/{id}
```

Validation des données entrantes avant modification.

Codes retournés :

* 200 : modification réussie
* 400 : données invalides
* 404 : contact inexistant

---

# 7. Règles de validation et rejet des imports

Un enregistrement ERP est rejeté si :

## Identifiant ERP

* L'identifiant externe est absent

Exemple :

```
"id_erp": null
```

---

## Société

La société est obligatoire.

Exemple rejeté :

```
"societe": ""
```

---

## Contact

Le nom du contact est obligatoire.

---

## Email / téléphone

Au moins un des deux champs doit être présent :

* Email
* Téléphone

Un contact sans moyen de communication est rejeté.

---

## Email

L'email doit respecter un format valide.

Exemple rejeté :

```
luc.bernard@atelier-bernard
```

---

## Téléphone

Les caractères suivants sont supprimés :

* espaces
* points
* tirets

Exemple :

```
07.55.44.33.22
```

devient :

```
0755443322
```

---

## Date de création

Les formats acceptés sont :

```
yyyy-MM-dd
dd/MM/yyyy
```

Une date impossible est rejetée.

Exemple :

```
2024-13-01
```

---

## Chiffre d'affaires

Le chiffre d'affaires doit être positif.

Exemple rejeté :

```
-5000
```

---

# 8. Gestion des doublons

L'identifiant ERP (`ExternalId`) est utilisé comme clé métier.

Lorsqu'un contact possède déjà cet identifiant :

* aucun nouveau contact n'est créé
* les informations existantes sont mises à jour

Exemple :

```
C-1001
```

présent deux fois dans le fichier :

→ premier passage : création
→ deuxième passage : mise à jour

---

# 9. Choix techniques

## Séparation DTO / Model

Les DTO permettent de séparer :

* le format reçu depuis l'ERP
* le modèle interne utilisé par l'application

Cela évite de coupler directement l'application au format externe.

---

## Service d'import

La logique d'import est isolée dans un service dédié afin de :

* faciliter les tests
* éviter de mettre la logique métier dans les contrôleurs
* permettre l'évolution du connecteur ERP

---

## Repository Pattern

Le repository permet de séparer :

* la logique métier
* le stockage des données

Cela facilite une migration future vers MongoDB.

---

# 10. Limites actuelles

Par manque de temps :

* MongoDB n'a pas été implémenté
* Les tests automatisés n'ont pas été ajoutés
* L'authentification n'a pas été ajoutée
* L'interface utilisateur reste volontairement simple

---

# 11. Améliorations possibles avec une journée supplémentaire

Avec plus de temps, les améliorations prévues seraient :

* Implémentation MongoDB complète
* Ajout de tests unitaires sur le service d'import
* Ajout de tests d'intégration API
* Amélioration de la gestion des erreurs avec un format standardisé
* Ajout d'une meilleure gestion du tri et filtrage
* Ajout d'un système de logs pour suivre les imports
* Amélioration de l'interface utilisateur

---

# 12. Utilisation d'outils d'assistance

Des outils d'assistance IA ont été utilisés pour :

* obtenir des suggestions d'architecture
* vérifier certaines implémentations
* améliorer la lisibilité du code

Chaque partie générée ou proposée a été vérifiée et adaptée afin de comprendre et justifier les choix techniques effectués.


# 13. Analyse du code legacy (Bonus)

## 13.1 Problèmes identifiés

### Critique

#### Informations sensibles en dur

La chaîne de connexion MongoDB est directement écrite dans le code :

```csharp
public static string connString =
"mongodb://prod_user:Passw0rd!@10.0.0.12:27017";
```

**Problèmes :**
- Nom d'utilisateur, mot de passe et adresse du serveur exposés.
- Impossible de changer d'environnement sans modifier le code.
- Risque important de fuite d'informations sensibles.

**Amélioration :**
- Déplacer la configuration dans `appsettings.json` ou des variables d'environnement.
- Utiliser le système de configuration d'ASP.NET Core.

---

### Élevé

#### Classe avec plusieurs responsabilités

`ContactManager` :
- accède à MongoDB ;
- exporte des fichiers CSV ;
- nettoie les données ;
- synchronise avec un ERP.

Cette classe ne respecte pas le principe de responsabilité unique (Single Responsibility Principle).

**Amélioration :**
Créer plusieurs services spécialisés :

- ContactRepository
- ContactExportService
- ContactCleanupService
- ContactSyncService

---

#### Absence de gestion des erreurs

Aucune exception n'est gérée lors :

- de la connexion MongoDB ;
- de l'écriture du fichier ;
- des appels HTTP.

Une seule erreur peut interrompre tout le traitement.

**Amélioration :**
- Ajouter des blocs `try/catch`.
- Journaliser les erreurs.
- Continuer le traitement lorsque cela est possible.

---

### Moyen

#### Chargement complet de la collection

```csharp
var docs = col.Find(new BsonDocument()).ToList();
```

Tous les documents sont chargés en mémoire.

**Risques :**
- consommation mémoire importante ;
- baisse des performances avec un grand volume de données.

**Amélioration :**
Traiter les données par lots ou utiliser un curseur MongoDB.

---

## 13.2 Plan de refactorisation progressif

Comme cette classe est utilisée en production et qu'aucun test n'existe, la refactorisation doit être progressive.

### Étape 1

- Sécuriser la configuration.
- Retirer les informations sensibles du code.
- Ajouter des logs.

### Étape 2

Écrire des tests sur le comportement actuel afin de garantir qu'aucune régression n'apparaisse pendant la refactorisation.

### Étape 3

Séparer progressivement les responsabilités :

- ContactRepository
- ContactExportService
- ContactCleanupService
- ContactSyncService

### Étape 4

Ajouter :

- validation des données ;
- gestion des exceptions ;
- gestion des erreurs réseau.

### Étape 5

Optimiser les performances :

- traitement par lots ;
- appels HTTP asynchrones ;
- amélioration des accès MongoDB.

---

## 13.3 Tests à écrire en priorité

### Test d'export CSV

Vérifier que le fichier CSV est correctement généré avec les bonnes données.

### Test du nettoyage

Vérifier qu'un contact sans email est supprimé conformément à la règle métier.

### Test de synchronisation ERP

Vérifier que les appels HTTP sont correctement effectués et que les erreurs sont correctement gérées.

### Test de récupération des contacts

Vérifier que les données récupérées depuis la base sont correctement traitées.

Ces tests permettent de sécuriser les fonctionnalités existantes avant toute refactorisation importante.