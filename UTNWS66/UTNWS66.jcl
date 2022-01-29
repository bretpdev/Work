#UTNWS66.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS66.NWS66R1
then
rm ${reportdir}/UNWS66.NWS66R1
fi
if test -a ${reportdir}/UNWS66.NWS66RZ
then
rm ${reportdir}/UNWS66.NWS66RZ
fi
if test -a ${reportdir}/UNWS66.NWS66R2
then
rm ${reportdir}/UNWS66.NWS66R2
fi

# run the program

sas ${codedir}/UTNWS66.sas -log ${reportdir}/UNWS66.NWS66R1  -mautosource
