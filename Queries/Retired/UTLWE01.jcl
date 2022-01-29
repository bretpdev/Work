#UTLWE01.jcl  Accounts in Workgroup 15
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE01.LWE01R1
   then
        rm ${reportdir}/ULWE01.LWE01R1
fi
if test -a ${reportdir}/ULWE01.LWE01R2
   then
        rm ${reportdir}/ULWE01.LWE01R2
fi

# run the program

sas ${codedir}/UTLWE01.sas -log ${reportdir}/ULWE01.LWE01R1  -mautosource
