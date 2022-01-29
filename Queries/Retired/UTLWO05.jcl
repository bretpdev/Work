#UTLWO05.jcl DISBURSEMENTS NOT CONVERTED TO SERVICING
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO05.LWO05R1
then
rm ${reportdir}/ULWO05.LWO05R1
fi
if test -a ${reportdir}/ULWO05.LWO05R2
then
rm ${reportdir}/ULWO05.LWO05R2
fi
if test -a ${reportdir}/ULWO05.LWO05R3
then
rm ${reportdir}/ULWO05.LWO05R3
fi
if test -a ${reportdir}/ULWO05.LWO05R4
then
rm ${reportdir}/ULWO05.LWO05R4
fi
if test -a ${reportdir}/ULWO05.LWO05R5
then
rm ${reportdir}/ULWO05.LWO05R5
fi
if test -a ${reportdir}/ULWO05.LWO05R6
then
rm ${reportdir}/ULWO05.LWO05R6
fi
if test -a ${reportdir}/ULWO05.LWO05R7
then
rm ${reportdir}/ULWO05.LWO05R7
fi
if test -a ${reportdir}/ULWO05.LWO05R8
then
rm ${reportdir}/ULWO05.LWO05R8
fi
if test -a ${reportdir}/ULWO05.LWO05R9
then
rm ${reportdir}/ULWO05.LWO05R9
fi
if test -a ${reportdir}/ULWO05.LWO05R10
then
rm ${reportdir}/ULWO05.LWO05R10
fi
if test -a ${reportdir}/ULWO05.LWO05R11
then
rm ${reportdir}/ULWO05.LWO05R11
fi
if test -a ${reportdir}/ULWO05.LWO05R12
then
rm ${reportdir}/ULWO05.LWO05R12
fi
if test -a ${reportdir}/ULWO05.LWO05R13
then
rm ${reportdir}/ULWO05.LWO05R13
fi
if test -a ${reportdir}/ULWO05.LWO05R14
then
rm ${reportdir}/ULWO05.LWO05R14
fi
if test -a ${reportdir}/ULWO05.LWO05R15
then
rm ${reportdir}/ULWO05.LWO05R15
fi
if test -a ${reportdir}/ULWO05.LWO05R16
then
rm ${reportdir}/ULWO05.LWO05R16
fi
if test -a ${reportdir}/ULWO05.LWO05R17
then
rm ${reportdir}/ULWO05.LWO05R17
fi
if test -a ${reportdir}/ULWO05.LWO05R18
then
rm ${reportdir}/ULWO05.LWO05R18
fi
if test -a ${reportdir}/ULWO05.LWO05R19
then
rm ${reportdir}/ULWO05.LWO05R19
fi
if test -a ${reportdir}/ULWO05.LWO05R20
then
rm ${reportdir}/ULWO05.LWO05R20
fi
if test -a ${reportdir}/ULWO05.LWO05R21
then
rm ${reportdir}/ULWO05.LWO05R21
fi
if test -a ${reportdir}/ULWO05.LWO05R22
then
rm ${reportdir}/ULWO05.LWO05R22
fi
if test -a ${reportdir}/ULWO05.LWO05R23
then
rm ${reportdir}/ULWO05.LWO05R23
fi
if test -a ${reportdir}/ULWO05.LWO05R24
then
rm ${reportdir}/ULWO05.LWO05R24
fi
if test -a ${reportdir}/ULWO05.LWO05R25
then
rm ${reportdir}/ULWO05.LWO05R25
fi
if test -a ${reportdir}/ULWO05.LWO05RZ
then
rm ${reportdir}/ULWO05.LWO05RZ
fi

# run the program

sas ${codedir}/UTLWO05.sas -log ${reportdir}/ULWO05.LWO05R1  -mautosource
