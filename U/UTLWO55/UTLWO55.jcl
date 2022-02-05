#UTLWO55.jcl Alignment Forbearance and New Loan Canceled QC Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO55.LWO55R1
then
rm ${reportdir}/ULWO55.LWO55R1
fi
if test -a ${reportdir}/ULWO55.LWO55R2
then
rm ${reportdir}/ULWO55.LWO55R2
fi
if test -a ${reportdir}/ULWO55.LWO55RZ
then
rm ${reportdir}/ULWO55.LWO55RZ
fi

# run the program

sas ${codedir}/UTLWO55.sas -log ${reportdir}/ULWO55.LWO55R1  -mautosource
