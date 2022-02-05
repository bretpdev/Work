#UTNWC06.jcl Monthly CornerStone Bankruptcy Data - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC06.NWC06R1
then
rm ${reportdir}/UNWC06.NWC06R1
fi
if test -a ${reportdir}/UNWC06.NWC06R2
then
rm ${reportdir}/UNWC06.NWC06R2
fi
if test -a ${reportdir}/UNWC06.NWC06R3
then
rm ${reportdir}/UNWC06.NWC06R3
fi
if test -a ${reportdir}/UNWC06.NWC06R4
then
rm ${reportdir}/UNWC06.NWC06R4
fi

# run the program

sas ${codedir}/UTNWC06.sas -log ${reportdir}/UNWC06.NWC06R1  -mautosource
