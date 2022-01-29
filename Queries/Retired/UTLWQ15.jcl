#UTLWQ15.jcl  Total Estimated Anticipated Disbursement Report by school
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ15.LWQ15R1
   then
        rm ${reportdir}/ULWQ15.LWQ15R1
fi
if test -a ${reportdir}/ULWQ15.LWQ15R2
   then
        rm ${reportdir}/ULWQ15.LWQ15R2
fi

# run the program

sas ${codedir}/UTLWQ15.sas -log ${reportdir}/ULWQ15.LWQ15R1  -mautosource
