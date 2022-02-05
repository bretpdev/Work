#UTNWS82.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS82.NWS82R1
then
rm ${reportdir}/UNWS82.NWS82R1
fi
if test -a ${reportdir}/UNWS82.NWS82RZ
then
rm ${reportdir}/UNWS82.NWS82RZ
fi
if test -a ${reportdir}/UNWS82.NWS82R2
then
rm ${reportdir}/UNWS82.NWS82R2
fi

# run the program

sas ${codedir}/UTNWS82.sas -log ${reportdir}/UNWS82.NWS82R1  -mautosource
