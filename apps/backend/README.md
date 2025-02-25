# Manuel - Morales | Backend Template

## Pending Implementations

```csharp
// TODO: Add API title for Swagger
builder.Services.AddSwaggerGenWithAuth("Clean Architecture API");
```

In the `Presentation` project, you'll find the `Program`. You can add the API
title and description to the Swagger documentation.

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Verification Code</title>
  </head>
  <body
    style="
      font-family: Helvetica, Arial, sans-serif;
      line-height: 1.6;
      color: #333;
      margin: 0;
      padding: 0;
      background-color: #f9f9f9;
    "
  >
    <div
      style="
        max-width: 600px;
        margin: 0 auto;
        padding: 20px;
        background-color: #ffffff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
      "
    >
      <div style="text-align: center; padding: 20px 0">
        <h1>Web Application</h1>
      </div>

      <div style="padding: 20px; background-color: #f9f9f9; border-radius: 4px; margin-bottom: 20px">
        <h2>Verification code</h2>
        <p>Enter the following verification code when prompted:</p>

        <div style="font-size: 32px; font-weight: bold; text-align: center; margin: 20px 0">{{otpCode}}</div>

        <p>To protect your account, do not share this code.</p>
      </div>

      <div style="margin-top: 20px; padding: 15px; background-color: #f9f9f9; border-radius: 4px; font-size: 13px">
        <h3>Didn't request this?</h3>
        <p>If you didn't make this request, you can safely ignore this email.</p>
      </div>

      <div style="font-size: 12px; color: #666; margin-top: 30px; border-top: 1px solid #eee; padding-top: 20px">
        <p>This is an automated message, please do not reply to this email.</p>
      </div>
    </div>
  </body>
</html>
```

In the `Domain` project, you'll the Verify Email Template, You only need to update the Name of your application instead of the `Web Application` Name.

## Scripts

- **dev**: Initializes the .NET project in Development configuration.

  ```sh
  pnpm dev
  ```

- **restore**: Restores the .NET project dependencies.

  ```sh
  pnpm restore
  ```

- **build**: Builds the .NET project in Release configuration without restoring
  dependencies.

  ```sh
  pnpm build
  ```

- **publish**: Publishes the .NET project in Release configuration without
  restoring or building.

  ```sh
  pnpm publish
  ```

- **migrate:add**: Adds a new migration to the `Infrastructure` project.

  ```sh
  pnpm migrate:add <MigrationName>
  ```

- **ef:install**: Installs the Entity Framework CLI tool.

  ```sh
  pnpm ef:install
  ```

- **ef:bundle**: Bundles the migrations into a single migration.

  ```sh
  pnpm ef:bundle
  ```
