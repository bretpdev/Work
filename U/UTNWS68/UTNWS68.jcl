#UTNWS68.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS68.NWS68R1
then
rm ${reportdir}/UNWS68.NWS68R1
fi
if test -a ${reportdir}/UNWS68.NWS68RZ
then
rm ${reportdir}/UNWS67.NWS67RZ
fi
if test -a ${reportdir}/UNWS68.NWS68R2
then
rm ${reportdir}/UNWS68.NWS68R2
fi

if test -a ${reportdir}/UNWS68.NWS68R3
then
rm ${reportdir}/UNWS68.NWS68R3
fi

# run the program

sas ${codedir}/UTNWS68.sas -log ${reportdir}/UNWS68.NWS68R1  -mautosource
