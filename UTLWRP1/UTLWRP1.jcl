#UTLWRP1.jcl  Varous Repurchase reports + comma-delim files
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWRP1.LWRP1R1
   then
        rm ${reportdir}/ULWRP1.LWRP1R1
fi
if test -a ${reportdir}/ULWRP1.LWRP1R2
   then
        rm ${reportdir}/ULWRP1.LWRP1R2
fi
if test -a ${reportdir}/ULWRP1.LWRP1R3
   then
        rm ${reportdir}/ULWRP1.LWRP1R3
fi
if test -a ${reportdir}/ULWRP1.LWRP1R4
   then
        rm ${reportdir}/ULWRP1.LWRP1R4
fi
if test -a ${reportdir}/ULWRP1.LWRP1R5
   then
        rm ${reportdir}/ULWRP1.LWRP1R5
fi
if test -a ${reportdir}/ULWRP1.LWRP1R6
   then
        rm ${reportdir}/ULWRP1.LWRP1R6
fi
if test -a ${reportdir}/ULWRP1.LWRP1R7
   then
        rm ${reportdir}/ULWRP1.LWRP1R7
fi
if test -a ${reportdir}/ULWRP1.LWRP1R8
   then
        rm ${reportdir}/ULWRP1.LWRP1R8
fi
if test -a ${reportdir}/ULWRP1.LWRP1R9
   then
        rm ${reportdir}/ULWRP1.LWRP1R9
fi
if test -a ${reportdir}/ULWRP1.LWRP1R10
   then
        rm ${reportdir}/ULWRP1.LWRP1R10
fi
if test -a ${reportdir}/ULWRP1.LWRP1R11
   then
        rm ${reportdir}/ULWRP1.LWRP1R11
fi
if test -a ${reportdir}/ULWRP1.LWRP1R12
   then
        rm ${reportdir}/ULWRP1.LWRP1R12
fi
if test -a ${reportdir}/ULWRP1.LWRP1R13
   then
        rm ${reportdir}/ULWRP1.LWRP1R13
fi
if test -a ${reportdir}/ULWRP1.LWRP1R14
   then
        rm ${reportdir}/ULWRP1.LWRP1R14
fi
if test -a ${reportdir}/ULWRP1.LWRP1R15
   then
        rm ${reportdir}/ULWRP1.LWRP1R15
fi
if test -a ${reportdir}/ULWRP1.LWRP1R16
   then
        rm ${reportdir}/ULWRP1.LWRP1R16
fi
if test -a ${reportdir}/ULWRP1.LWRP1R17
   then
        rm ${reportdir}/ULWRP1.LWRP1R17
fi
if test -a ${reportdir}/ULWRP1.LWRP1R18
   then
        rm ${reportdir}/ULWRP1.LWRP1R18
fi
if test -a ${reportdir}/ULWRP1.LWRP1R19
   then
        rm ${reportdir}/ULWRP1.LWRP1R19
fi
if test -a ${reportdir}/ULWRP1.LWRP1R20
   then
        rm ${reportdir}/ULWRP1.LWRP1R20
fi
if test -a ${reportdir}/ULWRP1.LWRP1R21
   then
        rm ${reportdir}/ULWRP1.LWRP1R21
fi
if test -a ${reportdir}/ULWRP1.LWRP1R22
   then
        rm ${reportdir}/ULWRP1.LWRP1R22
fi
if test -a ${reportdir}/ULWRP1.LWRP1R23
   then
        rm ${reportdir}/ULWRP1.LWRP1R23
fi
if test -a ${reportdir}/ULWRP1.LWRP1R24
   then
        rm ${reportdir}/ULWRP1.LWRP1R24
fi
if test -a ${reportdir}/ULWRP1.LWRP1R25
   then
        rm ${reportdir}/ULWRP1.LWRP1R25
fi
if test -a ${reportdir}/ULWRP1.LWRP1R26
   then
        rm ${reportdir}/ULWRP1.LWRP1R26
fi
if test -a ${reportdir}/ULWRP1.LWRP1R27
   then
        rm ${reportdir}/ULWRP1.LWRP1R27
fi
if test -a ${reportdir}/ULWRP1.LWRP1R28
   then
        rm ${reportdir}/ULWRP1.LWRP1R28
fi
if test -a ${reportdir}/ULWRP1.LWRP1R29
   then
        rm ${reportdir}/ULWRP1.LWRP1R29
fi
if test -a ${reportdir}/ULWRP1.LWRP1R30
   then
        rm ${reportdir}/ULWRP1.LWRP1R30
fi

# run the program

sas ${codedir}/UTLWRP1.sas -log ${reportdir}/ULWRP1.LWRP1R1  -mautosource