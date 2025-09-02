-- Crear la base de datos si no existe
IF DB_ID('PMDb') IS NULL
    CREATE DATABASE PMDb;
GO

USE PMDb;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- Tabla de usuarios
CREATE TABLE [dbo].[User](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Email] NVARCHAR(150) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Role] NVARCHAR(20) NOT NULL CHECK ([Role] IN ('Admin','Editor')),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- Tabla de categorías
CREATE TABLE [dbo].[Category](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL UNIQUE,
    [ParentCategoryId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Category_Parent FOREIGN KEY ([ParentCategoryId])
        REFERENCES [dbo].[Category]([Id])
);
GO

-- Tabla de productos
CREATE TABLE [dbo].[Product](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Sku] NVARCHAR(50) NOT NULL UNIQUE,
    [Name] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Price] DECIMAL(18,2) NOT NULL CHECK ([Price] >= 0),
    [CategoryId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Product_Category FOREIGN KEY ([CategoryId])
        REFERENCES [dbo].[Category]([Id])
);
GO

-- Tabla de imágenes de producto
CREATE TABLE [dbo].[ProductImage](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ProductId] INT NOT NULL,
    [Url] NVARCHAR(400) NOT NULL,
    [AltText] NVARCHAR(150) NULL,
    [SortOrder] INT NOT NULL DEFAULT(0),
    CONSTRAINT FK_ProductImage_Product FOREIGN KEY ([ProductId])
        REFERENCES [dbo].[Product]([Id]) ON DELETE CASCADE
);
GO

-- Índices
CREATE INDEX IX_Product_Name ON [dbo].[Product]([Name]);
CREATE INDEX IX_Product_Category ON [dbo].[Product]([CategoryId]);
CREATE INDEX IX_Product_IsActive_CreatedAt ON [dbo].[Product]([IsActive],[CreatedAt] DESC);
CREATE INDEX IX_ProductImage_ProductId_SortOrder ON [dbo].[ProductImage]([ProductId], [SortOrder]);
GO