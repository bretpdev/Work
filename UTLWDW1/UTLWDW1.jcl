#UTLWDW1.jcl UHEAA Data Warehouse
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWDW1.LWDW1R1
then
rm ${reportdir}/ULWDW1.LWDW1R1
fi
if test -a ${reportdir}/ULWDW1.LWDW1RZ
then
rm ${reportdir}/ULWDW1.LWDW1RZ
fi
if test -a ${reportdir}/ULWDW1.LWDW1R2
then
rm ${reportdir}/ULWDW1.LWDW1R2
fi
if test -a ${reportdir}/ULWDW1.LWDW1R3
then
rm ${reportdir}/ULWDW1.LWDW1R3
fi
if test -a ${reportdir}/ULWDW1.LWDW1R4
then
rm ${reportdir}/ULWDW1.LWDW1R4
fi
if test -a ${reportdir}/ULWDW1.LWDW1R5
then
rm ${reportdir}/ULWDW1.LWDW1R5
fi
if test -a ${reportdir}/ULWDW1.LWDW1R6
then
rm ${reportdir}/ULWDW1.LWDW1R6
fi
if test -a ${reportdir}/ULWDW1.LWDW1R7
then
rm ${reportdir}/ULWDW1.LWDW1R7
fi
if test -a ${reportdir}/ULWDW1.LWDW1R8
then
rm ${reportdir}/ULWDW1.LWDW1R8
fi
if test -a ${reportdir}/ULWDW1.LWDW1R9
then
rm ${reportdir}/ULWDW1.LWDW1R9
fi
if test -a ${reportdir}/ULWDW1.LWDW1R10
then
rm ${reportdir}/ULWDW1.LWDW1R10
fi
if test -a ${reportdir}/ULWDW1.LWDW1R11
then
rm ${reportdir}/ULWDW1.LWDW1R11
fi
if test -a ${reportdir}/ULWDW1.LWDW1R12
then
rm ${reportdir}/ULWDW1.LWDW1R12
fi
if test -a ${reportdir}/ULWDW1.LWDW1R13
then
rm ${reportdir}/ULWDW1.LWDW1R13
fi
if test -a ${reportdir}/ULWDW1.LWDW1R14
then
rm ${reportdir}/ULWDW1.LWDW1R14
fi
if test -a ${reportdir}/ULWDW1.LWDW1R15
then
rm ${reportdir}/ULWDW1.LWDW1R15
fi
if test -a ${reportdir}/ULWDW1.LWDW1R16
then
rm ${reportdir}/ULWDW1.LWDW1R16
fi
if test -a ${reportdir}/ULWDW1.LWDW1R17
then
rm ${reportdir}/ULWDW1.LWDW1R17
fi
if test -a ${reportdir}/ULWDW1.LWDW1R18
then
rm ${reportdir}/ULWDW1.LWDW1R18
fi
if test -a ${reportdir}/ULWDW1.LWDW1R19
then
rm ${reportdir}/ULWDW1.LWDW1R19
fi
if test -a ${reportdir}/ULWDW1.LWDW1R20
then
rm ${reportdir}/ULWDW1.LWDW1R20
fi
if test -a ${reportdir}/ULWDW1.LWDW1R21
then
rm ${reportdir}/ULWDW1.LWDW1R21
fi
if test -a ${reportdir}/ULWDW1.LWDW1R22
then
rm ${reportdir}/ULWDW1.LWDW1R22
fi
if test -a ${reportdir}/ULWDW1.LWDW1R23
then
rm ${reportdir}/ULWDW1.LWDW1R23
fi
if test -a ${reportdir}/ULWDW1.LWDW1R24
then
rm ${reportdir}/ULWDW1.LWDW1R24
fi
if test -a ${reportdir}/ULWDW1.LWDW1R25
then
rm ${reportdir}/ULWDW1.LWDW1R25
fi
if test -a ${reportdir}/ULWDW1.LWDW1R26
then
rm ${reportdir}/ULWDW1.LWDW1R26
fi
if test -a ${reportdir}/ULWDW1.LWDW1R27
then
rm ${reportdir}/ULWDW1.LWDW1R27
fi
if test -a ${reportdir}/ULWDW1.LWDW1R28
then
rm ${reportdir}/ULWDW1.LWDW1R28
fi
if test -a ${reportdir}/ULWDW1.LWDW1R29
then
rm ${reportdir}/ULWDW1.LWDW1R29
fi
if test -a ${reportdir}/ULWDW1.LWDW1R30
then
rm ${reportdir}/ULWDW1.LWDW1R30
fi
if test -a ${reportdir}/ULWDW1.LWDW1R31
then
rm ${reportdir}/ULWDW1.LWDW1R31
fi
if test -a ${reportdir}/ULWDW1.LWDW1R32
then
rm ${reportdir}/ULWDW1.LWDW1R32
fi
if test -a ${reportdir}/ULWDW1.LWDW1R33
then
rm ${reportdir}/ULWDW1.LWDW1R33
fi
if test -a ${reportdir}/ULWDW1.LWDW1R34
then
rm ${reportdir}/ULWDW1.LWDW1R34
fi
if test -a ${reportdir}/ULWDW1.LWDW1R35
then
rm ${reportdir}/ULWDW1.LWDW1R35
fi
if test -a ${reportdir}/ULWDW1.LWDW1R36
then
rm ${reportdir}/ULWDW1.LWDW1R36
fi
if test -a ${reportdir}/ULWDW1.LWDW1R37
then
rm ${reportdir}/ULWDW1.LWDW1R37
fi
if test -a ${reportdir}/ULWDW1.LWDW1R48
then
rm ${reportdir}/ULWDW1.LWDW1R48
fi
if test -a ${reportdir}/ULWDW1.LWDW1R53
then
rm ${reportdir}/ULWDW1.LWDW1R53
fi

# run the program

sas ${codedir}/UTLWDW1.sas -log ${reportdir}/ULWDW1.LWDW1R1  -mautosource
