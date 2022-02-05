#UTLWU09.jcl Unauthorized Institution Record Updates QC
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU09.LWU09R1
then
rm ${reportdir}/ULWU09.LWU09R1
fi
if test -a ${reportdir}/ULWU09.LWU09R2
then
rm ${reportdir}/ULWU09.LWU09R2
fi
if test -a ${reportdir}/ULWU09.LWU09RZ
then
rm ${reportdir}/ULWU09.LWU09RZ
fi

# run the program

sas ${codedir}/UTLWU09.sas -log ${reportdir}/ULWU09.LWU09R1  -mautosource
