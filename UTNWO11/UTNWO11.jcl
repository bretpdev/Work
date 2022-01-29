#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWO11.NWO11R1
then
rm ${reportdir}/UNWO11.NWO11R1
fi
if test -a ${reportdir}/UNWO11.NWO11R2
then
rm ${reportdir}/UNWO11.NWO11R2
fi

# run the program

sas ${codedir}/UTNWO11.sas -log ${reportdir}/UNWO11.NWO11R1  -mautosource
