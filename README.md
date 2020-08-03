# DasContract
A visual language to define contracts between people. 

## Usage
This project requieres .NET Core 3.1 installed and works on Windows, Linux and MacOS platforms.

1. In terminal, go to root directory of this project.
2. (Optional) Replace mortgage example in DasContract.Blockchain.Solidity.Test/example.dascontract with your own DAS Contract code.
3. While still in root directory, run `dotnet run DasContract.Blockchain.Solidity.Test/Program.cs` command.
4. The output Solidity code is shown in terminal and is also generated to code.sol file in root directory of this project.

# Editor
## Introduction

DasContract Editor is a product of my Bachelor thesis.

Live page: https://dascontracteditor.azurewebsites.net/

Repository of a case study contract: https://github.com/CCMiResearch/DEMOCaseStudies/tree/master/Blockchain/Mortgage

Project DasContract focuses on enabling easy creation and managing of SmartContracts. This project – DasContract Editor – serves as an easy to use SmartContract editor. DasContracs produced by this editor can be published to a DasContract store and further processed. 

## Getting started

### Compiling back-end

Compiling back-end and all its C# code is as easy as running "build" action in a Visual Studio editor or via console.

The project you want to run is named `DasContract.Editor.Server`.

### Compiling front-end

Front-end files such as scripts and styles must be compiled using a TypeScript compiler, SASS compiler, and webpack. Each project that utilizes any front-end files provides a set of scripts via package.json file for compiling, building, etc. These are especially useful if you plan to develop, fix or expand the front-end of this project.

The easiest way to compile everything is to run a _builder.exe_ file inside the main project `DasContract.Editor.Server`.

`.\builder.exe --config builder.config.xml`

If you encounter some permission problems with the npm, hit me up. 
