#UTLWU08.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU08.LWU08R1
then
rm ${reportdir}/ULWU08.LWU08R1
fi
if test -a ${reportdir}/ULWU08.LWU08R2
then
rm ${reportdir}/ULWU08.LWU08R2
fi
if test -a ${reportdir}/ULWU08.LWU08R3
then
rm ${reportdir}/ULWU08.LWU08R3
fi
if test -a ${reportdir}/ULWU08.LWU08RZ
then
rm ${reportdir}/ULWU08.LWU08RZ
fi

# run the program

sas ${codedir}/UTLWU08.sas -log ${reportdir}/ULWU08.LWU08R1  -mautosource
