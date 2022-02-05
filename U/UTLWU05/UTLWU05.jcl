#UTLWU05.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU05.LWU05RZ
then
rm ${reportdir}/ULWU05.LWU05RZ
fi
if test -a ${reportdir}/ULWU05.LWU05R1
then
rm ${reportdir}/ULWU05.LWU05R1
fi
if test -a ${reportdir}/ULWU05.LWU05R2
then
rm ${reportdir}/ULWU05.LWU05R2
fi

# run the program

sas ${codedir}/UTLWU05.sas -log ${reportdir}/ULWU05.LWU05R1  -mautosource
