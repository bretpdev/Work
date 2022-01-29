#UTNWDW1.jcl CornerStone Data Warehouse
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWDW1.NWDW1R1
then
rm ${reportdir}/UNWDW1.NWDW1R1
fi
if test -a ${reportdir}/UNWDW1.NWDW1RZ
then
rm ${reportdir}/UNWDW1.NWDW1RZ
fi
if test -a ${reportdir}/UNWDW1.NWDW1R2
then
rm ${reportdir}/UNWDW1.NWDW1R2
fi
if test -a ${reportdir}/UNWDW1.NWDW1R4
then
rm ${reportdir}/UNWDW1.NWDW1R4
fi
if test -a ${reportdir}/UNWDW1.NWDW1R6
then
rm ${reportdir}/UNWDW1.NWDW1R6
fi
if test -a ${reportdir}/UNWDW1.NWDW1R7
then
rm ${reportdir}/UNWDW1.NWDW1R7
fi
if test -a ${reportdir}/UNWDW1.NWDW1R8
then
rm ${reportdir}/UNWDW1.NWDW1R8
fi
if test -a ${reportdir}/UNWDW1.NWDW1R9
then
rm ${reportdir}/UNWDW1.NWDW1R9
fi
if test -a ${reportdir}/UNWDW1.NWDW1R11
then
rm ${reportdir}/UNWDW1.NWDW1R11
fi
if test -a ${reportdir}/UNWDW1.NWDW1R12
then
rm ${reportdir}/UNWDW1.NWDW1R12
fi
if test -a ${reportdir}/UNWDW1.NWDW1R13
then
rm ${reportdir}/UNWDW1.NWDW1R13
fi
if test -a ${reportdir}/UNWDW1.NWDW1R14
then
rm ${reportdir}/UNWDW1.NWDW1R14
fi
if test -a ${reportdir}/UNWDW1.NWDW1R16
then
rm ${reportdir}/UNWDW1.NWDW1R16
fi
if test -a ${reportdir}/UNWDW1.NWDW1R17
then
rm ${reportdir}/UNWDW1.NWDW1R17
fi
if test -a ${reportdir}/UNWDW1.NWDW1R20
then
rm ${reportdir}/UNWDW1.NWDW1R20
fi
if test -a ${reportdir}/UNWDW1.NWDW1R23
then
rm ${reportdir}/UNWDW1.NWDW1R23
fi
if test -a ${reportdir}/UNWDW1.NWDW1R25
then
rm ${reportdir}/UNWDW1.NWDW1R25
fi
if test -a ${reportdir}/UNWDW1.NWDW1R26
then
rm ${reportdir}/UNWDW1.NWDW1R26
fi
if test -a ${reportdir}/UNWDW1.NWDW1R29
then
rm ${reportdir}/UNWDW1.NWDW1R29
fi
if test -a ${reportdir}/UNWDW1.NWDW1R32
then
rm ${reportdir}/UNWDW1.NWDW1R32
fi
if test -a ${reportdir}/UNWDW1.NWDW1R37
then
rm ${reportdir}/UNWDW1.NWDW1R37
fi
if test -a ${reportdir}/UNWDW1.NWDW1R44
then
rm ${reportdir}/UNWDW1.NWDW1R44
fi
if test -a ${reportdir}/UNWDW1.NWDW1R49
then
rm ${reportdir}/UNWDW1.NWDW1R49
fi

# run the program

sas ${codedir}/UTNWDW1.sas -log ${reportdir}/UNWDW1.NWDW1R1  -mautosource
