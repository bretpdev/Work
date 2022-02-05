#UTLWB06.jcl Cohort Borr 300 or More Days Delinquent
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWB06.LWB06R1
then
rm ${reportdir}/ULWB06.LWB06R1
fi
if test -a ${reportdir}/ULWB06.LWB06RZ
then
rm ${reportdir}/ULWB06.LWB06RZ
fi
if test -a ${reportdir}/ULWB06.LWB06R2
then
rm ${reportdir}/ULWB06.LWB06R2
fi

# run the program

sas ${codedir}/UTLWB06.sas -log ${reportdir}/ULWB06.LWB06R1  -mautosource
