#UTNWO04.jcl Monthly CornerStone Bankruptcy Aging Data - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWO04.NW007R1
then
rm ${reportdir}/UNWO04.NWO04R1
fi
if test -a ${reportdir}/UNWO04.NWO04R2
then
rm ${reportdir}/UNWO04.NWO04R2
fi

# run the program

sas ${codedir}/UTNWO04.sas -log ${reportdir}/UNWO04.NWO04R1  -mautosource
