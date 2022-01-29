#UTLWM41.jcl Default Rehab Col Cost Amnesty
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM41.LWM41R1
then
rm ${reportdir}/ULWM41.LWM41R1
fi
if test -a ${reportdir}/ULWM41.LWM41RZ
then
rm ${reportdir}/ULWM41.LWM41RZ
fi
if test -a ${reportdir}/ULWM41.LWM41R2
then
rm ${reportdir}/ULWM41.LWM41R2
fi

# run the program

sas ${codedir}/UTLWM41.sas -log ${reportdir}/ULWM41.LWM41R1  -mautosource
