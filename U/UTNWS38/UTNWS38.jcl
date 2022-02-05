#UTNWS38.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS38.NWS38R1
then
rm ${reportdir}/UNWS38.NWS38R1
fi
if test -a ${reportdir}/UNWS38.NWS38RZ
then
rm ${reportdir}/UNWS38.NWS38RZ
fi
if test -a ${reportdir}/UNWS38.NWS38R2
then
rm ${reportdir}/UNWS38.NWS38R2
fi
if test -a ${reportdir}/UNWS38.NWS38R3
then
rm ${reportdir}/UNWS38.NWS38R3
fi
if test -a ${reportdir}/UNWS38.NWS38R4
then
rm ${reportdir}/UNWS38.NWS38R4
fi

# run the program

sas ${codedir}/UTNWS38.sas -log ${reportdir}/UNWS38.NWS38R1  -mautosource
