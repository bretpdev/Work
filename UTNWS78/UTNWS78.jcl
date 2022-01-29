#UTNWS78.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS78.NWS78R1
then
rm ${reportdir}/UNWS78.NWS78R1
fi
if test -a ${reportdir}/UNWS78.NWS78RZ
then
rm ${reportdir}/UNWS78.NWS78RZ
fi
if test -a ${reportdir}/UNWS78.NWS78R2
then
rm ${reportdir}/UNWS78.NWS78R2
fi
if test -a ${reportdir}/UNWS78.NWS78R3
then
rm ${reportdir}/UNWS78.NWS78R3
fi
if test -a ${reportdir}/UNWS78.NWS78R4
then
rm ${reportdir}/UNWS78.NWS78R4
fi
if test -a ${reportdir}/UNWS78.NWS78R5
then
rm ${reportdir}/UNWS78.NWS78R5
fi


# run the program

sas ${codedir}/UTNWS78.sas -log ${reportdir}/UNWS78.NWS78R1  -mautosource
