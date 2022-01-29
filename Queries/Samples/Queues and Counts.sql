SELECT WQ20.PF_REQ_ACT, COUNT(1) AS Outstanding, MIN(WQ20.WD_ACT_REQ) AS MinDate, WQ10.WF_USR_OWN_QUE AS QueueOwner, WQ10.WM_QUE_FUL FROM CDW..WQ20_TSK_QUE WQ20 INNER JOIN CDW..WQ10_TSK_QUE_DFN WQ10 ON WQ10.WF_QUE = WQ20.WF_QUE WHERE WQ20.WC_STA_WQUE20 NOT IN('X','C') GROUP BY WQ20.PF_REQ_ACT, WQ10.WF_USR_OWN_QUE, WQ10.WM_QUE_FUL ORDER BY Outstanding
SELECT WQ20.PF_REQ_ACT, COUNT(1) AS Outstanding, MIN(WQ20.WD_ACT_REQ) AS MinDate, WQ10.WF_USR_OWN_QUE AS QueueOwner, WQ10.WM_QUE_FUL FROM UDW..WQ20_TSK_QUE WQ20 INNER JOIN UDW..WQ10_TSK_QUE_DFN WQ10 ON WQ10.WF_QUE = WQ20.WF_QUE WHERE WQ20.WC_STA_WQUE20 NOT IN('X','C') GROUP BY WQ20.PF_REQ_ACT, WQ10.WF_USR_OWN_QUE, WQ10.WM_QUE_FUL ORDER BY Outstanding

