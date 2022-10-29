# Policy Model

The project contains classes which represents [policy model](https://learn.microsoft.com/en-us/azure/api-management/api-management-policies).

## Available policies

List of policies which are modelled. Currently modelled 9/56.

### General elements (3/3)

- [X] Policy document
- [X] Policy fragment
- [X] Base policy

### Access restriction policies (1/9)

- [X] Check HTTP header
- [ ] Get authorization context
- [ ] Limit call rate by subscription
- [ ] Limit call rate by key
- [ ] Restrict caller IPs
- [ ] Set usage quota by subscription
- [ ] Set usage quota by key 
- [ ] Validate JWT
- [ ] Validate client certificate 

### Advanced policies (2/16)
- [ ] Control flow
- [ ] Forward request
- [ ] Limit concurrency
- [ ] Log to event hub
- [ ] Emit metrics
- [ ] Mock response
- [ ] Retry
- [ ] Return response
- [ ] Send one way request
- [ ] Send request
- [ ] Set HTTP proxy
- [ ] Set variable
- [X] Set request method
- [X] Set status code
- [ ] Trace
- [ ] Wait

### Authentication policies (0/3)

- [ ] Authenticate with Basic
- [ ] Authenticate with client certificate
- [ ] Authenticate with managed identity

### Caching policies (0/3)

- [ ] Get from cache
- [ ] Store to cache
- [ ] Get value from cache
- [ ] Store value in cache
- [ ] Remove value from cache

### Cross-domain policies (0/3)

- [ ] Allow cross-domain calls
- [ ] CORS
- [ ] JSONP

### Dapr integration policies (0/3)

- [ ] Send request to a service
- [ ] Send message to Pub/Sub topic
- [ ] Trigger output binding

### GraphQL API policies (0/2)

- [ ] Validate GraphQL request
- [ ] Set GraphQL resolver

### Transformation policies (2/10)

- [ ] Convert JSON to XML
- [ ] Convert XML to JSON
- [ ] Find and replace string in body
- [ ] Mask URLs in content
- [ ] Set backend service 
- [X] Set body
- [X] Set HTTP header
- [ ] Set query string parameter
- [ ] Rewrite URL
- [ ] Transform XML using an XSLT

### Validation policies (0/4)

- [ ] Validate content
- [ ] Validate parameters
- [ ] Validate headers
- [ ] Validate status code