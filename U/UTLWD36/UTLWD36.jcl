#UTLWD36.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWD36.LWD36R1
then
rm ${reportdir}/ULWD36.LWD36R1
fi
if test -a ${reportdir}/ULWD36.LWD36R2
then
rm ${reportdir}/ULWD36.LWD36R2
fi
if test -a ${reportdir}/ULWD36.LWD36RZ
then
rm ${reportdir}/ULWD36.LWD36RZ
fi

# run the program

sas ${codedir}/UTLWD36.sas -log ${reportdir}/ULWD36.LWD36R1  -mautosource
