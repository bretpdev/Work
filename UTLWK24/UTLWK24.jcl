#UTLWK24.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK24.LWK24R1
then
rm ${reportdir}/ULWK24.LWK24R1
fi
if test -a ${reportdir}/ULWK24.LWK24R2
then
rm ${reportdir}/ULWK24.LWK24R2
fi
if test -a ${reportdir}/ULWK24.LWK24R3
then
rm ${reportdir}/ULWK24.LWK24R3
fi
if test -a ${reportdir}/ULWK24.LWK24R4
then
rm ${reportdir}/ULWK24.LWK24R4
fi
if test -a ${reportdir}/ULWK24.LWK24RZ
then
rm ${reportdir}/ULWK24.LWK24RZ
fi

# run the program

sas ${codedir}/UTLWK24.sas -log ${reportdir}/ULWK24.LWK24R1  -mautosource
