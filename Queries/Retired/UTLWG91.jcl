#UTLWG91.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG91.LWG91R1
then
rm ${reportdir}/ULWG91.LWG91R1
fi
if test -a ${reportdir}/ULWG91.LWG91R2
then
rm ${reportdir}/ULWG91.LWG91R2
fi
if test -a ${reportdir}/ULWG91.LWG91R3
then
rm ${reportdir}/ULWG91.LWG91R3
fi
if test -a ${reportdir}/ULWG91.LWG91R4
then
rm ${reportdir}/ULWG91.LWG91R4
fi
if test -a ${reportdir}/ULWG91.LWG91R5
then
rm ${reportdir}/ULWG91.LWG91R5
fi
if test -a ${reportdir}/ULWG91.LWG91R6
then
rm ${reportdir}/ULWG91.LWG91R6
fi
if test -a ${reportdir}/ULWG91.LWG91R7
then
rm ${reportdir}/ULWG91.LWG91R7
fi
if test -a ${reportdir}/ULWG91.LWG91R8
then
rm ${reportdir}/ULWG91.LWG91R8
fi
if test -a ${reportdir}/ULWG91.LWG91R9
then
rm ${reportdir}/ULWG91.LWG91R9
fi
if test -a ${reportdir}/ULWG91.LWG91R10
then
rm ${reportdir}/ULWG91.LWG91R10
fi
if test -a ${reportdir}/ULWG91.LWG91R11
then
rm ${reportdir}/ULWG91.LWG91R11
fi
if test -a ${reportdir}/ULWG91.LWG91R12
then
rm ${reportdir}/ULWG91.LWG91R12
fi
if test -a ${reportdir}/ULWG91.LWG91R13
then
rm ${reportdir}/ULWG91.LWG91R13
fi
if test -a ${reportdir}/ULWG91.LWG91R14
then
rm ${reportdir}/ULWG91.LWG91R14
fi
if test -a ${reportdir}/ULWG91.LWG91R15
then
rm ${reportdir}/ULWG91.LWG91R15
fi
if test -a ${reportdir}/ULWG91.LWG91R16
then
rm ${reportdir}/ULWG91.LWG91R16
fi
if test -a ${reportdir}/ULWG91.LWG91R17
then
rm ${reportdir}/ULWG91.LWG91R17
fi
if test -a ${reportdir}/ULWG91.LWG91R18
then
rm ${reportdir}/ULWG91.LWG91R18
fi
if test -a ${reportdir}/ULWG91.LWG91R19
then
rm ${reportdir}/ULWG91.LWG91R19
fi
if test -a ${reportdir}/ULWG91.LWG91R20
then
rm ${reportdir}/ULWG91.LWG91R20
fi
if test -a ${reportdir}/ULWG91.LWG91R21
then
rm ${reportdir}/ULWG91.LWG91R21
fi
if test -a ${reportdir}/ULWG91.LWG91R22
then
rm ${reportdir}/ULWG91.LWG91R22
fi
if test -a ${reportdir}/ULWG91.LWG91R23
then
rm ${reportdir}/ULWG91.LWG91R23
fi
if test -a ${reportdir}/ULWG91.LWG91R24
then
rm ${reportdir}/ULWG91.LWG91R24
fi
if test -a ${reportdir}/ULWG91.LWG91R26
then
rm ${reportdir}/ULWG91.LWG91R26
fi
if test -a ${reportdir}/ULWG91.LWG91R27
then
rm ${reportdir}/ULWG91.LWG91R27
fi
if test -a ${reportdir}/ULWG91.LWG91R28
then
rm ${reportdir}/ULWG91.LWG91R28
fi
if test -a ${reportdir}/ULWG91.LWG91R29
then
rm ${reportdir}/ULWG91.LWG91R29
fi
if test -a ${reportdir}/ULWG91.LWG91R30
then
rm ${reportdir}/ULWG91.LWG91R30
fi
if test -a ${reportdir}/ULWG91.LWG91R31
then
rm ${reportdir}/ULWG91.LWG91R31
fi
if test -a ${reportdir}/ULWG91.LWG91R32
then
rm ${reportdir}/ULWG91.LWG91R32
fi
if test -a ${reportdir}/ULWG91.LWG91R33
then
rm ${reportdir}/ULWG91.LWG91R33
fi
if test -a ${reportdir}/ULWG91.LWG91R34
then
rm ${reportdir}/ULWG91.LWG91R34
fi
if test -a ${reportdir}/ULWG91.LWG91R35
then
rm ${reportdir}/ULWG91.LWG91R35
fi
if test -a ${reportdir}/ULWG91.LWG91R36
then
rm ${reportdir}/ULWG91.LWG91R36
fi
if test -a ${reportdir}/ULWG91.LWG91R37
then
rm ${reportdir}/ULWG91.LWG91R37
fi
if test -a ${reportdir}/ULWG91.LWG91R38
then
rm ${reportdir}/ULWG91.LWG91R38
fi
if test -a ${reportdir}/ULWG91.LWG91R39
then
rm ${reportdir}/ULWG91.LWG91R39
fi
if test -a ${reportdir}/ULWG91.LWG91R40
then
rm ${reportdir}/ULWG91.LWG91R40
fi
if test -a ${reportdir}/ULWG91.LWG91R41
then
rm ${reportdir}/ULWG91.LWG91R41
fi
if test -a ${reportdir}/ULWG91.LWG91R42
then
rm ${reportdir}/ULWG91.LWG91R42
fi
if test -a ${reportdir}/ULWG91.LWG91R43
then
rm ${reportdir}/ULWG91.LWG91R43
fi
if test -a ${reportdir}/ULWG91.LWG91R44
then
rm ${reportdir}/ULWG91.LWG91R44
fi
if test -a ${reportdir}/ULWG91.LWG91R45
then
rm ${reportdir}/ULWG91.LWG91R45
fi
if test -a ${reportdir}/ULWG91.LWG91R46
then
rm ${reportdir}/ULWG91.LWG91R46
fi
if test -a ${reportdir}/ULWG91.LWG91R47
then
rm ${reportdir}/ULWG91.LWG91R47
fi
if test -a ${reportdir}/ULWG91.LWG91R48
then
rm ${reportdir}/ULWG91.LWG91R48
fi
if test -a ${reportdir}/ULWG91.LWG91R49
then
rm ${reportdir}/ULWG91.LWG91R49
fi
if test -a ${reportdir}/ULWG91.LWG91R50
then
rm ${reportdir}/ULWG91.LWG91R50
fi
if test -a ${reportdir}/ULWG91.LWG91R51
then
rm ${reportdir}/ULWG91.LWG91R51
fi
if test -a ${reportdir}/ULWG91.LWG91R52
then
rm ${reportdir}/ULWG91.LWG91R52
fi
if test -a ${reportdir}/ULWG91.LWG91R53
then
rm ${reportdir}/ULWG91.LWG91R53
fi
if test -a ${reportdir}/ULWG91.LWG91R54
then
rm ${reportdir}/ULWG91.LWG91R54
fi
if test -a ${reportdir}/ULWG91.LWG91RZ
then
rm ${reportdir}/ULWG91.LWG91RZ
fi

# run the program

sas ${codedir}/UTLWG91.sas -log ${reportdir}/ULWG91.LWG91R1  -mautosource
