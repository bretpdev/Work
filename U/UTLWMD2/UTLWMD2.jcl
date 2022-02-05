#UTLWMD2.jcl MauiDUDE PDEMS
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWMD2.LWMD2R1
then
rm ${reportdir}/ULWMD2.LWMD2R1
fi
if test -a ${reportdir}/ULWMD2.LWMD2RZ
then
rm ${reportdir}/ULWMD2.LWMD2RZ
fi
if test -a ${reportdir}/ULWMD2.LWMD2R2
then
rm ${reportdir}/ULWMD2.LWMD2R2
fi
if test -a ${reportdir}/ULWMD2.LWMD2R3
then
rm ${reportdir}/ULWMD2.LWMD2R3
fi
if test -a ${reportdir}/ULWMD2.LWMD2R4
then
rm ${reportdir}/ULWMD2.LWMD2R4
fi
if test -a ${reportdir}/ULWMD2.LWMD2R5
then
rm ${reportdir}/ULWMD2.LWMD2R5
fi
if test -a ${reportdir}/ULWMD2.LWMD2R6
then
rm ${reportdir}/ULWMD2.LWMD2R6
fi

# run the program

sas ${codedir}/UTLWMD2.sas -log ${reportdir}/ULWMD2.LWMD2R1  -mautosource
