#UTLWC04.jcl  Claims in Process
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWC04.LWC04R1
   then
        rm ${reportdir}/ULWC04.LWC04R1
fi
if test -a ${reportdir}/ULWC04.LWC04R2
   then
        rm ${reportdir}/ULWC04.LWC04R2
fi

# run the program

sas ${codedir}/UTLWC04.sas -log ${reportdir}/ULWC04.LWC04R1  -mautosource
