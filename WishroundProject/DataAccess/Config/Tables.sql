CREATE TABLE [dbo].[Wishes] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [PublicId]    NVARCHAR (36)  NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [Code]        NVARCHAR (MAX) NOT NULL,
    [Cost]        FLOAT (53)     NOT NULL,
    [Currency]    NVARCHAR (50)  NOT NULL,
    [ImageURL]    NVARCHAR (MAX) NULL,
    [IsCompleted] BIT            NOT NULL,
    [UserId]      NVARCHAR (36)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([PublicId] ASC)
);


CREATE TABLE [dbo].[Orders] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [PublicId]    NVARCHAR (36)  NOT NULL,
    [Status] NVARCHAR (36) NOT NULL,
    [WishId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Orders_ToWishes] FOREIGN KEY ([WishId]) REFERENCES [dbo].[Wishes] ([Id])
);

