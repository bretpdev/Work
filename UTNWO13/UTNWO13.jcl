#UTNWO13.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWO13.NWO13R1
then
rm ${reportdir}/UNWO13.NWO13R1
fi
if test -a ${reportdir}/UNWO13.NWO13RZ
then
rm ${reportdir}/UNWO13.NWO13RZ
fi
if test -a ${reportdir}/UNWO13.NWO13R2
then
rm ${reportdir}/UNWO13.NWO13R2
fi
if test -a ${reportdir}/UNWO13.NWO13R2
then
rm ${reportdir}/UNWO13.NWO13R2
fi

# run the program

sas ${codedir}/UTNWO13.sas -log ${reportdir}/UNWO13.NWO13R1  -mautosource
