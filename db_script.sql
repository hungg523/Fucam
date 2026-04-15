-- Tạo Cơ sở dữ liệu (Database) nếu chưa tồn tại
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Fucamp')
BEGIN
    CREATE DATABASE [Fucamp];
END
GO

USE [Fucamp];
GO

-- Tạo Bảng (Table) Registrations
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Registrations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Registrations] (
        [Id]          INT            IDENTITY (1, 1) NOT NULL,
        [ParentName]  NVARCHAR (100) NOT NULL,
        [PhoneNumber] NVARCHAR (20)  NULL,
        [Email]       NVARCHAR (100) NULL,
        [ChildInfo]   NVARCHAR (200) NOT NULL,
        [Status]      INT            CONSTRAINT [DF_Registrations_Status] DEFAULT ((0)) NOT NULL,
        [CreatedAt]   DATETIME2 (7)  CONSTRAINT [DF_Registrations_CreatedAt] DEFAULT (getdate()) NOT NULL,
        
        CONSTRAINT [PK_Registrations] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Tạo Bảng (Table) AdminUsers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdminUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AdminUsers] (
        [Id]           INT            IDENTITY (1, 1) NOT NULL,
        [Username]     NVARCHAR (50)  NOT NULL,
        [PasswordHash] NVARCHAR (MAX) NOT NULL,
        
        CONSTRAINT [PK_AdminUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Cập nhật cột Status nếu bảng đã tồn tại sẵn
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Registrations]') AND name = 'Status')
BEGIN
    ALTER TABLE [dbo].[Registrations] ADD [Status] INT NOT NULL DEFAULT 0;
END
GO

-- Cập nhật cột Notes nếu bảng đã tồn tại sẵn
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Registrations]') AND name = 'Notes')
BEGIN
    ALTER TABLE [dbo].[Registrations] ADD [Notes] NVARCHAR(500) NULL;
END
GO
