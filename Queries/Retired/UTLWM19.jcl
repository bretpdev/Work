#UTLWM19.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM19.LWM19R1
then
rm ${reportdir}/ULWM19.LWM19R1
fi
if test -a ${reportdir}/ULWM19.LWM19R2
then
rm ${reportdir}/ULWM19.LWM19R2
fi
if test -a ${reportdir}/ULWM19.LWM19R3
then
rm ${reportdir}/ULWM19.LWM19R3
fi
if test -a ${reportdir}/ULWM19.LWM19R4
then
rm ${reportdir}/ULWM19.LWM19R4
fi
if test -a ${reportdir}/ULWM19.LWM19RZ
then
rm ${reportdir}/ULWM19.LWM19RZ
fi

# run the program

sas ${codedir}/UTLWM19.sas -log ${reportdir}/ULWM19.LWM19R1  -mautosource
