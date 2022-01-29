#UTLWG48.jcl  Stafford Certs Older than 14 Days
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG48.LWG48RZ
   then
        rm ${reportdir}/ULWG48.LWG48RZ
fi
if test -a ${reportdir}/ULWG48.LWG48R1
   then
        rm ${reportdir}/ULWG48.LWG48R1
fi
if test -a ${reportdir}/ULWG48.LWG48R2
   then
        rm ${reportdir}/ULWG48.LWG48R2
fi
if test -a ${reportdir}/ULWG48.LWG48R3
   then
        rm ${reportdir}/ULWG48.LWG48R3
fi
if test -a ${reportdir}/ULWG48.LWG48R4
   then
        rm ${reportdir}/ULWG48.LWG48R4
fi
if test -a ${reportdir}/ULWG48.LWG48R5
   then
        rm ${reportdir}/ULWG48.LWG48R5
fi
if test -a ${reportdir}/ULWG48.LWG48R6
   then
        rm ${reportdir}/ULWG48.LWG48R6
fi
if test -a ${reportdir}/ULWG48.LWG48R7
   then
        rm ${reportdir}/ULWG48.LWG48R7
fi
if test -a ${reportdir}/ULWG48.LWG48R8
   then
        rm ${reportdir}/ULWG48.LWG48R8
fi
if test -a ${reportdir}/ULWG48.LWG48R9
   then
        rm ${reportdir}/ULWG48.LWG48R9
fi
if test -a ${reportdir}/ULWG48.LWG48R10
   then
        rm ${reportdir}/ULWG48.LWG48R10
fi
if test -a ${reportdir}/ULWG48.LWG48R11
   then
        rm ${reportdir}/ULWG48.LWG48R11
fi
if test -a ${reportdir}/ULWG48.LWG48R12
   then
        rm ${reportdir}/ULWG48.LWG48R12
fi
if test -a ${reportdir}/ULWG48.LWG48R13
   then
        rm ${reportdir}/ULWG48.LWG48R13
fi
if test -a ${reportdir}/ULWG48.LWG48R14
   then
        rm ${reportdir}/ULWG48.LWG48R14
fi
if test -a ${reportdir}/ULWG48.LWG48R15
   then
        rm ${reportdir}/ULWG48.LWG48R15
fi
if test -a ${reportdir}/ULWG48.LWG48R16
   then
        rm ${reportdir}/ULWG48.LWG48R16
fi
if test -a ${reportdir}/ULWG48.LWG48R17
   then
        rm ${reportdir}/ULWG48.LWG48R17
fi
if test -a ${reportdir}/ULWG48.LWG48R18
   then
        rm ${reportdir}/ULWG48.LWG48R18
fi
if test -a ${reportdir}/ULWG48.LWG48R19
   then
        rm ${reportdir}/ULWG48.LWG48R19
fi
if test -a ${reportdir}/ULWG48.LWG48R20
   then
        rm ${reportdir}/ULWG48.LWG48R20
fi
if test -a ${reportdir}/ULWG48.LWG48R21
   then
        rm ${reportdir}/ULWG48.LWG48R21
fi
if test -a ${reportdir}/ULWG48.LWG48R22
   then
        rm ${reportdir}/ULWG48.LWG48R22
fi
if test -a ${reportdir}/ULWG48.LWG48R23
   then
        rm ${reportdir}/ULWG48.LWG48R23
fi
if test -a ${reportdir}/ULWG48.LWG48R24
   then
        rm ${reportdir}/ULWG48.LWG48R24
fi
if test -a ${reportdir}/ULWG48.LWG48R25
   then
        rm ${reportdir}/ULWG48.LWG48R25
fi
if test -a ${reportdir}/ULWG48.LWG48R26
   then
        rm ${reportdir}/ULWG48.LWG48R26
fi
if test -a ${reportdir}/ULWG48.LWG48R27
   then
        rm ${reportdir}/ULWG48.LWG48R27
fi
if test -a ${reportdir}/ULWG48.LWG48R28
   then
        rm ${reportdir}/ULWG48.LWG48R28
fi
if test -a ${reportdir}/ULWG48.LWG48R29
   then
        rm ${reportdir}/ULWG48.LWG48R29
fi
if test -a ${reportdir}/ULWG48.LWG48R30
   then
        rm ${reportdir}/ULWG48.LWG48R30
fi
if test -a ${reportdir}/ULWG48.LWG48R31
   then
        rm ${reportdir}/ULWG48.LWG48R31
fi
if test -a ${reportdir}/ULWG48.LWG48R32
   then
        rm ${reportdir}/ULWG48.LWG48R32
fi
if test -a ${reportdir}/ULWG48.LWG48R33
   then
        rm ${reportdir}/ULWG48.LWG48R33
fi
if test -a ${reportdir}/ULWG48.LWG48R34
   then
        rm ${reportdir}/ULWG48.LWG48R34
fi
if test -a ${reportdir}/ULWG48.LWG48R35
   then
        rm ${reportdir}/ULWG48.LWG48R35
fi
if test -a ${reportdir}/ULWG48.LWG48R36
   then
        rm ${reportdir}/ULWG48.LWG48R36
fi
if test -a ${reportdir}/ULWG48.LWG48R37
   then
        rm ${reportdir}/ULWG48.LWG48R37
fi
if test -a ${reportdir}/ULWG48.LWG48R38
   then
        rm ${reportdir}/ULWG48.LWG48R38
fi
if test -a ${reportdir}/ULWG48.LWG48R39
   then
        rm ${reportdir}/ULWG48.LWG48R39
fi
if test -a ${reportdir}/ULWG48.LWG48R40
   then
        rm ${reportdir}/ULWG48.LWG48R40
fi
if test -a ${reportdir}/ULWG48.LWG48R41
   then
        rm ${reportdir}/ULWG48.LWG48R41
fi
if test -a ${reportdir}/ULWG48.LWG48R42
   then
        rm ${reportdir}/ULWG48.LWG48R42
fi
if test -a ${reportdir}/ULWG48.LWG48R43
   then
        rm ${reportdir}/ULWG48.LWG48R43
fi
if test -a ${reportdir}/ULWG48.LWG48R44
   then
        rm ${reportdir}/ULWG48.LWG48R44
fi
if test -a ${reportdir}/ULWG48.LWG48R45
   then
        rm ${reportdir}/ULWG48.LWG48R45
fi
if test -a ${reportdir}/ULWG48.LWG48R46
   then
        rm ${reportdir}/ULWG48.LWG48R46
fi
if test -a ${reportdir}/ULWG48.LWG48R47
   then
        rm ${reportdir}/ULWG48.LWG48R47
fi
if test -a ${reportdir}/ULWG48.LWG48R48
   then
        rm ${reportdir}/ULWG48.LWG48R48
fi
if test -a ${reportdir}/ULWG48.LWG48R49
   then
        rm ${reportdir}/ULWG48.LWG48R49
fi
if test -a ${reportdir}/ULWG48.LWG48R50
   then
        rm ${reportdir}/ULWG48.LWG48R50
fi
if test -a ${reportdir}/ULWG48.LWG48R51
   then
        rm ${reportdir}/ULWG48.LWG48R51
fi
if test -a ${reportdir}/ULWG48.LWG48R52
   then
        rm ${reportdir}/ULWG48.LWG48R52
fi
if test -a ${reportdir}/ULWG48.LWG48R53
   then
        rm ${reportdir}/ULWG48.LWG48R53
fi
if test -a ${reportdir}/ULWG48.LWG48R54
   then
        rm ${reportdir}/ULWG48.LWG48R54
fi
if test -a ${reportdir}/ULWG48.LWG48R55
   then
        rm ${reportdir}/ULWG48.LWG48R55
fi
if test -a ${reportdir}/ULWG48.LWG48R56
   then
        rm ${reportdir}/ULWG48.LWG48R56
fi
if test -a ${reportdir}/ULWG48.LWG48R57
   then
        rm ${reportdir}/ULWG48.LWG48R57
fi
if test -a ${reportdir}/ULWG48.LWG48R58
   then
        rm ${reportdir}/ULWG48.LWG48R58
fi
if test -a ${reportdir}/ULWG48.LWG48R59
   then
        rm ${reportdir}/ULWG48.LWG48R59
fi
if test -a ${reportdir}/ULWG48.LWG48R60
   then
        rm ${reportdir}/ULWG48.LWG48R60
fi
if test -a ${reportdir}/ULWG48.LWG48R61
   then
        rm ${reportdir}/ULWG48.LWG48R61
fi
if test -a ${reportdir}/ULWG48.LWG48R62
   then
        rm ${reportdir}/ULWG48.LWG48R62
fi
if test -a ${reportdir}/ULWG48.LWG48R63
   then
        rm ${reportdir}/ULWG48.LWG48R63
fi
if test -a ${reportdir}/ULWG48.LWG48R64
   then
        rm ${reportdir}/ULWG48.LWG48R64
fi
if test -a ${reportdir}/ULWG48.LWG48R65
   then
        rm ${reportdir}/ULWG48.LWG48R65
fi
if test -a ${reportdir}/ULWG48.LWG48R66
   then
        rm ${reportdir}/ULWG48.LWG48R66
fi
if test -a ${reportdir}/ULWG48.LWG48R67
   then
        rm ${reportdir}/ULWG48.LWG48R67
fi
if test -a ${reportdir}/ULWG48.LWG48R68
   then
        rm ${reportdir}/ULWG48.LWG48R68
fi
if test -a ${reportdir}/ULWG48.LWG48R69
   then
        rm ${reportdir}/ULWG48.LWG48R69
fi
if test -a ${reportdir}/ULWG48.LWG48R70
   then
        rm ${reportdir}/ULWG48.LWG48R70
fi
if test -a ${reportdir}/ULWG48.LWG48R71
   then
        rm ${reportdir}/ULWG48.LWG48R71
fi
if test -a ${reportdir}/ULWG48.LWG48R72
   then
        rm ${reportdir}/ULWG48.LWG48R72
fi
if test -a ${reportdir}/ULWG48.LWG48R73
   then
        rm ${reportdir}/ULWG48.LWG48R73
fi

# run the program

sas ${codedir}/UTLWG48.sas -log ${reportdir}/ULWG48.LWG48R1  -mautosource
