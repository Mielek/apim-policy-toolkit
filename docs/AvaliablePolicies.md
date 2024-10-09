# Avaliable Policies

Project is in development stage. That means that not all policies are implemented yet. In this document you can find
list of implemented policies and their description.

#### :white_check_mark: Implemented policies

* authentication-basic
* authentication-managed-identity
* base
* check-header
* choose
* cors
* forward-request
* ip-filter
* mock-response
* quota
* rate-limit-by-key
* rate-limit
* return-response
* rewrite-uri
* send-request
* set-body
* set-header
* set-method
* set-query-parameter
* set-variable
* validate-jwt

#### :x: Not implemented policies

## InlinePolicy

Inline Policy is a workaround until all the policies are implemented. It allows you to include policy not implemented
yet to the document.

```csharp
c.InlinePolicy("<set-backend-service base-url=\"https://internal.contoso.example\" />");
```
