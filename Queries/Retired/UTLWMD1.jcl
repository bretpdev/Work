#UTLWMD1.jcl Dude Daily Update
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWMD1.LWMD1R1
then
rm ${reportdir}/ULWMD1.LWMD1R1
fi
if test -a ${reportdir}/ULWMD1.LWMD1RZ
then
rm ${reportdir}/ULWMD1.LWMD1RZ
fi
if test -a ${reportdir}/ULWMD1.LWMD1R2
then
rm ${reportdir}/ULWMD1.LWMD1R2
fi
if test -a ${reportdir}/ULWMD1.LWMD1R3
then
rm ${reportdir}/ULWMD1.LWMD1R3
fi
if test -a ${reportdir}/ULWMD1.LWMD1R4
then
rm ${reportdir}/ULWMD1.LWMD1R4
fi
if test -a ${reportdir}/ULWMD1.LWMD1R5
then
rm ${reportdir}/ULWMD1.LWMD1R5
fi
if test -a ${reportdir}/ULWMD1.LWMD1R6
then
rm ${reportdir}/ULWMD1.LWMD1R6
fi
if test -a ${reportdir}/ULWMD1.LWMD1R7
then
rm ${reportdir}/ULWMD1.LWMD1R7
fi
if test -a ${reportdir}/ULWMD1.LWMD1R8
then
rm ${reportdir}/ULWMD1.LWMD1R8
fi
if test -a ${reportdir}/ULWMD1.LWMD1R9
then
rm ${reportdir}/ULWMD1.LWMD1R9
fi
if test -a ${reportdir}/ULWMD1.LWMD1R10
then
rm ${reportdir}/ULWMD1.LWMD1R10
fi
if test -a ${reportdir}/ULWMD1.LWMD1R11
then
rm ${reportdir}/ULWMD1.LWMD1R11
fi
if test -a ${reportdir}/ULWMD1.LWMD1R12
then
rm ${reportdir}/ULWMD1.LWMD1R12
fi
if test -a ${reportdir}/ULWMD1.LWMD1R13
then
rm ${reportdir}/ULWMD1.LWMD1R13
fi
if test -a ${reportdir}/ULWMD1.LWMD1R14
then
rm ${reportdir}/ULWMD1.LWMD1R14
fi
if test -a ${reportdir}/ULWMD1.LWMD1R15
then
rm ${reportdir}/ULWMD1.LWMD1R15
fi
if test -a ${reportdir}/ULWMD1.LWMD1R16
then
rm ${reportdir}/ULWMD1.LWMD1R16
fi
if test -a ${reportdir}/ULWMD1.LWMD1R17
then
rm ${reportdir}/ULWMD1.LWMD1R17
fi
if test -a ${reportdir}/ULWMD1.LWMD1R18
then
rm ${reportdir}/ULWMD1.LWMD1R18
fi
if test -a ${reportdir}/ULWMD1.LWMD1R19
then
rm ${reportdir}/ULWMD1.LWMD1R19
fi
if test -a ${reportdir}/ULWMD1.LWMD1R20
then
rm ${reportdir}/ULWMD1.LWMD1R20
fi
if test -a ${reportdir}/ULWMD1.LWMD1R21
then
rm ${reportdir}/ULWMD1.LWMD1R21
fi
if test -a ${reportdir}/ULWMD1.LWMD1R22
then
rm ${reportdir}/ULWMD1.LWMD1R22
fi

# run the program

sas ${codedir}/UTLWMD1.sas -log ${reportdir}/ULWMD1.LWMD1R1  -mautosource
