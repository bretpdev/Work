#UTLWD37.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWD37.LWD37R1
then
rm ${reportdir}/ULWD37.LWD37R1
fi
if test -a ${reportdir}/ULWD37.LWD37R2
then
rm ${reportdir}/ULWD37.LWD37R2
fi
if test -a ${reportdir}/ULWD37.LWD37R3
then
rm ${reportdir}/ULWD37.LWD37R3
fi
if test -a ${reportdir}/ULWD37.LWD37RZ
then
rm ${reportdir}/ULWD37.LWD37RZ
fi

# run the program

sas ${codedir}/UTLWD37.sas -log ${reportdir}/ULWD37.LWD37R1  -mautosource
