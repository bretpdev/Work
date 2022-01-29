#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC17.NWC17R1
then
rm ${reportdir}/UNWC17.NWC17R1
fi
if test -a ${reportdir}/UNWC17.NWC17R2
then
rm ${reportdir}/UNWC17.NWC17R2
fi
if test -a ${reportdir}/UNWC17.NWC17R3
then
rm ${reportdir}/UNWC17.NWC17R3
fi
if test -a ${reportdir}/UNWC17.NWC17R4
then
rm ${reportdir}/UNWC17.NWC17R4
fi

# run the program

sas ${codedir}/UTNWC17.sas -log ${reportdir}/UNWC17.NWC17R1  -mautosource
