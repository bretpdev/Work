#UTLWE22.jcl  Deseret 1st CU EOM DATA
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE22.LWE22R1
   then
        rm ${reportdir}/ULWE22.LWE22R1
fi
if test -a ${reportdir}/ULWE22.LWE22R2
   then
        rm ${reportdir}/ULWE22.LWE22R2
fi

# run the program

sas ${codedir}/UTLWE22.sas -log ${reportdir}/ULWE22.LWE22R1  -mautosource
