SELECT
	T0."DocEntry"
FROM ODRF T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	left JOIN OPOR T2 ON T2."DocNum" = T0."DocNum"
WHERE
T0."ObjType" = '22'
	AND T1."Status" <> 'Y'
	AND T0."OwnerCode" = '{0}'
	AND T2."DocEntry" = '{1}'