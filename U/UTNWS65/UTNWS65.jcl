#UTNWS65.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS65.NWS65R1
then
rm ${reportdir}/UNWS65.NWS65R1
fi
if test -a ${reportdir}/UNWS65.NWS65RZ
then
rm ${reportdir}/UNWS65.NWS65RZ
fi
if test -a ${reportdir}/UNWS65.NWS65R2
then
rm ${reportdir}/UNWS65.NWS65R2
fi

# run the program

sas ${codedir}/UTNWS65.sas -log ${reportdir}/UNWS65.NWS65R1  -mautosource
