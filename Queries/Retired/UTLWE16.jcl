#UTLWE16.jcl MONTHLY REALLOCATIONS DISBURSEMENT TRANSACTIONS
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWE16.LWE16R1
then
rm ${reportdir}/ULWE16.LWE16R1
fi
if test -a ${reportdir}/ULWE16.LWE16R2
then
rm ${reportdir}/ULWE16.LWE16R2
fi
if test -a ${reportdir}/ULWE16.LWE16R3
then
rm ${reportdir}/ULWE16.LWE16R3
fi
if test -a ${reportdir}/ULWE16.LWE16R4
then
rm ${reportdir}/ULWE16.LWE16R4
fi
if test -a ${reportdir}/ULWE16.LWE16R5
then
rm ${reportdir}/ULWE16.LWE16R5
fi
if test -a ${reportdir}/ULWE16.LWE16R6
then
rm ${reportdir}/ULWE16.LWE16R6
fi
if test -a ${reportdir}/ULWE16.LWE16R7
then
rm ${reportdir}/ULWE16.LWE16R7
fi
if test -a ${reportdir}/ULWE16.LWE16R8
then
rm ${reportdir}/ULWE16.LWE16R8
fi
if test -a ${reportdir}/ULWE16.LWE16R9
then
rm ${reportdir}/ULWE16.LWE16R9
fi
if test -a ${reportdir}/ULWE16.LWE16R10
then
rm ${reportdir}/ULWE16.LWE16R10
fi
if test -a ${reportdir}/ULWE16.LWE16R11
then
rm ${reportdir}/ULWE16.LWE16R11
fi
if test -a ${reportdir}/ULWE16.LWE16R12
then
rm ${reportdir}/ULWE16.LWE16R12
fi
if test -a ${reportdir}/ULWE16.LWE16R13
then
rm ${reportdir}/ULWE16.LWE16R13
fi
if test -a ${reportdir}/ULWE16.LWE16R14
then
rm ${reportdir}/ULWE16.LWE16R14
fi
if test -a ${reportdir}/ULWE16.LWE16R15
then
rm ${reportdir}/ULWE16.LWE16R15
fi
if test -a ${reportdir}/ULWE16.LWE16R16
then
rm ${reportdir}/ULWE16.LWE16R16
fi
if test -a ${reportdir}/ULWE16.LWE16R17
then
rm ${reportdir}/ULWE16.LWE16R17
fi
if test -a ${reportdir}/ULWE16.LWE16R18
then
rm ${reportdir}/ULWE16.LWE16R18
fi
if test -a ${reportdir}/ULWE16.LWE16R19
then
rm ${reportdir}/ULWE16.LWE16R19
fi
if test -a ${reportdir}/ULWE16.LWE16R20
then
rm ${reportdir}/ULWE16.LWE16R20
fi
if test -a ${reportdir}/ULWE16.LWE16R21
then
rm ${reportdir}/ULWE16.LWE16R21
fi
if test -a ${reportdir}/ULWE16.LWE16R22
then
rm ${reportdir}/ULWE16.LWE16R22
fi
if test -a ${reportdir}/ULWE16.LWE16R23
then
rm ${reportdir}/ULWE16.LWE16R23
fi
if test -a ${reportdir}/ULWE16.LWE16R24
then
rm ${reportdir}/ULWE16.LWE16R24
fi
if test -a ${reportdir}/ULWE16.LWE16R25
then
rm ${reportdir}/ULWE16.LWE16R25
fi
if test -a ${reportdir}/ULWE16.LWE16R26
then
rm ${reportdir}/ULWE16.LWE16R26
fi
if test -a ${reportdir}/ULWE16.LWE16R27
then
rm ${reportdir}/ULWE16.LWE16R27
fi
if test -a ${reportdir}/ULWE16.LWE16R28
then
rm ${reportdir}/ULWE16.LWE16R28
fi
if test -a ${reportdir}/ULWE16.LWE16R29
then
rm ${reportdir}/ULWE16.LWE16R29
fi
if test -a ${reportdir}/ULWE16.LWE16R30
then
rm ${reportdir}/ULWE16.LWE16R30
fi
if test -a ${reportdir}/ULWE16.LWE16RZ
then
rm ${reportdir}/ULWE16.LWE16RZ
fi

# run the program

sas ${codedir}/UTLWE16.sas -log ${reportdir}/ULWE16.LWE16R1  -mautosource
