SELECT
    T0."LineId",
	T2."empID",
	COALESCE(T2."userId", -1) AS "userId",
	COALESCE(T2."email", '') AS "email"
FROM "@S7T_APROVOVLPO1" T0	
	INNER JOIN OHEM T2 ON COALESCE(T0."U_CodApr", 0) = T2."empID"
WHERE
	{0} BETWEEN T0."U_AlcadaDe" AND T0."U_AlcadaAte"

