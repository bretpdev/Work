#UTNWC11.JCL CornerStone Loans Consolidated - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC11.NWC11R1
then
rm ${reportdir}/UNWC11.NWC11R1
fi
if test -a ${reportdir}/UNWC11.NWC11R2
then
rm ${reportdir}/UNWC11.NWC11R2
fi

# run the program

sas ${codedir}/UTNWC11.sas -log ${reportdir}/UNWC11.NWC11R1  -mautosource
