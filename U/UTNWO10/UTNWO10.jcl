#UNWO10.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWO10.NWO10R1
then
rm ${reportdir}/UNWO10.NWO10R1
fi
if test -a ${reportdir}/UNWO10.NWO10RZ
then
rm ${reportdir}/UNWO10.NWO10RZ
fi
if test -a ${reportdir}/UNWO10.NWO10R2
then
rm ${reportdir}/UNWO10.NWO10R2
fi


# run the program

sas ${codedir}/UTNWO10.sas -log ${reportdir}/UNWO10.NWO10R1  -mautosource
