#UTLWO84.jcl Interest Statements to those in Forbearance
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO84.LWO84R1
then
rm ${reportdir}/ULWO84.LWO84R1
fi
if test -a ${reportdir}/ULWO84.LWO84R2
then
rm ${reportdir}/ULWO84.LWO84R2
fi
if test -a ${reportdir}/ULWO84.LWO84R3
then
rm ${reportdir}/ULWO84.LWO84R3
fi
if test -a ${reportdir}/ULWO84.LWO84R4
then
rm ${reportdir}/ULWO84.LWO84R4
fi
if test -a ${reportdir}/ULWO84.LWO84R5
then
rm ${reportdir}/ULWO84.LWO84R5
fi
if test -a ${reportdir}/ULWO84.LWO84R6
then
rm ${reportdir}/ULWO84.LWO84R6
fi
if test -a ${reportdir}/ULWO84.LWO84R7
then
rm ${reportdir}/ULWO84.LWO84R7
fi
if test -a ${reportdir}/ULWO84.LWO84R8
then
rm ${reportdir}/ULWO84.LWO84R8
fi
if test -a ${reportdir}/ULWO84.LWO84RZ
then
rm ${reportdir}/ULWO84.LWO84RZ
fi

# run the program

sas ${codedir}/UTLWO84.sas -log ${reportdir}/ULWO84.LWO84R1  -mautosource
