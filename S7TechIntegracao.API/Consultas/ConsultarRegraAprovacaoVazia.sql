SELECT 
	T0."DocEntry",
	T0."ObjType",
	T3."DocNum",
	T0."DraftEntry"
FROM "OWDD" T0
	left JOIN "@S7T_OWDD" T1 ON T1."U_DraftEntry" = T0."DraftEntry" and T1."U_ObjType" = T0."ObjType"
	inner join "ODRF" T3 on T3."DocEntry"  =  T0."DraftEntry"
WHERE T0."DraftEntry" = {0}
and T0."ObjType" ={1}
and T1."U_DraftEntry" is null
