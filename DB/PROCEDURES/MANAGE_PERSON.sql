IF OBJECT_ID('dbo.MANAGE_PERSON', 'P') IS NOT NULL
	DROP PROCEDURE dbo.MANAGE_PERSON	
GO

CREATE PROCEDURE dbo.MANAGE_PERSON
(
	@ID INT,
	@FIRSTNAME VARCHAR(100),
	@LASTNAME VARCHAR(100),
	@DOB DATE,
	@WEIGHT SMALLINT
)
AS
BEGIN	
	IF @ID = 0
		INSERT INTO dbo.PERSON(FIRSTNAME, LASTNAME, DOB, WEIGHT) VALUES(@FIRSTNAME, @LASTNAME, @DOB, @WEIGHT)
	ELSE IF @ID > 0
		UPDATE dbo.PERSON SET FIRSTNAME = @FIRSTNAME, LASTNAME = @LASTNAME, DOB = @DOB, WEIGHT = @WEIGHT WHERE ID = @ID

	IF @@ROWCOUNT > 0
		SELECT 200 AS Code, '1' AS Id, 'Operación efectuada con éxito' AS Description
	ELSE
		SELECT 500 AS Code, '0' AS Id, 'No fue posible efectuar la operación' AS Description
END