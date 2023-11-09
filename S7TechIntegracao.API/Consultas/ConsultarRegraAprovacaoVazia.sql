SELECT 
	T0."Code",
	T0."U_DraftEntry",
	T0."U_IsDraft",
	T0."U_Status",
	T0."U_ObjType",
	T1."LineId", 
	T1."U_empID", 
	T1."U_UserID", 
	T1."U_CentroCusto" 
FROM "@S7T_OWDD" T0
	INNER JOIN "@S7T_WDD1" T1 ON T1."Code" = T0."Code"
WHERE T0."U_DraftEntry" = {0}
and T0."U_ObjType" ={1}