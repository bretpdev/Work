#UTLWM15.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM15.LWM15R1
then
rm ${reportdir}/ULWM15.LWM15R1
fi
if test -a ${reportdir}/ULWM15.LWM15R2
then
rm ${reportdir}/ULWM15.LWM15R2
fi
if test -a ${reportdir}/ULWM15.LWM15R3
then
rm ${reportdir}/ULWM15.LWM15R3
fi
if test -a ${reportdir}/ULWM15.LWM15R4
then
rm ${reportdir}/ULWM15.LWM15R4
fi
if test -a ${reportdir}/ULWM15.LWM15RZ
then
rm ${reportdir}/ULWM15.LWM15RZ
fi

# run the program

sas ${codedir}/UTLWM15.sas -log ${reportdir}/ULWM15.LWM15R1  -mautosource
