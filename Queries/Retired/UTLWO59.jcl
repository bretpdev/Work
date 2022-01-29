#UTLWO59.jcl Referral Lender Loans Received Previous Day
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO59.LWO59R1
then
rm ${reportdir}/ULWO59.LWO59R1
fi
if test -a ${reportdir}/ULWO59.LWO59R2
then
rm ${reportdir}/ULWO59.LWO59R2
fi
if test -a ${reportdir}/ULWO59.LWO59RZ
then
rm ${reportdir}/ULWO59.LWO59RZ
fi

# run the program

sas ${codedir}/UTLWO59.sas -log ${reportdir}/ULWO59.LWO59R1  -mautosource
