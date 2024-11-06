# Available Policies

The Project is in the development stage.
That means that not all policies are implemented yet.
In this document, you can find a list of implemented policies. For policy details, see the Azure API Management [policy reference](https://learn.microsoft.com/azure/api-management/api-management-policies).

#### :white_check_mark: Implemented policies

* authentication-basic
* authentication-managed-identity
* base
* cache-lookup
* cache-store
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
* set-backend-service
* set-body
* set-header
* set-method
* set-query-parameter
* set-variable
* validate-jwt

Policies not listed here are not implemented yet.

## InlinePolicy

InlinePolicy is a workaround until all the policies are implemented.
It allows you to include policy not implemented yet to the document.

```csharp
c.InlinePolicy("<set-backend-service base-url=\"https://internal.contoso.example\" />");
```
