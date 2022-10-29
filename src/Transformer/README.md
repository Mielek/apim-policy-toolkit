# Policy transformer tool

The project contains source code for policy transformer tool. It should be used to convert script build policy model to xml document.

## Installation

`dotnet tool install --add-source ../output Mielek.Transformer`

## Usage

`dotnet policy-transformer --s ./source --t ./target`

### Options

#### Required
- source [s] - define path to folder where source of builder scripts are located
- target [t] - define path to folder where generated xml documents should be placed 

#### Optional
- extension [e] - define extension patter for files (default *.csx)
- add-imports - true/false flag to define if script runner should add imports to script files (default true)
- remove-directives - true/false flag to define if script runner should remove directives