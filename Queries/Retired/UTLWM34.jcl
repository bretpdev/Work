#UTLWM34.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM34.LWM34R1
then
rm ${reportdir}/ULWM34.LWM34R1
fi
if test -a ${reportdir}/ULWM34.LWM34R2
then
rm ${reportdir}/ULWM34.LWM34R2
fi
if test -a ${reportdir}/ULWM34.LWM34R3
then
rm ${reportdir}/ULWM34.LWM34R3
fi
if test -a ${reportdir}/ULWM34.LWM34R4
then
rm ${reportdir}/ULWM34.LWM34R4
fi
if test -a ${reportdir}/ULWM34.LWM34RZ
then
rm ${reportdir}/ULWM34.LWM34RZ
fi

# run the program

sas ${codedir}/UTLWM34.sas -log ${reportdir}/ULWM34.LWM34R1  -mautosource
