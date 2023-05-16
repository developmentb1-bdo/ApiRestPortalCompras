	UPDATE T1
	SET T1."U_Status" = '{0}'
FROM "@S7T_OWDD" T1
	INNER JOIN "@S7T_WDD1" T0 ON T0."Code" = T1."Code"
WHERE
	T0."U_empID" = {1}
	AND T1."U_DraftEntry" = {2}
	AND T1."U_ObjType" = '22'
	AND T0."U_Remarks" = '{3}'
	AND T0."U_CentroCusto" is null
	