#UTLWE24.jcl   
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE24.LWE24R1
   then
        rm ${reportdir}/ULWE24.LWE24R1
fi
if test -a ${reportdir}/ULWE24.LWE24R2
   then
        rm ${reportdir}/ULWE24.LWE24R2
fi

# run the program

sas ${codedir}/UTLWE24.sas -log ${reportdir}/ULWE24.LWE24R1  -mautosource
