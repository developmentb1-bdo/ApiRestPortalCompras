UPDATE T0
	SET T0."U_Status" = '{0}', T0."U_IsDraft" = '{3}', T0."U_DocEntry" = {4}
FROM "@S7T_OWDD" T0 
	INNER JOIN "@S7T_WDD1" T1 ON T0."Code" = T1."Code"
WHERE
	T0."U_empID" = {1}
	AND T1."U_DraftEntry" = {2}
	AND T1."U_ObjType" = '22'