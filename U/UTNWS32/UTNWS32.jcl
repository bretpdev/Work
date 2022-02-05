#UTNWS32.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS32.NWS32R1
then
rm ${reportdir}/UNWS32.NWS32R1
fi
if test -a ${reportdir}/UNWS32.NWS32RZ
then
rm ${reportdir}/UNWS32.NWS32RZ
fi
if test -a ${reportdir}/UNWS32.NWS32R2
then
rm ${reportdir}/UNWS32.NWS32R2
fi

# run the program

sas ${codedir}/UTNWS32.sas -log ${reportdir}/UNWS32.NWS32R1  -mautosource
