#UTLWD29.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWD29.LWD29R1
then
rm ${reportdir}/ULWD29.LWD29R1
fi
if test -a ${reportdir}/ULWD29.LWD29R2
then
rm ${reportdir}/ULWD29.LWD29R2
fi
if test -a ${reportdir}/ULWD29.LWD29RZ
then
rm ${reportdir}/ULWD29.LWD29RZ
fi

# run the program

sas ${codedir}/UTLWD29.sas -log ${reportdir}/ULWD29.LWD29R1  -mautosource
