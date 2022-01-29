#UTLWG89.jcl Denial Code D7 No Waiver
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG89.LWG89R1
then
rm ${reportdir}/ULWG89.LWG89R1
fi
if test -a ${reportdir}/ULWG89.LWG89R2
then
rm ${reportdir}/ULWG89.LWG89R2
fi
if test -a ${reportdir}/ULWG89.LWG89RZ
then
rm ${reportdir}/ULWG89.LWG89RZ
fi

# run the program

sas ${codedir}/UTLWG89.sas -log ${reportdir}/ULWG89.LWG89R1  -mautosource
