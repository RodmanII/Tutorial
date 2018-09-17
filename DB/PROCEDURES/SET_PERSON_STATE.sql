IF OBJECT_ID('dbo.SET_PERSON_STATE', 'P') IS NOT NULL
	DROP PROCEDURE dbo.SET_PERSON_STATE
GO

CREATE PROCEDURE dbo.SET_PERSON_STATE
(
	@PERSON_ID INT,
	@OP_TYPE TINYINT
)
AS
BEGIN	
	DECLARE @CURR_STATE TINYINT
	DECLARE @NEW_STATE TINYINT

	SELECT @CURR_STATE = ACTIVE FROM dbo.PERSON WHERE ID = @PERSON_ID

	--Si no es nulo, es porque existe un registro con el id enviado
	IF @CURR_STATE IS NOT NULL
		BEGIN
			IF @OP_TYPE = 1
				BEGIN
					SET @NEW_STATE = 2

					IF @CURR_STATE = 2
						SET @NEW_STATE = 1			

					UPDATE dbo.PERSON SET ACTIVE = @NEW_STATE WHERE ID = @PERSON_ID
				END
			ELSE IF @OP_TYPE = 2
				DELETE FROM dbo.PERSON WHERE ID = @PERSON_ID

			IF @@ROWCOUNT > 0
				SELECT 200 AS Code, '1' AS Id, 'Operación efectuada con éxito' AS Description
			ELSE
				SELECT 500 AS Code, '0' AS Id, 'No fue posible efecutar la operación' AS Description
		END
	ELSE
		SELECT 500 AS Code, '0' AS Id, 'El id de persona indicado no existe' AS Description

END