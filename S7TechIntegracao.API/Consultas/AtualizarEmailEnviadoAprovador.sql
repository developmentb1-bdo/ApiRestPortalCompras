UPDATE T1
	SET T1."U_EmailEnv" = '{3}'
FROM "@S7T_WDD1" T1 
	INNER JOIN "@S7T_OWDD" T0  ON T0."Code" = T1."Code"
WHERE
	T1."U_empID" = {0}
	AND T1."Code" = '{1}'
	AND T1."LineId" = {2}