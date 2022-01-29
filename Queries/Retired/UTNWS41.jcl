#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS41.NWS41R1
then
rm ${reportdir}/UNWS41.NWS41R1
fi
if test -a ${reportdir}/UNWS41.NWS41R2
then
rm ${reportdir}/UNWS41.NWS41R2
fi

# run the program

sas ${codedir}/UTNWS41.sas -log ${reportdir}/UNWS41.NWS41R1  -mautosource
