# DNAI Core

This project aims to simplify AI creation through a visual editor combined with a plugin able to understand and execute AI behaviours in any other third party program.
The Core is the heart of the project, able to generate and manage AIs, as well as communicate with the other bricks constituting the DNAI solution.

## Getting Started

This project requires Visual Studio Enterprise 2017 to be built and executed. The VS file contains all the needed projects to compile and run the Core, as well as the plugin.
See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them:

- [Visual Studio Enterprise 2017](https://duly-eip.visualstudio.com/Duly?line=15&lineStyle=plain&lineEnd=15&lineStartColumn=3&lineEndColumn=32)
- [Unity 2017](https://unity3d.com/)

An account is needed for Unity 2017. Any personnal account is enough.

Packages might be missing for Visual Studio Enterprise. If so, do the following:
```
Open Visual Studio Installer
```
This will show you the features currently installed for Visual Studio.
```
Select "Modify" and tick the missing components, then update.
```

### Installing

How to get a development env running:

Clone the project into any folder. Then open Visual Studio

```
Ctrl+Shift+B will build all the projects.
```

If the plugin build fails, make sure the following paths exists within the Unity Project:
```
Assets/Standard Assets/DNAI/
Assets/Standard Assets/DNAI/Plugins/
Assets/Standard Assets/DNAI/Editor/
```

You can test the plugin by opening the Unity project, then selecting Window => DNAI, or pressing **Alt+D**.

## Running the tests

Explain how to run the automated tests for this system
Running the tests is made easy within Visual Studio. Press **Ctrl+R/Ctrl+A** to run all the tests.
You can also open the **Test Window** or run a single test manually, through the Unit Tests Projects. For more information, see [how to write and use unit tests](https://msdn.microsoft.com/en-us/library/hh694602.aspx).

### Core tests

Tests for the Core library.

### Plugin tests

Tests for the Unity plugin. If you want to run the tests inside Visual Studio, comment the line:

```
#define UNITY
```

### Network tests

Tests for the network library. Make sure everything is running so the tests can connect to the needed IP addresses.

### Creating new tests

Inside Visual Studio, create a new ***Project -> Test Unit***, then add your tests inside of it. One testing unit should only target one project.

## Deployment

Librairies can be updated using the *Nuget Package System* within Visual Studio. The plugin itself should be deployed the the *Unity Store*, by using the Unity Deploying Tools.

## Built With

* [Visual Studio](http://www.dropwizard.io/1.0.2/docs/) - Used to code and compile
* [Unity](https://maven.apache.org/) - Plugin
* [Newton Soft](https://rometools.github.io/rome/) - Json Management

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [Team Foundation Server](https://www.visualstudio.com/team-services/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

## Authors

* **Fernand Veyrier** - *Initial work*
* **Quentin Gasparotto** - *Core*
* **Victor Gouet** - *GUI*

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

* Hat tip to anyone who's code was used
* Inspiration
* etc
