#UTNWS19.JCL Skip RPS - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS19.NWS19R1
then
rm ${reportdir}/UNWS19.NWS19R1
fi
if test -a ${reportdir}/UNWS19.NWS19R2
then
rm ${reportdir}/UNWS19.NWS19R2
fi
if test -a ${reportdir}/UNWS19.NWS19R3
then
rm ${reportdir}/UNWS19.NWS19R3
fi

# run the program

sas ${codedir}/UTNWS19.sas -log ${reportdir}/UNWS19.NWS19R1  -mautosource
