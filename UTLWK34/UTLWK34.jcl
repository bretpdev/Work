#UTLWK34.jcl TILP Borrowers - No PDEM Record in OneLINK
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK34.LWK34R1
then
rm ${reportdir}/ULWK34.LWK34R1
fi
if test -a ${reportdir}/ULWK34.LWK34R2
then
rm ${reportdir}/ULWK34.LWK34R2
fi
if test -a ${reportdir}/ULWK34.LWK34RZ
then
rm ${reportdir}/ULWK34.LWK34RZ
fi

# run the program

sas ${codedir}/UTLWK34.sas -log ${reportdir}/ULWK34.LWK34R1  -mautosource
