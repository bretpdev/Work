#UTNWS88.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS88.NWS88R1
then
rm ${reportdir}/UNWS88.NWS88R1
fi
if test -a ${reportdir}/UNWS88.NWS88RZ
then
rm ${reportdir}/UNWS88.NWS88RZ
fi
if test -a ${reportdir}/UNWS88.NWS88R2
then
rm ${reportdir}/UNWS88.NWS88R2
fi
if test -a ${reportdir}/UNWS88.NWS88R3
then
rm ${reportdir}/UNWS88.NWS88R3
fi

# run the program

sas ${codedir}/UTNWS88.sas -log ${reportdir}/UNWS88.NWS88R1  -mautosource
