#UTLWQ14.jcl  Systems Support Cancelled Queue Statistic
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ14.LWQ14R1
   then
        rm ${reportdir}/ULWQ14.LWQ14R1
fi
if test -a ${reportdir}/ULWQ14.LWQ14R2
   then
        rm ${reportdir}/ULWQ14.LWQ14R2
fi

# run the program

sas ${codedir}/UTLWQ14.sas -log ${reportdir}/ULWQ14.LWQ14R1  -mautosource
