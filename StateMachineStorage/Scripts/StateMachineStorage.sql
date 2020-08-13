IF NOT EXISTS (SELECT *
FROM sys.schemas
WHERE name = N'STATE_MACHINE')
EXEC('CREATE SCHEMA [STATE_MACHINE] AUTHORIZATION [dbo]');
GO

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_SMImplementation_SMDefinition]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[SMImplementation]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[SMImplementation] DROP CONSTRAINT [FK_SMImplementation_SMDefinition]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_SMImplementation_ElementType]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[SMImplementation]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[SMImplementation] DROP CONSTRAINT FK_SMImplementation_ElementType
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_SMImplementation_Element]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[SMImplementation]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[SMImplementation] DROP CONSTRAINT FK_SMImplementation_Element
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_Element_ElementType]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[Element]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[Element] DROP CONSTRAINT FK_Element_ElementType
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_Element_SMDefinition]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[Element]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[Element] DROP CONSTRAINT FK_Element_SMDefinition
END


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='EventLog')
BEGIN
DROP TABLE STATE_MACHINE.EventLog
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='ElementType')
BEGIN
DROP TABLE STATE_MACHINE.ElementType
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='Element')
BEGIN
DROP TABLE STATE_MACHINE.Element
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='SMImplementation')
BEGIN
DROP TABLE STATE_MACHINE.SMImplementation
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='SMDefinition')
BEGIN
DROP TABLE STATE_MACHINE.SMDefinition
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='TransitionTrigger')
BEGIN
DROP TABLE STATE_MACHINE.TransitionTrigger
END

CREATE TABLE STATE_MACHINE.EventLog
(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[CallerMethod] nvarchar(200),
[Message] nvarchar(max),
[Exception] nvarchar(max),
[Date] DateTime NOT NULL
)

CREATE TABLE STATE_MACHINE.ElementType(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[Name] nvarchar(250)
)

CREATE TABLE STATE_MACHINE.TransitionTrigger(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[Name] nvarchar(250)
)

CREATE TABLE STATE_MACHINE.Element(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[ElementTypeId] bigint NOT NULL,
[SMDefinitionId] bigint NOT NULL,
[OldStateId] bigint NULL,
[NewStateId] bigint NULL,
[TransitionTriggerId] bigint NULL,
[Name] nvarchar(250),
)

CREATE TABLE STATE_MACHINE.SMDefinition(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[InitialStateId] bigint NULL,
[Agenda] nvarchar(250),
[Name] nvarchar(250),
[Version] nvarchar(100),
[Definition] nvarchar(MAX)
)

CREATE TABLE STATE_MACHINE.SMImplementation(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[ElementId] bigint NOT NULL,
[ElementTypeId] bigint NOT NULL,
[SMDefinitionId] bigint NOT NULL,
[Name] nvarchar(250),
[Implemetation] nvarchar(MAX)
)

ALTER TABLE STATE_MACHINE.SMImplementation
ADD CONSTRAINT FK_SMImplementation_SMDefinition FOREIGN KEY (SMDefinitionId) REFERENCES STATE_MACHINE.SMDefinition (ID) ON DELETE CASCADE

ALTER TABLE STATE_MACHINE.SMImplementation
ADD CONSTRAINT FK_SMImplementation_Element FOREIGN KEY (ElementId) REFERENCES STATE_MACHINE.Element (ID) ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.SMImplementation
ADD CONSTRAINT FK_SMImplementation_ElementType FOREIGN KEY (ElementTypeId) REFERENCES STATE_MACHINE.ElementType (ID) ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.Element
ADD CONSTRAINT FK_Element_ElementType FOREIGN KEY (ElementTypeId) REFERENCES STATE_MACHINE.ElementType (ID)  ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.Element
ADD CONSTRAINT FK_Element_SMDefinition FOREIGN KEY (SMDefinitionId) REFERENCES STATE_MACHINE.SMDefinition (ID)  ON DELETE NO ACTION

INSERT INTO STATE_MACHINE.ElementType VALUES ('state')
INSERT INTO STATE_MACHINE.ElementType VALUES ('transition')