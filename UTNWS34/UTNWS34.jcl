#UTNWS34.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS34.NWS34R1
then
rm ${reportdir}/UNWS34.NWS34R1
fi
if test -a ${reportdir}/UNWS34.NWS34RZ
then
rm ${reportdir}/UNWS34.NWS34RZ
fi
if test -a ${reportdir}/UNWS34.NWS34R2
then
rm ${reportdir}/UNWS34.NWS34R2
fi
if test -a ${reportdir}/UNWS34.NWS34R3
then
rm ${reportdir}/UNWS34.NWS34R3
fi


# run the program

sas ${codedir}/UTNWS34.sas -log ${reportdir}/UNWS34.NWS34R1  -mautosource
