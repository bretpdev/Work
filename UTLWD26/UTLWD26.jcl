#UTLWD26.jcl Offset Payment Received after Bankruptcy Filing
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD26.LWD26R1
   then
        rm ${reportdir}/ULWD26.LWD26R1
fi
if test -a ${reportdir}/ULWD26.LWD26R2
   then
        rm ${reportdir}/ULWD26.LWD26R2
fi

# run the program

sas ${codedir}/UTLWD26.sas -log ${reportdir}/ULWD26.LWD26R1  -mautosource
