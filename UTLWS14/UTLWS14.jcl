#UTLWS14.jcl - Billing Statements
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS14.LWS14R1
   then
        rm ${reportdir}/ULWS14.LWS14R1
fi
if test -a /sas/whse/progrevw/ULWS14.LWS14R2
   then
        rm /sas/whse/progrevw/ULWS14.LWS14R2
fi
if test -a /sas/whse/progrevw/ULWS14.LWS14R4
   then
        rm /sas/whse/progrevw/ULWS14.LWS14R4
fi
if test -a ${reportdir}/ULWS14.LWS14R6
   then
        rm ${reportdir}/ULWS14.LWS14R6
fi
if test -a ${reportdir}/ULWS14.LWS14R7
   then
        rm ${reportdir}/ULWS14.LWS14R7
fi
if test -a ${reportdir}/ULWS14.LWS14R8
   then
        rm ${reportdir}/ULWS14.LWS14R8
fi
if test -a ${reportdir}/ULWS14.LWS14R10
   then
        rm ${reportdir}/ULWS14.LWS14R10
fi
if test -a ${reportdir}/ULWS14.LWS14R12
   then
        rm ${reportdir}/ULWS14.LWS14R12
fi
if test -a ${reportdir}/ULWS14.LWS14R13
   then
        rm ${reportdir}/ULWS14.LWS14R13
fi
if test -a ${reportdir}/ULWS14.LWS14R15
   then
        rm ${reportdir}/ULWS14.LWS14R15
fi
if test -a ${reportdir}/ULWS14.LWS14R16
   then
        rm ${reportdir}/ULWS14.LWS14R16
fi
if test -a ${reportdir}/ULWS14.LWS14R17
   then
        rm ${reportdir}/ULWS14.LWS14R17
fi
if test -a ${reportdir}/ULWS14.LWS14R18
   then
        rm ${reportdir}/ULWS14.LWS14R18
fi
if test -a ${reportdir}/ULWS14.LWS14R19
   then
        rm ${reportdir}/ULWS14.LWS14R19
fi
if test -a ${reportdir}/ULWS14.LWS14R20
   then
        rm ${reportdir}/ULWS14.LWS14R20
fi
if test -a ${reportdir}/ULWS14.LWS14R21
   then
        rm ${reportdir}/ULWS14.LWS14R21
fi
if test -a ${reportdir}/ULWS14.LWS14R27
   then
        rm ${reportdir}/ULWS14.LWS14R27
fi
if test -a ${reportdir}/ULWS14.LWS14R29
   then
        rm ${reportdir}/ULWS14.LWS14R29
fi
if test -a ${reportdir}/ULWS14.LWS14R31
   then
        rm ${reportdir}/ULWS14.LWS14R31
fi
if test -a ${reportdir}/ULWS14.LWS14R32
   then
        rm ${reportdir}/ULWS14.LWS14R32
fi
if test -a ${reportdir}/ULWS14.LWS14R33
   then
        rm ${reportdir}/ULWS14.LWS14R33
fi
if test -a ${reportdir}/ULWS14.LWS14R34
   then
        rm ${reportdir}/ULWS14.LWS14R34
fi
if test -a ${reportdir}/ULWS14.LWS14R35
   then
        rm ${reportdir}/ULWS14.LWS14R35
fi
if test -a ${reportdir}/ULWS14.LWS14R36
   then
        rm ${reportdir}/ULWS14.LWS14R36
fi
if test -a ${reportdir}/ULWS14.LWS14R37
   then
        rm ${reportdir}/ULWS14.LWS14R37
fi
if test -a ${reportdir}/ULWS14.LWS14R38
   then
        rm ${reportdir}/ULWS14.LWS14R38
fi
if test -a ${reportdir}/ULWS14.LWS14R39
   then
        rm ${reportdir}/ULWS14.LWS14R39
fi
if test -a ${reportdir}/ULWS14.LWS14R99
   then
        rm ${reportdir}/ULWS14.LWS14R99
fi

# run the program

sas ${codedir}/UTLWS14.sas -log ${reportdir}/ULWS14.LWS14R1  -mautosource
