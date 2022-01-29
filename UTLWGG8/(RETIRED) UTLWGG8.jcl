#UTLWGG8.jcl Enrollment Updates on Nelnet Borrowers
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWGG8.LWGG8R1
then
rm ${reportdir}/ULWGG8.LWGG8R1
fi
if test -a ${reportdir}/ULWGG8.LWGG8RZ
then
rm ${reportdir}/ULWGG8.LWGG8RZ
fi
if test -a ${reportdir}/ULWGG8.LWGG8R2
then
rm ${reportdir}/ULWGG8.LWGG8R2
fi

# run the program

sas ${codedir}/UTLWGG8.sas -log ${reportdir}/ULWGG8.LWGG8R1  -mautosource
