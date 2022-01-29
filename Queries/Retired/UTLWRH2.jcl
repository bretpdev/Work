#UTLWRH2.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWRH2.LWRH2R1
   then
        rm ${reportdir}/ULWRH2.LWRH2R1
fi
if test -a ${reportdir}/ULWRH2.LWRH2R2
   then
        rm ${reportdir}/ULWRH2.LWRH2R2
fi
if test -a ${reportdir}/ULWRH2.LWRH2R3
   then
        rm ${reportdir}/ULWRH2.LWRH2R3
fi
if test -a ${reportdir}/ULWRH2.LWRH2R4
   then
        rm ${reportdir}/ULWRH2.LWRH2R4
fi
if test -a ${reportdir}/ULWRH2.LWRH2R5
   then
        rm ${reportdir}/ULWRH2.LWRH2R5
fi
# run the program

sas ${codedir}/UTLWRH2.sas -log ${reportdir}/ULWRH2.LWRH2R1  -mautosource
