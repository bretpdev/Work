#UTLWS25.jcl Consolidation Marketing 2007 for AutoPay Borrowers
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS25.LWS25R1
then
rm ${reportdir}/ULWS25.LWS25R1
fi
if test -a ${reportdir}/ULWS25.LWS25RZ
then
rm ${reportdir}/ULWS25.LWS25RZ
fi
if test -a ${reportdir}/ULWS25.LWS25R2
then
rm ${reportdir}/ULWS25.LWS25R2
fi
if test -a ${reportdir}/ULWS25.LWS25R3
then
rm ${reportdir}/ULWS25.LWS25R3
fi

# run the program

sas ${codedir}/UTLWS25.sas -log ${reportdir}/ULWS25.LWS25R1  -mautosource
