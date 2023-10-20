SELECT {2}
	T0."ItemCode",
	T0."ItemName"
FROM OITM T0
WHERE
	COALESCE(T0."U_S7T_ItemPortal", 'N') = 'Y'
	AND (COALESCE(T0."ItemCode", '') LIKE '%{0}%' OR '{0}' = '')
	AND (COALESCE(T0."ItemName", '') LIKE '%{1}%' OR '{1}' = '')
	AND COALESCE(T0."validFor", 'N') = 'Y'
ORDER BY T0."ItemCode"