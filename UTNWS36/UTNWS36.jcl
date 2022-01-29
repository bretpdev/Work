#UTNWS36.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS36.NWS36R1
then
rm ${reportdir}/UNWS36.NWS36R1
fi
if test -a ${reportdir}/UNWS36.NWS36RZ
then
rm ${reportdir}/UNWS36.NWS36RZ
fi
if test -a ${reportdir}/UNWS36.NWS36R2
then
rm ${reportdir}/UNWS36.NWS36R2
fi
if test -a ${reportdir}/UNWS36.NWS36R3
then
rm ${reportdir}/UNWS36.NWS36R3
fi
if test -a ${reportdir}/UNWS36.NWS36R4
then
rm ${reportdir}/UNWS36.NWS36R4
fi
if test -a ${reportdir}/UNWS36.NWS36R5
then
rm ${reportdir}/UNWS36.NWS36R5
fi
if test -a ${reportdir}/UNWS36.NWS36R6
then
rm ${reportdir}/UNWS36.NWS36R6
fi
if test -a ${reportdir}/UNWS36.NWS36R7
then
rm ${reportdir}/UNWS36.NWS36R7
fi

# run the program

sas ${codedir}/UTNWS36.sas -log ${reportdir}/UNWS36.NWS36R1  -mautosource
