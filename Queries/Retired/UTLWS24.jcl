#UTLWS24.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS24.LWS24R1
then
rm ${reportdir}/ULWS24.LWS24R1
fi
if test -a ${reportdir}/ULWS24.LWS24R2
then
rm ${reportdir}/ULWS24.LWS24R2
fi
if test -a ${reportdir}/ULWS24.LWS24R3
then
rm ${reportdir}/ULWS24.LWS24R3
fi
if test -a ${reportdir}/ULWS24.LWS24R4
then
rm ${reportdir}/ULWS24.LWS24R4
fi
if test -a ${reportdir}/ULWS24.LWS24RZ
then
rm ${reportdir}/ULWS24.LWS24RZ
fi

# run the program

sas ${codedir}/UTLWS24.sas -log ${reportdir}/ULWS24.LWS24R1  -mautosource
