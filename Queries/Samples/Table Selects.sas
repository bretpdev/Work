*These two programs do the same thing.  The first uses a table select while the
second uses a subquery.  Note the SQL processing times for each (the 
table select is MUCH more efficient than the subquery).;


NOTE: Remote submit to CYPRUS commencing.
177  OPTIONS NOCENTER NODATE NONUMBER LS=132 SYMBOLGEN;
NOTE: PROCEDURE SQL used:
      real time           5:02.55			<<<<<<<<<<<<<<<<<<<<
      cpu time            0.04 seconds		<<<<<<<<<<<<<<<<<<<<


178  PROC SQL;
179  CONNECT TO DB2 (DATABASE=DLGSUTWH);
180  CREATE TABLE DEMO AS
181  SELECT *
182  FROM CONNECTION TO DB2 (
183  SELECT  integer(A.bf_ssn)               as SSN,
184          RTRIM(D.DM_PRS_LST)||', '||RTRIM(D.DM_PRS_1)||'
'||RTRIM(D.DM_PRS_MID)
185                                          AS NAME,
186          A.af_apl_id||A.af_apl_id_sfx    as CLUID,
187          C.AC_LON_TYP                    AS LONTYP,
188          C.AD_PRC                        AS GTYDT,
189          C.AA_GTE_LON_AMT                AS GTYAMT,
190          C.AF_CUR_LON_OPS_LDR            AS CURLDR,
191          C.AF_CUR_LON_SER_AGY            AS CURSVC,
192          E.LF_BKR_DKT                    AS BKRDKT,
193          E.LD_BKR_FIL                    AS FILDT
194  FROM  OLWHRM1.DC01_LON_CLM_INF A INNER JOIN
195      (SELECT B.BF_SSN
196      FROM OLWHRM1.DC01_LON_CLM_INF B
197      WHERE (
198              (B.LC_STA_DC10 = '03'
199              AND B.LC_PCL_REA IN ('BC','BH','BO')
200              AND DATE(B.LD_STA_UPD_DC10) BETWEEN &BEG AND &END
201              )
202          OR
203              (B.LC_STA_DC10 = '03'
204              AND B.LC_AUX_STA = '07'
205              AND B.LD_AUX_STA_UPD BETWEEN &BEG AND &END
206              )
207          )
208      AND EXISTS
209          (SELECT *
210          FROM OLWHRM1.GA14_LON_STA X INNER JOIN OLWHRM1.GA01_APP Y
211              ON X.AF_APL_ID = Y.AF_APL_ID
212          WHERE B.BF_SSN = Y.DF_PRS_ID_BR
213          AND X.AC_STA_GA14 = 'A'     /*ONLY ACTIVE GA14 LOAN STATUS DATA*/
214          AND X.AC_LON_STA_TYP IN
215          ('CR','DA','FB','IA','ID','IG','IM','RP','UA','UB','UC','UD','UI')
216          )
217      ) B
218          ON A.bf_ssn = B.BF_SSN
219      INNER JOIN OLWHRM1.GA10_LON_APP C
220          on a.af_apl_id = C.af_apl_id
221          and a.af_apl_id_sfx = C.af_apl_id_sfx
222      INNER JOIN OLWHRM1.PD01_PDM_INF D
223          on A.bf_ssn = D.DF_PRS_ID
224      LEFT OUTER JOIN OLWHRM1.DC18_BKR E
225          on E.af_apl_id = A.af_apl_id
226          and E.af_apl_id_sfx = A.af_apl_id_sfx
227  /*ORDER BY A.bf_ssn, A.af_apl_id||A.af_apl_id_sfx*/
228  );
NOTE: Table WORK.DEMO created, with 49 rows and 10 columns.


*******************************************************************;

679  libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
NOTE: Libref WORKLOCL was successfully assigned as follows:
      Engine:        REMOTE
      Physical Name: /sas/tmp/SAS_work452D00012770_f1n05en
680  RSUBMIT;
NOTE: Remote submit to CYPRUS commencing.
52   OPTIONS NOCENTER NODATE NONUMBER LS=132 SYMBOLGEN;
NOTE: PROCEDURE SQL used:
      real time           29:15.84			<<<<<<<<<<<<<<<<<<<<<<<<<<
      cpu time            0.19 seconds		<<<<<<<<<<<<<<<<<<<<<<<<<<


53   PROC SQL;
54   CONNECT TO DB2 (DATABASE=DLGSUTWH);
55   CREATE TABLE DEMO AS
56   SELECT *
57   FROM CONNECTION TO DB2 (
58   SELECT  integer(A.bf_ssn)               as SSN,
59           RTRIM(D.DM_PRS_LST)||', '||RTRIM(D.DM_PRS_1)||'
'||RTRIM(D.DM_PRS_MID)
60                                           AS NAME,
61           A.af_apl_id||A.af_apl_id_sfx    as CLUID,
62           C.AC_LON_TYP                    AS LONTYP,
63           C.AD_PRC                        AS GTYDT,
64           C.AA_GTE_LON_AMT                AS GTYAMT,
65           C.AF_CUR_LON_OPS_LDR            AS CURLDR,
66           C.AF_CUR_LON_SER_AGY            AS CURSVC,
67           E.LF_BKR_DKT                    AS BKRDKT,
68           E.LD_BKR_FIL                    AS FILDT
69   FROM  OLWHRM1.DC01_LON_CLM_INF A INNER JOIN OLWHRM1.DC01_LON_CLM_INF B
70           ON A.bf_ssn = B.BF_SSN
71       INNER JOIN OLWHRM1.GA10_LON_APP C
72           on a.af_apl_id = C.af_apl_id
73           and a.af_apl_id_sfx = C.af_apl_id_sfx
74       INNER JOIN OLWHRM1.PD01_PDM_INF D
75           on A.bf_ssn = D.DF_PRS_ID
76       LEFT OUTER JOIN OLWHRM1.DC18_BKR E
77           on E.af_apl_id = A.af_apl_id
78           and E.af_apl_id_sfx = A.af_apl_id_sfx
79   WHERE (
80           (B.LC_STA_DC10 = '03'
81           AND B.LC_PCL_REA IN ('BC','BH','BO')
82           AND DATE(B.LD_STA_UPD_DC10) BETWEEN &BEG AND &END
83           )
84   OR
85           (B.LC_STA_DC10 = '03'
86           AND B.LC_AUX_STA = '07'
87           AND B.LD_AUX_STA_UPD BETWEEN &BEG AND &END
88           )
89       )
90   AND EXISTS
91       (SELECT *
92       FROM OLWHRM1.GA14_LON_STA X INNER JOIN OLWHRM1.GA01_APP Y
93           ON X.AF_APL_ID = Y.AF_APL_ID
94       WHERE B.BF_SSN = Y.DF_PRS_ID_BR
95       AND X.AC_STA_GA14 = 'A'     /*ONLY ACTIVE GA14 LOAN STATUS DATA*/
96       AND X.AC_LON_STA_TYP IN
97       ('CR','DA','FB','IA','ID','IG','IM','RP','UA','UB','UC','UD','UI')
98       )
99   /*ORDER BY A.bf_ssn, A.af_apl_id||A.af_apl_id_sfx*/
100  );
NOTE: Table WORK.DEMO created, with 49 rows and 10 columns.


