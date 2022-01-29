#UTNWR02.jcl 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWR02.NWR02R1
then
rm ${reportdir}/UNWR02.NWR02R1
fi
if test -a ${reportdir}/UNWR02.NWR02R2
then
rm ${reportdir}/UNWR02.NWR02R2
fi

# run the program

sas ${codedir}/UTNWR02.sas -log ${reportdir}/UNWR02.NWR02R1  -mautosource
