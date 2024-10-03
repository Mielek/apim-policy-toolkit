﻿# Contributor Guide

In this guide, we will cover the following topics:

1. Setting up the development environment for this repository.
2. Building, testing and packing the project.
3. How to contribute to this repository.

## Set up environment

### Dev container

Repository contains a dev container which contains all the required SDKs to develop toolkit.
See dev container [website](https://containers.dev/supporting) for more information how to run it.

### Setting up the development environment

* Check that you have latest [.NET SDK 8 sdk](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) version installed.
* Clone the repository.
* Run `dotnet restore` to restore the dependencies.
* Run `dotnet build` to build the project.

## Building, testing and packing projects

### Toolkit solution

From the repository root directory:

* Run `dotnet build` to build the project.
* Run `dotnet test` to run the tests.
* Run `dotnet pack` to pack the project. The package will be created in the `output` directory.

### Set up the example solution

* Open repository folder in terminal.
* Run script to set up example solution or check [manual](#manual-example-project-build) project build.
    ```shell
    setup-example.cmd
    ```
* Open `examples` folder in your IDE of choice (tested: VS, VS code, Raider).

### Manual example project build

* Run command to build the project.
    ```shell
    dotnet build
    ```
* Run command to create nuget package. Package will be created in `output` folder.
    ```shell
    dotnet pack
    ```
* Open `examples` folder in terminal.
* Run command to build the example project.
   ```shell
   dotnet build
   ```
* Run command to test the example project.
   ```shell
   dotnet test
   ```
* Run command restore compiler tool in the example project.
   ```shell
   dotnet tool restore
  ```
* Run command to compile policies in the example project.
   ```shell
   dotnet policy-compiler --s .\source\ --o .\target\ --format true
   ```

## Contributor guidelines

TODO - Add guidelines for contributing to this repository.