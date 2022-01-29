#UTLWG26.jcl  Loan Origination Cancelled Queue Statistic
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG26.LWG26R1
   then
        rm ${reportdir}/ULWG26.LWG26R1
fi
if test -a ${reportdir}/ULWG26.LWG26R2
   then
        rm ${reportdir}/ULWG26.LWG26R2
fi

# run the program

sas ${codedir}/UTLWG26.sas -log ${reportdir}/ULWG26.LWG26R1  -mautosource
