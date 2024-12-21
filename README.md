# .NET_Microservices

## APIS

### Catalog API

1. `GET /items`
2. `GET /items/{id}`
3. `POST /items`
4. `PUT /items/{id}`
5. `DELETE /items/{id}`

### Inventory API

1. `POST /items` - Adds an items to a player's inventory bag
2. `GET /items?userId={userId}` - Gets all items of a player

### Situation At Hand (Problem with sync communication):

1. Suppose our dependent services are down so our service's latency increases
2. We also need to take care of `SLA(Service Level Agreement)` for our service
3. Its not guaranteed that all the dependent services have sae SLA hence impacting our own SLA

- These are the reasons why we opt for Async communication style
- Due to lack of coupling between the microservices the autonomy of the microservices are enforced
- Also async data propagation is possible across services

### Async communication in our project

1. Wheneber a new item is added/updated/deleted to catalog we publish an event to the message broker
2. The event doesn't need to have all the details of the item, only the ones that are necessary by other services(Inventory here)
3. Our client services(Inventory) will listen to these events and create its own collection of catalog items in its own Database
4. As long as we have highly available message broker the list of catalog items in the inventory service will be eventually consistent
5. Hence no need of any API calls to Catalog microservice hence no added latency(removed inter-service latency)

- Note: We could have directly used `RabbitMQ` in our code but it would have coupled our code and in future if we wanted to move to some other message broker we would need to rewrit the code again instead we will use `Mass Transit` (Open source distributed application framework for .NET)
