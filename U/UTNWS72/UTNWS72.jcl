#UTNWS72.Weekly Portfolio Balance Report - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS72.NWS72R1
then
rm ${reportdir}/UNWS72.NWS72R1
fi
if test -a ${reportdir}/UNWS72.NWS72R2
then
rm ${reportdir}/UNWS72.NWS72R2
fi

# run the program

sas ${codedir}/UTNWS72.sas -log ${reportdir}/UNWS72.NWS72R1  -mautosource
