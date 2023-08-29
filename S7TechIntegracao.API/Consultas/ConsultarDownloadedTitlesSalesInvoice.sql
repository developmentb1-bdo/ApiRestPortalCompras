SELECT
A."DocEntry",
A."DocTime" as "HourCreateTitle",
A."CardCode",
A."DocNum",
A."CANCELED" as "Cancelled",
C."TaxIdNum" as "FederalTaxId",
A."CreateDate" as "CreateDateSales", 
A."DocTime" as "CreateHourSales",
TO_VARCHAR (TO_DATE(E."DocDate"), 'YYYYMMDD')|| ':'|| E."DocTime"  as "CreateDate"  ,
F."Id" as "IdFilialIntBank",
C."AddtnlId" as "InscMunicipalFilial"
FROM
OINV A
INNER JOIN INV6 B ON B."DocEntry" = A."DocEntry"
INNER JOIN OBPL C ON C."BPLId" = A."BPLId"
INNER JOIN RCT2 D ON D."DocEntry" = A."DocEntry"
inner JOIN ORCT E ON E."DocEntry" = D."DocNum"
LEFT JOIN "IntegrationBank"."IV_IB_CompanyLocal" F ON F."BPLId" = A."BPLId" and "CompanyDb" = '{3}'
WHERE
B."TotalBlck" <> B."InsTotal"
AND (B."Status" = 'C' OR (B."Status" = 'O' AND E."Canceled" = 'Y') OR (B."Status" = 'O' AND A."PaidToDate" > 0) )
AND D."DocEntry" is not null
{0}
{1}
{2}
group by   A."DocEntry", A."CreateDate", A."DocTime", A."CardCode", A."DocNum", C."TaxIdNum",A."CANCELED", E."DocDate", E."DocTime",E."CreateDate",F."Id",C."AddtnlId"
order by E."CreateDate", E."DocTime" asc
LIMIT {4} offset {5};



