#UTNWS28.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS28.NWS28R1
then
rm ${reportdir}/UNWS28.NWS28R1
fi
if test -a ${reportdir}/UNWS28.NWS28RZ
then
rm ${reportdir}/UNWS28.NWS28RZ
fi
if test -a ${reportdir}/UNWS28.NWS28R2
then
rm ${reportdir}/UNWS28.NWS28R2
fi
if test -a ${reportdir}/UNWS28.NWS28R3
then
rm ${reportdir}/UNWS28.NWS28R3
fi

# run the program

sas ${codedir}/UTNWS28.sas -log ${reportdir}/UNWS28.NWS28R1  -mautosource
