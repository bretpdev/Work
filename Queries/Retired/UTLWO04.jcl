#UTLWO04.jcl DISBURSEMENTS IN THE PRIOR MONTH CONVERTED TO SERVICING IN CURRENT MONTH
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO04.LWO04R1
then
rm ${reportdir}/ULWO04.LWO04R1
fi
if test -a ${reportdir}/ULWO04.LWO04R2
then
rm ${reportdir}/ULWO04.LWO04R2
fi
if test -a ${reportdir}/ULWO04.LWO04R3
then
rm ${reportdir}/ULWO04.LWO04R3
fi
if test -a ${reportdir}/ULWO04.LWO04R4
then
rm ${reportdir}/ULWO04.LWO04R4
fi
if test -a ${reportdir}/ULWO04.LWO04R5
then
rm ${reportdir}/ULWO04.LWO04R5
fi
if test -a ${reportdir}/ULWO04.LWO04R6
then
rm ${reportdir}/ULWO04.LWO04R6
fi
if test -a ${reportdir}/ULWO04.LWO04R7
then
rm ${reportdir}/ULWO04.LWO04R7
fi
if test -a ${reportdir}/ULWO04.LWO04R8
then
rm ${reportdir}/ULWO04.LWO04R8
fi
if test -a ${reportdir}/ULWO04.LWO04R9
then
rm ${reportdir}/ULWO04.LWO04R9
fi
if test -a ${reportdir}/ULWO04.LWO04R10
then
rm ${reportdir}/ULWO04.LWO04R10
fi
if test -a ${reportdir}/ULWO04.LWO04R11
then
rm ${reportdir}/ULWO04.LWO04R11
fi
if test -a ${reportdir}/ULWO04.LWO04R12
then
rm ${reportdir}/ULWO04.LWO04R12
fi
if test -a ${reportdir}/ULWO04.LWO04R13
then
rm ${reportdir}/ULWO04.LWO04R13
fi
if test -a ${reportdir}/ULWO04.LWO04R14
then
rm ${reportdir}/ULWO04.LWO04R14
fi
if test -a ${reportdir}/ULWO04.LWO04R15
then
rm ${reportdir}/ULWO04.LWO04R15
fi
if test -a ${reportdir}/ULWO04.LWO04R16
then
rm ${reportdir}/ULWO04.LWO04R16
fi
if test -a ${reportdir}/ULWO04.LWO04R17
then
rm ${reportdir}/ULWO04.LWO04R17
fi
if test -a ${reportdir}/ULWO04.LWO04R18
then
rm ${reportdir}/ULWO04.LWO04R18
fi
if test -a ${reportdir}/ULWO04.LWO04R19
then
rm ${reportdir}/ULWO04.LWO04R19
fi
if test -a ${reportdir}/ULWO04.LWO04R20
then
rm ${reportdir}/ULWO04.LWO04R20
fi
if test -a ${reportdir}/ULWO04.LWO04R21
then
rm ${reportdir}/ULWO04.LWO04R21
fi
if test -a ${reportdir}/ULWO04.LWO04R22
then
rm ${reportdir}/ULWO04.LWO04R22
fi
if test -a ${reportdir}/ULWO04.LWO04R23
then
rm ${reportdir}/ULWO04.LWO04R23
fi
if test -a ${reportdir}/ULWO04.LWO04R24
then
rm ${reportdir}/ULWO04.LWO04R24
fi
if test -a ${reportdir}/ULWO04.LWO04R25
then
rm ${reportdir}/ULWO04.LWO04R25
fi
if test -a ${reportdir}/ULWO04.LWO04R26
then
rm ${reportdir}/ULWO04.LWO04R26
fi
if test -a ${reportdir}/ULWO04.LWO04R27
then
rm ${reportdir}/ULWO04.LWO04R27
fi
if test -a ${reportdir}/ULWO04.LWO04R28
then
rm ${reportdir}/ULWO04.LWO04R28
fi
if test -a ${reportdir}/ULWO04.LWO04R29
then
rm ${reportdir}/ULWO04.LWO04R29
fi
if test -a ${reportdir}/ULWO04.LWO04RZ
then
rm ${reportdir}/ULWO04.LWO04RZ
fi

# run the program

sas ${codedir}/UTLWO04.sas -log ${reportdir}/ULWO04.LWO04R1  -mautosource
