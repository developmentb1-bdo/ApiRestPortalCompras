SELECT
	T0."ID",
	T0."Usage"
FROM OITM T0
WHERE
	COALESCE(T0."U_S7T_TipoItem", '02') = '01'
	AND COALESCE(T0."U_S7T_AtivoImobilizado", 'N') = 'Y'