#UTLWN09.jcl Lender Yearly Loan Sale Totals
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWN09.LWN09RZ
then
rm ${reportdir}/ULWN09.LWN09RZ
fi
if test -a ${reportdir}/ULWN09.LWN09R1
then
rm ${reportdir}/ULWN09.LWN09R1
fi
if test -a ${reportdir}/ULWN09.LWN09R2
then
rm ${reportdir}/ULWN09.LWN09R2
fi
if test -a ${reportdir}/ULWN09.LWN09R3
then
rm ${reportdir}/ULWN09.LWN09R3
fi
if test -a ${reportdir}/ULWN09.LWN09R4
then
rm ${reportdir}/ULWN09.LWN09R4
fi
if test -a ${reportdir}/ULWN09.LWN09R5
then
rm ${reportdir}/ULWN09.LWN09R5
fi
if test -a ${reportdir}/ULWN09.LWN09R6
then
rm ${reportdir}/ULWN09.LWN09R6
fi
if test -a ${reportdir}/ULWN09.LWN09R7
then
rm ${reportdir}/ULWN09.LWN09R7
fi
if test -a ${reportdir}/ULWN09.LWN09R8
then
rm ${reportdir}/ULWN09.LWN09R8
fi
if test -a ${reportdir}/ULWN09.LWN09R9
then
rm ${reportdir}/ULWN09.LWN09R9
fi
if test -a ${reportdir}/ULWN09.LWN09R10
then
rm ${reportdir}/ULWN09.LWN09R10
fi
if test -a ${reportdir}/ULWN09.LWN09R11
then
rm ${reportdir}/ULWN09.LWN09R11
fi
if test -a ${reportdir}/ULWN09.LWN09R12
then
rm ${reportdir}/ULWN09.LWN09R12
fi
if test -a ${reportdir}/ULWN09.LWN09R13
then
rm ${reportdir}/ULWN09.LWN09R13
fi
if test -a ${reportdir}/ULWN09.LWN09R14
then
rm ${reportdir}/ULWN09.LWN09R14
fi
if test -a ${reportdir}/ULWN09.LWN09R15
then
rm ${reportdir}/ULWN09.LWN09R15
fi
if test -a ${reportdir}/ULWN09.LWN09R16
then
rm ${reportdir}/ULWN09.LWN09R16
fi
if test -a ${reportdir}/ULWN09.LWN09R17
then
rm ${reportdir}/ULWN09.LWN09R17
fi
if test -a ${reportdir}/ULWN09.LWN09R18
then
rm ${reportdir}/ULWN09.LWN09R18
fi
if test -a ${reportdir}/ULWN09.LWN09R19
then
rm ${reportdir}/ULWN09.LWN09R19
fi
if test -a ${reportdir}/ULWN09.LWN09R20
then
rm ${reportdir}/ULWN09.LWN09R20
fi
if test -a ${reportdir}/ULWN09.LWN09R21
then
rm ${reportdir}/ULWN09.LWN09R21
fi
if test -a ${reportdir}/ULWN09.LWN09R22
then
rm ${reportdir}/ULWN09.LWN09R22
fi
if test -a ${reportdir}/ULWN09.LWN09R23
then
rm ${reportdir}/ULWN09.LWN09R23
fi
if test -a ${reportdir}/ULWN09.LWN09R24
then
rm ${reportdir}/ULWN09.LWN09R24
fi
if test -a ${reportdir}/ULWN09.LWN09R25
then
rm ${reportdir}/ULWN09.LWN09R25
fi
if test -a ${reportdir}/ULWN09.LWN09R26
then
rm ${reportdir}/ULWN09.LWN09R26
fi
if test -a ${reportdir}/ULWN09.LWN09R27
then
rm ${reportdir}/ULWN09.LWN09R27
fi
if test -a ${reportdir}/ULWN09.LWN09R28
then
rm ${reportdir}/ULWN09.LWN09R28
fi

# run the program

sas ${codedir}/UTLWN09.sas -log ${reportdir}/ULWN09.LWN09R1  -mautosource
