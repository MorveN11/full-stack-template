# Manuel - Morales | Clean Architecture Full Stack Template

## Tools and Technologies Needed

- [Dotnet 9.0 SDK](https://dotnet.microsoft.com/download)
- [node.js](https://nodejs.org/en/)
- [pnpm](https://pnpm.io/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

## Package JSON

```json
{
  "name": "template",
  "version": "1.2.0",
  "private": false,
  "description": "A Fullstack Template for .NET 9 and Next.js",
  "scripts": {
    "dev": "pnpm --parallel dev",
    "compose": "docker compose up --build -d",
    "stage": "docker compose -f 'compose-stage.yaml' up --build -d",
    "restore": "pnpm -F=backend restore",
    "build": "pnpm --parallel build",
    "publish": "pnpm run -F=backend publish",
    "migrate:add": "pnpm -F=backend migrate:add",
    "ef:install": "pnpm -F=backend ef:install",
    "ef:bundle": "pnpm -F=backend ef:bundle",
    "lint": "pnpm -F=frontend lint",
    "format": "pnpm -F=frontend format",
    "prepare": "husky"
  },
  "devDependencies": {
    "@commitlint/cli": "^19.5.0",
    "@semantic-release/changelog": "^6.0.3",
    "@semantic-release/commit-analyzer": "^12.0.0",
    "@semantic-release/git": "^10.0.1",
    "@semantic-release/github": "^10.0.5",
    "@semantic-release/npm": "^12.0.1",
    "@semantic-release/release-notes-generator": "^13.0.0",
    "commitizen": "^4.3.1",
    "cz-conventional-changelog": "^3.3.0",
    "husky": "^9.1.6",
    "semantic-release": "^23.1.1"
  },
  "config": {
    "commitizen": {
      "path": "./node_modules/cz-conventional-changelog"
    }
  }
}
```

Update the version of the API in the `package.json` file to 0.1.0. to reset the
version of the API. Update the description of the API in the `package.json` with
your own description. Update any other fields in the `package.json` file as
needed.

Remove `CHANGELOG.md` file to reset the changelog record.

Remove `LICENSE` file to reset the license, use your own license.

## Set Production Migrations

For production, you can use the `ef:bundle` command to bundle the migrations
into a single migration. This will allow you to deploy the application with a
single migration file. To execute the command, run the following:

```sh
pnpm ef:bundle
```

To execute this in a CI/CD pipeline, only uncomment this:

```yaml
# TODO: If you are going to Deploy your API, you can run the migrations here for your production database
# Only you need to add the connection string to your secrets
# - name: Run EF Migrations
#   run: ./efbundle --connection "${{ secrets.DB_CONNECTION_STRING }}"
```

In the `.github/workflows/release.yml` file, uncomment the section that runs the
migrations and set you DB_CONNECTION_STRING in your GitHub Secrets.

## Scripts

- **dev**: Starts frontend and backend in Development Configuration.

  ```sh
  pnpm dev
  ```

- **compose**: Starts only necessary containers for development like, papercut, redis, postgresql

  ```sh
  pnpm compose
  ```

- **stage**: Starts the backend and the frontend application with all the necessary containers to test the hole application like in production

  ```sh
  pnpm stage
  ```

- **restore**: Restores the .NET project dependencies.

  ```sh
  pnpm restore
  ```

- **build**: Builds the .NET and Next.js project.

  ```sh
  pnpm build
  ```

- **publish**: Publishes the .NET project.

  ```sh
  pnpm publish
  ```

- **migrate:add**: Adds a new migration to the .NET project.

  ```sh
  pnpm migrate:add <MigrationName>
  ```

- **ef:install**: Installs the Entity Framework tools.

  ```sh
  pnpm ef:install
  ```

- **ef:bundle**: Bundles the Entity Framework tools.

  ```sh
  pnpm ef:bundle
  ```

- **lint**: Lints the frontend project.

  ```sh
  pnpm lint
  ```

- **format**: Formats the frontend project.

  ```sh
  pnpm format
  ```

- **prepare**: Sets up Husky for managing Git hooks.

  ```sh
  pnpm prepare
  ```

## Ports - localhost

- **Frontend**: 3000
- **API**: 5001
  - **Swagger**: /swagger/index.html
- **Postgres**: 5432
- **Redis**: 6379
- **Papercut**: 8080
- **Seq**: 8081
