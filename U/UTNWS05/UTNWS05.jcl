#UTNWS05.JCL Billing Statements (FED)

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS05.NWS05R1
then
rm ${reportdir}/UNWS05.NWS05R1
fi
if test -a ${reportdir}/UNWS05.NWS05R2
then
rm ${reportdir}/UNWS05.NWS05R2
fi
if test -a ${reportdir}/UNWS05.NWS05R3
then
rm ${reportdir}/UNWS05.NWS05R3
fi
if test -a ${reportdir}/UNWS05.NWS05R4
then
rm ${reportdir}/UNWS05.NWS05R4
fi
if test -a ${reportdir}/UNWS05.NWS05R5
then
rm ${reportdir}/UNWS05.NWS05R5
fi
if test -a ${reportdir}/UNWS05.NWS05R9
then
rm ${reportdir}/UNWS05.NWS05R9
fi
if test -a ${reportdir}/UNWS05.NWS05R10
then
rm ${reportdir}/UNWS05.NWS05R10
fi
if test -a ${reportdir}/UNWS05.NWS05R11
then
rm ${reportdir}/UNWS05.NWS05R11
fi
if test -a ${reportdir}/UNWS05.NWS05R12
then
rm ${reportdir}/UNWS05.NWS05R12
fi
if test -a ${reportdir}/UNWS05.NWS05R13
then
rm ${reportdir}/UNWS05.NWS05R13
fi
if test -a ${reportdir}/UNWS05.NWS05R15
then
rm ${reportdir}/UNWS05.NWS05R15
fi
if test -a ${reportdir}/UNWS05.NWS05R16
then
rm ${reportdir}/UNWS05.NWS05R16
fi
if test -a ${reportdir}/UNWS05.NWS05R17
then
rm ${reportdir}/UNWS05.NWS05R17
fi
if test -a ${reportdir}/UNWS05.NWS05R20
then
rm ${reportdir}/UNWS05.NWS05R20
fi
if test -a ${reportdir}/UNWS05.NWS05R21
then
rm ${reportdir}/UNWS05.NWS05R21
fi
if test -a ${reportdir}/UNWS05.NWS05R22
then
rm ${reportdir}/UNWS05.NWS05R22
fi
if test -a ${reportdir}/UNWS05.NWS05R23
then
rm ${reportdir}/UNWS05.NWS05R23
fi


# run the program

sas ${codedir}/UTNWS05.sas -log ${reportdir}/UNWS05.NWS05R1  -mautosource
