-- Скрипт за създаване на таблица ApiKeys в базата данни UretimOtomasyon
-- Изпълнете този скрипт преди да стартирате API-то

USE [UretimOtomasyon]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApiKeys]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ApiKeys](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [DeviceId] [nvarchar](100) NOT NULL,
        [DeviceName] [nvarchar](200) NULL,
        [KeyValue] [nvarchar](500) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] [nvarchar](100) NULL,
        [LastUsedDate] [datetime2](7) NULL,
        [Description] [nvarchar](500) NULL,
        [ExpiryDate] [datetime2](7) NULL,
        [RequestCount] [bigint] NOT NULL DEFAULT 0,
        CONSTRAINT [PK_ApiKeys] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    -- Създаване на уникален индекс върху DeviceId (всеки клиент има един ключ)
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ApiKeys_DeviceId] ON [dbo].[ApiKeys]
    (
        [DeviceId] ASC
    )
    
    -- Индекс върху KeyValue за по-бързо търсене
    CREATE NONCLUSTERED INDEX [IX_ApiKeys_KeyValue] ON [dbo].[ApiKeys]
    (
        [KeyValue] ASC
    )
    
    -- Индекс за активни ключове
    CREATE NONCLUSTERED INDEX [IX_ApiKeys_IsActive_CreatedDate] ON [dbo].[ApiKeys]
    (
        [IsActive] ASC,
        [CreatedDate] DESC
    )
    
    PRINT 'Таблицата ApiKeys е създадена успешно!'
END
ELSE
BEGIN
    PRINT 'Таблицата ApiKeys вече съществува.'
END
GO

