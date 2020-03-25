CREATE TABLE [dbo].[straatnaam]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [straatnaam] NVARCHAR(250) NULL, 
    [niscode] INT NOT NULL, 
    CONSTRAINT [FK_straatnaam_gemeente] FOREIGN KEY ([niscode]) REFERENCES [gemeente]([niscode])
)
