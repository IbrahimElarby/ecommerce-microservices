# ECommerce Microservices (.NET)

This project demonstrates a modern microservices architecture using .NET.

## Technologies

- ASP.NET Core
- Docker
- RabbitMQ
- MassTransit
- Redis
- MongoDB
- PostgreSQL
- SQL Server
- Elasticsearch
- Kibana
- gRPC
- MediatR
- Serilog

## Microservices

- Catalog.API
- Basket.API
- Discount.API
- Ordering.API

## Infrastructure

- RabbitMQ
- Redis
- MongoDB
- PostgreSQL
- SQL Server
- Elasticsearch
- Kibana

## Run with Docker
docker compose up --build

Services:

| Service | Port |
|------|------|
Catalog API | 8000
Basket API | 8001
Discount API | 8002
Ordering API | 8003
RabbitMQ | 15672
Kibana | 5601

## Architecture

Event Driven Architecture using RabbitMQ + MassTransit.
