#UTLWU12.jcl QC Duplicate ACH setups
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU12.LWU12R1
then
rm ${reportdir}/ULWU12.LWU12R1
fi
if test -a ${reportdir}/ULWU12.LWU12RZ
then
rm ${reportdir}/ULWU12.LWU12RZ
fi
if test -a ${reportdir}/ULWU12.LWU12R2
then
rm ${reportdir}/ULWU12.LWU12R2
fi

# run the program

sas ${codedir}/UTLWU12.sas -log ${reportdir}/ULWU12.LWU12R1  -mautosource
