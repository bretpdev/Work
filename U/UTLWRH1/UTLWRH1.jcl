#UTLWRH1.jcl  rehabs
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWRH1.LWRH1RZ
   then
        rm ${reportdir}/ULWRH1.LWRH1RZ
fi
if test -a ${reportdir}/ULWRH1.LWRH1R1
   then
        rm ${reportdir}/ULWRH1.LWRH1R1
fi
if test -a ${reportdir}/ULWRH1.LWRH1R2
   then
        rm ${reportdir}/ULWRH1.LWRH1R2
fi
if test -a ${reportdir}/ULWRH1.LWRH1R3
   then
        rm ${reportdir}/ULWRH1.LWRH1R3
fi
if test -a ${reportdir}/ULWRH1.LWRH1R4
   then
        rm ${reportdir}/ULWRH1.LWRH1R4
fi
if test -a ${reportdir}/ULWRH1.LWRH1R5
   then
        rm ${reportdir}/ULWRH1.LWRH1R5
fi
if test -a ${reportdir}/ULWRH1.LWRH1R6
   then
        rm ${reportdir}/ULWRH1.LWRH1R6
fi
if test -a ${reportdir}/ULWRH1.LWRH1R7
   then
        rm ${reportdir}/ULWRH1.LWRH1R7
fi
if test -a ${reportdir}/ULWRH1.LWRH1R8
   then
        rm ${reportdir}/ULWRH1.LWRH1R8
fi
if test -a ${reportdir}/ULWRH1.LWRH1R9
   then
        rm ${reportdir}/ULWRH1.LWRH1R9
fi
if test -a ${reportdir}/ULWRH1.LWRH1R10
   then
        rm ${reportdir}/ULWRH1.LWRH1R10
fi
# run the program

sas ${codedir}/UTLWRH1.sas -log ${reportdir}/ULWRH1.LWRH1R1  -mautosource

RC=$?
if [ $RC = 99 ]
 then
   echo "There were no rehabs for todays processing"
   exit 0
fi
