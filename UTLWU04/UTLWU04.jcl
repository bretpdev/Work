#UTLWU04.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU04.LWU04R1
then
rm ${reportdir}/ULWU04.LWU04R1
fi
if test -a ${reportdir}/ULWU04.LWU04R2
then
rm ${reportdir}/ULWU04.LWU04R2
fi
if test -a ${reportdir}/ULWU04.LWU04RZ
then
rm ${reportdir}/ULWU04.LWU04RZ
fi

# run the program

sas ${codedir}/UTLWU04.sas -log ${reportdir}/ULWU04.LWU04R1  -mautosource
