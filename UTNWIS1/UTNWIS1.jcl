#UTNWIS1.jcl Monthly CornerStone Bankruptcy Aging Data - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWIS1.NWIS1R1
then
rm ${reportdir}/UNWIS1.NWIS1R1
fi
if test -a ${reportdir}/UNWIS1.NWIS1R2
then
rm ${reportdir}/UNWIS1.NWIS1R2
fi

# run the program

sas ${codedir}/UTNWIS1.sas -log ${reportdir}/UNWIS1.NWIS1R1  -mautosource
