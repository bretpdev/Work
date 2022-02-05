#UTNWS67.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS67.NWS67R1
then
rm ${reportdir}/UNWS67.NWS67R1
fi
if test -a ${reportdir}/UNWS67.NWS67RZ
then
rm ${reportdir}/UNWS67.NWS67RZ
fi
if test -a ${reportdir}/UNWS67.NWS67R2
then
rm ${reportdir}/UNWS67.NWS67R2
fi

# run the program

sas ${codedir}/UTNWS67.sas -log ${reportdir}/UNWS67.NWS67R1  -mautosource
