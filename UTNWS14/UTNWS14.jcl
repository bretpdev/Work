#UTNWS14.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS14.NWS14R1
then
rm ${reportdir}/UNWS14.NWS14R1
fi
if test -a ${reportdir}/UNWS14.NWS14RZ
then
rm ${reportdir}/UNWS14.NWS14RZ
fi
if test -a ${reportdir}/UNWS14.NWS14R2
then
rm ${reportdir}/UNWS14.NWS14R2
fi
if test -a ${reportdir}/UNWS14.NWS14R3
then
rm ${reportdir}/UNWS14.NWS14R3
fi
if test -a ${reportdir}/UNWS14.NWS14R4
then
rm ${reportdir}/UNWS14.NWS14R4
fi
if test -a ${reportdir}/UNWS14.NWS14R5
then
rm ${reportdir}/UNWS14.NWS14R5
fi
if test -a ${reportdir}/UNWS14.NWS14R6
then
rm ${reportdir}/UNWS14.NWS14R6
fi
if test -a ${reportdir}/UNWS14.NWS14R7
then
rm ${reportdir}/UNWS14.NWS14R7
fi
if test -a ${reportdir}/UNWS14.NWS14R8
then
rm ${reportdir}/UNWS14.NWS14R8
fi
if test -a ${reportdir}/UNWS14.NWS14R9
then
rm ${reportdir}/UNWS14.NWS14R9
fi
if test -a ${reportdir}/UNWS14.NWS14R10
then
rm ${reportdir}/UNWS14.NWS14R10
fi
if test -a ${reportdir}/UNWS14.NWS14R11
then
rm ${reportdir}/UNWS14.NWS14R11
fi
if test -a ${reportdir}/UNWS14.NWS14R12
then
rm ${reportdir}/UNWS14.NWS14R12
fi
if test -a ${reportdir}/UNWS14.NWS14R13
then
rm ${reportdir}/UNWS14.NWS14R13
fi
if test -a ${reportdir}/UNWS14.NWS14R14
then
rm ${reportdir}/UNWS14.NWS14R14
fi
if test -a ${reportdir}/UNWS14.NWS14R16
then
rm ${reportdir}/UNWS14.NWS14R16
fi
if test -a ${reportdir}/UNWS14.NWS14R17
then
rm ${reportdir}/UNWS14.NWS14R17
fi
if test -a ${reportdir}/UNWS14.NWS14R18
then
rm ${reportdir}/UNWS14.NWS14R18
fi
if test -a ${reportdir}/UNWS14.NWS14R19
then
rm ${reportdir}/UNWS14.NWS14R19
fi
if test -a ${reportdir}/UNWS14.NWS14R20
then
rm ${reportdir}/UNWS14.NWS14R20
fi
if test -a ${reportdir}/UNWS14.NWS14R21
then
rm ${reportdir}/UNWS14.NWS14R21
fi
if test -a ${reportdir}/UNWS14.NWS14R22
then
rm ${reportdir}/UNWS14.NWS14R22
fi
if test -a ${reportdir}/UNWS14.NWS14R23
then
rm ${reportdir}/UNWS14.NWS14R23
fi
if test -a ${reportdir}/UNWS14.NWS14R24
then
rm ${reportdir}/UNWS14.NWS14R24
fi
if test -a ${reportdir}/UNWS14.NWS14R25
then
rm ${reportdir}/UNWS14.NWS14R25
fi

# run the program

sas ${codedir}/UTNWS14.sas -log ${reportdir}/UNWS14.NWS14R1  -mautosource
