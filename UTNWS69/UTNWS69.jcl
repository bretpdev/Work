#UTNWS69.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS69.NWS69R1
then
rm ${reportdir}/UNWS69.NWS69R1
fi
if test -a ${reportdir}/UNWS69.NWS69R2
then
rm ${reportdir}/UNWS69.NWS69R2
fi
if test -a ${reportdir}/UNWS69.NWS69R3
then
rm ${reportdir}/UNWS69.NWS69R3
fi

# run the program

sas ${codedir}/UTNWS69.sas -log ${reportdir}/UNWS69.NWS69R1  -mautosource
