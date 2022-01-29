#UTLWU22.jcl Ref Add KLOANAPP QC
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU22.LWU22R1
then
rm ${reportdir}/ULWU22.LWU22R1
fi
if test -a ${reportdir}/ULWU22.LWU22R2
then
rm ${reportdir}/ULWU22.LWU22R2
fi
if test -a ${reportdir}/ULWU22.LWU22RZ
then
rm ${reportdir}/ULWU22.LWU22RZ
fi

# run the program

sas ${codedir}/UTLWU22.sas -log ${reportdir}/ULWU22.LWU22R1  -mautosource
