SELECT T2."U_S7T_AreaFuncional" 
FROM OOCR T0  INNER JOIN OCR1 T1 ON T0."OcrCode" = T1."OcrCode"
 INNER JOIN OPRC T2 ON T1."PrcCode" = T2."PrcCode" AND T0."OcrCode" = '{0}'