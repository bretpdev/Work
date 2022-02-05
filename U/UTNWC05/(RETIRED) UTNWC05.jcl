#UTNWC05.JCL CornerStone Loans Rehabilitated

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC05.NWC05R1
then
rm ${reportdir}/UNWC05.NWC05R1
fi
if test -a ${reportdir}/UNWC05.NWC05RZ
then
rm ${reportdir}/UNWC05.NWC05RZ
fi
if test -a ${reportdir}/UNWC05.NWC05R2
then
rm ${reportdir}/UNWC05.NWC05R2
fi
if test -a ${reportdir}/UNWC05.NWC05R3
then
rm ${reportdir}/UNWC05.NWC05R3
fi

# run the program

sas ${codedir}/UTNWC05.sas -log ${reportdir}/UNWC05.NWC05R1  -mautosource
