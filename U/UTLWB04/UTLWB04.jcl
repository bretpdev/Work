#UTLWB04.jcl Loan Not Included in Claim
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWB04.LWB04R1
then
rm ${reportdir}/ULWB04.LWB04R1
fi
if test -a ${reportdir}/ULWB04.LWB04RZ
then
rm ${reportdir}/ULWB04.LWB04RZ
fi
if test -a ${reportdir}/ULWB04.LWB04R2
then
rm ${reportdir}/ULWB04.LWB04R2
fi

# run the program

sas ${codedir}/UTLWB04.sas -log ${reportdir}/ULWB04.LWB04R1  -mautosource
