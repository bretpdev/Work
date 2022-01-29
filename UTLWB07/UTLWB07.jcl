#UTLWB06.jcl Cohort Borr 300 or More Days Delinquent
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWB07.LWB07R1
then
rm ${reportdir}/ULWB07.LWB07R1
fi
if test -a ${reportdir}/ULWB07.LWB07RZ
then
rm ${reportdir}/ULWB07.LWB07RZ
fi
if test -a ${reportdir}/ULWB07.LWB07R2
then
rm ${reportdir}/ULWB07.LWB07R2
fi

# run the program

sas ${codedir}/UTLWB07.sas -log ${reportdir}/ULWB07.LWB07R1  -mautosource
