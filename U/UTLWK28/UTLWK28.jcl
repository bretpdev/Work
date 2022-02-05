#UTLWK28.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK28.LWK28RZ
then
rm ${reportdir}/ULWK28.LWK28RZ
fi
if test -a ${reportdir}/ULWK28.LWK28R1
then
rm ${reportdir}/ULWK28.LWK28R1
fi
if test -a ${reportdir}/ULWK28.LWK28R2
then
rm ${reportdir}/ULWK28.LWK28R2
fi
if test -a ${reportdir}/ULWK28.LWK28R3
then
rm ${reportdir}/ULWK28.LWK28R3
fi
if test -a ${reportdir}/ULWK28.LWK28R4
then
rm ${reportdir}/ULWK28.LWK28R4
fi
if test -a ${reportdir}/ULWK28.LWK28R5
then
rm ${reportdir}/ULWK28.LWK28R5
fi
if test -a ${reportdir}/ULWK28.LWK28R6
then
rm ${reportdir}/ULWK28.LWK28R6
fi
if test -a ${reportdir}/ULWK28.LWK28R7
then
rm ${reportdir}/ULWK28.LWK28R7
fi
if test -a ${reportdir}/ULWK28.LWK28R8
then
rm ${reportdir}/ULWK28.LWK28R8
fi
if test -a ${reportdir}/ULWK28.LWK28R9
then
rm ${reportdir}/ULWK28.LWK28R9
fi
if test -a ${reportdir}/ULWK28.LWK28R11
then
rm ${reportdir}/ULWK28.LWK28R11
fi
if test -a ${reportdir}/ULWK28.LWK28R12
then
rm ${reportdir}/ULWK28.LWK28R12
fi

# run the program

sas ${codedir}/UTLWK28.sas -log ${reportdir}/ULWK28.LWK28R1  -mautosource
