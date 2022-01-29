#UTLWS05.jcl  Loan Sale Transfer Letter File
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS05.LWS05R1
   then
        rm ${reportdir}/ULWS05.LWS05R1
fi
if test -a ${reportdir}/ULWS05.LWS05R2
   then
        rm ${reportdir}/ULWS05.LWS05R2
fi
if test -a ${reportdir}/ULWS05.LWS05R3
   then
        rm ${reportdir}/ULWS05.LWS05R3
fi
if test -a ${reportdir}/ULWS05.LWS05R4
   then
        rm ${reportdir}/ULWS05.LWS05R4
fi
if test -a ${reportdir}/ULWS05.LWS05R5
   then
        rm ${reportdir}/ULWS05.LWS05R5
fi

# run the program

sas ${codedir}/UTLWS05.sas -log ${reportdir}/ULWS05.LWS05R1  -mautosource
