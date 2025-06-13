# Umami Cloud API Wrapper

This project provides a simplified API wrapper for interacting with the [Umami Cloud API](https://umami.is/), making it easier to request website statistics using human-friendly time ranges.

## Purpose

The Umami Cloud API requires time ranges to be specified in milliseconds since the Unix epoch. This can be cumbersome when you want to fetch stats for common periods like "today", "last 24 hours", "last 7 days", or "last 30 days" using a dashboard such as [Homepage](https://gethomepage.dev/).

This wrapper allows you to programmatically define these time ranges using intuitive labels, which are then automatically converted to the required millisecond format for the Umami API.

## Requirements

- You must sign up for an account on [Umami Cloud](https://umami.is/).
- After signing up, generate an API key from your Umami Cloud dashboard.
- Use this API key in the `x-umami-api-key` header for all requests to the wrapper.

## Features

- Exposes endpoints to fetch website stats using easy-to-understand time ranges.
- Handles conversion of time ranges (e.g., `24hours`, `7days`, `30days`, etc.) to the millisecond values expected by the Umami Cloud API.

## Example Usage (Local Testing)

To run the API locally for testing, use the following command:

```
dotnet run --launch-profile https
```

Then you can get stats for the last 7 days for a website:

```
GET /api/{websiteId}/stats?range=7days
Headers:
  x-umami-api-key: <your-api-key>
```

Supported ranges include:

- `today`
- `24hours`
- `7days`
- `30days`
- `60days`
- `90days`
- `6months`
- `9months`
- `1year`

## Running in Docker

You can run this API using Docker Compose. Here is an example service definition:

```yaml
services:
  umami-cloud-api-wrapper:
    container_name: umami-cloud-api-wrapper
    image: ghcr.io/briancollet/umami-cloud-api-wrapper:1.0.0
    environment:
      - ASPNETCORE_HTTP_PORTS=7887
    ports:
      - 7887:7887
    restart: unless-stopped
```

## Project Structure

- `Api/Controllers/UmamiController.cs`: Main API controller handling requests and time range conversion.
- `Api/Models/`: Data models for metrics and website stats.

## Getting Started

1. Clone the repository.
2. Build and run the project using .NET 9.
3. Use the provided endpoints to interact with the Umami Cloud API using simple time range queries.

## Feature Checklist

- [ ] Add other Umami endpoints such as `/api/websites/:websiteId/active` and `/api/websites/:websiteId/events`
- [ ] Logging
- [ ] Unit testing
- [x] Dockerize
- [ ] Minimal API version

Opening issues and submitting feature requests is encouraged and appreciated! If you have ideas for improvements, encounter bugs, or want to request new features, please open an issue or submit a pull request on this repository.
