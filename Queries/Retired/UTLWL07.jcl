#UTLWL07.jcl  LPP STALE ANTICIPATED DISBURSEMENT REPORT
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWL07.LWL07R1
   then
        rm ${reportdir}/ULWL07.LWL07R1
fi
if test -a ${reportdir}/ULWL07.LWL07R2
   then
        rm ${reportdir}/ULWL07.LWL07R2
fi
if test -a ${reportdir}/ULWL07.LWL07RZ
   then
        rm ${reportdir}/ULWL07.LWL07RZ
fi

# run the program

sas ${codedir}/UTLWL07.sas -log ${reportdir}/ULWL07.LWL07R1  -mautosource
