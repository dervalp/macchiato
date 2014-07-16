# Macchiato

This repository contains a Sitecore MVC demo site that can be deployed onto a Sitecore 8.x installation.

## Building

In order to build the Speak.Demo solution you will need to:

- Ensure that [NuGet package restore](http://docs.nuget.org/docs/reference/package-restore) is enabled 

- Your local developer environment can be defined in the `build\environment.props` file. Set the **LibsSrcPath** to reference a folder containing the Sitecore assemblies for 8.0. 
 

*DO NOT COMMIT changes made to the `build\environment.props` file back to GIT*
git status


### Running a build from the command line

The project can be built from the command line.

    .\build

The build will compile the project code, run all of the automated unit tests.



## Deploying the Speak.Demo site
  
### Prerequisites

You will need to have a local install of Sitecore that the Speak.Demo site can be deployed onto.

In the `build\environment.props` file set the **TestWebsitePath** and **TestWebsiteUrl** to reference your local Sitecore website.

Rename the `src\Speak.Demo\App_Config\Include\DataFolder.config.example` file to remove the example suffix and use the file to specify where your target website's data folder is located. Do not commit this change back to GIT as it will impact others working on the project.


### Deploying your changes

The following command will build and deploy the Speak.Demo site and install the content contained within the Speak.Demo.Content project into your local Sitecore website.

    .\deploy [<publish profile> [<config>]] 

Where:

* `<publish profile>` is a publish profile in the Properties folder of the Speak.Demo project. You do not need to specify the *.pubxml* file extension for the `<publish profile>` parameter. If no `<publish profile>` parameter is supplied on the command line it will default to `c-speak-demo`.

* `<config>` is the Visual Studio solution configuration to build with. This parameter will be defaulted to `Debug` if omitted from the command line.


*Note:* If you receive a Web deployment task error relating to the identity performing the deployment re-run the depoyment command from a command prompt using an account that has privileges to access the IIS configuration details.

As part of the deployment task the Speak.Demo Sitecore update package will be installed into the target website and after installation a smart publish will be run on the site.
 

### Running the post-deployment tests

From a command prompt configure the site url that you wish to test:

   set TestWebsiteUrl=<your site url>

Where:

* `<your site url>` is http://yourhostname


Then run the following command:

    .\build RunSmokeTests


The smoke tests are comprised of a set of cUrl commands to exercise simple GET and POST requests on the  website and Selenium WebDriver tests to exercise more interactive user journeys. The Selenium tests will require the Firefox browser to be installed on the machine running the tests. All other tools used by the smoke tests are contained within the Speak.Demo folder structure and are source controlled.

## Useful GIT commands

Using the following GIT aliases:

    [alias]
        ignore = update-index --assume-unchanged
        unignore = update-index --no-assume-unchanged
        ignored = !git ls-files -v | grep "^[[:lower:]]"



Then run the following commands to ignore changes to local development files:

    git ignore build\environment.props

    git ignore src\Speak.Demo\App_Config\Include\DataFolder.config


