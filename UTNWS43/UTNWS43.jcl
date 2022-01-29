#UTNWS43.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS43.NWS43R1
then
rm ${reportdir}/UNWS43.NWS43R1
fi
if test -a ${reportdir}/UNWS43.NWS43R2
then
rm ${reportdir}/UNWS43.NWS43R2
fi


# run the program

sas ${codedir}/UTNWS43.sas -log ${reportdir}/UNWS43.NWS43R1  -mautosource
