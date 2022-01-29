#UTLWM07.jcl FIRST PAYMENT MISSED SPECIAL CAMPAIGN
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWM07.LWM07R1
   then
        rm ${reportdir}/ULWM07.LWM07R1
fi
if test -a ${reportdir}/ULWM07.LWM07R2
   then
        rm ${reportdir}/ULWM07.LWM07R2
fi
if test -a ${reportdir}/ULWM07.LWM07R3
   then
        rm ${reportdir}/ULWM07.LWM07R3
fi

# run the program

sas ${codedir}/UTLWM07.sas -log ${reportdir}/ULWM07.LWM07R1  -mautosource
