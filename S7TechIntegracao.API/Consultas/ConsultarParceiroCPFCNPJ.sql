SELECT 
	T0."CardCode" 
FROM CRD7 T0
	INNER JOIN OCRD T1 ON T1."CardCode" = T0."CardCode"
WHERE 
	T0."AddrType" = 'S' 
	AND T0."Address" = ''
	AND 
	(
		REPLACE(REPLACE(REPLACE(T0."TaxId0", '.', ''), '/', ''), '-', '') = REPLACE(REPLACE(REPLACE('{0}', '.', ''), '/', ''), '-', '') 
		OR REPLACE(REPLACE(REPLACE(T0."TaxId4", '.', ''), '/', ''), '-', '') = REPLACE(REPLACE(REPLACE('{0}', '.', ''), '/', ''), '-', '')
		OR T0."TaxId5" = '{0}'
	)
	AND T1."CardType" = '{1}'