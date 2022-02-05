#UTNWS77.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS77.NWS77R1
then
rm ${reportdir}/UNWS77.NWS77R1
fi
if test -a ${reportdir}/UNWS77.NWS77RZ
then
rm ${reportdir}/UNWS77.NWS77RZ
fi
if test -a ${reportdir}/UNWS77.NWS77R2
then
rm ${reportdir}/UNWS77.NWS77R2
fi
if test -a ${reportdir}/UNWS77.NWS77R3
then
rm ${reportdir}/UNWS77.NWS77R3
fi


# run the program

sas ${codedir}/UTNWS77.sas -log ${reportdir}/UNWS77.NWS77R1  -mautosource
