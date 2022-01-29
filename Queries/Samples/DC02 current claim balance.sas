*DC02, BALANCE AND INTEREST SUBROUTINE

The safest approach whenever using DC02 should be to use 
it only on the right side of a left outer join to another 
DC table.  That way, all of the desired loans will always be 
picked up by the query, and any not in 03 status will have 
a null balance amount.  At least that is something that can 
be caught in testing.

For an example where claims of all statuses need current balances,
see On Demand job 'Postclaim Loan Status - Auditors.sas'.

==============================================================
To get CURRENT CLAIM BALANCE for open claims (03 STATUS), do: 
DC02_BAL_INT.LA_CLM_BAL - DC02_BAL_INT.la_clm_prj_col_cst

This includes other costs!
==============================================================

To get CURRENT CLAIM BALANCE for other claims (01,02,04 STATUS), do:
(DC01_LON_CLM_INF.la_clm_pri + DC01_LON_CLM_INF.la_clm_int)
- 
(DC01_LON_CLM_INF.la_pri_col)
+ 
(DC01_LON_CLM_INF.la_int_acr) 
- 
(DC01_LON_CLM_INF.la_int_col)
+
(DC01_LON_CLM_INF.la_leg_cst_acr)
- 
(DC01_LON_CLM_INF.la_leg_cst_col)
+
(DC01_LON_CLM_INF.la_oth_chr_acr)
- 
(DC01_LON_CLM_INF.la_oth_chr_col)
+
(DC01_LON_CLM_INF.la_col_cst_acr)
- 
(DC01_LON_CLM_INF.la_col_cst_col)

=================================================================
DC02_BAL_INT.LA_CLM_BAL - Equal to the LC05 'Outstanding Balance Due' amount on LC05, 
	but ONLY FOR OPEN CLAIMS (03 STATUS).  It includes:

TOTAL AMOUNT DEFAULTED 	(DC01_LON_CLM_INF.la_clm_pri + DC01_LON_CLM_INF.la_clm_int)
- 
PRINCIPAL COLLECTED	(DC01_LON_CLM_INF.la_pri_col)
+ 
INT ACCRUED THRU <current date> (DC01_LON_CLM_INF.la_int_acr + DC02_BAL_INT.la_clm_int_acr) 
- 
INTEREST COLLECTED (DC01_LON_CLM_INF.la_int_col)
+
LEGAL COSTS ACCRUED (DC01_LON_CLM_INF.la_leg_cst_acr)
- 
LEGAL COSTS COLLECTED (DC01_LON_CLM_INF.la_leg_cst_col)
+
OTHER CHARGES ACCRUED (DC01_LON_CLM_INF.la_oth_chr_acr)
- 
OTHER CHARGES COLLECTED (DC01_LON_CLM_INF.la_oth_chr_col)
+
COLLECTION COSTS ACCRUED (DC01_LON_CLM_INF.la_col_cst_acr)
- 
COLLECTION COSTS COLLECTED (DC01_LON_CLM_INF.la_col_cst_col)
+
COLLECTION FEES PROJECTED <current date> (DC02_BAL_INT.la_clm_prj_col_cst)
==============================================================