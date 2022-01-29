DECLARE @Begin DATE = 'XXXX-XX-XX'
DECLARE @Finish DATE = 'XXXX-XX-XX'
select * from CDW..PDXX_PRS_BKR where ((DC_BKR_STA = 'XX' AND DD_BKR_NTF <= @Finish) OR (DC_BKR_STA = 'XX' AND DD_BKR_STA between @Begin AND @Finish AND DD_BKR_NTF <= @Finish) OR (DC_BKR_STA = 'XX' AND DD_BKR_STA > @Finish AND DD_BKR_NTF <= @Finish)) and dc_bkr_typ ='  '
select * from openquery(legend, 'select * from pkub.pdXX_prs_bkr where DF_prs_id = ''XXXXXXXXX''')
select * from cdw..PDXX_PRS_NME where DF_PRS_ID = 'XXXXXXXXX'