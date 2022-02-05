#UTLWK25.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK25.LWK25R1
then
rm ${reportdir}/ULWK25.LWK25R1
fi
if test -a ${reportdir}/ULWK25.LWK25R2
then
rm ${reportdir}/ULWK25.LWK25R2
fi
if test -a ${reportdir}/ULWK25.LWK25R3
then
rm ${reportdir}/ULWK25.LWK25R3
fi
if test -a ${reportdir}/ULWK25.LWK25R4
then
rm ${reportdir}/ULWK25.LWK25R4
fi
if test -a ${reportdir}/ULWK25.LWK25RZ
then
rm ${reportdir}/ULWK25.LWK25RZ
fi

# run the program

sas ${codedir}/UTLWK25.sas -log ${reportdir}/ULWK25.LWK25R1  -mautosource
