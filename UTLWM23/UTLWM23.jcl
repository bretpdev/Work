#UTLWM23.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM23.LWM23R1
then
rm ${reportdir}/ULWM23.LWM23R1
fi
if test -a ${reportdir}/ULWM23.LWM23R2
then
rm ${reportdir}/ULWM23.LWM23R2
fi
if test -a ${reportdir}/ULWM23.LWM23R3
then
rm ${reportdir}/ULWM23.LWM23R3
fi
if test -a ${reportdir}/ULWM23.LWM23R4
then
rm ${reportdir}/ULWM23.LWM23R4
fi
if test -a ${reportdir}/ULWM23.LWM23R5
then
rm ${reportdir}/ULWM23.LWM23R5
fi
if test -a ${reportdir}/ULWM23.LWM23R6
then
rm ${reportdir}/ULWM23.LWM23R6
fi
if test -a ${reportdir}/ULWM23.LWM23R7
then
rm ${reportdir}/ULWM23.LWM23R7
fi
if test -a ${reportdir}/ULWM23.LWM23R8
then
rm ${reportdir}/ULWM23.LWM23R8
fi
if test -a ${reportdir}/ULWM23.LWM23R9
then
rm ${reportdir}/ULWM23.LWM23R9
fi
if test -a ${reportdir}/ULWM23.LWM23R10
then
rm ${reportdir}/ULWM23.LWM23R10
fi
if test -a ${reportdir}/ULWM23.LWM23RZ
then
rm ${reportdir}/ULWM23.LWM23RZ
fi

# run the program

sas ${codedir}/UTLWM23.sas -log ${reportdir}/ULWM23.LWM23R1  -mautosource
