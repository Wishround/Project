CREATE TABLE [dbo].[Wishes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Code] NVARCHAR(MAX) NOT NULL, 
    [Cost] FLOAT NOT NULL, 
    [Currency] NVARCHAR(50) NOT NULL, 
    [ImageURL] NVARCHAR(MAX) NULL, 
    [IsCompleted] BIT NOT NULL, 
    [UserId] NVARCHAR(36) NOT NULL
)
