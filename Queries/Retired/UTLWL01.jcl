#UTLWL01.jcl DAILY DISBURSEMENT REPORTS BY LENDER
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWL01.LWL01R1
then
rm ${reportdir}/ULWL01.LWL01R1
fi
if test -a ${reportdir}/ULWL01.LWL01R2
then
rm ${reportdir}/ULWL01.LWL01R2
fi
if test -a ${reportdir}/ULWL01.LWL01R3
then
rm ${reportdir}/ULWL01.LWL01R3
fi
if test -a ${reportdir}/ULWL01.LWL01R4
then
rm ${reportdir}/ULWL01.LWL01R4
fi
if test -a ${reportdir}/ULWL01.LWL01R5
then
rm ${reportdir}/ULWL01.LWL01R5
fi
if test -a ${reportdir}/ULWL01.LWL01R6
then
rm ${reportdir}/ULWL01.LWL01R6
fi
if test -a ${reportdir}/ULWL01.LWL01R7
then
rm ${reportdir}/ULWL01.LWL01R7
fi
if test -a ${reportdir}/ULWL01.LWL01R8
then
rm ${reportdir}/ULWL01.LWL01R8
fi
if test -a ${reportdir}/ULWL01.LWL01R9
then
rm ${reportdir}/ULWL01.LWL01R9
fi
if test -a ${reportdir}/ULWL01.LWL01R11
then
rm ${reportdir}/ULWL01.LWL01R11
fi
if test -a ${reportdir}/ULWL01.LWL01R12
then
rm ${reportdir}/ULWL01.LWL01R12
fi
if test -a ${reportdir}/ULWL01.LWL01R13
then
rm ${reportdir}/ULWL01.LWL01R13
fi
if test -a ${reportdir}/ULWL01.LWL01R14
then
rm ${reportdir}/ULWL01.LWL01R14
fi
if test -a ${reportdir}/ULWL01.LWL01R15
then
rm ${reportdir}/ULWL01.LWL01R15
fi
if test -a ${reportdir}/ULWL01.LWL01R16
then
rm ${reportdir}/ULWL01.LWL01R16
fi
if test -a ${reportdir}/ULWL01.LWL01R17
then
rm ${reportdir}/ULWL01.LWL01R17
fi
if test -a ${reportdir}/ULWL01.LWL01R18
then
rm ${reportdir}/ULWL01.LWL01R18
fi
if test -a ${reportdir}/ULWL01.LWL01R19
then
rm ${reportdir}/ULWL01.LWL01R19
fi
if test -a ${reportdir}/ULWL01.LWL01R20
then
rm ${reportdir}/ULWL01.LWL01R20
fi
if test -a ${reportdir}/ULWL01.LWL01R21
then
rm ${reportdir}/ULWL01.LWL01R21
fi
if test -a ${reportdir}/ULWL01.LWL01R22
then
rm ${reportdir}/ULWL01.LWL01R22
fi
if test -a ${reportdir}/ULWL01.LWL01R23
then
rm ${reportdir}/ULWL01.LWL01R23
fi
if test -a ${reportdir}/ULWL01.LWL01R24
then
rm ${reportdir}/ULWL01.LWL01R24
fi
if test -a ${reportdir}/ULWL01.LWL01R25
then
rm ${reportdir}/ULWL01.LWL01R25
fi
if test -a ${reportdir}/ULWL01.LWL01R26
then
rm ${reportdir}/ULWL01.LWL01R26
fi
if test -a ${reportdir}/ULWL01.LWL01R27
then
rm ${reportdir}/ULWL01.LWL01R27
fi
if test -a ${reportdir}/ULWL01.LWL01R28
then
rm ${reportdir}/ULWL01.LWL01R28
fi
if test -a ${reportdir}/ULWL01.LWL01R29
then
rm ${reportdir}/ULWL01.LWL01R29
fi
if test -a ${reportdir}/ULWL01.LWL01R30
then
rm ${reportdir}/ULWL01.LWL01R30
fi
if test -a ${reportdir}/ULWL01.LWL01R31
then
rm ${reportdir}/ULWL01.LWL01R31
fi
if test -a ${reportdir}/ULWL01.LWL01R32
then
rm ${reportdir}/ULWL01.LWL01R32
fi
if test -a ${reportdir}/ULWL01.LWL01R33
then
rm ${reportdir}/ULWL01.LWL01R33
fi
if test -a ${reportdir}/ULWL01.LWL01R34
then
rm ${reportdir}/ULWL01.LWL01R34
fi
if test -a ${reportdir}/ULWL01.LWL01RZ
then
rm ${reportdir}/ULWL01.LWL01RZ
fi

# run the program

sas ${codedir}/UTLWL01.sas -log ${reportdir}/ULWL01.LWL01R1  -mautosource
