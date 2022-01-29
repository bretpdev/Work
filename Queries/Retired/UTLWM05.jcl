#UTLWM05.jcl  Special Campaign - Chronic Delinquents
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWM05.LWM05R1
   then
        rm ${reportdir}/ULWM05.LWM05R1
fi
if test -a ${reportdir}/ULWM05.LWM05R2
   then
        rm ${reportdir}/ULWM05.LWM05R2
fi
if test -a ${reportdir}/ULWM05.LWM05R3
   then
        rm ${reportdir}/ULWM05.LWM05R3
fi
if test -a ${reportdir}/ULWM05.LWM05R4
   then
        rm ${reportdir}/ULWM05.LWM05R4
fi
if test -a ${reportdir}/ULWM05.LWM05R5
   then
        rm ${reportdir}/ULWM05.LWM05R5
fi

# run the program

sas ${codedir}/UTLWM05.sas -log ${reportdir}/ULWM05.LWM05R1  -mautosource
