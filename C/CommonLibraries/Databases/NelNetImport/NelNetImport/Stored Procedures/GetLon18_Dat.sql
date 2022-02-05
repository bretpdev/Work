CREATE PROCEDURE [dbo].[GetLon18_Dat]
    @DisbDataId varchar(9)
AS
    SELECT 
        [db_insur_fee] AS LA_DSB_FEE,
        '01' AS LC_DSB_FEE,
        '01' AS LC_DSB_FEE_PAY,
        '' AS LR_DSB_FEE, 
        CASE
           WHEN [db_cancel_date] != '000000' THEN  [db_insur_fee]
           ELSE ''
        END AS LA_DSB_FEE_RFD_PCV, 
        '' AS  LC_FEE_COL_STA, 
        '' AS LD_FEE_COL_STA, 
        '' AS LA_FEE_CAN, 
        '' AS LD_FEE_CAN, 
        '' AS LA_FEE_RPT_PD, 
        '' AS LD_FEE_RPT_PD, 
        '' AS LA_FEE_GTR_BAL, 
        '' AS LA_FEE_CAM_RPT, 
        '' AS LA_FEE_COL_AT_DSB, 
        '' AS LA_FEE_LDR_BAL, 
        '' AS LA_FEE_CAN_PCV_RFD, 
        '' AS LA_FEE_CAN_RFD, 
        '' AS LD_FEE_CAN_RFD 
    FROM    
        [ITEMSQLDF_Disbursement]
    WHERE
        disbursement_id = @DisbDataId

    UNION ALL 

    SELECT 
        [db_orig_fee] AS LA_DSB_FEE,
        '02' AS LC_DSB_FEE,
        '01' AS LC_DSB_FEE_PAY,
        '' AS LR_DSB_FEE, 
        CASE
           WHEN [db_cancel_date] != '000000' THEN  [db_orig_fee]
           ELSE ''
        END AS LA_DSB_FEE_RFD_PCV, 
        '' AS  LC_FEE_COL_STA, 
        '' AS LD_FEE_COL_STA, 
        '' AS LA_FEE_CAN, 
        '' AS LD_FEE_CAN, 
        '' AS LA_FEE_RPT_PD, 
        '' AS LD_FEE_RPT_PD, 
        '' AS LA_FEE_GTR_BAL, 
        '' AS LA_FEE_CAM_RPT, 
        '' AS LA_FEE_COL_AT_DSB, 
        '' AS LA_FEE_LDR_BAL, 
        '' AS LA_FEE_CAN_PCV_RFD, 
        '' AS LA_FEE_CAN_RFD, 
        '' AS LD_FEE_CAN_RFD 
    FROM    
        [ITEMSQLDF_Disbursement]
    WHERE
        disbursement_id = @DisbDataId
    
RETURN 0
