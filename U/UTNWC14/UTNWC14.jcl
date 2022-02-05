#UTNWC14.JCL Quarterly 10,000 Account Extracts CornerStone - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC14.NWC14R1
then
rm ${reportdir}/UNWC14.NWC14R1
fi
if test -a ${reportdir}/UNWC14.NWC14R2
then
rm ${reportdir}/UNWC14.NWC14R2
fi

# run the program

sas ${codedir}/UTNWC14.sas -log ${reportdir}/UNWC14.NWC14R1  -mautosource
