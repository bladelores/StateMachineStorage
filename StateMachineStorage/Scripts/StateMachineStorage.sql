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
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_SMImplementation_State]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[SMImplementation]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[SMImplementation] DROP CONSTRAINT [FK_SMImplementation_State]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_SMImplementation_Transition]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[SMImplementation]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[SMImplementation] DROP CONSTRAINT [FK_SMImplementation_Transition]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_Transition_StateOld]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[Transition]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[Transition] DROP CONSTRAINT [FK_Transition_StateOld]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_Transition_StateNew]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[Transition]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[Transition] DROP CONSTRAINT [FK_Transition_StateNew]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_Transition_TransitionTrigger]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[Transition]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[Transition] DROP CONSTRAINT [FK_Transition_TransitionTrigger]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_SMDefinition_State]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[SMDefinition]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[SMDefinition] DROP CONSTRAINT [FK_SMDefinition_State]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_State_SMDefinition]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[State]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[State] DROP CONSTRAINT [FK_State_SMDefinition]
END

IF EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[STATE_MACHINE].[FK_Transition_SMDefinition]')
AND parent_object_id = OBJECT_ID(N'[STATE_MACHINE].[Transition]')
)
BEGIN
ALTER TABLE [STATE_MACHINE].[Transition] DROP CONSTRAINT [FK_Transition_SMDefinition]
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='EventLog')
BEGIN
DROP TABLE STATE_MACHINE.EventLog
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='SMImplementation')
BEGIN
DROP TABLE STATE_MACHINE.SMImplementation
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='SMDefinition')
BEGIN
DROP TABLE STATE_MACHINE.SMDefinition
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='Transition')
BEGIN
DROP TABLE STATE_MACHINE.Transition
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema='STATE_MACHINE' and Table_Name='State')
BEGIN
DROP TABLE STATE_MACHINE.State
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

CREATE TABLE STATE_MACHINE.TransitionTrigger(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[Name] nvarchar(250)
)

CREATE TABLE STATE_MACHINE.State(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[SMDefinitionId] bigint NULL,
[Name] nvarchar(250),
)

CREATE TABLE STATE_MACHINE.Transition(
[ID] bigint  PRIMARY KEY  Identity(1,1),
[SMDefinitionId] bigint NOT NULL,
[OldStateId] bigint NOT NULL,
[NewStateId] bigint NOT NULL,
[TransitionTriggerId] bigint NOT NULL
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
[SMDefinitionId] bigint NOT NULL,
[Name] nvarchar(250),
[StateId] bigint NOT NULL,
[TransitionId] bigint NOT NULL,
[Implemetation] nvarchar(MAX)
)

ALTER TABLE STATE_MACHINE.SMImplementation
ADD CONSTRAINT FK_SMImplementation_SMDefinition FOREIGN KEY (SMDefinitionId) REFERENCES STATE_MACHINE.SMDefinition (ID) ON DELETE CASCADE

ALTER TABLE STATE_MACHINE.SMImplementation
ADD CONSTRAINT FK_SMImplementation_State FOREIGN KEY (StateId) REFERENCES STATE_MACHINE.State (ID) ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.SMImplementation
ADD CONSTRAINT FK_SMImplementation_Transition FOREIGN KEY (TransitionId) REFERENCES STATE_MACHINE.Transition (ID) ON DELETE CASCADE

ALTER TABLE STATE_MACHINE.State
ADD CONSTRAINT FK_State_SMDefinition FOREIGN KEY (SMDefinitionId) REFERENCES STATE_MACHINE.SMDefinition (ID)  ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.Transition
ADD CONSTRAINT FK_Transition_SMDefinition FOREIGN KEY (SMDefinitionId) REFERENCES STATE_MACHINE.SMDefinition (ID)  ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.Transition
ADD CONSTRAINT FK_Transition_StateOld FOREIGN KEY (OldStateId) REFERENCES STATE_MACHINE.State (ID)  ON DELETE NO ACTION

ALTER TABLE STATE_MACHINE.Transition
ADD CONSTRAINT FK_Transition_StateNew FOREIGN KEY (NewStateId) REFERENCES STATE_MACHINE.State (ID)  ON DELETE NO ACTION
ALTER TABLE STATE_MACHINE.Transition
ADD CONSTRAINT FK_Transition_TransitionTrigger FOREIGN KEY (TransitionTriggerId) REFERENCES STATE_MACHINE.TransitionTrigger (ID)  ON DELETE CASCADE

