SELECT
	T0."Code",
	T0."U_DocEntry",
	T0."U_DocDate",
	T0."U_Status",
	T0."U_IsDraft",
	T0."U_DraftEntry",
	T0."U_ObjType",
	T0."U_MaxReqr",
	T0."U_MaxRejReqr"
FROM "@S7T_OWDD" T0
WHERE
	T0."U_DraftEntry" = {0}
	AND (T0."U_ObjType" = '22' 	OR T0."U_ObjType" = '1470000113')
	