#UTLWU13.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU13.LWU13RZ
then
rm ${reportdir}/ULWU13.LWU13RZ
fi
if test -a ${reportdir}/ULWU13.LWU13R1
then
rm ${reportdir}/ULWU13.LWU13R1
fi
if test -a ${reportdir}/ULWU13.LWU13R2
then
rm ${reportdir}/ULWU13.LWU13R2
fi

# run the program

sas ${codedir}/UTLWU13.sas -log ${reportdir}/ULWU13.LWU13R1  -mautosource
