# Manuel - Morales | Backend Template

## Pending Implementations

```csharp
// TODO: Add API title for Swagger
builder.Services.AddSwaggerGenWithAuth("Clean Architecture API");
```

In the `Presentation` project, you'll find the `Program`. You can add the API
title and description to the Swagger documentation.

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
