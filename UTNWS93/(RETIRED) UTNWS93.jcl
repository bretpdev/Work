#UTNWS93.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS93.NWS93R1
then
rm ${reportdir}/UNWS93.NWS93R1
fi
if test -a ${reportdir}/UNWS93.NWS93RZ
then
rm ${reportdir}/UNWS93.NWS93RZ
fi
if test -a ${reportdir}/UNWS93.NWS93R2
then
rm ${reportdir}/UNWS93.NWS93R2
fi


# run the program

sas ${codedir}/UTNWS93.sas -log ${reportdir}/UNWS93.NWS93R1  -mautosource
