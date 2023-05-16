select
	T3."Code",
	T2."DraftEntry"	
	FROM ODRF T0		
		INNER JOIN OWDD T2 ON T2."DraftEntry" = T0."DocEntry" 
		INNER JOIN "@S7T_OWDD" T3 ON T3."U_DraftEntry" = T2."DraftEntry" 
		where 
		T0."DocNum" = '{0}'