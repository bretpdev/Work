#UTLWD39.jcl DEFAULT RATE
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWD39.LWD39RZ
then
rm ${reportdir}/ULWD39.LWD39RZ
fi
if test -a ${reportdir}/ULWD39.LWD39R1
then
rm ${reportdir}/ULWD39.LWD39R1
fi
if test -a ${reportdir}/ULWD39.LWD39R2
then
rm ${reportdir}/ULWD39.LWD39R2
fi

# run the program

sas ${codedir}/UTLWD39.sas -log ${reportdir}/ULWD39.LWD39R1  -mautosource
