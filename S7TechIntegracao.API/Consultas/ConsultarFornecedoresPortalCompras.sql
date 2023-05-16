SELECT	 
	 T0."CardCode",
	 T0."CardName",
	 T1."TaxId0" as "U_S7T_CNPJ",
	 T1."TaxId4" as "U_S7T_CPF",
	T0."Currency" ,
T0."validFor",
T0."validFrom",
T0."validTo",
T0."frozenFor",
T0."frozenFrom",
T0."frozenTo"
FROM OCRD T0 
INNER JOIN CRD7 T1 ON T0."CardCode" = T1."CardCode"
AND T1."Address" = ''
AND T1."AddrType" = 'S'
AND T0."frozenFor" = 'N'
WHERE T0."CardType" = 'S' 
order by T1."CardCode"