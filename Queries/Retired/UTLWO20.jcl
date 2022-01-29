#UTLWO20.jcl  Account Services Cancelled Queue Statistic
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO20.LWO20R1
   then
        rm ${reportdir}/ULWO20.LWO20R1
fi
if test -a ${reportdir}/ULWO20.LWO20R2
   then
        rm ${reportdir}/ULWO20.LWO20R2
fi

# run the program

sas ${codedir}/UTLWO20.sas -log ${reportdir}/ULWO20.LWO20R1  -mautosource
