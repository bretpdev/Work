#UTNWS89.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS89.NWS89R1
then
rm ${reportdir}/UNWS89.NWS89R1
fi
if test -a ${reportdir}/UNWS89.NWS89RZ
then
rm ${reportdir}/UNWS89.NWS89RZ
fi
if test -a ${reportdir}/UNWS89.NWS89R2
then
rm ${reportdir}/UNWS89.NWS89R2
fi


# run the program

sas ${codedir}/UTNWS89.sas -log ${reportdir}/UNWS89.NWS89R1  -mautosource
