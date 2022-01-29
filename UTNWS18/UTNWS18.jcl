#UTNWS18.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS18.NWS18R1
then
rm ${reportdir}/UNWS18.NWS18R1
fi
if test -a ${reportdir}/UNWS18.NWS18R2
then
rm ${reportdir}/UNWS18.NWS18R2
fi

# run the program

sas ${codedir}/UTNWS18.sas -log ${reportdir}/UNWS18.NWS18R1  -mautosource
