#UTLWU27.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU27.LWU27R1
then
rm ${reportdir}/ULWU27.LWU27R1
fi
if test -a ${reportdir}/ULWU27.LWU27R2
then
rm ${reportdir}/ULWU27.LWU27R2
fi
if test -a ${reportdir}/ULWU27.LWU27RZ
then
rm ${reportdir}/ULWU27.LWU27RZ
fi

# run the program

sas ${codedir}/UTLWU27.sas -log ${reportdir}/ULWU27.LWU27R1  -mautosource
