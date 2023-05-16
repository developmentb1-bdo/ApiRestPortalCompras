SELECT
	T2."empID",
	COALESCE(T2."userId", -1) AS "userId",
	COALESCE(T2."email", '') AS "email"
FROM "@S7T_APROVOPRQ" T0
	INNER JOIN "@S7T_APROVPRQ1" T1 ON T1."Code" = T0."Code"
	INNER JOIN OHEM T2 ON COALESCE(T1."U_CodApr", 0) = T2."empID"
WHERE
    T0."U_CentroCusto" = '{0}'
	AND T1."U_TipoDoc" = '{1}'
	