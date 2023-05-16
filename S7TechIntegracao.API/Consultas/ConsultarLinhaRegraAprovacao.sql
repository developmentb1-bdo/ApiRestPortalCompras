SELECT
	T1."LineId"
FROM "@S7T_APROVOPRQ" T0
	INNER JOIN "@S7T_APROVPRQ1" T1 ON T1."Code" = T0."Code"
	INNER JOIN OHEM T2 ON COALESCE(T1."U_CodApr", 0) = T2."empID"
WHERE
	{0} BETWEEN T1."U_AlcadaDe"  AND T1."U_AlcadaAte"
	AND T0."U_CentroCusto" = '{1}'
	AND T1."U_TipoDoc" = '{2}'