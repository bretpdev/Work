#UTNWO16.jcl 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UTNWO16.NWO16R1
then
rm ${reportdir}/UTNWO16.NWO16R1
fi
if test -a ${reportdir}/UTNWO16.NWO16R2
then
rm ${reportdir}/UTNWO16.NWO16R2
fi


# run the program

sas ${codedir}/UTNWO16.sas -log ${reportdir}/UTNWO16.NWO16R1  -mautosource
