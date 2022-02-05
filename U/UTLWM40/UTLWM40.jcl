#UTLWM40.jcl Default Payment Arrangement Amnesty
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM40.LWM40R1
then
rm ${reportdir}/ULWM40.LWM40R1
fi
if test -a ${reportdir}/ULWM40.LWM40RZ
then
rm ${reportdir}/ULWM40.LWM40RZ
fi
if test -a ${reportdir}/ULWM40.LWM40R2
then
rm ${reportdir}/ULWM40.LWM40R2
fi
if test -a ${reportdir}/ULWM40.LWM40R3
then
rm ${reportdir}/ULWM40.LWM40R3
fi
if test -a ${reportdir}/ULWM40.LWM40R4
then
rm ${reportdir}/ULWM40.LWM40R4
fi
if test -a ${reportdir}/ULWM40.LWM40R5
then
rm ${reportdir}/ULWM40.LWM40R5
fi

# run the program

sas ${codedir}/UTLWM40.sas -log ${reportdir}/ULWM40.LWM40R1  -mautosource
