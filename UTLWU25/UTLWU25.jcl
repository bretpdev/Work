#UTLWU25.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU25.LWU25R1
then
rm ${reportdir}/ULWU25.LWU25R1
fi
if test -a ${reportdir}/ULWU25.LWU25R2
then
rm ${reportdir}/ULWU25.LWU25R2
fi
if test -a ${reportdir}/ULWU25.LWU25R3
then
rm ${reportdir}/ULWU25.LWU25R3
fi
if test -a ${reportdir}/ULWU25.LWU25R4
then
rm ${reportdir}/ULWU25.LWU25R4
fi
if test -a ${reportdir}/ULWU25.LWU25RZ
then
rm ${reportdir}/ULWU25.LWU25RZ
fi

# run the program

sas ${codedir}/UTLWU25.sas -log ${reportdir}/ULWU25.LWU25R1  -mautosource
