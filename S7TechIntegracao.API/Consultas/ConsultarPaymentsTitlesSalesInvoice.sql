SELECT
	 T0."DocNum",
	 T1."DocEntry" ,
	 T2."DocEntry" as nf,
	 T2."DocNum",
	 T0."TrsfrDate" as DataTransferencia,
	 T0."Canceled",
	 T0."DocDate" as DataPagamento,  
	 Replace(T1."SumApplied",',','.') as "SumApplied",
	 T1."InstId" as "InstallmentId",
	 T3."InsTotal",
	 T3."PaidToDate",
	 case when (T0."TrsfrAcct" is null) then
	 T0."CashAcct"
	 else T0."TrsfrAcct"
	 end as "ContaDoRazao",
	T4."AcctName" as NomeDaConta	 
FROM ORCT T0  
INNER JOIN RCT2 T1 ON T0."DocEntry" = T1."DocNum"
inner join OINV T2 on T1."DocEntry" = T2."DocEntry" 
inner join INV6 T3 on T3."DocEntry" = T2."DocEntry" and T1."InstId" = T3."InstlmntID"
inner join OACT T4 on T4."AcctCode" = T0."TrsfrAcct" or T4."AcctCode" = T0."CashAcct"
where  T1."InvType"  = 13
AND T2."DocEntry" = '{0}'



