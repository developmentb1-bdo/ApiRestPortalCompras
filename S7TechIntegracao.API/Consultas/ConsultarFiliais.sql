SELECT 
	T0."BPLId", 
	T0."BPLName", 
	T0."TaxIdNum" AS "FederalTaxID" 
FROM 
	OBPL T0 
WHERE 
	T0."Disabled" = 'N';