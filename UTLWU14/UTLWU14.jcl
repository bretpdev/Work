#UTLWU14.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU14.LWU14RZ
then
rm ${reportdir}/ULWU14.LWU14RZ
fi
if test -a ${reportdir}/ULWU14.LWU14R2
then
rm ${reportdir}/ULWU14.LWU14R2
fi

# run the program

sas ${codedir}/UTLWU14.sas -log ${reportdir}/ULWU14.LWU14R1  -mautosource
