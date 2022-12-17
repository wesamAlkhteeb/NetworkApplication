IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [roles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [users] (
        [Id] int NOT NULL IDENTITY,
        [Username] nvarchar(40) NOT NULL,
        [Password] nvarchar(60) NOT NULL,
        [Email] nvarchar(40) NOT NULL,
        [Confirm] bit NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_users_roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [groups] (
        [GroupId] int NOT NULL IDENTITY,
        [GroupName] nvarchar(30) NOT NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_groups] PRIMARY KEY ([GroupId]),
        CONSTRAINT [FK_groups_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [loggings] (
        [LoggingId] int NOT NULL IDENTITY,
        [LoggingAction] nvarchar(100) NOT NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_loggings] PRIMARY KEY ([LoggingId]),
        CONSTRAINT [FK_loggings_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [files] (
        [FilesId] int NOT NULL IDENTITY,
        [FileName] nvarchar(60) NOT NULL,
        [FilePath] nvarchar(max) NOT NULL,
        [FileIdUses] int NOT NULL,
        [FileCreateDate] datetime2 NOT NULL,
        [GroupId] int NOT NULL,
        CONSTRAINT [PK_files] PRIMARY KEY ([FilesId]),
        CONSTRAINT [FK_files_groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [groups] ([GroupId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [permessionsGroups] (
        [PermessionsGroupId] int NOT NULL IDENTITY,
        [PermessionsGroupSharedId] int NOT NULL,
        [GroupId] int NOT NULL,
        CONSTRAINT [PK_permessionsGroups] PRIMARY KEY ([PermessionsGroupId]),
        CONSTRAINT [FK_permessionsGroups_groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [groups] ([GroupId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE TABLE [reports] (
        [ReportId] int NOT NULL IDENTITY,
        [State] nvarchar(6) NOT NULL,
        [Date] datetime2 NOT NULL,
        [FileId] int NOT NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_reports] PRIMARY KEY ([ReportId]),
        CONSTRAINT [FK_reports_files_FileId] FOREIGN KEY ([FileId]) REFERENCES [files] ([FilesId]) ON DELETE CASCADE,
        CONSTRAINT [FK_reports_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_files_GroupId] ON [files] ([GroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_groups_UserId] ON [groups] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_loggings_UserId] ON [loggings] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_permessionsGroups_GroupId] ON [permessionsGroups] ([GroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_reports_FileId] ON [reports] ([FileId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_reports_UserId] ON [reports] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    CREATE INDEX [IX_users_RoleId] ON [users] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221205184745_create_database')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221205184745_create_database', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207115630_add_concurrency')
BEGIN
    ALTER TABLE [files] ADD [Version] rowversion NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207115630_add_concurrency')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221207115630_add_concurrency', N'7.0.0');
END;
GO

COMMIT;
GO

