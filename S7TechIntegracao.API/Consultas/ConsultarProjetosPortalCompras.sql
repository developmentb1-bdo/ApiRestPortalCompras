SELECT {1}
	T0."PrjCode" AS "Code",
	T0."PrjName" AS "Name"
FROM OPRJ T0
WHERE
	T0."Locked" = 'N'
	AND T0."Active" = 'Y'
	AND (COALESCE(T0."PrjCode", '') LIKE '%{0}%' OR '{0}' = '')
ORDER BY T0."PrjCode"