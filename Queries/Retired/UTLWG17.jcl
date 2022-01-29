#UTLWG17.jcl  EOM Lender Total Anticipated Disbursement Reports by School
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG17.LWG17R1
   then
        rm ${reportdir}/ULWG17.LWG17R1
fi
if test -a ${reportdir}/ULWG17.LWG17R2
   then
        rm ${reportdir}/ULWG17.LWG17R2
fi
if test -a ${reportdir}/ULWG17.LWG17R3
   then
        rm ${reportdir}/ULWG17.LWG17R3
fi
if test -a ${reportdir}/ULWG17.LWG17R4
   then
        rm ${reportdir}/ULWG17.LWG17R4
fi
if test -a ${reportdir}/ULWG17.LWG17R5
   then
        rm ${reportdir}/ULWG17.LWG17R5
fi
if test -a ${reportdir}/ULWG17.LWG17R6
   then
        rm ${reportdir}/ULWG17.LWG17R6
fi
if test -a ${reportdir}/ULWG17.LWG17R7
   then
        rm ${reportdir}/ULWG17.LWG17R7
fi
if test -a ${reportdir}/ULWG17.LWG17R8
   then
        rm ${reportdir}/ULWG17.LWG17R8
fi
if test -a ${reportdir}/ULWG17.LWG17R9
   then
        rm ${reportdir}/ULWG17.LWG17R9
fi
if test -a ${reportdir}/ULWG17.LWG17R10
   then
        rm ${reportdir}/ULWG17.LWG17R10
fi
if test -a ${reportdir}/ULWG17.LWG17R11
   then
        rm ${reportdir}/ULWG17.LWG17R11
fi
if test -a ${reportdir}/ULWG17.LWG17R12
   then
        rm ${reportdir}/ULWG17.LWG17R12
fi
if test -a ${reportdir}/ULWG17.LWG17R13
   then
        rm ${reportdir}/ULWG17.LWG17R13
fi
if test -a ${reportdir}/ULWG17.LWG17R14
   then
        rm ${reportdir}/ULWG17.LWG17R14
fi
if test -a ${reportdir}/ULWG17.LWG17R15
   then
        rm ${reportdir}/ULWG17.LWG17R15
fi
if test -a ${reportdir}/ULWG17.LWG17R16
   then
        rm ${reportdir}/ULWG17.LWG17R16
fi
if test -a ${reportdir}/ULWG17.LWG17R17
   then
        rm ${reportdir}/ULWG17.LWG17R17
fi
if test -a ${reportdir}/ULWG17.LWG17R18
   then
        rm ${reportdir}/ULWG17.LWG17R18
fi
if test -a ${reportdir}/ULWG17.LWG17R19
   then
        rm ${reportdir}/ULWG17.LWG17R19
fi
if test -a ${reportdir}/ULWG17.LWG17R20
   then
        rm ${reportdir}/ULWG17.LWG17R20
fi
if test -a ${reportdir}/ULWG17.LWG17R21
   then
        rm ${reportdir}/ULWG17.LWG17R21
fi
if test -a ${reportdir}/ULWG17.LWG17R22
   then
        rm ${reportdir}/ULWG17.LWG17R22
fi
if test -a ${reportdir}/ULWG17.LWG17R23
   then
        rm ${reportdir}/ULWG17.LWG17R23
fi
if test -a ${reportdir}/ULWG17.LWG17R24
   then
        rm ${reportdir}/ULWG17.LWG17R24
fi
if test -a ${reportdir}/ULWG17.LWG17R25
   then
        rm ${reportdir}/ULWG17.LWG17R25
fi
if test -a ${reportdir}/ULWG17.LWG17R90
   then
        rm ${reportdir}/ULWG17.LWG17R90
fi
if test -a ${reportdir}/ULWG17.LWG17R91
   then
        rm ${reportdir}/ULWG17.LWG17R91
fi
if test -a ${reportdir}/ULWG17.LWG17R96
   then
        rm ${reportdir}/ULWG17.LWG17R96
fi
if test -a ${reportdir}/ULWG17.LWG17R97
   then
        rm ${reportdir}/ULWG17.LWG17R97
fi
if test -a ${reportdir}/ULWG17.LWG17R98
   then
        rm ${reportdir}/ULWG17.LWG17R98
fi
if test -a ${reportdir}/ULWG17.LWG17R99
   then
        rm ${reportdir}/ULWG17.LWG17R99
fi

# run the program

sas ${codedir}/UTLWG17.sas -log ${reportdir}/ULWG17.LWG17R1  -mautosource
