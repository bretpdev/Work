#UTLWK38.jcl Skip New Loan Employer Review
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK38.LWK38R1
then
rm ${reportdir}/ULWK38.LWK38R1
fi
if test -a ${reportdir}/ULWK38.LWK38RZ
then
rm ${reportdir}/ULWK38.LWK38RZ
fi
if test -a ${reportdir}/ULWK38.LWK38R2
then
rm ${reportdir}/ULWK38.LWK38R2
fi

# run the program

sas ${codedir}/UTLWK38.sas -log ${reportdir}/ULWK38.LWK38R1  -mautosource
