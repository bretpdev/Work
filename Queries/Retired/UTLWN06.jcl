#UTLWN06.jcl MONTH-END DATA FILE FOR AMERICA FIRST CREDIT UNION
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWN06.LWN06R1
then
rm ${reportdir}/ULWN06.LWN06R1
fi
if test -a ${reportdir}/ULWN06.LWN06R2
then
rm ${reportdir}/ULWN06.LWN06R2
fi

# run the program

sas ${codedir}/UTLWN06.sas -log ${reportdir}/ULWN06.LWN06R1  -mautosource
