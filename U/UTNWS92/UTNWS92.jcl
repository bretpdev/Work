#UTNWS92.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS92.NWS92R1
then
rm ${reportdir}/UNWS92.NWS92R1
fi
if test -a ${reportdir}/UNWS92.NWS92RZ
then
rm ${reportdir}/UNWS92.NWS92RZ
fi
if test -a ${reportdir}/UNWS92.NWS92R2
then
rm ${reportdir}/UNWS92.NWS92R2
fi


# run the program

sas ${codedir}/UTNWS92.sas -log ${reportdir}/UNWS92.NWS92R1  -mautosource
