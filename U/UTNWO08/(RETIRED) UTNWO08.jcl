#UNWO08.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWO08.NWO08R1
then
rm ${reportdir}/UNWO08.NWO08R1
fi
if test -a ${reportdir}/UNWO08.NWO08RZ
then
rm ${reportdir}/UNWO08.NWO08RZ
fi
if test -a ${reportdir}/UNWO08.NWO08R2
then
rm ${reportdir}/UNWO08.NWO08R2
fi


# run the program

sas ${codedir}/UTNWO08.sas -log ${reportdir}/UNWO08.NWO08R1  -mautosource
