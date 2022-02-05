#UTLWS02.jcl  Loan Servicing Center Delinquency Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS02.LWS02R1
   then
        rm ${reportdir}/ULWS02.LWS02R1
fi
if test -a ${reportdir}/ULWS02.LWS02R2
   then
        rm ${reportdir}/ULWS02.LWS02R2
fi
if test -a ${reportdir}/ULWS02.LWS02R3
   then
        rm ${reportdir}/ULWS02.LWS02R3
fi
if test -a ${reportdir}/ULWS02.LWS02R4
   then
        rm ${reportdir}/ULWS02.LWS02R4
fi

# run the program

sas ${codedir}/UTLWS02.sas -log ${reportdir}/ULWS02.LWS02R1  -mautosource
