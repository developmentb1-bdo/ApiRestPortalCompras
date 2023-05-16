Select 
	A."U_CdVerificador" AS "U_CodigoVerificador",
	A."U_NrRetNFSE" AS "NumNfse"
From "@SKILL_NOFSNFSE001" A
INNER join OINV B ON B."DocEntry" = A."U_NrDocEntry"
WHERE A."U_NrDocEntry" = {0}
