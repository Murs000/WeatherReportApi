# WeatherReportApi

## Overview

**WeatherReportApi** is a comprehensive weather reporting system that fetches weather data from external sources (OpenWeatherMap) and sends detailed reports via email to subscribers. The system uses Quartz.NET for scheduling jobs (daily, weekly, and hourly) and stores the weather forecasts and subscriber information in a PostgreSQL database. This API allows users to manage subscriptions, receive weather updates, and access statistics regarding subscribers and reports sent.

---

## Table of Contents

1. [Technologies Used](#technologies-used)
2. [Project Structure](#project-structure)
3. [Quartz Job Scheduling](#quartz-job-scheduling)
4. [Services and Controllers](#services-and-controllers)
5. [Exception Handling Middleware](#exception-handling-middleware)
6. [Configuration](#configuration)
7. [Installation and Setup](#installation-and-setup)
8. [Usage](#usage)

---

## Technologies Used

- **.NET 8**
- **PostgreSQL**
- **Quartz.NET** (for job scheduling)
- **AutoMapper** (for DTO mapping)
- **OpenWeatherMap API** (for weather data)
- **Swagger** (for API documentation)
- **Newtonsoft.Json** (for JSON handling)

---

## Project Structure

The project follows a layered architecture with a separation of concerns, focusing on scalability and maintainability.

- **Data Layer**: Handles the database and repositories using Entity Framework Core (PostgreSQL).
- **Business Layer**: Contains business logic such as email preparation, weather data retrieval, and statistics calculation.
- **API Layer**: Exposes REST endpoints for managing forecasts, weather data, and reports.

### Key Directories:
- `Controllers/`: Handles all HTTP requests and coordinates with services.
- `Services/`: Contains core logic for email sending, weather data fetching, and job handling.
- `DTOs/`: Data transfer objects used to structure input/output between the layers.
- `QuartzJobScheduler/`: Configures scheduled tasks for sending reports.
- `Middleware/`: Custom exception handling middleware.
- `appsettings.json`: Stores configuration settings such as API keys, job schedules, and email settings.

---

## Quartz Job Scheduling

The **Quartz.NET Job Scheduler** is responsible for scheduling recurring tasks to send weather reports:

1. **Daily Email Job**: Sends a weather report to subscribers every day.
2. **Weekly Email Job**: Sends a summary of weekly weather reports to subscribers.
3. **Hourly Job**: Fetches and stores weather data every hour.

These jobs are configured using Quartz expressions set in `appsettings.json` for easy management. Each job is registered in the `QuartzJobScheduler.cs` file and triggered automatically based on predefined schedules.

### Example Job Configuration (from `appsettings.json`):

```json
{
  "QuartzSettings": {
    "DailyJobCron": "0 0 8 * * ?",   // 8 AM daily
    "WeeklyJobCron": "0 0 8 ? * MON", // 8 AM every Monday
    "HourlyJobCron": "0 0 * * * ?"    // Every hour
  }
}
```

---

## Services and Controllers

### 1. **Email Service**
This service sends fancy HTML emails to subscribers. Emails can be sent daily or weekly, and they include weather details fetched from the OpenWeatherMap API. The email body is dynamically created using the current weather data and subscriber information.

### 2. **Weather API Service**
This service retrieves current weather data and weekly forecasts from the OpenWeatherMap API, converting the responses into structured DTOs for further use. The API key and other relevant configurations are stored in `appsettings.json`.

### 3. **Job Service**
This service is responsible for coordinating the tasks of sending emails and saving reports to the database. It uses the email and weather services to gather data and dispatch it to subscribers.

### 4. **Controllers**
   - **Forecast Controller**: Manages CRUD operations for weather forecasts.
   - **Weather API Controller**: Fetches forecasts for a specific city.
   - **Statistics Controller**: Returns statistical data on subscribers and sent emails.
   - **Job Controller**: Monitors and manages Quartz jobs.
   - **Email Controller**: Allows manual triggering of email reports.

---

## Exception Handling Middleware

To ensure the robustness of the API, a custom **Exception Handling Middleware** is used to capture and handle all exceptions consistently. It provides meaningful error messages to the API consumers by returning structured JSON error responses.

### Sample Exception Response:
```json
{
  "message": "Internal Server Error",
  "statusCode": 500
}
```

This middleware also logs the exceptions for future debugging and can be extended to handle custom exceptions.

---

## Configuration

Configuration settings are managed via `appsettings.json`, including external API keys, email settings, database connection strings, and Quartz job schedules.

### Key Configuration Sections:
- **Logging**: Default logging levels.
- **Connection Strings**: PostgreSQL connection details.
- **External APIs**: Settings for OpenWeatherMap API.
- **Email Settings**: SMTP configuration for sending emails.
- **Quartz Job Schedules**: Cron expressions for scheduling jobs.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=WeatherDB;Username=your_user;Password=your_password"
  },
  "ExternalAPI": {
    "WeatherAPI": {
      "BaseUrl": "http://api.openweathermap.org/data/2.5",
      "ApiKey": "your_api_key",
      "Units": "metric"
    }
  },
  "EmailSettings": {
    "SMTPHost": "smtp.mailtrap.io",
    "SMTPPort": 587,
    "EnableSSL": true,
    "FromEmail": "noreply@airwaz.com"
  }
}
```

---

## Installation and Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-username/airwaz-report-api.git
   ```

2. **Update the `appsettings.json`**:
   - Add your **PostgreSQL** connection string.
   - Add your **OpenWeatherMap API Key**.
   - Add your **SMTP settings** for email configuration.

3. **Build and Run**:
   - Build the project using the .NET CLI:
     ```bash
     dotnet build
     ```
   - Run the application:
     ```bash
     dotnet run
     ```

4. **Access the API**:
   - Once the API is running, you can access the Swagger documentation at `http://localhost:<port>/swagger` to explore and test the endpoints.

---

## Usage

### 1. **Weather Forecast Retrieval**
   - Use the `/api/weather` endpoint to fetch weather data for a specific city.
   
### 2. **Daily and Weekly Reports**
   - Automatically scheduled jobs will send weather reports to subscribers daily and weekly.
   
### 3. **View Statistics**
   - Use the `/api/statistics` endpoint to retrieve insights on subscribers and sent reports.

### 4. **Manage Subscribers**
   - Use the `/api/forecast` controller to add, update, or remove subscribers.
