#UTNWS40.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS40.NWS40R1
then
rm ${reportdir}/UNWS40.NWS40R1
fi
if test -a ${reportdir}/UNWS40.NWS40R2
then
rm ${reportdir}/UNWS40.NWS40R2
fi


# run the program

sas ${codedir}/UTNWS40.sas -log ${reportdir}/UNWS40.NWS40R1  -mautosource
