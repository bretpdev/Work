#UTLWGG2.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWGG2.LWGG2R1
   then
        rm ${reportdir}/ULWGG2.LWGG2R1
fi
if test -a ${reportdir}/ULWGG2.LWGG2R2
   then
        rm ${reportdir}/ULWGG2.LWGG2R2
fi

# run the program

sas ${codedir}/UTLWGG2.sas -log ${reportdir}/ULWGG2.LWGG2R1  -mautosource
