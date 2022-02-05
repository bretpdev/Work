#UTLWU29.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU29.LWU29R1
then
rm ${reportdir}/ULWU29.LWU29R1
fi
if test -a ${reportdir}/ULWU29.LWU29R2
then
rm ${reportdir}/ULWU29.LWU29R2
fi
if test -a ${reportdir}/ULWU29.LWU29R3
then
rm ${reportdir}/ULWU29.LWU29R3
fi
if test -a ${reportdir}/ULWU29.LWU29R4
then
rm ${reportdir}/ULWU29.LWU29R4
fi
if test -a ${reportdir}/ULWU29.LWU29RZ
then
rm ${reportdir}/ULWU29.LWU29RZ
fi

# run the program

sas ${codedir}/UTLWU29.sas -log ${reportdir}/ULWU29.LWU29R1  -mautosource
