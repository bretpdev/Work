#UTLWK17.jcl Compass 30-Day Skip
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK17.LWK17R1
then
rm ${reportdir}/ULWK17.LWK17R1
fi
if test -a ${reportdir}/ULWK17.LWK17RZ
then
rm ${reportdir}/ULWK17.LWK17RZ
fi
if test -a ${reportdir}/ULWK17.LWK17R2
then
rm ${reportdir}/ULWK17.LWK17R2
fi
if test -a ${reportdir}/ULWK17.LWK17R3
then
rm ${reportdir}/ULWK17.LWK17R3
fi

# run the program

sas ${codedir}/UTLWK17.sas -log ${reportdir}/ULWK17.LWK17R1  -mautosource
