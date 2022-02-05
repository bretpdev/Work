#UTNWS35.jcl CornerStone Data Warehouse
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS35.NWS35R1
then
rm ${reportdir}/UNWS35.NWS35R1
fi
if test -a ${reportdir}/UNWS35.NWS35RZ
then
rm ${reportdir}/UNWS35.NWS35RZ
fi
if test -a ${reportdir}/UNWS35.NWS35R2
then
rm ${reportdir}/UNWS35.NWS35R2
fi
if test -a ${reportdir}/UNWS35.NWS35R3
then
rm ${reportdir}/UNWS35.NWS35R3
fi
if test -a ${reportdir}/UNWS35.NWS35R4
then
rm ${reportdir}/UNWS35.NWS35R4
fi
if test -a ${reportdir}/UNWS35.NWS35R5
then
rm ${reportdir}/UNWS35.NWS35R5
fi


# run the program

sas ${codedir}/UTNWS35.sas -log ${reportdir}/UNWS35.NWS35R1  -mautosource
