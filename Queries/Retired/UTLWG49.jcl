#UTLWG49.jcl  PLUS Certs Older than 14 Days
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG49.LWG49R1
   then
        rm ${reportdir}/ULWG49.LWG49R1
fi
if test -a ${reportdir}/ULWG49.LWG49R2
   then
        rm ${reportdir}/ULWG49.LWG49R2
fi
if test -a ${reportdir}/ULWG49.LWG49R3
   then
        rm ${reportdir}/ULWG49.LWG49R3
fi
if test -a ${reportdir}/ULWG49.LWG49R4
   then
        rm ${reportdir}/ULWG49.LWG49R4
fi
if test -a ${reportdir}/ULWG49.LWG49R5
   then
        rm ${reportdir}/ULWG49.LWG49R5
fi
if test -a ${reportdir}/ULWG49.LWG49R6
   then
        rm ${reportdir}/ULWG49.LWG49R6
fi
if test -a ${reportdir}/ULWG49.LWG49R7
   then
        rm ${reportdir}/ULWG49.LWG49R7
fi
if test -a ${reportdir}/ULWG49.LWG49R8
   then
        rm ${reportdir}/ULWG49.LWG49R8
fi
if test -a ${reportdir}/ULWG49.LWG49R9
   then
        rm ${reportdir}/ULWG49.LWG49R9
fi
if test -a ${reportdir}/ULWG49.LWG49R10
   then
        rm ${reportdir}/ULWG49.LWG49R10
fi
if test -a ${reportdir}/ULWG49.LWG49R11
   then
        rm ${reportdir}/ULWG49.LWG49R11
fi
if test -a ${reportdir}/ULWG49.LWG49R12
   then
        rm ${reportdir}/ULWG49.LWG49R12
fi
if test -a ${reportdir}/ULWG49.LWG49R13
   then
        rm ${reportdir}/ULWG49.LWG49R13
fi
if test -a ${reportdir}/ULWG49.LWG49R14
   then
        rm ${reportdir}/ULWG49.LWG49R14
fi
if test -a ${reportdir}/ULWG49.LWG49R15
   then
        rm ${reportdir}/ULWG49.LWG49R15
fi
if test -a ${reportdir}/ULWG49.LWG49R16
   then
        rm ${reportdir}/ULWG49.LWG49R16
fi
if test -a ${reportdir}/ULWG49.LWG49R17
   then
        rm ${reportdir}/ULWG49.LWG49R17
fi
if test -a ${reportdir}/ULWG49.LWG49R18
   then
        rm ${reportdir}/ULWG49.LWG49R18
fi
if test -a ${reportdir}/ULWG49.LWG49R19
   then
        rm ${reportdir}/ULWG49.LWG49R19
fi
if test -a ${reportdir}/ULWG49.LWG49R20
   then
        rm ${reportdir}/ULWG49.LWG49R20
fi
if test -a ${reportdir}/ULWG49.LWG49R21
   then
        rm ${reportdir}/ULWG49.LWG49R21
fi
if test -a ${reportdir}/ULWG49.LWG49R22
   then
        rm ${reportdir}/ULWG49.LWG49R22
fi
if test -a ${reportdir}/ULWG49.LWG49R23
   then
        rm ${reportdir}/ULWG49.LWG49R23
fi
if test -a ${reportdir}/ULWG49.LWG49R24
   then
        rm ${reportdir}/ULWG49.LWG49R24
fi
if test -a ${reportdir}/ULWG49.LWG49R25
   then
        rm ${reportdir}/ULWG49.LWG49R25
fi
if test -a ${reportdir}/ULWG49.LWG49R26
   then
        rm ${reportdir}/ULWG49.LWG49R26
fi
if test -a ${reportdir}/ULWG49.LWG49R27
   then
        rm ${reportdir}/ULWG49.LWG49R27
fi
if test -a ${reportdir}/ULWG49.LWG49R28
   then
        rm ${reportdir}/ULWG49.LWG49R28
fi
if test -a ${reportdir}/ULWG49.LWG49R29
   then
        rm ${reportdir}/ULWG49.LWG49R29
fi
if test -a ${reportdir}/ULWG49.LWG49R30
   then
        rm ${reportdir}/ULWG49.LWG49R30
fi
if test -a ${reportdir}/ULWG49.LWG49R31
   then
        rm ${reportdir}/ULWG49.LWG49R31
fi
if test -a ${reportdir}/ULWG49.LWG49R32
   then
        rm ${reportdir}/ULWG49.LWG49R32
fi
if test -a ${reportdir}/ULWG49.LWG49R33
   then
        rm ${reportdir}/ULWG49.LWG49R33
fi
if test -a ${reportdir}/ULWG49.LWG49R34
   then
        rm ${reportdir}/ULWG49.LWG49R34
fi
if test -a ${reportdir}/ULWG49.LWG49R35
   then
        rm ${reportdir}/ULWG49.LWG49R35
fi
if test -a ${reportdir}/ULWG49.LWG49R36
   then
        rm ${reportdir}/ULWG49.LWG49R36
fi
if test -a ${reportdir}/ULWG49.LWG49R37
   then
        rm ${reportdir}/ULWG49.LWG49R37
fi
if test -a ${reportdir}/ULWG49.LWG49R38
   then
        rm ${reportdir}/ULWG49.LWG49R38
fi
if test -a ${reportdir}/ULWG49.LWG49R39
   then
        rm ${reportdir}/ULWG49.LWG49R39
fi
if test -a ${reportdir}/ULWG49.LWG49R40
   then
        rm ${reportdir}/ULWG49.LWG49R40
fi
if test -a ${reportdir}/ULWG49.LWG49R41
   then
        rm ${reportdir}/ULWG49.LWG49R41
fi
if test -a ${reportdir}/ULWG49.LWG49R42
   then
        rm ${reportdir}/ULWG49.LWG49R42
fi
if test -a ${reportdir}/ULWG49.LWG49R43
   then
        rm ${reportdir}/ULWG49.LWG49R43
fi
if test -a ${reportdir}/ULWG49.LWG49R44
   then
        rm ${reportdir}/ULWG49.LWG49R44
fi
if test -a ${reportdir}/ULWG49.LWG49R45
   then
        rm ${reportdir}/ULWG49.LWG49R45
fi
if test -a ${reportdir}/ULWG49.LWG49R46
   then
        rm ${reportdir}/ULWG49.LWG49R46
fi
if test -a ${reportdir}/ULWG49.LWG49R47
   then
        rm ${reportdir}/ULWG49.LWG49R47
fi
if test -a ${reportdir}/ULWG49.LWG49R48
   then
        rm ${reportdir}/ULWG49.LWG49R48
fi
if test -a ${reportdir}/ULWG49.LWG49R49
   then
        rm ${reportdir}/ULWG49.LWG49R49
fi
if test -a ${reportdir}/ULWG49.LWG49R50
   then
        rm ${reportdir}/ULWG49.LWG49R50
fi
if test -a ${reportdir}/ULWG49.LWG49R51
   then
        rm ${reportdir}/ULWG49.LWG49R51
fi
if test -a ${reportdir}/ULWG49.LWG49R52
   then
        rm ${reportdir}/ULWG49.LWG49R52
fi
if test -a ${reportdir}/ULWG49.LWG49R53
   then
        rm ${reportdir}/ULWG49.LWG49R53
fi
if test -a ${reportdir}/ULWG49.LWG49R54
   then
        rm ${reportdir}/ULWG49.LWG49R54
fi
if test -a ${reportdir}/ULWG49.LWG49R55
   then
        rm ${reportdir}/ULWG49.LWG49R55
fi
if test -a ${reportdir}/ULWG49.LWG49R56
   then
        rm ${reportdir}/ULWG49.LWG49R56
fi
if test -a ${reportdir}/ULWG49.LWG49R57
   then
        rm ${reportdir}/ULWG49.LWG49R57
fi
if test -a ${reportdir}/ULWG49.LWG49R58
   then
        rm ${reportdir}/ULWG49.LWG49R58
fi
if test -a ${reportdir}/ULWG49.LWG49R59
   then
        rm ${reportdir}/ULWG49.LWG49R59
fi
if test -a ${reportdir}/ULWG49.LWG49R60
   then
        rm ${reportdir}/ULWG49.LWG49R60
fi
if test -a ${reportdir}/ULWG49.LWG49R61
   then
        rm ${reportdir}/ULWG49.LWG49R61
fi
if test -a ${reportdir}/ULWG49.LWG49R62
   then
        rm ${reportdir}/ULWG49.LWG49R62
fi
if test -a ${reportdir}/ULWG49.LWG49R63
   then
        rm ${reportdir}/ULWG49.LWG49R63
fi
if test -a ${reportdir}/ULWG49.LWG49R64
   then
        rm ${reportdir}/ULWG49.LWG49R64
fi
if test -a ${reportdir}/ULWG49.LWG49R65
   then
        rm ${reportdir}/ULWG49.LWG49R65
fi
if test -a ${reportdir}/ULWG49.LWG49R66
   then
        rm ${reportdir}/ULWG49.LWG49R66
fi
if test -a ${reportdir}/ULWG49.LWG49R67
   then
        rm ${reportdir}/ULWG49.LWG49R67
fi
if test -a ${reportdir}/ULWG49.LWG49R68
   then
        rm ${reportdir}/ULWG49.LWG49R68
fi
if test -a ${reportdir}/ULWG49.LWG49R69
   then
        rm ${reportdir}/ULWG49.LWG49R69
fi
if test -a ${reportdir}/ULWG49.LWG49R70
   then
        rm ${reportdir}/ULWG49.LWG49R70
fi
if test -a ${reportdir}/ULWG49.LWG49R71
   then
        rm ${reportdir}/ULWG49.LWG49R71
fi
if test -a ${reportdir}/ULWG49.LWG49R72
   then
        rm ${reportdir}/ULWG49.LWG49R72
fi
if test -a ${reportdir}/ULWG49.LWG49R73
   then
        rm ${reportdir}/ULWG49.LWG49R73
fi
# run the program

sas ${codedir}/UTLWG49.sas -log ${reportdir}/ULWG49.LWG49R1  -mautosource
