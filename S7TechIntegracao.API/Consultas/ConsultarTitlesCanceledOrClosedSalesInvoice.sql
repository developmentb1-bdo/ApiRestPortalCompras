SELECT
A."DocEntry",
A."DocTime" as "HourCreateTitle",
A."CardCode",
A."DocNum",
C."TaxIdNum" as "FederalTaxId",
A."CANCELED" as "Cancelled",
TO_VARCHAR (TO_DATE(A."CreateDate"), 'YYYYMMDD')|| ':'|| A."DocTime"  as "CreateDate",
D."Id" as "IdFilialIntBank"
FROM
OINV A
INNER JOIN INV6 B ON B."DocEntry" = A."DocEntry"
INNER JOIN OBPL C ON C."BPLId" = A."BPLId"
Left JOIN "IntegrationBank"."IV_IB_CompanyLocal" D ON D."BPLId" = A."BPLId" and "CompanyDb" = '{3}'
left join INV1 F on F."BaseEntry" = A."DocEntry"
left Join OINV E on E."CANCELED" = 'C' and E."DocEntry" = F."DocEntry"
WHERE
B."TotalBlck" <> B."InsTotal"
{0}
{1}
{2}
group by   A."DocEntry", A."CreateDate", A."DocTime", A."CardCode", A."DocNum",C."TaxIdNum",A."CANCELED", D."Id"
order by A."CreateDate", A."DocTime" asc
LIMIT {4} offset {5};
