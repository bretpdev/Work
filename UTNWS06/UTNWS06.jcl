#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS06.NWS06R1
then
rm ${reportdir}/UNWS06.NWS06R1
fi
if test -a ${reportdir}/UNWS06.NWS06R2
then
rm ${reportdir}/UNWS06.NWS06R2
fi
if test -a ${reportdir}/UNWS06.NWS06R3
then
rm ${reportdir}/UNWS06.NWS06R3
fi
if test -a ${reportdir}/UNWS06.NWS06R4
then
rm ${reportdir}/UNWS06.NWS06R4
fi

# run the program

sas ${codedir}/UTNWS06.sas -log ${reportdir}/UNWS06.NWS06R1  -mautosource
