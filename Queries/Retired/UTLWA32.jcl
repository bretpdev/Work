#UTLWA32.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA32.LWA32R1
then
rm ${reportdir}/ULWA32.LWA32R1
fi
if test -a ${reportdir}/ULWA32.LWA32R2
then
rm ${reportdir}/ULWA32.LWA32R2
fi
if test -a ${reportdir}/ULWA32.LWA32R3
then
rm ${reportdir}/ULWA32.LWA32R3
fi
if test -a ${reportdir}/ULWA32.LWA32R4
then
rm ${reportdir}/ULWA32.LWA32R4
fi
if test -a ${reportdir}/ULWA32.LWA32R5
then
rm ${reportdir}/ULWA32.LWA32R5
fi
if test -a ${reportdir}/ULWA32.LWA32R6
then
rm ${reportdir}/ULWA32.LWA32R6
fi
if test -a ${reportdir}/ULWA32.LWA32R7
then
rm ${reportdir}/ULWA32.LWA32R7
fi
if test -a ${reportdir}/ULWA32.LWA32R8
then
rm ${reportdir}/ULWA32.LWA32R8
fi
if test -a ${reportdir}/ULWA32.LWA32R9
then
rm ${reportdir}/ULWA32.LWA32R9
fi
if test -a ${reportdir}/ULWA32.LWA32RZ
then
rm ${reportdir}/ULWA32.LWA32RZ
fi

# run the program

sas ${codedir}/UTLWA32.sas -log ${reportdir}/ULWA32.LWA32R1  -mautosource
