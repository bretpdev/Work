#UTNWS12.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS12.NWS12R1
then
rm ${reportdir}/UNWS12.NWS12R1
fi
if test -a ${reportdir}/UNWS12.NWS12RZ
then
rm ${reportdir}/UNWS12.NWS12RZ
fi
if test -a ${reportdir}/UNWS12.NWS12R2
then
rm ${reportdir}/UNWS12.NWS12R2
fi
if test -a ${reportdir}/UNWS12.NWS12R3
then
rm ${reportdir}/UNWS12.NWS12R3
fi
if test -a ${reportdir}/UNWS12.NWS12R4
then
rm ${reportdir}/UNWS12.NWS12R4
fi
if test -a ${reportdir}/UNWS12.NWS12R5
then
rm ${reportdir}/UNWS12.NWS12R5
fi
if test -a ${reportdir}/UNWS12.NWS12R6
then
rm ${reportdir}/UNWS12.NWS12R6
fi
if test -a ${reportdir}/UNWS12.NWS12R7
then
rm ${reportdir}/UNWS12.NWS12R7
fi
if test -a ${reportdir}/UNWS12.NWS12R8
then
rm ${reportdir}/UNWS12.NWS12R8
fi
if test -a ${reportdir}/UNWS12.NWS12R9
then
rm ${reportdir}/UNWS12.NWS12R9
fi
if test -a ${reportdir}/UNWS12.NWS12R10
then
rm ${reportdir}/UNWS12.NWS12R10
fi
if test -a ${reportdir}/UNWS12.NWS12R11
then
rm ${reportdir}/UNWS12.NWS12R11
fi
if test -a ${reportdir}/UNWS12.NWS12R12
then
rm ${reportdir}/UNWS12.NWS12R12
fi
if test -a ${reportdir}/UNWS12.NWS12R13
then
rm ${reportdir}/UNWS12.NWS12R13
fi
if test -a ${reportdir}/UNWS12.NWS12R14
then
rm ${reportdir}/UNWS12.NWS12R14
fi
if test -a ${reportdir}/UNWS12.NWS12R15
then
rm ${reportdir}/UNWS12.NWS12R15
fi
if test -a ${reportdir}/UNWS12.NWS12R16
then
rm ${reportdir}/UNWS12.NWS12R16
fi
if test -a ${reportdir}/UNWS12.NWS12R17
then
rm ${reportdir}/UNWS12.NWS12R17
fi
if test -a ${reportdir}/UNWS12.NWS12R18
then
rm ${reportdir}/UNWS12.NWS12R18
fi

# run the program

sas ${codedir}/UTNWS12.sas -log ${reportdir}/UNWS12.NWS12R1  -mautosource
