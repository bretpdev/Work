#UTLWO62.jcl Referral Lender Stats Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO62.LWO62R1
then
rm ${reportdir}/ULWO62.LWO62R1
fi
if test -a ${reportdir}/ULWO62.LWO62R2
then
rm ${reportdir}/ULWO62.LWO62R2
fi
if test -a ${reportdir}/ULWO62.LWO62RZ
then
rm ${reportdir}/ULWO62.LWO62RZ
fi

# run the program

sas ${codedir}/UTLWO62.sas -log ${reportdir}/ULWO62.LWO62R1  -mautosource
