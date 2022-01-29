#UTNWS94.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS94.NWS94R1
then
rm ${reportdir}/UNWS94.NWS94R1
fi
if test -a ${reportdir}/UNWS94.NWS94RZ
then
rm ${reportdir}/UNWS94.NWS94RZ
fi
if test -a ${reportdir}/UNWS94.NWS94R2
then
rm ${reportdir}/UNWS94.NWS94R2
fi


# run the program

sas ${codedir}/UTNWS94.sas -log ${reportdir}/UNWS94.NWS94R1  -mautosource
