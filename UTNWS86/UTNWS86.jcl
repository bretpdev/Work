#UTNWS86.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS86.NWS86R1
then
rm ${reportdir}/UNWS86.NWS86R1
fi
if test -a ${reportdir}/UNWS86.NWS86RZ
then
rm ${reportdir}/UNWS86.NWS86RZ
fi
if test -a ${reportdir}/UNWS86.NWS86R2
then
rm ${reportdir}/UNWS86.NWS86R2
fi

# run the program

sas ${codedir}/UTNWS86.sas -log ${reportdir}/UNWS86.NWS86R1  -mautosource
