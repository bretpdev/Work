#UTLWK39.jcl Skip RPS
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK39.LWK39R1
then
rm ${reportdir}/ULWK39.LWK39R1
fi
if test -a ${reportdir}/ULWK39.LWK39RZ
then
rm ${reportdir}/ULWK39.LWK39RZ
fi
if test -a ${reportdir}/ULWK39.LWK39R2
then
rm ${reportdir}/ULWK39.LWK39R2
fi

# run the program

sas ${codedir}/UTLWK39.sas -log ${reportdir}/ULWK39.LWK39R1  -mautosource
