#UTLWO78.jcl Web Refund Notifications Within the Past 7 Days
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO78.LWO78R1
then
rm ${reportdir}/ULWO78.LWO78R1
fi
if test -a ${reportdir}/ULWO78.LWO78R2
then
rm ${reportdir}/ULWO78.LWO78R2
fi
if test -a ${reportdir}/ULWO78.LWO78RZ
then
rm ${reportdir}/ULWO78.LWO78RZ
fi

# run the program

sas ${codedir}/UTLWO78.sas -log ${reportdir}/ULWO78.LWO78R1  -mautosource
