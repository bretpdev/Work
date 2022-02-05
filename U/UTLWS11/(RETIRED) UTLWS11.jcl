#UTLWS11.jcl BORROWER SERVICES FORBEARANCE
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS11.LWS11R1
then
rm ${reportdir}/ULWS11.LWS11R1
fi
if test -a ${reportdir}/ULWS11.LWS11R2
then
rm ${reportdir}/ULWS11.LWS11R2
fi
if test -a ${reportdir}/ULWS11.LWS11R3
then
rm ${reportdir}/ULWS11.LWS11R3
fi
if test -a ${reportdir}/ULWS11.LWS11R4
then
rm ${reportdir}/ULWS11.LWS11R4
fi
if test -a ${reportdir}/ULWS11.LWS11R5
then
rm ${reportdir}/ULWS11.LWS11R5
fi
if test -a ${reportdir}/ULWS11.LWS11R6
then
rm ${reportdir}/ULWS11.LWS11R6
fi
if test -a ${reportdir}/ULWS11.LWS11R7
then
rm ${reportdir}/ULWS11.LWS11R7
fi
if test -a ${reportdir}/ULWS11.LWS11R8
then
rm ${reportdir}/ULWS11.LWS11R8
fi
if test -a ${reportdir}/ULWS11.LWS11R9
then
rm ${reportdir}/ULWS11.LWS11R9
fi
if test -a ${reportdir}/ULWS11.LWS11R10
then
rm ${reportdir}/ULWS11.LWS11R10
fi
if test -a ${reportdir}/ULWS11.LWS11R11
then
rm ${reportdir}/ULWS11.LWS11R11
fi
if test -a ${reportdir}/ULWS11.LWS11R12
then
rm ${reportdir}/ULWS11.LWS11R12
fi
if test -a ${reportdir}/ULWS11.LWS11R13
then
rm ${reportdir}/ULWS11.LWS11R13
fi
if test -a ${reportdir}/ULWS11.LWS11R14
then
rm ${reportdir}/ULWS11.LWS11R14
fi
if test -a ${reportdir}/ULWS11.LWS11R15
then
rm ${reportdir}/ULWS11.LWS11R15
fi
if test -a ${reportdir}/ULWS11.LWS11R16
then
rm ${reportdir}/ULWS11.LWS11R16
fi
if test -a ${reportdir}/ULWS11.LWS11R17
then
rm ${reportdir}/ULWS11.LWS11R17
fi
if test -a ${reportdir}/ULWS11.LWS11R18
then
rm ${reportdir}/ULWS11.LWS11R18
fi
if test -a ${reportdir}/ULWS11.LWS11R19
then
rm ${reportdir}/ULWS11.LWS11R19
fi
if test -a ${reportdir}/ULWS11.LWS11R26
then
rm ${reportdir}/ULWS11.LWS11R26
fi
if test -a ${reportdir}/ULWS11.LWS11R27
then
rm ${reportdir}/ULWS11.LWS11R27
fi
if test -a ${reportdir}/ULWS11.LWS11R28
then
rm ${reportdir}/ULWS11.LWS11R28
fi
if test -a ${reportdir}/ULWS11.LWS11R29
then
rm ${reportdir}/ULWS11.LWS11R29
fi
if test -a ${reportdir}/ULWS11.LWS11RZ
then
rm ${reportdir}/ULWS11.LWS11RZ
fi

# run the program

sas ${codedir}/UTLWS11.sas -log ${reportdir}/ULWS11.LWS11R1  -mautosource
