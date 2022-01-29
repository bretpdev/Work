#UTLWM18.jcl Exitcalls for Loan Management
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM18.LWM18R1
then
rm ${reportdir}/ULWM18.LWM18R1
fi
if test -a ${reportdir}/ULWM18.LWM18R2
then
rm ${reportdir}/ULWM18.LWM18R2
fi
if test -a ${reportdir}/ULWM18.LWM18RZ
then
rm ${reportdir}/ULWM18.LWM18RZ
fi

# run the program

sas ${codedir}/UTLWM18.sas -log ${reportdir}/ULWM18.LWM18R1  -mautosource
