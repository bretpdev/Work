#UTNWR01.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWR01.NWR01R1
then
rm ${reportdir}/UNWR01.NWR01R1
fi
if test -a ${reportdir}/UNWR01.NWR01RZ
then
rm ${reportdir}/UNWR01.NWR01RZ
fi
if test -a ${reportdir}/UNWR01.NWR01R2
then
rm ${reportdir}/UNWR01.NWR01R2
fi
if test -a ${reportdir}/UNWR01.NWR01R3
then
rm ${reportdir}/UNWR01.NWR01R3
fi
if test -a ${reportdir}/UNWR01.NWR01R4
then
rm ${reportdir}/UNWR01.NWR01R4
fi
if test -a ${reportdir}/UNWR01.NWR01R5
then
rm ${reportdir}/UNWR01.NWR01R5
fi
if test -a ${reportdir}/UNWR01.NWR01R6
then
rm ${reportdir}/UNWR01.NWR01R6
fi
if test -a ${reportdir}/UNWR01.NWR01R7
then
rm ${reportdir}/UNWR01.NWR01R7
fi
if test -a ${reportdir}/UNWR01.NWR01R8
then
rm ${reportdir}/UNWR01.NWR01R8
fi
# run the program

sas ${codedir}/UTNWR01.sas -log ${reportdir}/UNWR01.NWR01R1  -mautosource
