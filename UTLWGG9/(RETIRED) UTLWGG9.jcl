#UTLWGG9.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWGG9.LWGG9R1
then
rm ${reportdir}/ULWGG9.LWGG9R1
fi
if test -a ${reportdir}/ULWGG9.LWGG9R2
then
rm ${reportdir}/ULWGG9.LWGG9R2
fi
if test -a ${reportdir}/ULWGG9.LWGG9RZ
then
rm ${reportdir}/ULWGG9.LWGG9RZ
fi

# run the program

sas ${codedir}/UTLWGG9.sas -log ${reportdir}/ULWGG9.LWGG9R1  -mautosource
