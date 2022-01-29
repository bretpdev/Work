#UTNWO18.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWO18.NWO18R1
then
rm ${reportdir}/UNWO18.NWO18R1
fi
if test -a ${reportdir}/UNWO18.NWO18RZ
then
rm ${reportdir}/UNWO18.NWO18RZ
fi
if test -a ${reportdir}/UNWO18.NWO18R2
then
rm ${reportdir}/UNWO18.NWO18R2
fi
if test -a ${reportdir}/UNWO18.NWO18R3
then
rm ${reportdir}/UNWO18.NWO18R3
fi
if test -a ${reportdir}/UNWO18.NWO18R4
then
rm ${reportdir}/UNWO18.NWO18R4
fi
if test -a ${reportdir}/UNWO18.NWO18R5
then
rm ${reportdir}/UNWO18.NWO18R5
fi
if test -a ${reportdir}/UNWO18.NWO18R6
then
rm ${reportdir}/UNWO18.NWO18R6
fi


# run the program

sas ${codedir}/UTNWO18.sas -log ${reportdir}/UNWO18.NWO18R1  -mautosource
