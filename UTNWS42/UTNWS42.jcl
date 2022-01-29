#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS42.NWS42R1
then
rm ${reportdir}/UNWS42.NWS42R1
fi
if test -a ${reportdir}/UNWS42.NWS42R2
then
rm ${reportdir}/UNWS42.NWS42R2
fi

# run the program

sas ${codedir}/UTNWS42.sas -log ${reportdir}/UNWS42.NWS42R1  -mautosource
