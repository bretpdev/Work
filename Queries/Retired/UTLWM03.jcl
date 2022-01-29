#UTLWM03.jcl  Loan Servicing Center Delinquency Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWM03.LWM03R1
   then
        rm ${reportdir}/ULWM03.LWM03R1
fi
if test -a ${reportdir}/ULWM03.LWM03R2
   then
        rm ${reportdir}/ULWM03.LWM03R2
fi
if test -a ${reportdir}/ULWM03.LWM03R3
   then
        rm ${reportdir}/ULWM03.LWM03R3
fi
if test -a ${reportdir}/ULWM03.LWM03R4
   then
        rm ${reportdir}/ULWM03.LWM03R4
fi
if test -a ${reportdir}/ULWM03.LWM03R5
   then
        rm ${reportdir}/ULWM03.LWM03R5
fi
if test -a ${reportdir}/ULWM03.LWM03R6
   then
        rm ${reportdir}/ULWM03.LWM03R6
fi

# run the program

sas ${codedir}/UTLWM03.sas -log ${reportdir}/ULWM03.LWM03R1  -mautosource
