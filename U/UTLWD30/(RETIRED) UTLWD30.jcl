#UTLWD30.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWD30.LWD30R1
then
rm ${reportdir}/ULWD30.LWD30R1
fi
if test -a ${reportdir}/ULWD30.LWD30R2
then
rm ${reportdir}/ULWD30.LWD30R2
fi
if test -a ${reportdir}/ULWD30.LWD30R3
then
rm ${reportdir}/ULWD30.LWD30R3
fi
if test -a ${reportdir}/ULWD30.LWD30RZ
then
rm ${reportdir}/ULWD30.LWD30RZ
fi

# run the program

sas ${codedir}/UTLWD30.sas -log ${reportdir}/ULWD30.LWD30R1  -mautosource
