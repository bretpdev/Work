#UTLWD01.jcl  DEFAULTED LOAN DETAIL REPORTS
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD01.LWD01R1
   then
        rm ${reportdir}/ULWD01.LWD01R1
fi
if test -a ${reportdir}/ULWD01.LWD01R2
   then
        rm ${reportdir}/ULWD01.LWD01R2
fi
if test -a ${reportdir}/ULWD01.LWD01R3
   then
        rm ${reportdir}/ULWD01.LWD01R3
fi
if test -a ${reportdir}/ULWD01.LWD01R4
   then
        rm ${reportdir}/ULWD01.LWD01R4
fi
if test -a ${reportdir}/ULWD01.LWD01R10
   then
        rm ${reportdir}/ULWD01.LWD01R10
fi
if test -a ${reportdir}/ULWD01.LWD01R11
   then
        rm ${reportdir}/ULWD01.LWD01R11
fi
if test -a ${reportdir}/ULWD01.LWD01R12
   then
        rm ${reportdir}/ULWD01.LWD01R12
fi
if test -a ${reportdir}/ULWD01.LWD01R13
   then
        rm ${reportdir}/ULWD01.LWD01R13
fi
if test -a ${reportdir}/ULWD01.LWD01R14
   then
        rm ${reportdir}/ULWD01.LWD01R14
fi
if test -a ${reportdir}/ULWD01.LWD01R15
   then
        rm ${reportdir}/ULWD01.LWD01R15
fi
if test -a ${reportdir}/ULWD01.LWD01R16
   then
        rm ${reportdir}/ULWD01.LWD01R16
fi
if test -a ${reportdir}/ULWD01.LWD01R17
   then
        rm ${reportdir}/ULWD01.LWD01R17
fi
if test -a ${reportdir}/ULWD01.LWD01R18
   then
        rm ${reportdir}/ULWD01.LWD01R18
fi
if test -a ${reportdir}/ULWD01.LWD01R19
   then
        rm ${reportdir}/ULWD01.LWD01R19
fi
if test -a ${reportdir}/ULWD01.LWD01R20
   then
        rm ${reportdir}/ULWD01.LWD01R20
fi
if test -a ${reportdir}/ULWD01.LWD01R21
   then
        rm ${reportdir}/ULWD01.LWD01R21
fi
if test -a ${reportdir}/ULWD01.LWD01R22
   then
        rm ${reportdir}/ULWD01.LWD01R22
fi
if test -a ${reportdir}/ULWD01.LWD01R23
   then
        rm ${reportdir}/ULWD01.LWD01R23
fi
if test -a ${reportdir}/ULWD01.LWD01R24
   then
        rm ${reportdir}/ULWD01.LWD01R24
fi
if test -a ${reportdir}/ULWD01.LWD01R25
   then
        rm ${reportdir}/ULWD01.LWD01R25
fi
if test -a ${reportdir}/ULWD01.LWD01R26
   then
        rm ${reportdir}/ULWD01.LWD01R26
fi
if test -a ${reportdir}/ULWD01.LWD01R27
   then
        rm ${reportdir}/ULWD01.LWD01R27
fi
if test -a ${reportdir}/ULWD01.LWD01R28
   then
        rm ${reportdir}/ULWD01.LWD01R28
fi
if test -a ${reportdir}/ULWD01.LWD01R29
   then
        rm ${reportdir}/ULWD01.LWD01R29
fi
if test -a ${reportdir}/ULWD01.LWD01R30
   then
        rm ${reportdir}/ULWD01.LWD01R30
fi
if test -a ${reportdir}/ULWD01.LWD01R31
   then
        rm ${reportdir}/ULWD01.LWD01R31
fi
if test -a ${reportdir}/ULWD01.LWD01R32
   then
        rm ${reportdir}/ULWD01.LWD01R32
fi
if test -a ${reportdir}/ULWD01.LWD01R33
   then
        rm ${reportdir}/ULWD01.LWD01R33
fi
if test -a ${reportdir}/ULWD01.LWD01R34
   then
        rm ${reportdir}/ULWD01.LWD01R34
fi
if test -a ${reportdir}/ULWD01.LWD01R35
   then
        rm ${reportdir}/ULWD01.LWD01R35
fi
if test -a ${reportdir}/ULWD01.LWD01R36
   then
        rm ${reportdir}/ULWD01.LWD01R36
fi
if test -a ${reportdir}/ULWD01.LWD01R37
   then
        rm ${reportdir}/ULWD01.LWD01R37
fi
if test -a ${reportdir}/ULWD01.LWD01R38
   then
        rm ${reportdir}/ULWD01.LWD01R38
fi
if test -a ${reportdir}/ULWD01.LWD01R39
   then
        rm ${reportdir}/ULWD01.LWD01R39
fi
if test -a ${reportdir}/ULWD01.LWD01R40
   then
        rm ${reportdir}/ULWD01.LWD01R40
fi
if test -a ${reportdir}/ULWD01.LWD01R41
   then
        rm ${reportdir}/ULWD01.LWD01R41
fi
if test -a ${reportdir}/ULWD01.LWD01R42
   then
        rm ${reportdir}/ULWD01.LWD01R42
fi
if test -a ${reportdir}/ULWD01.LWD01R43
   then
        rm ${reportdir}/ULWD01.LWD01R43
fi
if test -a ${reportdir}/ULWD01.LWD01R44
   then
        rm ${reportdir}/ULWD01.LWD01R44
fi
if test -a ${reportdir}/ULWD01.LWD01R45
   then
        rm ${reportdir}/ULWD01.LWD01R45
fi
if test -a ${reportdir}/ULWD01.LWD01R46
   then
        rm ${reportdir}/ULWD01.LWD01R46
fi
if test -a ${reportdir}/ULWD01.LWD01R47
   then
        rm ${reportdir}/ULWD01.LWD01R47
fi
if test -a ${reportdir}/ULWD01.LWD01R48
   then
        rm ${reportdir}/ULWD01.LWD01R48
fi
if test -a ${reportdir}/ULWD01.LWD01R49
   then
        rm ${reportdir}/ULWD01.LWD01R49
fi
if test -a ${reportdir}/ULWD01.LWD01R50
   then
        rm ${reportdir}/ULWD01.LWD01R50
fi
if test -a ${reportdir}/ULWD01.LWD01R51
   then
        rm ${reportdir}/ULWD01.LWD01R51
fi
if test -a ${reportdir}/ULWD01.LWD01R52
   then
        rm ${reportdir}/ULWD01.LWD01R52
fi
if test -a ${reportdir}/ULWD01.LWD01R53
   then
        rm ${reportdir}/ULWD01.LWD01R53
fi
if test -a ${reportdir}/ULWD01.LWD01R54
   then
        rm ${reportdir}/ULWD01.LWD01R54
fi
if test -a ${reportdir}/ULWD01.LWD01R55
   then
        rm ${reportdir}/ULWD01.LWD01R55
fi
if test -a ${reportdir}/ULWD01.LWD01R56
   then
        rm ${reportdir}/ULWD01.LWD01R56
fi
if test -a ${reportdir}/ULWD01.LWD01R57
   then
        rm ${reportdir}/ULWD01.LWD01R57
fi
if test -a ${reportdir}/ULWD01.LWD01R58
   then
        rm ${reportdir}/ULWD01.LWD01R58
fi
if test -a ${reportdir}/ULWD01.LWD01R59
   then
        rm ${reportdir}/ULWD01.LWD01R59
fi

# run the program

sas ${codedir}/UTLWD01.sas -log ${reportdir}/ULWD01.LWD01R1  -mautosource
