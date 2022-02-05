#UTLWD41.jcl Borrower Benefit Detail Wyoming Loans - New
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWD41.LWD41R1
then
rm ${reportdir}/ULWD41.LWD41R1
fi
if test -a ${reportdir}/ULWD41.LWD41RZ
then
rm ${reportdir}/ULWD41.LWD41RZ
fi
if test -a ${reportdir}/ULWD41.LWD41R2
then
rm ${reportdir}/ULWD41.LWD41R2
fi

# run the program

sas ${codedir}/UTLWD41.sas -log ${reportdir}/ULWD41.LWD41R1  -mautosource
