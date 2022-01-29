#UTLWM04.jcl  Special Campaign High Balances
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWM04.LWM04R1
   then
        rm ${reportdir}/ULWM04.LWM04R1
fi
if test -a ${reportdir}/ULWM04.LWM04R2
   then
        rm ${reportdir}/ULWM04.LWM04R2
fi
if test -a ${reportdir}/ULWM04.LWM04R3
   then
        rm ${reportdir}/ULWM04.LWM04R3
fi

# run the program

sas ${codedir}/UTLWM04.sas -log ${reportdir}/ULWM04.LWM04R1  -mautosource
