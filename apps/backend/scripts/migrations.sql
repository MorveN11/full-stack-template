CREATE TABLE IF NOT EXISTS public."__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE TABLE public.roles (
        id uuid NOT NULL,
        name text NOT NULL,
        created_at timestamptz NOT NULL DEFAULT (CURRENT_TIMESTAMP),
        updated_at timestamptz DEFAULT (CURRENT_TIMESTAMP),
        is_active boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT pk_roles PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE TABLE public.users (
        id uuid NOT NULL,
        email text NOT NULL,
        password_hash text NOT NULL,
        email_verified boolean NOT NULL,
        created_at timestamptz NOT NULL DEFAULT (CURRENT_TIMESTAMP),
        updated_at timestamptz DEFAULT (CURRENT_TIMESTAMP),
        is_active boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT pk_users PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE TABLE public.email_verification_tokens (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        created_on_utc timestamp with time zone NOT NULL,
        CONSTRAINT pk_email_verification_tokens PRIMARY KEY (id),
        CONSTRAINT fk_email_verification_tokens_users_user_id FOREIGN KEY (user_id) REFERENCES public.users (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE TABLE public.refresh_tokens (
        id uuid NOT NULL,
        token character varying(200) NOT NULL,
        user_id uuid NOT NULL,
        expired_on_utc timestamp with time zone NOT NULL,
        CONSTRAINT pk_refresh_tokens PRIMARY KEY (id),
        CONSTRAINT fk_refresh_tokens_users_user_id FOREIGN KEY (user_id) REFERENCES public.users (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE TABLE public.user_roles (
        user_id uuid NOT NULL,
        role_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone,
        is_active boolean NOT NULL,
        CONSTRAINT pk_user_roles PRIMARY KEY (role_id, user_id),
        CONSTRAINT fk_user_roles_roles_role_id FOREIGN KEY (role_id) REFERENCES public.roles (id) ON DELETE CASCADE,
        CONSTRAINT fk_user_roles_users_user_id FOREIGN KEY (user_id) REFERENCES public.users (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE INDEX ix_email_verification_tokens_user_id ON public.email_verification_tokens (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE UNIQUE INDEX ix_refresh_tokens_token ON public.refresh_tokens (token);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE INDEX ix_refresh_tokens_user_id ON public.refresh_tokens (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE UNIQUE INDEX ix_roles_name ON public.roles (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE INDEX ix_user_roles_user_id ON public.user_roles (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    CREATE UNIQUE INDEX ix_users_email ON public.users (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM public."__EFMigrationsHistory" WHERE "migration_id" = '20250203162839_IntialMigrations') THEN
    INSERT INTO public."__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20250203162839_IntialMigrations', '9.0.0');
    END IF;
END $EF$;
COMMIT;

