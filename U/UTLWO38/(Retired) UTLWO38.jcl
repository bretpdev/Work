#UTLWO38.jcl New Disclosures w/Extra Pymt Amt on ACH
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO38.LWO38R1
then
rm ${reportdir}/ULWO38.LWO38R1
fi
if test -a ${reportdir}/ULWO38.LWO38RZ
then
rm ${reportdir}/ULWO38.LWO38RZ
fi
if test -a ${reportdir}/ULWO38.LWO38R2
then
rm ${reportdir}/ULWO38.LWO38R2
fi

# run the program

sas ${codedir}/UTLWO38.sas -log ${reportdir}/ULWO38.LWO38R1  -mautosource
