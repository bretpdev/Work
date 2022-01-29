#UTNWS85.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS85.NWS85R1
then
rm ${reportdir}/UNWS85.NWS85R1
fi
if test -a ${reportdir}/UNWS85.NWS85RZ
then
rm ${reportdir}/UNWS85.NWS85RZ
fi
if test -a ${reportdir}/UNWS85.NWS85R2
then
rm ${reportdir}/UNWS85.NWS85R2
fi

# run the program

sas ${codedir}/UTNWS85.sas -log ${reportdir}/UNWS85.NWS85R1  -mautosource
