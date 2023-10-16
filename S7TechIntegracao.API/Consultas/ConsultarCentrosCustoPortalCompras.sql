SELECT {1}
	T0."PrcCode" AS "FactorCode",
	T0."PrcName" AS "FactorDescription",
	T0."Active" as "Active"
FROM OPRC T0
WHERE
	T0."Locked" = 'N'
	AND T0."Active" = 'Y'
	AND T0."DimCode" = 2
	AND (COALESCE(T0."PrcCode", '') LIKE '%{0}%' OR '{0}' = '')
ORDER BY T0."PrcCode"