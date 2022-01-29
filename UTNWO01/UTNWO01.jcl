#UTNWO01.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWO01.NWO01R1
then
rm ${reportdir}/UNWO01.NWO01R1
fi
if test -a ${reportdir}/UNWO01.NWO01RZ
then
rm ${reportdir}/UNWO01.NWO01RZ
fi
if test -a ${reportdir}/UNWO01.NWO01R2
then
rm ${reportdir}/UNWO01.NWO01R2
fi

# run the program

sas ${codedir}/UTNWO01.sas -log ${reportdir}/UNWO01.NWO01R1  -mautosource
