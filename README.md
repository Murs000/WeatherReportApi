# WeatherReportApi

## Overview
**WeatherReportApi** is a comprehensive weather reporting system that fetches weather data from external sources (OpenWeatherMap) and sends detailed reports via email to subscribers. The system uses Quartz.NET for scheduling jobs (daily, weekly) and stores the weather forecasts and subscriber information in a PostgreSQL database. This API allows users to manage subscriptions, receive weather updates, and access statistics regarding subscribers and reports sent.

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
    "DailyEmailJobSchedule": "0 0 8 * * ?",
    "WeeklyEmailJobSchedule": "0 0 8 ? * 1",
    "HourlyReportJobSchedule": "0 0 0/1 * * ?"
  }
}
```

---

## Services and Controllers

### **1. Email Service**
This service is responsible for sending HTML emails to subscribers. It prepares emails dynamically using the current weather data and subscriber information, ensuring that relevant updates are included.

### **2. Weather API Service**
This service retrieves current weather data and weekly forecasts from the OpenWeatherMap API, converting the responses into structured DTOs for further use. The API key and other relevant configurations are stored in `appsettings.json`.

### **3. Job Service**
This service coordinates the tasks of sending emails and saving reports to the database. It leverages the email and weather services to gather data and dispatch it to subscribers.

### **4. Controllers**
- **Forecast Controller**: Manages CRUD operations for weather forecasts.
- **Weather API Controller**: Fetches forecasts for specific cities.
- **Statistics Controller**: Returns statistical data on subscribers and sent emails.
- **Job Controller**: Monitors and manages Quartz jobs.
- **Email Controller**: Allows manual triggering of email reports.
- **Admin Controller**: Manages user roles and reviews statistics, allowing administrators to see all users and subscribers.

*Note: The **Version 1** endpoints are deprecated and included for testing and CRUD operations only. **Version 2** focuses on application-centric features and provides a robust structure for real-world usage.*

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
  "ExternalApi": {
    "WeatherApiBaseUrl": "https://api.openweathermap.org/data/2.5/",
    "ApiModeCurrent": "weather",
    "ApiModeForWeek": "forecast",
    "ApiKey": "Your_api_key",
    "Units": "metric"
  },
  "EmailSettings": {
    "FromEmail": "Excample@gmail.com",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "Excample@gmail.com",
    "SmtpPassword": "Password_for_Less_secure_apps"
  }
}
```

---

## Installation and Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Murs000/WeatherReportApi.git
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

#### Steps to Set Up with Docker Compose:

1. **Create a `docker-compose.yml` file**:  
   If you haven't already, create a `docker-compose.yml` file in the root directory of your project. Here's an example of how it should look (replace the placeholder values with your own):
   
   ```yaml
   version: '3.8'

   services:
     weatherreportapi:
       image: murs000/weather_report_api:latest  # Imagine from my DockerHub
       container_name: WeatherReport.API
       build:
         context: .
         dockerfile: WeatherReport.API/Dockerfile
       environment:
         - ASPNETCORE_URLS=http://+:5109
         - ConnectionStrings__DefaultConnection=Host=postgresdb;Port=5432;Database=WeatherDB;Username=myuser;Password=mysecretpassword

         # API Key and Email Settings (replace with real values)
         - ExternalApi__ApiKey=abcd1234fakeapikeyabcd5678
         - Jwt__AccessKey=dummyjwtaccesstoken123456==
         - EmailSettings__FromEmail=fakeemail@example.com
         - EmailSettings__SmtpUsername=smtpuser@example.com
         - EmailSettings__SmtpPassword=smtppassword123

       ports:
         - "5109:5109"
       depends_on:
         - postgresdb

     postgresdb:
       image: postgres:latest
       container_name: WeatherReport.DB
       environment:
         POSTGRES_USER: myuser
         POSTGRES_PASSWORD: mysecretpassword
         POSTGRES_DB: WeatherDB
       ports:
         - "5433:5432"
       volumes:
         - ./containers/products-db:/var/lib/postgresql/data
   ```

1. **Run the Docker Compose Setup**:
   Once your `docker-compose.yml` is ready, you can use the following command to build and run the containers:
   ```bash
   docker-compose up -d
   ```
   This will download the necessary images, start the PostgreSQL and API containers, and expose the services as defined in the compose file.

2. **Access the API in Docker**:
   - After the containers are running, you can access the WeatherReport API at `http://localhost:5109` and check Swagger documentation at `http://localhost:5109/swagger/index.html`.
   - PostgreSQL will be available at `localhost:5433`.

3. **Stopping the Containers**:
   To stop and remove the containers, use:
   ```bash
   docker-compose down
   ```

---

## Usage

### **1. Authentication**
- Users can authenticate to the API to manage their subscriptions and access weather data.

### **2. User and Subscriber Management**
- The API allows users to get their current subscriber details, change their subscription preferences, update their city for weather updates, and modify their credentials.

### **3. Daily and Weekly Reports**
- **Daily** and **weekly reports** are automatically scheduled based on user subscriptions, ensuring relevant updates are sent.

### **4. Admin Panel**
- Administrators can manage **user roles** and review statistics for all users and subscribers.

### **5. View Statistics**
- Use the `/api/v2/Admin/stats` endpoint to retrieve insights on subscribers and sent reports.

> Additionally, **Version 2** includes detailed **Swagger annotations** for all endpoints, providing reliable examples and enhancing API documentation for easier integration and usage.
