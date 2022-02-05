#UTLWU06.jcl INV ADDR FOR BORR/REF QC
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU06.LWU06R1
then
rm ${reportdir}/ULWU06.LWU06R1
fi
if test -a ${reportdir}/ULWU06.LWU06R2
then
rm ${reportdir}/ULWU06.LWU06R2
fi
if test -a ${reportdir}/ULWU06.LWU06RZ
then
rm ${reportdir}/ULWU06.LWU06RZ
fi

# run the program

sas ${codedir}/UTLWU06.sas -log ${reportdir}/ULWU06.LWU06R1  -mautosource
