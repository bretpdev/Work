#UTLWBK1.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWBK1.LWBK1R1
   then
        rm ${reportdir}/ULWBK1.LWBK1R1
fi
if test -a ${reportdir}/ULWBK1.LWBK1R2
   then
        rm ${reportdir}/ULWBK1.LWBK1R2
fi

# run the program

sas ${codedir}/UTLWBK1.sas -log ${reportdir}/ULWBK1.LWBK1R1  -mautosource
