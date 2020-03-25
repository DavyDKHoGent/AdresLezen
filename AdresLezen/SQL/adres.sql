CREATE TABLE [dbo].[adres]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [straatnaamid] INT NOT NULL, 
    [huisnummer] NVARCHAR(10) NOT NULL, 
    [appartementnummer] NVARCHAR(25) NULL, 
    [busnummer] NVARCHAR(25) NULL, 
    [huisnummerlabel] NVARCHAR(100) NULL, 
    [adreslocatieid] INT NULL, 
    CONSTRAINT [FK_adres_adreslocatie] FOREIGN KEY ([adreslocatieid]) REFERENCES [adreslocatie]([Id]), 
    CONSTRAINT [FK_adres_straatnaam] FOREIGN KEY ([straatnaamid]) REFERENCES [straatnaam]([Id])
)
