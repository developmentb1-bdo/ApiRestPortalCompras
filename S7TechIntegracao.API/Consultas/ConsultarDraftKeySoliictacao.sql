SELECT
	T0."DocEntry"
FROM ODRF T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	INNER JOIN OPRQ T2 ON T2."DocNum" = T0."DocNum"
WHERE
	T0."ObjType" = '1470000113'	
	AND T1."Status" <> 'Y'
	AND T0."Requester" = '{0}'
	AND T2."DocEntry" = '{1}'