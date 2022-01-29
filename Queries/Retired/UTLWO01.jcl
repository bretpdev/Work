#UTLWO01.jcl  OneLINK Disclosure Print Job
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO01.LWO01R1
   then
        rm ${reportdir}/ULWO01.LWO01R1
fi
if test -a ${reportdir}/ULWO01.LWO01R2
   then
        rm ${reportdir}/ULWO01.LWO01R2
fi
if test -a ${reportdir}/ULWO01.LWO01R3
   then
        rm ${reportdir}/ULWO01.LWO01R3
fi
if test -a ${reportdir}/ULWO01.LWO01R4
   then
        rm ${reportdir}/ULWO01.LWO01R4
fi
if test -a ${reportdir}/ULWO01.LWO01R5
   then
        rm ${reportdir}/ULWO01.LWO01R5
fi
if test -a ${reportdir}/ULWO01.LWO01R6
   then
        rm ${reportdir}/ULWO01.LWO01R6
fi
if test -a ${reportdir}/ULWO01.LWO01R7
   then
        rm ${reportdir}/ULWO01.LWO01R7
fi
if test -a ${reportdir}/ULWO01.LWO01R8
   then
        rm ${reportdir}/ULWO01.LWO01R8
fi
if test -a ${reportdir}/ULWO01.LWO01R9
   then
        rm ${reportdir}/ULWO01.LWO01R9
fi
if test -a ${reportdir}/ULWO01.LWO01R10
   then
        rm ${reportdir}/ULWO01.LWO01R10
fi
if test -a ${reportdir}/ULWO01.LWO01R11
   then
        rm ${reportdir}/ULWO01.LWO01R11
fi
if test -a ${reportdir}/ULWO01.LWO01R12
   then
        rm ${reportdir}/ULWO01.LWO01R12
fi
if test -a ${reportdir}/ULWO01.LWO01R13
   then
        rm ${reportdir}/ULWO01.LWO01R13
fi
if test -a ${reportdir}/ULWO01.LWO01R14
   then
        rm ${reportdir}/ULWO01.LWO01R14
fi
if test -a ${reportdir}/ULWO01.LWO01R15
   then
        rm ${reportdir}/ULWO01.LWO01R15
fi
if test -a ${reportdir}/ULWO01.LWO01R16
   then
        rm ${reportdir}/ULWO01.LWO01R16
fi
if test -a ${reportdir}/ULWO01.LWO01R17
   then
        rm ${reportdir}/ULWO01.LWO01R17
fi
if test -a ${reportdir}/ULWO01.LWO01R18
   then
        rm ${reportdir}/ULWO01.LWO01R18
fi
if test -a ${reportdir}/ULWO01.LWO01R19
   then
        rm ${reportdir}/ULWO01.LWO01R19
fi
if test -a ${reportdir}/ULWO01.LWO01R20
   then
        rm ${reportdir}/ULWO01.LWO01R20
fi
if test -a ${reportdir}/ULWO01.LWO01RZ
   then
        rm ${reportdir}/ULWO01.LWO01RZ
fi
# run the program

sas ${codedir}/UTLWO01.sas -log ${reportdir}/ULWO01.LWO01R1  -mautosource
