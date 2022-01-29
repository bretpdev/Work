#UTNWC19.JCL Billing Statements (FED)

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWC19.NWC19R1
then
rm ${reportdir}/UNWC19.NWC19R1
fi
if test -a ${reportdir}/UNWC19.NWC19R2
then
rm ${reportdir}/UNWC19.NWC19R2
fi


# run the program

sas ${codedir}/UTNWC19.sas -log ${reportdir}/UNWC19.NWC19R1  -mautosource
