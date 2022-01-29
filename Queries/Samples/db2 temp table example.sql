

declare @ssn table (bf_ssn char(9))

insert into @ssn
exec(
'
DECLARE GLOBAL TEMPORARY TABLE SESSION.NOPD30_LOANS (ACC_ID  CHAR(9)) ON COMMIT PRESERVE ROWS; 
INSERT INTO SESSION.NOPD30_LOANS select distinct bf_ssn from PKUB.ln10_lon where bf_ssn = ''001780577''; 
select l.*from SESSION.NOPD30_LOANS l inner join pkub.ln10_lon ln10 on ln10.bf_ssn = l.ACC_ID
') at legend_test_vuk1

select * from @ssn
