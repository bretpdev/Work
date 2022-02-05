CREATE TABLE [textcoord].[UiFields]
(
	[UiFieldId] INT NOT NULL PRIMARY KEY, --no IDENTITY, as this should always explicitly match the code enum value
	[FieldName] VARCHAR(50) NOT NULL
)