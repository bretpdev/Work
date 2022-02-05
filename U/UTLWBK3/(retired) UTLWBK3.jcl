#UTLWBK3.jcl  Chapter 13 Bankruptcy - Trustee not Paying
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWBK3.LWBK3R1
   then
        rm ${reportdir}/ULWBK3.LWBK3R1
fi
if test -a ${reportdir}/ULWBK3.LWBK3R2
   then
        rm ${reportdir}/ULWBK3.LWBK3R2
fi
if test -a ${reportdir}/ULWBK3.LWBK3RZ
   then
        rm ${reportdir}/ULWBK3.LWBK3RZ
fi
# run the program

sas ${codedir}/UTLWBK3.sas -log ${reportdir}/ULWBK3.LWBK3R1  -mautosource
