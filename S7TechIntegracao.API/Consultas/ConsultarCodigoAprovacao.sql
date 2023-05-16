SELECT 
	T0."WddCode"
FROM OWDD T0
WHERE
	T0."DraftEntry" = {0}
	--AND T0."IsDraft" = 'Y'
	--AND T0."Status" = 'W'