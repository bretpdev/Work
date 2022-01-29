#UTLWH02.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWH02.LWH02R1
then
rm ${reportdir}/ULWH02.LWH02R1
fi
if test -a ${reportdir}/ULWH02.LWH02R2
then
rm ${reportdir}/ULWH02.LWH02R2
fi
if test -a ${reportdir}/ULWH02.LWH02R3
then
rm ${reportdir}/ULWH02.LWH02R3
fi
if test -a ${reportdir}/ULWH02.LWH02R4
then
rm ${reportdir}/ULWH02.LWH02R4
fi
if test -a ${reportdir}/ULWH02.LWH02R5
then
rm ${reportdir}/ULWH02.LWH02R5
fi
if test -a ${reportdir}/ULWH02.LWH02R6
then
rm ${reportdir}/ULWH02.LWH02R6
fi
if test -a ${reportdir}/ULWH02.LWH02R7
then
rm ${reportdir}/ULWH02.LWH02R7
fi
if test -a ${reportdir}/ULWH02.LWH02R8
then
rm ${reportdir}/ULWH02.LWH02R8
fi
if test -a ${reportdir}/ULWH02.LWH02R9
then
rm ${reportdir}/ULWH02.LWH02R9
fi
if test -a ${reportdir}/ULWH02.LWH02R10
then
rm ${reportdir}/ULWH02.LWH02R10
fi
if test -a ${reportdir}/ULWH02.LWH02R11
then
rm ${reportdir}/ULWH02.LWH02R11
fi
if test -a ${reportdir}/ULWH02.LWH02R12
then
rm ${reportdir}/ULWH02.LWH02R12
fi
if test -a ${reportdir}/ULWH02.LWH02R13
then
rm ${reportdir}/ULWH02.LWH02R13
fi
if test -a ${reportdir}/ULWH02.LWH02R14
then
rm ${reportdir}/ULWH02.LWH02R14
fi
if test -a ${reportdir}/ULWH02.LWH02R15
then
rm ${reportdir}/ULWH02.LWH02R15
fi
if test -a ${reportdir}/ULWH02.LWH02R16
then
rm ${reportdir}/ULWH02.LWH02R16
fi
if test -a ${reportdir}/ULWH02.LWH02RZ
then
rm ${reportdir}/ULWH02.LWH02RZ
fi

# run the program

sas ${codedir}/UTLWH02.sas -log ${reportdir}/ULWH02.LWH02R1  -mautosource
