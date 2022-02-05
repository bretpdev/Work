#UTNWO12.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWO12.NWO12R1
then
rm ${reportdir}/UNWO12.NWO12R1
fi
if test -a ${reportdir}/UNWO12.NWO12RZ
then
rm ${reportdir}/UNWO12.NWO12RZ
fi
if test -a ${reportdir}/UNWO12.NWO12R2
then
rm ${reportdir}/UNWO12.NWO12R2
fi

# run the program

sas ${codedir}/UTNWO12.sas -log ${reportdir}/UNWO12.NWO12R1  -mautosource
