#UTNWC07.jcl Monthly CornerStone Bankruptcy Aging Data - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC07.NWC07R1
then
rm ${reportdir}/UNWC07.NWC07R1
fi
if test -a ${reportdir}/UNWC07.NWC07R2
then
rm ${reportdir}/UNWC07.NWC07R2
fi
if test -a ${reportdir}/UNWC07.NWC07R3
then
rm ${reportdir}/UNWC07.NWC07R3
fi
if test -a ${reportdir}/UNWC07.NWC07R4
then
rm ${reportdir}/UNWC07.NWC07R4
fi

# run the program

sas ${codedir}/UTNWC07.sas -log ${reportdir}/UNWC07.NWC07R1  -mautosource
