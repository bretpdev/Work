#UTNWS90.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS90.NWS90R1
then
rm ${reportdir}/UNWS90.NWS90R1
fi
if test -a ${reportdir}/UNWS90.NWS90RZ
then
rm ${reportdir}/UNWS90.NWS90RZ
fi
if test -a ${reportdir}/UNWS90.NWS90R2
then
rm ${reportdir}/UNWS90.NWS90R2
fi
if test -a ${reportdir}/UNWS90.NWS90R3
then
rm ${reportdir}/UNWS90.NWS90R3
fi

# run the program

sas ${codedir}/UTNWS90.sas -log ${reportdir}/UNWS90.NWS90R1  -mautosource
