#UTLWA08.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA08.LWA08R1
then
rm ${reportdir}/ULWA08.LWA08R1
fi
if test -a ${reportdir}/ULWA08.LWA08R2
then
rm ${reportdir}/ULWA08.LWA08R2
fi
if test -a ${reportdir}/ULWA08.LWA08R3
then
rm ${reportdir}/ULWA08.LWA08R3
fi
if test -a ${reportdir}/ULWA08.LWA08R4
then
rm ${reportdir}/ULWA08.LWA08R4
fi
if test -a ${reportdir}/ULWA08.LWA08R5
then
rm ${reportdir}/ULWA08.LWA08R5
fi
if test -a ${reportdir}/ULWA08.LWA08R6
then
rm ${reportdir}/ULWA08.LWA08R6
fi
if test -a ${reportdir}/ULWA08.LWA08R7
then
rm ${reportdir}/ULWA08.LWA08R7
fi
if test -a ${reportdir}/ULWA08.LWA08R8
then
rm ${reportdir}/ULWA08.LWA08R8
fi
if test -a ${reportdir}/ULWA08.LWA08R9
then
rm ${reportdir}/ULWA08.LWA08R9
fi
if test -a ${reportdir}/ULWA08.LWA08R10
then
rm ${reportdir}/ULWA08.LWA08R10
fi
if test -a ${reportdir}/ULWA08.LWA08R11
then
rm ${reportdir}/ULWA08.LWA08R11
fi
if test -a ${reportdir}/ULWA08.LWA08R12
then
rm ${reportdir}/ULWA08.LWA08R12
fi
if test -a ${reportdir}/ULWA08.LWA08R13
then
rm ${reportdir}/ULWA08.LWA08R13
fi
if test -a ${reportdir}/ULWA08.LWA08R14
then
rm ${reportdir}/ULWA08.LWA08R14
fi
if test -a ${reportdir}/ULWA08.LWA08R15
then
rm ${reportdir}/ULWA08.LWA08R15
fi
if test -a ${reportdir}/ULWA08.LWA08R16
then
rm ${reportdir}/ULWA08.LWA08R16
fi
if test -a ${reportdir}/ULWA08.LWA08R17
then
rm ${reportdir}/ULWA08.LWA08R17
fi
if test -a ${reportdir}/ULWA08.LWA08R18
then
rm ${reportdir}/ULWA08.LWA08R18
fi
if test -a ${reportdir}/ULWA08.LWA08R19
then
rm ${reportdir}/ULWA08.LWA08R19
fi
if test -a ${reportdir}/ULWA08.LWA08R20
then
rm ${reportdir}/ULWA08.LWA08R20
fi
if test -a ${reportdir}/ULWA08.LWA08R21
then
rm ${reportdir}/ULWA08.LWA08R21
fi
if test -a ${reportdir}/ULWA08.LWA08R22
then
rm ${reportdir}/ULWA08.LWA08R22
fi
if test -a ${reportdir}/ULWA08.LWA08R23
then
rm ${reportdir}/ULWA08.LWA08R23
fi
if test -a ${reportdir}/ULWA08.LWA08R24
then
rm ${reportdir}/ULWA08.LWA08R24
fi
if test -a ${reportdir}/ULWA08.LWA08R25
then
rm ${reportdir}/ULWA08.LWA08R25
fi
if test -a ${reportdir}/ULWA08.LWA08RZ
then
rm ${reportdir}/ULWA08.LWA08RZ
fi

# run the program

sas ${codedir}/UTLWA08.sas -log ${reportdir}/ULWA08.LWA08R1  -mautosource

RC=$?
if [ $RC = 99 ]
 then
   echo "There were no loan sales for todays processing"
   exit 0
fi



