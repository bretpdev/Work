#UTLWG27.jcl  PLUS Preapproval by School
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG27.LWG27R1
   then
        rm ${reportdir}/ULWG27.LWG27R1
fi
if test -a ${reportdir}/ULWG27.LWG27R2
   then
        rm ${reportdir}/ULWG27.LWG27R2
fi
if test -a ${reportdir}/ULWG27.LWG27R3
   then
        rm ${reportdir}/ULWG27.LWG27R3
fi
if test -a ${reportdir}/ULWG27.LWG27R4
   then
        rm ${reportdir}/ULWG27.LWG27R4
fi
if test -a ${reportdir}/ULWG27.LWG27R5
   then
        rm ${reportdir}/ULWG27.LWG27R5
fi
if test -a ${reportdir}/ULWG27.LWG27R6
   then
        rm ${reportdir}/ULWG27.LWG27R6
fi
if test -a ${reportdir}/ULWG27.LWG27R7
   then
        rm ${reportdir}/ULWG27.LWG27R7
fi
if test -a ${reportdir}/ULWG27.LWG27R8
   then
        rm ${reportdir}/ULWG27.LWG27R8
fi
if test -a ${reportdir}/ULWG27.LWG27R9
   then
        rm ${reportdir}/ULWG27.LWG27R9
fi
if test -a ${reportdir}/ULWG27.LWG27R10
   then
        rm ${reportdir}/ULWG27.LWG27R10
fi
if test -a ${reportdir}/ULWG27.LWG27R11
   then
        rm ${reportdir}/ULWG27.LWG27R11
fi
if test -a ${reportdir}/ULWG27.LWG27R12
   then
        rm ${reportdir}/ULWG27.LWG27R12
fi
if test -a ${reportdir}/ULWG27.LWG27R13
   then
        rm ${reportdir}/ULWG27.LWG27R13
fi
if test -a ${reportdir}/ULWG27.LWG27R14
   then
        rm ${reportdir}/ULWG27.LWG27R14
fi
if test -a ${reportdir}/ULWG27.LWG27R15
   then
        rm ${reportdir}/ULWG27.LWG27R15
fi
if test -a ${reportdir}/ULWG27.LWG27R16
   then
        rm ${reportdir}/ULWG27.LWG27R16
fi
if test -a ${reportdir}/ULWG27.LWG27R17
   then
        rm ${reportdir}/ULWG27.LWG27R17
fi
if test -a ${reportdir}/ULWG27.LWG27R18
   then
        rm ${reportdir}/ULWG27.LWG27R18
fi
if test -a ${reportdir}/ULWG27.LWG27R19
   then
        rm ${reportdir}/ULWG27.LWG27R19
fi
if test -a ${reportdir}/ULWG27.LWG27R20
   then
        rm ${reportdir}/ULWG27.LWG27R20
fi
if test -a ${reportdir}/ULWG27.LWG27R21
   then
        rm ${reportdir}/ULWG27.LWG27R21
fi
if test -a ${reportdir}/ULWG27.LWG27R22
   then
        rm ${reportdir}/ULWG27.LWG27R22
fi
if test -a ${reportdir}/ULWG27.LWG27R23
   then
        rm ${reportdir}/ULWG27.LWG27R23
fi
if test -a ${reportdir}/ULWG27.LWG27R24
   then
        rm ${reportdir}/ULWG27.LWG27R24
fi
if test -a ${reportdir}/ULWG27.LWG27R25
   then
        rm ${reportdir}/ULWG27.LWG27R25
fi
if test -a ${reportdir}/ULWG27.LWG27R26
   then
        rm ${reportdir}/ULWG27.LWG27R26
fi
if test -a ${reportdir}/ULWG27.LWG27R27
   then
        rm ${reportdir}/ULWG27.LWG27R27
fi
if test -a ${reportdir}/ULWG27.LWG27R28
   then
        rm ${reportdir}/ULWG27.LWG27R28
fi
if test -a ${reportdir}/ULWG27.LWG27R29
   then
        rm ${reportdir}/ULWG27.LWG27R29
		fi
if test -a ${reportdir}/ULWG27.LWG27R30
   then
        rm ${reportdir}/ULWG27.LWG27R30
fi
if test -a ${reportdir}/ULWG27.LWG27R31
   then
        rm ${reportdir}/ULWG27.LWG27R31
fi
if test -a ${reportdir}/ULWG27.LWG27R32
   then
        rm ${reportdir}/ULWG27.LWG27R32
fi
if test -a ${reportdir}/ULWG27.LWG27R33
   then
        rm ${reportdir}/ULWG27.LWG27R33
fi
if test -a ${reportdir}/ULWG27.LWG27R35
   then
        rm ${reportdir}/ULWG27.LWG27R35
fi
if test -a ${reportdir}/ULWG27.LWG27R37
   then
        rm ${reportdir}/ULWG27.LWG27R37
fi
if test -a ${reportdir}/ULWG27.LWG27R39
   then
        rm ${reportdir}/ULWG27.LWG27R39
fi
if test -a ${reportdir}/ULWG27.LWG27R41
   then
        rm ${reportdir}/ULWG27.LWG27R41
fi
if test -a ${reportdir}/ULWG27.LWG27R43
   then
        rm ${reportdir}/ULWG27.LWG27R43
fi
if test -a ${reportdir}/ULWG27.LWG27R45
   then
        rm ${reportdir}/ULWG27.LWG27R45
fi
if test -a ${reportdir}/ULWG27.LWG27R47
   then
        rm ${reportdir}/ULWG27.LWG27R47
fi
if test -a ${reportdir}/ULWG27.LWG27R49
   then
        rm ${reportdir}/ULWG27.LWG27R49
fi
if test -a ${reportdir}/ULWG27.LWG27R51
   then
        rm ${reportdir}/ULWG27.LWG27R51
fi
if test -a ${reportdir}/ULWG27.LWG27R52
   then
        rm ${reportdir}/ULWG27.LWG27R52
fi
if test -a ${reportdir}/ULWG27.LWG27R53
   then
        rm ${reportdir}/ULWG27.LWG27R53
fi
if test -a ${reportdir}/ULWG27.LWG27R55
   then
        rm ${reportdir}/ULWG27.LWG27R55
fi
if test -a ${reportdir}/ULWG27.LWG27R57
   then
        rm ${reportdir}/ULWG27.LWG27R57
fi
if test -a ${reportdir}/ULWG27.LWG27R59
   then
        rm ${reportdir}/ULWG27.LWG27R59
fi
if test -a ${reportdir}/ULWG27.LWG27R61
   then
        rm ${reportdir}/ULWG27.LWG27R61
fi
if test -a ${reportdir}/ULWG27.LWG27R63
   then
        rm ${reportdir}/ULWG27.LWG27R63
fi
if test -a ${reportdir}/ULWG27.LWG27R65
   then
        rm ${reportdir}/ULWG27.LWG27R65
fi
if test -a ${reportdir}/ULWG27.LWG27R67
   then
        rm ${reportdir}/ULWG27.LWG27R67
fi
if test -a ${reportdir}/ULWG27.LWG27R69
   then
        rm ${reportdir}/ULWG27.LWG27R69
fi
if test -a ${reportdir}/ULWG27.LWG27R71
   then
        rm ${reportdir}/ULWG27.LWG27R71
fi
if test -a ${reportdir}/ULWG27.LWG27R73
   then
        rm ${reportdir}/ULWG27.LWG27R73
fi
if test -a ${reportdir}/ULWG27.LWG27R75
   then
        rm ${reportdir}/ULWG27.LWG27R75
fi
if test -a ${reportdir}/ULWG27.LWG27R76
   then
        rm ${reportdir}/ULWG27.LWG27R76
fi
if test -a ${reportdir}/ULWG27.LWG27R77
   then
        rm ${reportdir}/ULWG27.LWG27R77
fi
if test -a ${reportdir}/ULWG27.LWG27R78
   then
        rm ${reportdir}/ULWG27.LWG27R78
fi
if test -a ${reportdir}/ULWG27.LWG27R79
   then
        rm ${reportdir}/ULWG27.LWG27R79
fi
if test -a ${reportdir}/ULWG27.LWG27R80
   then
        rm ${reportdir}/ULWG27.LWG27R80
fi
if test -a ${reportdir}/ULWG27.LWG27R81
   then
        rm ${reportdir}/ULWG27.LWG27R81
fi
if test -a ${reportdir}/ULWG27.LWG27R82
   then
        rm ${reportdir}/ULWG27.LWG27R82
fi
if test -a ${reportdir}/ULWG27.LWG27R84
   then
        rm ${reportdir}/ULWG27.LWG27R84
fi
if test -a ${reportdir}/ULWG27.LWG27R85
   then
        rm ${reportdir}/ULWG27.LWG27R85
fi
if test -a ${reportdir}/ULWG27.LWG27R86
   then
        rm ${reportdir}/ULWG27.LWG27R86
fi
if test -a ${reportdir}/ULWG27.LWG27R87
   then
        rm ${reportdir}/ULWG27.LWG27R87
fi
if test -a ${reportdir}/ULWG27.LWG27R88
   then
        rm ${reportdir}/ULWG27.LWG27R88
fi
if test -a ${reportdir}/ULWG27.LWG27R89
   then
        rm ${reportdir}/ULWG27.LWG27R89
fi
if test -a ${reportdir}/ULWG27.LWG27R97
   then
        rm ${reportdir}/ULWG27.LWG27R97
fi
if test -a ${reportdir}/ULWG27.LWG27R98
   then
        rm ${reportdir}/ULWG27.LWG27R98
fi
if test -a ${reportdir}/ULWG27.LWG27R99
   then
        rm ${reportdir}/ULWG27.LWG27R99
fi
if test -a ${reportdir}/ULWG27.LWG27R90
   then
        rm ${reportdir}/ULWG27.LWG27R90
fi
if test -a ${reportdir}/ULWG27.LWG27R91
   then
        rm ${reportdir}/ULWG27.LWG27R91
fi
if test -a ${reportdir}/ULWG27.LWG27R92
   then
        rm ${reportdir}/ULWG27.LWG27R92
fi
if test -a ${reportdir}/ULWG27.LWG27R93
   then
        rm ${reportdir}/ULWG27.LWG27R93
fi
if test -a ${reportdir}/ULWG27.LWG27R94
   then
        rm ${reportdir}/ULWG27.LWG27R94
fi
if test -a ${reportdir}/ULWG27.LWG27R95
   then
        rm ${reportdir}/ULWG27.LWG27R95
fi
if test -a ${reportdir}/ULWG27.LWG27R96
   then
        rm ${reportdir}/ULWG27.LWG27R96
fi
if test -a ${reportdir}/ULWG27.LWG27R97
   then
        rm ${reportdir}/ULWG27.LWG27R97
fi
if test -a ${reportdir}/ULWG27.LWG27RZ
   then
        rm ${reportdir}/ULWG27.LWG27RZ
fi

# run the program

sas ${codedir}/UTLWG27.sas -log ${reportdir}/ULWG27.LWG27R1  -mautosource
