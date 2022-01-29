#UTNWR06.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWR06.NWR06R1
then
rm ${reportdir}/UNWR06.NWR06R1
fi
if test -a ${reportdir}/UNWR06.NWR06R2
then
rm ${reportdir}/UNWR06.NWR06R2
fi
if test -a ${reportdir}/UNWR06.NWR06R3
then
rm ${reportdir}/UNWR06.NWR06R3
fi
if test -a ${reportdir}/UNWR06.NWR06R4
then
rm ${reportdir}/UNWR06.NWR06R4
fi

# run the program

sas ${codedir}/UTNWR06.sas -log ${reportdir}/UNWR06.NWR06R1  -mautosource
