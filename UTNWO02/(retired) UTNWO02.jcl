#UTNWO02.JCL Billing Statements (FED)

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWO02.NWO02R1
then
rm ${reportdir}/UNWO02.NWO02R1
fi
if test -a ${reportdir}/UNWO02.NWO02R2
then
rm ${reportdir}/UNWO02.NWO02R2
fi
if test -a ${reportdir}/UNWO02.NWO02R3
then
rm ${reportdir}/UNWO02.NWO02R3
fi
if test -a ${reportdir}/UNWO02.NWO02R4
then
rm ${reportdir}/UNWO02.NWO02R4
fi
if test -a ${reportdir}/UNWO02.NWO02R5
then
rm ${reportdir}/UNWO02.NWO02R5
fi


# run the program

sas ${codedir}/UTNWO02.sas -log ${reportdir}/UNWO02.NWO02R1  -mautosource
