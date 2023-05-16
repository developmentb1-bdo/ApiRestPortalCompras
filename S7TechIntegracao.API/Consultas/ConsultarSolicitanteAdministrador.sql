SELECT
	COALESCE(T0."U_S7T_TipoUsuario", '01') AS "S7T_TipoUsuario"
FROM OHEM T0
WHERE
	T0."empID" = {0}