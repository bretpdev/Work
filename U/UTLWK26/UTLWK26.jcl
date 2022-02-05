#UTLWK26.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK26.LWK26R1
then
rm ${reportdir}/ULWK26.LWK26R1
fi
if test -a ${reportdir}/ULWK26.LWK26R2
then
rm ${reportdir}/ULWK26.LWK26R2
fi
if test -a ${reportdir}/ULWK26.LWK26RZ
then
rm ${reportdir}/ULWK26.LWK26RZ
fi

# run the program

sas ${codedir}/UTLWK26.sas -log ${reportdir}/ULWK26.LWK26R1  -mautosource
