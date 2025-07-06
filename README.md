# R.A.T - Remote Access Trojan

Projet R.A.T avec un serveur et un client (victim) développé en .NET.

---

## Description

Ce projet permet de gérer une connexion entre un serveur et un client distant (victim).  
Le serveur écoute les connexions entrantes tandis que le client exécutable se lance sur la machine cible et établit une connexion avec le serveur.

---

## Installation

Clonez le dépôt GitHub :

  - git clone https://github.com/Casperyber/R.A.T.git
  - cd R.A.T

## Compilation

### Serveur

Ouvrez le dossier du serveur dans Visual Studio Code 2022 (ou Visual Studio 2022).

Compilez et lancez le serveur directement via l’IDE.

### Client (Victim)

Pour compiler le client en un exécutable Windows autonome :

  - dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:SelfContained=true

Cette commande génère un .exe autonome dans le dossier bin/Release/net8.0-windows/win-x64/publish/.

Copiez cet exécutable sur la machine cible.

## Utilisation

Lancez le serveur (depuis Visual Studio Code 2022 ou Visual Studio 2022).

Exécutez le client compilé sur la machine cible.

Le client établira automatiquement une connexion au serveur.

À partir du serveur, vous pourrez interagir avec la machine cliente selon les fonctionnalités implémentées.

## Remarques importantes

Sécurité : Ce projet est à utiliser uniquement à des fins légales, éthiques et pédagogiques.

Dépendances : Assurez-vous que .NET 8 est installé pour compiler le projet.

Limitations GitHub : Les fichiers de plus de 100 Mo ne peuvent pas être poussés vers GitHub sans Git LFS. Nettoyez votre historique Git si nécessaire.

## Liens utiles

Git Large File Storage (LFS)

Documentation dotnet publish

