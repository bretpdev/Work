#UTLWG50.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG50.LWG50RZ
then
rm ${reportdir}/ULWG50.LWG50RZ
fi
if test -a ${reportdir}/ULWG50.LWG50R1
then
rm ${reportdir}/ULWG50.LWG50R1
fi
if test -a ${reportdir}/ULWG50.LWG50R2
then
rm ${reportdir}/ULWG50.LWG50R2
fi
if test -a ${reportdir}/ULWG50.LWG50R3
then
rm ${reportdir}/ULWG50.LWG50R3
fi
if test -a ${reportdir}/ULWG50.LWG50R4
then
rm ${reportdir}/ULWG50.LWG50R4
fi
if test -a ${reportdir}/ULWG50.LWG50R5
then
rm ${reportdir}/ULWG50.LWG50R5
fi
if test -a ${reportdir}/ULWG50.LWG50R6
then
rm ${reportdir}/ULWG50.LWG50R6
fi
if test -a ${reportdir}/ULWG50.LWG50R7
then
rm ${reportdir}/ULWG50.LWG50R7
fi
if test -a ${reportdir}/ULWG50.LWG50R8
then
rm ${reportdir}/ULWG50.LWG50R8
fi
if test -a ${reportdir}/ULWG50.LWG50R9
then
rm ${reportdir}/ULWG50.LWG50R9
fi
if test -a ${reportdir}/ULWG50.LWG50R10
then
rm ${reportdir}/ULWG50.LWG50R10
fi
if test -a ${reportdir}/ULWG50.LWG50R11
then
rm ${reportdir}/ULWG50.LWG50R11
fi
if test -a ${reportdir}/ULWG50.LWG50R12
then
rm ${reportdir}/ULWG50.LWG50R12
fi
if test -a ${reportdir}/ULWG50.LWG50R13
then
rm ${reportdir}/ULWG50.LWG50R13
fi
if test -a ${reportdir}/ULWG50.LWG50R14
then
rm ${reportdir}/ULWG50.LWG50R14
fi
if test -a ${reportdir}/ULWG50.LWG50R15
then
rm ${reportdir}/ULWG50.LWG50R15
fi
if test -a ${reportdir}/ULWG50.LWG50R16
then
rm ${reportdir}/ULWG50.LWG50R16
fi
if test -a ${reportdir}/ULWG50.LWG50R17
then
rm ${reportdir}/ULWG50.LWG50R17
fi
if test -a ${reportdir}/ULWG50.LWG50R18
then
rm ${reportdir}/ULWG50.LWG50R18
fi
if test -a ${reportdir}/ULWG50.LWG50R19
then
rm ${reportdir}/ULWG50.LWG50R19
fi
if test -a ${reportdir}/ULWG50.LWG50R20
then
rm ${reportdir}/ULWG50.LWG50R20
fi
if test -a ${reportdir}/ULWG50.LWG50R21
then
rm ${reportdir}/ULWG50.LWG50R21
fi
if test -a ${reportdir}/ULWG50.LWG50R22
then
rm ${reportdir}/ULWG50.LWG50R22
fi
if test -a ${reportdir}/ULWG50.LWG50R23
then
rm ${reportdir}/ULWG50.LWG50R23
fi
if test -a ${reportdir}/ULWG50.LWG50R24
then
rm ${reportdir}/ULWG50.LWG50R24
fi

# run the program

sas ${codedir}/UTLWG50.sas -log ${reportdir}/ULWG50.LWG50R1  -mautosource
