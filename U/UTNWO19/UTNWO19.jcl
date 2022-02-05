#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWO19.NWO19R1
then
rm ${reportdir}/UNWO19.NWO19R1
fi
if test -a ${reportdir}/UNWO19.NWO19R2
then
rm ${reportdir}/UNWO19.NWO19R2
fi

# run the program

sas ${codedir}/UTNWO19.sas -log ${reportdir}/UNWO19.NWO19R1  -mautosource
