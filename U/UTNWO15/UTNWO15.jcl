#UTNWO15.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWO15.NWO15R1
then
rm ${reportdir}/UNWO15.NWO15R1
fi
if test -a ${reportdir}/UNWO15.NWO15RZ
then
rm ${reportdir}/UNWO15.NWO15RZ
fi
if test -a ${reportdir}/UNWO15.NWO15R2
then
rm ${reportdir}/UNWO15.NWO15R2
fi

# run the program

sas ${codedir}/UTNWO15.sas -log ${reportdir}/UNWO15.NWO15R1  -mautosource
