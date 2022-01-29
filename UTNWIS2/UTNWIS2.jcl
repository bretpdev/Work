#UTNWIS2.jcl Monthly CornerStone Bankruptcy Aging Data - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWIS2.NWIS2R1
then
rm ${reportdir}/UNWIS2.NWIS2R1
fi
if test -a ${reportdir}/UNWIS2.NWIS2R2
then
rm ${reportdir}/UNWIS2.NWIS2R2
fi

# run the program

sas ${codedir}/UTNWIS2.sas -log ${reportdir}/UNWIS2.NWIS2R1  -mautosource
