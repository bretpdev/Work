#UTNWS64.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS64.NWS64R1
then
rm ${reportdir}/UNWS64.NWS64R1
fi
if test -a ${reportdir}/UNWS64.NWS64RZ
then
rm ${reportdir}/UNWS64.NWS64RZ
fi
if test -a ${reportdir}/UNWS64.NWS64R2
then
rm ${reportdir}/UNWS64.NWS64R2
fi

# run the program

sas ${codedir}/UTNWS64.sas -log ${reportdir}/UNWS64.NWS64R1  -mautosource
