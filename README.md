# Introduction

DasContract Editor is a product of my Bachelor thesis.

Project DasContract focuses on enabling easy creation and managing of SmartContracts. This project – DasContract Editor – serves as an easy to use SmartContract editor. DasContracs produces by this editor can be published to a DasContract store and further processed. 

# Getting started

## Compiling back-end

Compiling back-end and all its C# code is as easy as running "build" action in a Visual Studio editor or via console.

The project you want to run is named `DasContract.Editor.Server`.

## Compiling front-end

Front-end files such as scripts and styles must be compiled using a TypeScript compiler, SASS compiler, and webpack. Each project that utilizes any front-end files provides a set of scripts via package.json file for compiling, building, etc. These are especially useful if you plan to develop, fix or expand the front-end of this project.

The easiest way to compile everything is to run a _builder.exe_ file inside the main project `DasContract.Editor.Server`.

`.\builder.exe --config builder.config.xml`

The builder is set to run automatically on the first build (right after fresh clone). This may, however, cause permission problems.

If you encounter some permission problems with the npm, hit me up. 
