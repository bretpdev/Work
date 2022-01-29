#UTLWK31.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK31.LWK31R1
then
rm ${reportdir}/ULWK31.LWK31R1
fi
if test -a ${reportdir}/ULWK31.LWK31R2
then
rm ${reportdir}/ULWK31.LWK31R2
fi
if test -a ${reportdir}/ULWK31.LWK31R3
then
rm ${reportdir}/ULWK31.LWK31R3
fi
if test -a ${reportdir}/ULWK31.LWK31RZ
then
rm ${reportdir}/ULWK31.LWK31RZ
fi

# run the program

sas ${codedir}/UTLWK31.sas -log ${reportdir}/ULWK31.LWK31R1  -mautosource
