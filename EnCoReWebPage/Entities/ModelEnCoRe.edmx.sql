
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/13/2021 11:33:18
-- Generated from EDMX file: D:\Visual Studio 2019\source\Culture Web\EnCoReWebPage\EnCoReWebPage\Entities\ModelEnCoRe.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BD_EnCoRe];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Article_Categorie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [FK_Article_Categorie];
GO
IF OBJECT_ID(N'[dbo].[FK_Article_Utilisateur]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [FK_Article_Utilisateur];
GO
IF OBJECT_ID(N'[dbo].[FK_ArticleArticle_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleTags] DROP CONSTRAINT [FK_ArticleArticle_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_Commentaire_Article]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Commentaires] DROP CONSTRAINT [FK_Commentaire_Article];
GO
IF OBJECT_ID(N'[dbo].[FK_TagArticle_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleTags] DROP CONSTRAINT [FK_TagArticle_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_Utilisateur_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Utilisateurs] DROP CONSTRAINT [FK_Utilisateur_Role];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Articles];
GO
IF OBJECT_ID(N'[dbo].[ArticleTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticleTags];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Commentaires]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Commentaires];
GO
IF OBJECT_ID(N'[dbo].[PasswordRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PasswordRequests];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Utilisateurs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Utilisateurs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Articles'
CREATE TABLE [dbo].[Articles] (
    [Id] uniqueidentifier  NOT NULL,
    [Image] nvarchar(max)  NOT NULL,
    [Titre] nvarchar(max)  NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL,
    [Parution] datetime  NOT NULL,
    [CategorieId] uniqueidentifier  NOT NULL,
    [UtilisateurId] uniqueidentifier  NOT NULL,
    [Valider] bit  NOT NULL
);
GO

-- Creating table 'ArticleTags'
CREATE TABLE [dbo].[ArticleTags] (
    [Id] uniqueidentifier  NOT NULL,
    [ArticleId] uniqueidentifier  NOT NULL,
    [TagId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] uniqueidentifier  NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Commentaires'
CREATE TABLE [dbo].[Commentaires] (
    [Id] uniqueidentifier  NOT NULL,
    [Date] datetime  NOT NULL,
    [CommentateurNomPrenoms] nvarchar(max)  NOT NULL,
    [CommentateurEmail] nvarchar(max)  NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL,
    [Valider] bit  NOT NULL,
    [ArticleId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PasswordRequests'
CREATE TABLE [dbo].[PasswordRequests] (
    [Id] uniqueidentifier  NOT NULL,
    [RequestDate] datetime  NOT NULL,
    [UtilisateurId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] uniqueidentifier  NOT NULL,
    [Nom] nvarchar(max)  NOT NULL,
    [Article] bit  NOT NULL,
    [Categorie] bit  NOT NULL,
    [Role_] bit  NOT NULL,
    [Utilisateur] bit  NOT NULL,
    [Valider_Article] bit  NOT NULL,
    [Valider_Commentaire] bit  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] uniqueidentifier  NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Utilisateurs'
CREATE TABLE [dbo].[Utilisateurs] (
    [Id] uniqueidentifier  NOT NULL,
    [Nom] nchar(50)  NOT NULL,
    [Prenoms] nchar(120)  NOT NULL,
    [Photo] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Login] nvarchar(max)  NULL,
    [PWD] nvarchar(max)  NULL,
    [Apropos] nvarchar(max)  NULL,
    [Blog] nvarchar(max)  NULL,
    [Actif] bit  NOT NULL,
    [RoleId] uniqueidentifier  NOT NULL,
    [UtilisateurId] uniqueidentifier  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [PK_Articles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticleTags'
ALTER TABLE [dbo].[ArticleTags]
ADD CONSTRAINT [PK_ArticleTags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Commentaires'
ALTER TABLE [dbo].[Commentaires]
ADD CONSTRAINT [PK_Commentaires]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [RequestDate], [UtilisateurId] in table 'PasswordRequests'
ALTER TABLE [dbo].[PasswordRequests]
ADD CONSTRAINT [PK_PasswordRequests]
    PRIMARY KEY CLUSTERED ([Id], [RequestDate], [UtilisateurId] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Utilisateurs'
ALTER TABLE [dbo].[Utilisateurs]
ADD CONSTRAINT [PK_Utilisateurs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategorieId] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [FK_Article_Categorie]
    FOREIGN KEY ([CategorieId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Article_Categorie'
CREATE INDEX [IX_FK_Article_Categorie]
ON [dbo].[Articles]
    ([CategorieId]);
GO

-- Creating foreign key on [UtilisateurId] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [FK_Article_Utilisateur]
    FOREIGN KEY ([UtilisateurId])
    REFERENCES [dbo].[Utilisateurs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Article_Utilisateur'
CREATE INDEX [IX_FK_Article_Utilisateur]
ON [dbo].[Articles]
    ([UtilisateurId]);
GO

-- Creating foreign key on [ArticleId] in table 'ArticleTags'
ALTER TABLE [dbo].[ArticleTags]
ADD CONSTRAINT [FK_ArticleArticle_Tag]
    FOREIGN KEY ([ArticleId])
    REFERENCES [dbo].[Articles]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticleArticle_Tag'
CREATE INDEX [IX_FK_ArticleArticle_Tag]
ON [dbo].[ArticleTags]
    ([ArticleId]);
GO

-- Creating foreign key on [ArticleId] in table 'Commentaires'
ALTER TABLE [dbo].[Commentaires]
ADD CONSTRAINT [FK_Commentaire_Article]
    FOREIGN KEY ([ArticleId])
    REFERENCES [dbo].[Articles]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Commentaire_Article'
CREATE INDEX [IX_FK_Commentaire_Article]
ON [dbo].[Commentaires]
    ([ArticleId]);
GO

-- Creating foreign key on [TagId] in table 'ArticleTags'
ALTER TABLE [dbo].[ArticleTags]
ADD CONSTRAINT [FK_TagArticle_Tag]
    FOREIGN KEY ([TagId])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TagArticle_Tag'
CREATE INDEX [IX_FK_TagArticle_Tag]
ON [dbo].[ArticleTags]
    ([TagId]);
GO

-- Creating foreign key on [RoleId] in table 'Utilisateurs'
ALTER TABLE [dbo].[Utilisateurs]
ADD CONSTRAINT [FK_Utilisateur_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Utilisateur_Role'
CREATE INDEX [IX_FK_Utilisateur_Role]
ON [dbo].[Utilisateurs]
    ([RoleId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------