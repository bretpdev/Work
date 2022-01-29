#UTNWS96.Defense Inquiries By School - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS96.NWS96R1
then
rm ${reportdir}/UNWS96.NWS96R1
fi
if test -a ${reportdir}/UNWS96.NWS96R2
then
rm ${reportdir}/UNWS96.NWS96R2
fi

# run the program

sas ${codedir}/UTNWS96.sas -log ${reportdir}/UNWS96.NWS96R1  -mautosource
