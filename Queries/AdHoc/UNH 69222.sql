select WindowsUserName, FirstName, LastName, AesUserId as UT# from Csys..SYSA_DAT_Users u where status = 'active' and u.BusinessUnit in (1, 10, 3, 53, 11, 24, 55, 64)

--select * from csys..GENR_LST_BusinessUnits

--1, 10, 3, 53, 11, 24, 55, 64