#UTLWM35.jcl AMNESTY HIGH BALANCE
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM35.LWM35R1
then
rm ${reportdir}/ULWM35.LWM35R1
fi
if test -a ${reportdir}/ULWM35.LWM35R2
then
rm ${reportdir}/ULWM35.LWM35R2
fi
if test -a ${reportdir}/ULWM35.LWM35R3
then
rm ${reportdir}/ULWM35.LWM35R3
fi
if test -a ${reportdir}/ULWM35.LWM35RZ
then
rm ${reportdir}/ULWM35.LWM35RZ
fi

# run the program

sas ${codedir}/UTLWM35.sas -log ${reportdir}/ULWM35.LWM35R1  -mautosource
