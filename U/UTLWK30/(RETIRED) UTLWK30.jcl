#UTLWK30.jcl Default Skips  - High Balance and Old Loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK30.LWK30R1
then
rm ${reportdir}/ULWK30.LWK30R1
fi
if test -a ${reportdir}/ULWK30.LWK30R2
then
rm ${reportdir}/ULWK30.LWK30R2
fi
if test -a ${reportdir}/ULWK30.LWK30RZ
then
rm ${reportdir}/ULWK30.LWK30RZ
fi

# run the program

sas ${codedir}/UTLWK30.sas -log ${reportdir}/ULWK30.LWK30R1  -mautosource
