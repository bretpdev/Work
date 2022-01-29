#UTLWM02.jcl  Loan Servicing Center Delinquency Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWM02.LWM02R1
   then
        rm ${reportdir}/ULWM02.LWM02R1
fi
if test -a ${reportdir}/ULWM02.LWM02R2
   then
        rm ${reportdir}/ULWM02.LWM02R2
fi
if test -a ${reportdir}/ULWM02.LWM02R3
   then
        rm ${reportdir}/ULWM02.LWM02R3
fi
if test -a ${reportdir}/ULWM02.LWM02R4
   then
        rm ${reportdir}/ULWM02.LWM02R4
fi

# run the program

sas ${codedir}/UTLWM02.sas -log ${reportdir}/ULWM02.LWM02R1  -mautosource
