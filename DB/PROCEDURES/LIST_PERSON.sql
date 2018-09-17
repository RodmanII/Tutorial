IF OBJECT_ID('dbo.LIST_PERSON', 'P') IS NOT NULL
	DROP PROCEDURE dbo.LIST_PERSON

GO

CREATE PROCEDURE dbo.LIST_PERSON
(
	@PERSON_ID INT,
	@START INT,
	@END INT	
)
AS
BEGIN
	DECLARE @QUERY NVARCHAR(400)
	SET @QUERY = 'SELECT ID AS Id, FIRSTNAME AS Firstname, LASTNAME AS Lastname, CAST(DOB AS CHAR(10)) AS DoB, AGE AS Age, WEIGHT AS Weight, ACTIVE AS Enabled FROM dbo.PERSON' 

	IF @PERSON_ID > 0
		SET @QUERY = @QUERY + ' WHERE ID = @PERSON_ID'
	ELSE
		SET @QUERY = @QUERY + ' ORDER BY ID ASC OFFSET @START ROWS FETCH NEXT @END ROWS ONLY'

	EXEC sp_executesql @QUERY, N'@PERSON_ID INT, @START INT, @END INT', @PERSON_ID, @START, @END;

	IF @@ROWCOUNT > 0
		SELECT 200 AS Code, '1' AS Id, 'Operación efectuada con éxito' AS [Description]
	ELSE
		SELECT 500 AS Code, '0' AS Id, 'No se encontraron registros' AS [Description]

END