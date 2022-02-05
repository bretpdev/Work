#UTNWS61.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS61.NWS61R1
then
rm ${reportdir}/UNWS61.NWS61R1
fi
if test -a ${reportdir}/UNWS61.NWS61RZ
then
rm ${reportdir}/UNWS61.NWS61RZ
fi
if test -a ${reportdir}/UNWS61.NWS61R2
then
rm ${reportdir}/UNWS61.NWS61R2
fi
if test -a ${reportdir}/UNWS61.NWS61R3
then
rm ${reportdir}/UNWS61.NWS61R3
fi
if test -a ${reportdir}/UNWS61.NWS61R4
then
rm ${reportdir}/UNWS61.NWS61R4
fi

# run the program

sas ${codedir}/UTNWS61.sas -log ${reportdir}/UNWS61.NWS61R1  -mautosource
