# Example project

This is an example of repository which is using the policy toolkit.

## Builder scripts
The policy builder scripts are located directly under source folder. The expression library is in the source/expressions folder.

By running `create-policies.cmd` script you can transform builder scripts and expressions scripts to xml policies. The resulting xml documents will be available under target folder.

## Testing
The test folder contains tests for expressions. It is using the Expressions.Testing library to load, mock context and run expressions.

## Enabling intellisense in script files
The intellisense should be available in script files by default in the example folder. If you do not have it try to build the libraries and restart omnisharp application.

The most important element for enabling intellisense in scripts is `starter.rsp` file. It contains a description for omnisharp with libraries loaded by default as well as default using commands.
