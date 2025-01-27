# PriceAlert WebAPI

This project demonstrates a simple WebAPI service for setting, checking, and removing low-price alerts for securities.

## Features

- **Set Low Price Alert**: Allows users to set alerts for a security if its price falls below a specified limit.
- **Get Alerts**: Retrieves a list of all registered alerts.
- **Check Alerts**: Checks and returns alerts that would be triggered if a security’s price reaches a certain level.
- **Remove Alert**: Removes a specific alert from the system.

## API Endpoints

### `POST /SetLowPriceAlert`

Registers an alert for the specified user, security, and price.

**Request Body**:
```json
{
  "username": "user123",
  "ticker": "AAPL",
  "priceLimit": 150.00
}
```

### `GET /GetAlerts`

Returns all active alerts.

**Response**:
```json
[
  { "username": "user123", "ticker": "AAPL", "priceLimit": 150.00 },
  { "username": "user456", "ticker": "GOOG", "priceLimit": 2800.00 }
]
```

### `GET /CheckAlerts`

Checks and returns alerts that would be triggered if a security’s price reaches the given value.

**Request Body**:
```json
{
  "ticker": "AAPL",
  "price": 149.50
}
```

**Response**:
```json
[
  { "username": "user123", "ticker": "AAPL", "priceLimit": 150.00 }
]
```

### `DELETE /RemoveAlert`

Removes a specific alert based on the provided username and ticker.

**Request Body**:
```json
{
  "username": "user123",
  "ticker": "AAPL"
}
```

**Response**:
```json
{
  "message": "Alert for user123 on AAPL removed successfully."
}
```

## Database Tables

### 1. **Alerts Table**
This table stores all active price alerts set by users.

- `AlertId` (Primary Key): A unique identifier for each alert.
- `Username`: The user who set the alert.
- `Ticker`: The security symbol for the alert (e.g., AAPL, GOOG).
- `PriceLimit`: The price at which the alert is triggered.
- `DateCreated`: The date and time when the alert was created.

```sql
CREATE TABLE Alerts (
    AlertId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Ticker NVARCHAR(10) NOT NULL,
    PriceLimit DECIMAL(18, 2) NOT NULL,
    DateCreated DATETIME DEFAULT GETDATE()
);
```

### 2. **TriggeredAlerts Table**
This table logs each time an alert is triggered.

- `TriggerId` (Primary Key): A unique identifier for each triggered alert.
- `AlertId` (Foreign Key): References the `AlertId` in the Alerts table.
- `DateTriggered`: The date and time when the alert was triggered.

```sql
CREATE TABLE TriggeredAlerts (
    TriggerId INT PRIMARY KEY IDENTITY(1,1),
    AlertId INT,
    DateTriggered DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AlertId) REFERENCES Alerts(AlertId)
);
```

### 3. **ApiActivity Table**
This table logs all API activity for auditing purposes.

- `ActivityId` (Primary Key): A unique identifier for each API activity record.
- `Endpoint`: The endpoint that was called (e.g., `/SetLowPriceAlert`, `/GetAlerts`).
- `Timestamp`: The date and time when the API call occurred.
- `RequestPayload`: The data sent in the request body.
- `ResponsePayload`: The data returned in the response.

```sql
CREATE TABLE ApiActivity (
    ActivityId INT PRIMARY KEY IDENTITY(1,1),
    Endpoint NVARCHAR(255) NOT NULL,
    Timestamp DATETIME DEFAULT GETDATE(),
    RequestPayload NVARCHAR(MAX),
    ResponsePayload NVARCHAR(MAX)
);
```

## Setup Instructions

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build and run the application.
