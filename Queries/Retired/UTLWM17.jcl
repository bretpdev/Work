#UTLWM17.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM17.LWM17R1
then
rm ${reportdir}/ULWM17.LWM17R1
fi
if test -a ${reportdir}/ULWM17.LWM17R2
then
rm ${reportdir}/ULWM17.LWM17R2
fi
if test -a ${reportdir}/ULWM17.LWM17RZ
then
rm ${reportdir}/ULWM17.LWM17RZ
fi

# run the program

sas ${codedir}/UTLWM17.sas -log ${reportdir}/ULWM17.LWM17R1  -mautosource
