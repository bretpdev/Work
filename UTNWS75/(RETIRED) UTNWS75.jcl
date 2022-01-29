#UTNWS75.30+ Day Delinquent- No contact - FED New

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS75.NWS75R1
then
rm ${reportdir}/UNWS75.NWS75R1
fi
if test -a ${reportdir}/UNWS75.NWS75R2
then
rm ${reportdir}/UNWS75.NWS75R2
fi

# run the program

sas ${codedir}/UTNWS75.sas -log ${reportdir}/UNWS75.NWS75R1  -mautosource
