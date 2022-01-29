#UTNWS25.JCL CornerStone Monthly IBR Tracking

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS25.NWS25R1
then
rm ${reportdir}/UNWS25.NWS25R1
fi
if test -a ${reportdir}/UNWS25.NWS25R2
then
rm ${reportdir}/UNWS25.NWS25R2
fi

# run the program

sas ${codedir}/UTNWS25.sas -log ${reportdir}/UNWS25.NWS25R1  -mautosource
