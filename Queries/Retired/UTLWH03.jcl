#UTLWH03.jcl 
#
#set environment variables
 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWH03.LWH03R1
then
rm ${reportdir}/ULWH03.LWH03R1
fi
if test -a ${reportdir}/ULWH03.LWH03RZ
then
rm ${reportdir}/ULWH03.LWH03RZ
fi
if test -a ${reportdir}/ULWH03.LWH03R2
then
rm ${reportdir}/ULWH03.LWH03R2
fi
if test -a ${reportdir}/ULWH03.LWH03R3
then
rm ${reportdir}/ULWH03.LWH03R3
fi
if test -a ${reportdir}/ULWH03.LWH03R4
then
rm ${reportdir}/ULWH03.LWH03R4
fi
if test -a ${reportdir}/ULWH03.LWH03R5
then
rm ${reportdir}/ULWH03.LWH03R5
fi
if test -a ${reportdir}/ULWH03.LWH03R6
then
rm ${reportdir}/ULWH03.LWH03R6
fi
if test -a ${reportdir}/ULWH03.LWH03R7
then
rm ${reportdir}/ULWH03.LWH03R7
fi
if test -a ${reportdir}/ULWH03.LWH03R8
then
rm ${reportdir}/ULWH03.LWH03R8
fi
if test -a ${reportdir}/ULWH03.LWH03R9
then
rm ${reportdir}/ULWH03.LWH03R9
fi
if test -a ${reportdir}/ULWH03.LWH03R10
then
rm ${reportdir}/ULWH03.LWH03R10
fi
if test -a ${reportdir}/ULWH03.LWH03R11
then
rm ${reportdir}/ULWH03.LWH03R11
fi
if test -a ${reportdir}/ULWH03.LWH03R12
then
rm ${reportdir}/ULWH03.LWH03R12
fi
if test -a ${reportdir}/ULWH03.LWH03R13
then
rm ${reportdir}/ULWH03.LWH03R13
fi
if test -a ${reportdir}/ULWH03.LWH03R14
then
rm ${reportdir}/ULWH03.LWH03R14
fi
if test -a ${reportdir}/ULWH03.LWH03R15
then
rm ${reportdir}/ULWH03.LWH03R15
fi
if test -a ${reportdir}/ULWH03.LWH03R16
then
rm ${reportdir}/ULWH03.LWH03R16
fi
if test -a ${reportdir}/ULWH03.LWH03R17
then
rm ${reportdir}/ULWH03.LWH03R17
fi
if test -a ${reportdir}/ULWH03.LWH03R18
then
rm ${reportdir}/ULWH03.LWH03R18
fi
if test -a ${reportdir}/ULWH03.LWH03R19
then
rm ${reportdir}/ULWH03.LWH03R19
fi
if test -a ${reportdir}/ULWH03.LWH03R20
then
rm ${reportdir}/ULWH03.LWH03R20
fi
if test -a ${reportdir}/ULWH03.LWH03R21
then
rm ${reportdir}/ULWH03.LWH03R21
fi
if test -a ${reportdir}/ULWH03.LWH03R22
then
rm ${reportdir}/ULWH03.LWH03R22
fi
if test -a ${reportdir}/ULWH03.LWH03R23
then
rm ${reportdir}/ULWH03.LWH03R23
fi
if test -a ${reportdir}/ULWH03.LWH03R24
then
rm ${reportdir}/ULWH03.LWH03R24
fi
if test -a ${reportdir}/ULWH03.LWH03R25
then
rm ${reportdir}/ULWH03.LWH03R25
fi
if test -a ${reportdir}/ULWH03.LWH03R26
then
rm ${reportdir}/ULWH03.LWH03R26
fi
if test -a ${reportdir}/ULWH03.LWH03R27
then
rm ${reportdir}/ULWH03.LWH03R27
fi
if test -a ${reportdir}/ULWH03.LWH03R29
then
rm ${reportdir}/ULWH03.LWH03R29
fi
if test -a ${reportdir}/ULWH03.LWH03R31
then
rm ${reportdir}/ULWH03.LWH03R31
fi
if test -a ${reportdir}/ULWH03.LWH03R33
then
rm ${reportdir}/ULWH03.LWH03R33
fi
if test -a ${reportdir}/ULWH03.LWH03R35
then
rm ${reportdir}/ULWH03.LWH03R35
fi
if test -a ${reportdir}/ULWH03.LWH03R37
then
rm ${reportdir}/ULWH03.LWH03R37
fi
if test -a ${reportdir}/ULWH03.LWH03R39
then
rm ${reportdir}/ULWH03.LWH03R39
fi
if test -a ${reportdir}/ULWH03.LWH03R41
then
rm ${reportdir}/ULWH03.LWH03R41
fi
if test -a ${reportdir}/ULWH03.LWH03R43
then
rm ${reportdir}/ULWH03.LWH03R43
fi
if test -a ${reportdir}/ULWH03.LWH03R45
then
rm ${reportdir}/ULWH03.LWH03R45
fi
if test -a ${reportdir}/ULWH03.LWH03R47
then
rm ${reportdir}/ULWH03.LWH03R47
fi
if test -a ${reportdir}/ULWH03.LWH03R49
then
rm ${reportdir}/ULWH03.LWH03R49
fi
if test -a ${reportdir}/ULWH03.LWH03R51
then
rm ${reportdir}/ULWH03.LWH03R51
fi
if test -a ${reportdir}/ULWH03.LWH03R53
then
rm ${reportdir}/ULWH03.LWH03R53
fi
if test -a ${reportdir}/ULWH03.LWH03R55
then
rm ${reportdir}/ULWH03.LWH03R55
fi
if test -a ${reportdir}/ULWH03.LWH03R57
then
rm ${reportdir}/ULWH03.LWH03R57
fi
if test -a ${reportdir}/ULWH03.LWH03R59
then
rm ${reportdir}/ULWH03.LWH03R59
fi
if test -a ${reportdir}/ULWH03.LWH03R61
then
rm ${reportdir}/ULWH03.LWH03R61
fi
if test -a ${reportdir}/ULWH03.LWH03R63
then
rm ${reportdir}/ULWH03.LWH03R63
fi
if test -a ${reportdir}/ULWH03.LWH03R65
then
rm ${reportdir}/ULWH03.LWH03R65
fi
if test -a ${reportdir}/ULWH03.LWH03R67
then
rm ${reportdir}/ULWH03.LWH03R67
fi
if test -a ${reportdir}/ULWH03.LWH03R69
then
rm ${reportdir}/ULWH03.LWH03R69
fi
if test -a ${reportdir}/ULWH03.LWH03R71
then
rm ${reportdir}/ULWH03.LWH03R71
fi
if test -a ${reportdir}/ULWH03.LWH03R73
then
rm ${reportdir}/ULWH03.LWH03R73
fi
if test -a ${reportdir}/ULWH03.LWH03R75
then
rm ${reportdir}/ULWH03.LWH03R75
fi
if test -a ${reportdir}/ULWH03.LWH03R77
then
rm ${reportdir}/ULWH03.LWH03R77
fi
if test -a ${reportdir}/ULWH03.LWH03R79
then
rm ${reportdir}/ULWH03.LWH03R79
fi
if test -a ${reportdir}/ULWH03.LWH03R80
then
rm ${reportdir}/ULWH03.LWH03R80
fi
if test -a ${reportdir}/ULWH03.LWH03R82
then
rm ${reportdir}/ULWH03.LWH03R82
fi
if test -a ${reportdir}/ULWH03.LWH03R81
then
rm ${reportdir}/ULWH03.LWH03R81
fi
if test -a ${reportdir}/ULWH03.LWH03R84
then
rm ${reportdir}/ULWH03.LWH03R84
fi
if test -a ${reportdir}/ULWH03.LWH03R85
then
rm ${reportdir}/ULWH03.LWH03R85
fi
if test -a ${reportdir}/ULWH03.LWH03R86
then
rm ${reportdir}/ULWH03.LWH03R86
fi
if test -a ${reportdir}/ULWH03.LWH03R52
then
rm ${reportdir}/ULWH03.LWH03R52
fi
if test -a ${reportdir}/ULWH03.LWH03R78
then
rm ${reportdir}/ULWH03.LWH03R78
fi
if test -a ${reportdir}/ULWH03.LWH03R87
then
rm ${reportdir}/ULWH03.LWH03R87
fi
if test -a ${reportdir}/ULWH03.LWH03R88
then
rm ${reportdir}/ULWH03.LWH03R88
fi
if test -a ${reportdir}/ULWH03.LWH03R89
then
rm ${reportdir}/ULWH03.LWH03R89
fi
if test -a ${reportdir}/ULWH03.LWH03R90
then
rm ${reportdir}/ULWH03.LWH03R90
fi
if test -a ${reportdir}/ULWH03.LWH03R91
then
rm ${reportdir}/ULWH03.LWH03R91
fi
if test -a ${reportdir}/ULWH03.LWH03R92
then
rm ${reportdir}/ULWH03.LWH03R92
fi
if test -a ${reportdir}/ULWH03.LWH03R93
then
rm ${reportdir}/ULWH03.LWH03R93
fi
if test -a ${reportdir}/ULWH03.LWH03R94
then
rm ${reportdir}/ULWH03.LWH03R94
fi
if test -a ${reportdir}/ULWH03.LWH03R95
then
rm ${reportdir}/ULWH03.LWH03R95
fi
if test -a ${reportdir}/ULWH03.LWH03R96
then
rm ${reportdir}/ULWH03.LWH03R96
fi
if test -a ${reportdir}/ULWH03.LWH03R97
then
rm ${reportdir}/ULWH03.LWH03R97
fi
if test -a ${reportdir}/ULWH03.LWH03R98
then
rm ${reportdir}/ULWH03.LWH03R98
fi
if test -a ${reportdir}/ULWH03.LWH03R99
then
rm ${reportdir}/ULWH03.LWH03R99
fi

# run the program
sas ${codedir}/UTLWH03.sas -log ${reportdir}/ULWH03.LWH03R1  -mautosource
