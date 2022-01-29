DECLARE @DATA TABLE (BF_SSN INT)
INSERT INTO @DATA VALUES
(015686165),
(028702014),
(244236267),
(250535878),
(253259456),
(255739715),
(259619422),
(348648445),
(352784923),
(385900741),
(431695110),
(431751438),
(441801665),
(493925914),
(502988032),
(513684057),
(513885713),
(518137986),
(518139827),
(518948682),
(519020824),
(519086523),
(519132426),
(520046650),
(521390412),
(525137995),
(527218356),
(528275916),
(528391883),
(528438040),
(528457711),
(528473708),
(528510905),
(528538245),
(528575320),
(528610022),
(528612546),
(528753781),
(528812678),
(528851210),
(528852429),
(528870724),
(528897099),
(528918493),
(528957376),
(528983944),
(529230050),
(529293980),
(529310835),
(529356273),
(529392259),
(529411528),
(529412109),
(529471207),
(529518435),
(529572261),
(529716083),
(529772693),
(529774770),
(529793271),
(529838073),
(529855476),
(529897295),
(529952654),
(529963492),
(529973402),
(530159630),
(532087405),
(545727336),
(547294099),
(555554679),
(556879573),
(563977600),
(571175828),
(572311950),
(575172760),
(576536825),
(585749138),
(601725249),
(607145534),
(642073042),
(646074667),
(646125422)







SELECT DISTINCT
DC02.BF_SSN,
COUNT(*) AS LOAN_COUNT,
SUM(ISNULL(LA_CLM_BAL,0.00) - ISNULL(LA_CLM_PRJ_COL_CST,0.00)) AS PRINCIPAL_INTEREST
FROM
@DATA D
INNER JOIN ODW..DC02_BAL_INT DC02
ON CAST(DC02.BF_SSN AS INT) = D.BF_SSN
AND DC02.LA_CLM_BAL > 0
GROUP BY
DC02.BF_SSN