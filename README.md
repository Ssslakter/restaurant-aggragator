# Restaurant Aggregator

## Contents
* [Components](#components)
* [How to Run](#how-to-run)
* [Configuration](#configuration)
* [Features](#features)
* [Alternative Solutions](#alternative-solutions)

## Components
* **MVC** - an admin panel that allows managing restaurants and users/roles
* **API** - the main service for working with restaurants
* **Auth** - service for user authentication
* **Notifications** - service for sending notifications

## How to Run
1. Raise dependencies through the `docker-compose.DB.yml` file
2. It's important that the environment is `Development` during the first run; in this environment, code runs that automatically adds service accounts and an admin to the database
3. Run one or several components

Admin credentials
```sh
123@admin.com
String123
```

## Configuration
All necessary settings, such as Jwt configs, database credentials, etc., are stored in the `appsettings.json` file in each component that is launched.
## Features
1. When forming an order in the API, the current price of the dish is fixed, even if it changes later
2. To obtain the name of the courier and client, the API service uses Auth.Client to send requests to the Auth service using service account credentials
3. When changing the order status in the API, a notification is sent to the RabbitMQ queue, which is processed by the Notifications service
4. Cookie authorization implemented in MVC
5. In MVC, it's possible to add staff to a restaurant via select
6. Custom errors and middleware for logging
7. Configuration settings, attributes, swagger moved to a separate Infra component

## Alternative Solutions
* MVC has access to the API and Auth databases, which avoids extra synchronous requests, but this violates the principles of microservice architecture. It would be possible to create a package for the API client and send requests through it and Auth.Client
* Removing MVC...
