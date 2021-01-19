IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [Address] (
        [Id] int NOT NULL IDENTITY,
        [HouseNumber] nvarchar(max) NULL,
        [RoadNumber] nvarchar(max) NULL,
        [District] nvarchar(max) NULL,
        CONSTRAINT [PK_Address] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [AppUsers] (
        [Id] nvarchar(450) NOT NULL,
        [CountryCode] nvarchar(max) NULL,
        [MobileNumber] nvarchar(max) NULL,
        [LookingForBride] bit NOT NULL,
        [RegisterByMobileNumber] bit NOT NULL,
        [Email] nvarchar(max) NULL,
        [ValidFrom] datetime2 NOT NULL,
        [ValidTill] datetime2 NOT NULL,
        [UserRole] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        [IsVarified] bit NOT NULL,
        [IsLoggedin] bit NOT NULL,
        CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [CurrentProfession] (
        [Id] int NOT NULL IDENTITY,
        [JobDesignation] nvarchar(max) NULL,
        [OfficeName] nvarchar(max) NULL,
        [SalaryRange] nvarchar(max) NULL,
        CONSTRAINT [PK_CurrentProfession] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [FamilyInfo] (
        [Id] int NOT NULL IDENTITY,
        CONSTRAINT [PK_FamilyInfo] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [BasicInfo] (
        [Id] int NOT NULL IDENTITY,
        [ProfileCreatingFor] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [MaritalStatus] nvarchar(max) NOT NULL,
        [Dob] datetime2 NOT NULL,
        [ContactNumber] nvarchar(max) NOT NULL,
        [Height_cm] int NOT NULL,
        [PresentAddressId] int NOT NULL,
        [Religion] nvarchar(max) NOT NULL,
        [ReligionCast] nvarchar(max) NOT NULL,
        [email] nvarchar(max) NULL,
        [PermanentAddress] nvarchar(max) NULL,
        [PaternalHomeDistrict] nvarchar(max) NULL,
        [MaternalHomeDistrict] nvarchar(max) NULL,
        [PropertyInfo] nvarchar(max) NULL,
        [OtherBasicInfo] nvarchar(max) NULL,
        CONSTRAINT [PK_BasicInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BasicInfo_Address_PresentAddressId] FOREIGN KEY ([PresentAddressId]) REFERENCES [Address] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [EducationInfo] (
        [Id] int NOT NULL IDENTITY,
        [CurrentJobId] int NULL,
        CONSTRAINT [PK_EducationInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EducationInfo_CurrentProfession_CurrentJobId] FOREIGN KEY ([CurrentJobId]) REFERENCES [CurrentProfession] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [FamilyMember] (
        [Id] int NOT NULL IDENTITY,
        [FamilyMemberName] nvarchar(max) NULL,
        [FamilyMemberOccupation] nvarchar(max) NULL,
        [Relationship] nvarchar(max) NULL,
        [FamilyInfoId] int NULL,
        CONSTRAINT [PK_FamilyMember] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FamilyMember_FamilyInfo_FamilyInfoId] FOREIGN KEY ([FamilyInfoId]) REFERENCES [FamilyInfo] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [Education] (
        [Id] int NOT NULL IDENTITY,
        [Degree] nvarchar(max) NULL,
        [InstituteName] nvarchar(max) NULL,
        [PassingYear] nvarchar(max) NULL,
        [Result] nvarchar(max) NULL,
        [EducationInfoId] int NULL,
        CONSTRAINT [PK_Education] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Education_EducationInfo_EducationInfoId] FOREIGN KEY ([EducationInfoId]) REFERENCES [EducationInfo] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE TABLE [Users] (
        [Id] nvarchar(450) NOT NULL,
        [CountryCode] nvarchar(max) NULL,
        [MobileNumber] nvarchar(max) NULL,
        [LookingForBride] bit NOT NULL,
        [RegisterByMobileNumber] bit NOT NULL,
        [Email] nvarchar(max) NULL,
        [ValidFrom] datetime2 NOT NULL,
        [ValidTill] datetime2 NOT NULL,
        [IsPublished] bit NOT NULL,
        [BasicInfoId] int NULL,
        [FamilyInfoId] int NULL,
        [EducationInfoId] int NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_BasicInfo_BasicInfoId] FOREIGN KEY ([BasicInfoId]) REFERENCES [BasicInfo] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Users_EducationInfo_EducationInfoId] FOREIGN KEY ([EducationInfoId]) REFERENCES [EducationInfo] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Users_FamilyInfo_FamilyInfoId] FOREIGN KEY ([FamilyInfoId]) REFERENCES [FamilyInfo] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_BasicInfo_PresentAddressId] ON [BasicInfo] ([PresentAddressId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_Education_EducationInfoId] ON [Education] ([EducationInfoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_EducationInfo_CurrentJobId] ON [EducationInfo] ([CurrentJobId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_FamilyMember_FamilyInfoId] ON [FamilyMember] ([FamilyInfoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_Users_BasicInfoId] ON [Users] ([BasicInfoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_Users_EducationInfoId] ON [Users] ([EducationInfoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    CREATE INDEX [IX_Users_FamilyInfoId] ON [Users] ([FamilyInfoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200926134003_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200926134003_Initial', N'3.1.8');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201001115953_Add_Language_in_AppUser')
BEGIN
    ALTER TABLE [Users] ADD [LanguageChoice] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201001115953_Add_Language_in_AppUser')
BEGIN
    ALTER TABLE [AppUsers] ADD [LanguageChoice] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201001115953_Add_Language_in_AppUser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201001115953_Add_Language_in_AppUser', N'3.1.8');
END;

GO

