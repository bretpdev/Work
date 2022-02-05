#UTNWS91.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS91.NWS91R1
then
rm ${reportdir}/UNWS91.NWS91R1
fi
if test -a ${reportdir}/UNWS91.NWS91RZ
then
rm ${reportdir}/UNWS91.NWS91RZ
fi
if test -a ${reportdir}/UNWS91.NWS91R2
then
rm ${reportdir}/UNWS91.NWS91R2
fi
if test -a ${reportdir}/UNWS91.NWS91R3
then
rm ${reportdir}/UNWS91.NWS91R3
fi
if test -a ${reportdir}/UNWS91.NWS91R4
then
rm ${reportdir}/UNWS91.NWS91R4
fi


# run the program

sas ${codedir}/UTNWS91.sas -log ${reportdir}/UNWS91.NWS91R1  -mautosource
