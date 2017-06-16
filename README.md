#Core

##Introduction

Ce dépôt est utilisé pour le développement de la partie interne de la suite logicielle Duly, à savoir le Core.

Ce projet ce présente sous la forme d'une biblitothèque de classe en C# UWP déployé sous forme de paquet nuget.

Ce readme présente l'architecture du dépôt, les fonctionnalités qui sont implémentées au projet et les pratiques de développement du projet.

##Architecture du dépôt
    -   Duly
        -   CorePackage
            -   Properties
                -   ...
            -   Class1.cs
            -   CorePackage.csproj
            -   CorePackage.sln
        -   CoreTest
            -   Assets
                -   ...
            -   Properties
                -   ...
            -   CoreTest.csproj
            -   Package.appxmanifest
            -   UnitTest.cs
            -   UnitTestApp.xaml
            -   UnitTestApp.xaml.cs
        -   .gitattributes
        -   .gitignore
        -   README.md

##Fonctionnalités

Le projet possède les fonctionnalités suivantes:


##Développement du projet

Pour le développement de ce projet, nous adoptons le mode de développement de gitflow.

Nous allons avoir une branche `master` principale qui va contenir toutes nos release et sur laquelle seront fixée nos étiquettes de version.

Nous allons avoir une branche `develop` qui servira à faire tourner les tests unitaires sur notre projet. Aucune modification du code n'est permis sur cette branche.

Pour chaque fonctionnalité, il vous faudra créer une branche de `feature` du nom de la fonctionnalité que vous voulez implémenter. Une fonctionalité est implémentée lorsque les éléments suivants sont présents:

*   Le code de la fonctionnalité dans le projet `CorePackage`
*   Les tests unitaire correspondants dans une classe à part du projet `CoreTest`
*   La partie `Fonctionnalités` de ce readme est actualisée avec la votre

Un fois que vous avez terminé une fonctionnalité, vous allez devoir la merge sur la branche `develop` ce qui devrai lancer les tests unitaires pour une éventuelle correction de bugs.
Normalement, des tâches sont mises directement dans le team foundation service pour chaque fonctionnalité. Lorsque vous lancez le développement d'une fonctionnalité sur le dépôt, n'oubliez donc pas de lier la tâche du team foundation service à la branche de `feature`.

Lorsque vous trouvez un bug, il vous faudra créer une branche de `hotfix` du nom du bug rencontré.

Lorsque suffisemment de fonctionnalités sont implémentées, nous pouvons décider de faire une `release`. Il nous faudra créer une branche de `release` à partir de la branche `develop`. Celle-ci nous permettra de faire les derniers correctifs avant la release finale avec le merger et l'étiquetage de la version sur la `master`.

Gitflow est initialisé sur le dépôt et vous pourrez l'utiliser au travers de GitKraken qui simplifie tous les mécanismes.