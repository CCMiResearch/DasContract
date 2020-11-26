# DasContract
A visual language allowing to define blockchain smart contracts between people. It is based on the extended combination of DEMO modeling language, BPMN and UML.

## Supported BPMN elements
The modelling part of DasContract is mainly an extension of the [BPMN](https://camunda.com/bpmn). The capabilities of the language are mostly demonstrated in the provided case studies. The following BPMN elements are supported in DasContract v2.0:

- user tasks, business tasks and script tasks
- call activities and subprocesses
- multi instance task types
- exclusive and parallel gateways
- start and end events
- timer boundary events

## Editor
The editor provides a simple interface for the user to create DasContract diagrams, defining the structure of the desired contract.
Currently, the editor only supports the DasContract v1.0. It is also currently located in a separate [repository](https://github.com/drozdik-m/das-contract-editor)

## Solidity Converter
The converter allows to automatically transform .dascontract files created using the editor into [Solidity smart contract language](https://docs.soliditylang.org/en/v0.7.4/), which can be then deployed onto the Ethereum blockchain. It supports the conversion of DasContract v2.0, an example of a conversion can be found in DasContract.CaseStudies/elections.

### Locally deploying the converted code
A number of tools exist, which allow to deploy the Solidity code onto a local testing blockchain.

The easiest way to test the code is the [Remix editor](https://remix.ethereum.org/), which provides a Solidity compiler and contract deployment directly in the browser. It also provides a simple interface, allowing the user to directly interact with the deployed blockchain.

Another tool that can be used is [Ganache](https://www.trufflesuite.com/ganache), which allows to create an Ethereum blockchain on the local machine and provides an interface to interact with the blockchain. Whilst being more complicated to setup, it allows to develop and test more complex applications alongside the blockchain.

## Case Studies
This repository also contains two exemplar case studies, one for each version of the DasContract language. The diagrams and the converted code can be found in the DasContract.CaseStudies folder.

## Authors
This project is created as part of Marek Skotnica's PhD thesis and research. 

Contributors: Jan Frait, Jan Klicpera, Martin Drozdík, and Ondřej Šelder.
