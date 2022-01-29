#UTLWO23.jcl  quarterly origination fee calculation
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO23.LWO23RZ
   then
        rm ${reportdir}/ULWO23.LWO23RZ
fi
if test -a ${reportdir}/ULWO23.LWO23R1
   then
        rm ${reportdir}/ULWO23.LWO23R1
fi
if test -a ${reportdir}/ULWO23.LWO23R2
   then
        rm ${reportdir}/ULWO23.LWO23R2
fi
if test -a ${reportdir}/ULWO23.LWO23R3
   then
        rm ${reportdir}/ULWO23.LWO23R3
fi
if test -a ${reportdir}/ULWO23.LWO23R5
   then
        rm ${reportdir}/ULWO23.LWO23R5
fi
if test -a ${reportdir}/ULWO23.LWO23R6
   then
        rm ${reportdir}/ULWO23.LWO23R6
fi
if test -a ${reportdir}/ULWO23.LWO23R7
   then
        rm ${reportdir}/ULWO23.LWO23R7
fi
if test -a ${reportdir}/ULWO23.LWO23R9
   then
        rm ${reportdir}/ULWO23.LWO23R9
fi
if test -a ${reportdir}/ULWO23.LWO23R10
   then
        rm ${reportdir}/ULWO23.LWO23R10
fi
if test -a ${reportdir}/ULWO23.LWO23R11
   then
        rm ${reportdir}/ULWO23.LWO23R11
fi
if test -a ${reportdir}/ULWO23.LWO23R12
   then
        rm ${reportdir}/ULWO23.LWO23R12
fi
if test -a ${reportdir}/ULWO23.LWO23R13
   then
        rm ${reportdir}/ULWO23.LWO23R13
fi
if test -a ${reportdir}/ULWO23.LWO23R14
   then
        rm ${reportdir}/ULWO23.LWO23R14
fi
if test -a ${reportdir}/ULWO23.LWO23R15
   then
        rm ${reportdir}/ULWO23.LWO23R15
fi
if test -a ${reportdir}/ULWO23.LWO23R16
   then
        rm ${reportdir}/ULWO23.LWO23R16
fi
if test -a ${reportdir}/ULWO23.LWO23R17
   then
        rm ${reportdir}/ULWO23.LWO23R17
fi
if test -a ${reportdir}/ULWO23.LWO23R18
   then
        rm ${reportdir}/ULWO23.LWO23R18
fi

# run the program

sas ${codedir}/UTLWO23.sas -log ${reportdir}/ULWO23.LWO23R1  -mautosource
